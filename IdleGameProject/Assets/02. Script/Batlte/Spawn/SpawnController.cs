using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Engine.Core;
using UnityEngine;
using IdleProject.Battle.AI;
using IdleProject.Battle.Character;
using IdleProject.Battle.Character.Skill;
using IdleProject.Battle.Effect;
using IdleProject.Battle.Projectile;
using IdleProject.Battle.UI;
using IdleProject.Core;
using IdleProject.Core.GameData;
using IdleProject.Core.ObjectPool;
using IdleProject.Data;
using UnityEngine.Serialization;
using CharacterController = IdleProject.Battle.Character.CharacterController;

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

        public async UniTask SpawnCharacterAtInfo(CharacterAIType aiType, SpawnInfoData spawnInfo)
        {
            if(string.IsNullOrEmpty(spawnInfo.frontLeftCharacterName) is false)
                await SpawnCharacter(aiType, SpawnPositionType.FrontLeft, spawnInfo.frontLeftCharacterName);
            
            if(string.IsNullOrEmpty(spawnInfo.frontMiddleCharacterName) is false)
                await SpawnCharacter(aiType, SpawnPositionType.FrontMiddle, spawnInfo.frontMiddleCharacterName);
            
            if(string.IsNullOrEmpty(spawnInfo.frontRightCharacterName) is false)
                await SpawnCharacter(aiType, SpawnPositionType.FrontLeft, spawnInfo.frontRightCharacterName);
            
            if(string.IsNullOrEmpty(spawnInfo.rearLeftCharacterName) is false)
                await SpawnCharacter(aiType, SpawnPositionType.RearLeft, spawnInfo.rearLeftCharacterName);
            
            if(string.IsNullOrEmpty(spawnInfo.rearRightCharacterName) is false)
                await SpawnCharacter(aiType, SpawnPositionType.RearRight, spawnInfo.rearRightCharacterName);
        }
        
        public async UniTask SpawnCharacter(CharacterAIType aiType, SpawnPositionType spawnPositionType,
            string characterName)
        {
            var controller = await CreateCharacter(characterName, aiType);

            GameManager.GetCurrentSceneManager<BattleManager>().AddCharacterController(controller);

            SetCharacterPosition(controller, aiType, spawnPositionType);
        }

        private void SetCharacterPosition(CharacterController character, CharacterAIType aiType,
            SpawnPositionType spawnPositionType)
        {
            SpawnInfo spawnInfo = aiType == CharacterAIType.Player ? player : enemy;
            var spawnPosition = spawnInfo.spawnFormation.GetSpawnPosition(spawnPositionType);

            character.transform.SetParent(spawnInfo.spawnObject);
            character.transform.position = spawnPosition;
            character.transform.Rotate(spawnInfo.spawnFormation.transform.rotation.eulerAngles);
        }

        public async Task<CharacterController> CreateCharacter(string characterName, CharacterAIType aiType)
        {
            var controller = await AddressableManager.Instance.Controller.LoadAssetAsync<CharacterController>("Prefab/Character/Character.prefab");
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



        private void SetStat(CharacterController controller, CharacterData data)
        {
            var statSystem = new StatSystem();
            statSystem.SetStatData(data.stat);

            controller.StatSystem = statSystem;
        }

        private async UniTask SetModel(CharacterController controller, CharacterData data)
        {
            await ResourceLoader.InstantiateCharacterModel(data.addressValue.characterName, controller);
            var characterOffset =  controller.gameObject.AddComponent<CharacterOffset>();
            characterOffset.Initialized();

            controller.offset = characterOffset;
        }

        private async UniTask SetAnimation(CharacterController controller, CharacterData data)
        {
            var animationController = await ResourceLoader.GetAnimation(data.addressValue.characterAnimationName);
            controller.AnimController = new AnimationController(controller.GetComponentInChildren<Animator>(), controller.GetComponentInChildren<AnimationEventHandler>());
            controller.AnimController.SetAnimationController(animationController);
        }
        
        private async UniTask SetPoolableObject(CharacterController controller, CharacterData data)
        {
            controller.GetAttackHitEffect = await CreatePool<BattleEffect>(PoolableType.Effect, data.addressValue.attackHitEffectAddress);
            controller.GetSkillHitEffect = await CreatePool<BattleEffect>(PoolableType.Effect, data.addressValue.skillHitEffectAddress);
            controller.GetAttackProjectile = await CreatePool<BattleProjectile>(PoolableType.Projectile, data.addressValue.attackProjectileAddress);
            controller.GetSkillProjectile = await CreatePool<BattleProjectile>(PoolableType.Projectile, data.addressValue.skillProjectileAddress);
        }
        
        private void SetSkill(CharacterController controllerInstance, CharacterData data)
        {
            var skillName = $"{typeof(CharacterSkill).FullName}{data.addressValue.characterName}, {typeof(CharacterSkill).Assembly}";

            if (Type.GetType(skillName) is not null)
            {
                controllerInstance.CharacterSkill = ReflectionController.CreateInstance<CharacterSkill>(skillName);
                controllerInstance.CharacterSkill.Controller = controllerInstance;
                controllerInstance.CharacterSkill.SetAnimationEvent(controllerInstance.AnimController.AnimEventHandler);
            }
        }

        private void AddCharacterAI(CharacterController controller, CharacterAIType aiType)
        {
            var aiController = controller.gameObject.AddComponent<CharacterAIController>();
            aiController.aiType = aiType;

            aiController.Initialized();
            controller.characterAI = aiController;
        }

        private void AddCharacterUI(CharacterController controller, CharacterData data,CharacterAIType aiType)
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