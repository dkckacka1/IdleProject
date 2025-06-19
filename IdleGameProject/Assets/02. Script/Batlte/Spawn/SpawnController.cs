using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Engine.Core;
using UnityEngine;
using IdleProject.Battle.Effect;
using IdleProject.Battle.Projectile;
using IdleProject.Character;
using IdleProject.Character.AI;
using IdleProject.Character.Character;
using IdleProject.Character.Skill;
using IdleProject.Character.Stat;
using IdleProject.Character.UI;
using IdleProject.Core;
using IdleProject.Core.GameData;
using IdleProject.Core.ObjectPool;
using IdleProject.Core.Resource;
using IdleProject.Data;

namespace IdleProject.Battle.Spawn
{
    public enum SpawnPositionType
    {
        FrontMiddle,
        FrontRight,
        FrontLeft,
        RearRight,
        RearLeft,
    }

    [Serializable]
    public class SpawnInfo
    {
        public SpawnFormation spawnFormation;
        public Transform spawnObject;
    }

    public class SpawnController : MonoBehaviour
    {
        [SerializeField] private SpawnInfo player;
        [SerializeField] private SpawnInfo enemy;

        public async UniTask SpawnCharacterAtInfo(CharacterAIType aiType, FormationInfoData formationInfo)
        {
            if(string.IsNullOrEmpty(formationInfo.frontLeftCharacterName) is false)
                await SpawnCharacter(aiType, SpawnPositionType.FrontLeft, formationInfo.frontLeftCharacterName);
            
            if(string.IsNullOrEmpty(formationInfo.frontMiddleCharacterName) is false)
                await SpawnCharacter(aiType, SpawnPositionType.FrontMiddle, formationInfo.frontMiddleCharacterName);
            
            if(string.IsNullOrEmpty(formationInfo.frontRightCharacterName) is false)
                await SpawnCharacter(aiType, SpawnPositionType.FrontRight, formationInfo.frontRightCharacterName);
            
            if(string.IsNullOrEmpty(formationInfo.rearLeftCharacterName) is false)
                await SpawnCharacter(aiType, SpawnPositionType.RearLeft, formationInfo.rearLeftCharacterName);
            
            if(string.IsNullOrEmpty(formationInfo.rearRightCharacterName) is false)
                await SpawnCharacter(aiType, SpawnPositionType.RearRight, formationInfo.rearRightCharacterName);
        }
        
        public async UniTask SpawnCharacter(CharacterAIType aiType, SpawnPositionType spawnPositionType,
            string characterName)
        {
            var controller = await CreateCharacter(characterName, aiType);

            GameManager.GetCurrentSceneManager<BattleManager>().AddCharacterController(controller);

            SetCharacterPosition(controller, aiType, spawnPositionType);
        }

        private void SetCharacterPosition(BattleCharacterController battleCharacter, CharacterAIType aiType,
            SpawnPositionType spawnPositionType)
        {
            SpawnInfo spawnInfo = aiType == CharacterAIType.Player ? player : enemy;
            var spawnPosition = spawnInfo.spawnFormation.GetSpawnPosition(spawnPositionType);

            battleCharacter.transform.SetParent(spawnInfo.spawnObject);
            battleCharacter.transform.position = spawnPosition;
            battleCharacter.transform.Rotate(spawnInfo.spawnFormation.transform.rotation.eulerAngles);
        }

        public async Task<BattleCharacterController> CreateCharacter(string characterName, CharacterAIType aiType)
        {
            var character = await AddressableManager.Instance.Controller.LoadAssetAsync<GameObject>("Prefab/Character/Character.prefab");
            var controller = Instantiate(character).AddComponent<BattleCharacterController>();
            var data = DataManager.Instance.GetData<CharacterData>(characterName);
            var instance = Instantiate(controller);
            instance.name = characterName;

            await SetModel(instance, data);
            await SetAnimation(instance, data);
            await SetPoolableObject(instance, data);
            
            SetStat(instance, data);
            SetSkill(instance, data);
            AddCharacterUI(instance, data, aiType);
            AddCharacterAI(instance, aiType);

            instance.PublishEvent();
            instance.InitStats();

            return instance;
        }



        private void SetStat(BattleCharacterController controller, CharacterData data)
        {
            var statSystem = new StatSystem();
            statSystem.SetStatData(data.stat);

            controller.StatSystem = statSystem;
        }

        private async UniTask SetModel(BattleCharacterController controller, CharacterData data)
        {
            var model = await ResourceLoader.InstantiateCharacterModel(data.addressValue.characterName, controller);
            controller.SetModel(model);
            var characterOffset =  controller.gameObject.AddComponent<CharacterOffset>();
            characterOffset.Initialized();

            controller.offset = characterOffset;
        }

        private async UniTask SetAnimation(BattleCharacterController controller, CharacterData data)
        {
            var animationController = ResourceManager.Instance.GetAsset<RuntimeAnimatorController>(data.addressValue.characterAnimationName);
            controller.AnimController = new CharacterBattleAnimationController(controller.GetComponentInChildren<Animator>(), controller.gameObject.AddComponent<AnimationEventHandler>());
            controller.AnimController.SetAnimationController(animationController);
        }
        
        private async UniTask SetPoolableObject(BattleCharacterController controller, CharacterData data)
        {
            controller.GetAttackHitEffect = await CreatePool<BattleEffect>(PoolableType.Effect, data.addressValue.attackHitEffectAddress);
            controller.GetSkillHitEffect = await CreatePool<BattleEffect>(PoolableType.Effect, data.addressValue.skillHitEffectAddress);
            controller.GetAttackProjectile = await CreatePool<BattleProjectile>(PoolableType.Projectile, data.addressValue.attackProjectileAddress);
            controller.GetSkillProjectile = await CreatePool<BattleProjectile>(PoolableType.Projectile, data.addressValue.skillProjectileAddress);
        }
        
        private void SetSkill(BattleCharacterController controllerInstance, CharacterData data)
        {
            var skillName = $"{typeof(CharacterSkill).FullName}{data.addressValue.characterName}, {typeof(CharacterSkill).Assembly}";

            if (Type.GetType(skillName) is not null)
            {
                controllerInstance.CharacterSkill = ReflectionController.CreateInstance<CharacterSkill>(skillName);
                controllerInstance.CharacterSkill.Controller = controllerInstance;
                controllerInstance.CharacterSkill.SetAnimationEvent(controllerInstance.AnimController.AnimEventHandler);
            }
        }

        private void AddCharacterAI(BattleCharacterController controller, CharacterAIType aiType)
        {
            var aiController = controller.gameObject.AddComponent<CharacterAIController>();
            aiController.aiType = aiType;

            aiController.Initialized();
            controller.characterAI = aiController;
        }

        private void AddCharacterUI(BattleCharacterController controller, CharacterData data,CharacterAIType aiType)
        {
            CharacterUIController uiController = null;

            switch (aiType)
            {
                case CharacterAIType.Player:
                    uiController = controller.gameObject.AddComponent<PlayerCharacterUIController>();
                    break;
                case CharacterAIType.Enemy:
                    uiController = controller.gameObject.AddComponent<CharacterUIController>();
                    break;
            }

            uiController.Initialized(data, controller.StatSystem);
            controller.characterUI = uiController;
        }
        


        private async UniTask<Func<T>> CreatePool<T>(PoolableType poolableType, string address) where T : IPoolable
        {
            if (string.IsNullOrEmpty(address) is true) return null;

            await ResourceLoader.CreatePool(poolableType, address);
            return () => ResourceLoader.GetPoolableObject<T>(poolableType, address);
        }
    }
}