using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using Engine.Core;
using UnityEngine;
using IdleProject.Battle.AI;
using IdleProject.Battle.Character;
using IdleProject.Battle.Character.EventGroup;
using IdleProject.Battle.Character.Skill;
using IdleProject.Battle.Effect;
using IdleProject.Battle.Projectile;
using IdleProject.Battle.UI;
using IdleProject.Core;
using IdleProject.Core.GameData;
using IdleProject.Core.ObjectPool;
using IdleProject.Core.Resource;
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
        [FormerlySerializedAs("player")] [SerializeField] private SpawnInfo playerSpawnInfo;
        [FormerlySerializedAs("enemy")] [SerializeField] private SpawnInfo enemySpawnInfo;

        private BattleManager _battleManager;

        public void Initialize()
        {
            _battleManager = GameManager.GetCurrentSceneManager<BattleManager>();

            playerSpawnInfo.spawnFormation.SetDefaultSpawn(CharacterAIType.Player);
            enemySpawnInfo.spawnFormation.SetDefaultSpawn(CharacterAIType.Enemy);
        }

        public async UniTaskVoid SpawnCharacterBySpawnPosition(CharacterData data, SpawnPosition position)
        {
            var character = position.Character;
            if (character is not null)
            {
                RemoveCharacter(character, position.SpawnAIType);
            }

            await SpawnCharacter(position.SpawnAIType, position.positionType, data);
        }

        public async UniTask SpawnCharacterByFormation(CharacterAIType aiType, FormationInfo formationInfo)
        {
            await SetCharacterSpawn(aiType, SpawnPositionType.FrontLeft, formationInfo.frontLeftCharacterName);
            await SetCharacterSpawn(aiType, SpawnPositionType.FrontMiddle, formationInfo.frontMiddleCharacterName);
            await SetCharacterSpawn(aiType, SpawnPositionType.FrontRight, formationInfo.frontRightCharacterName);
            await SetCharacterSpawn(aiType, SpawnPositionType.RearLeft, formationInfo.rearLeftCharacterName);
            await SetCharacterSpawn(aiType, SpawnPositionType.RearRight, formationInfo.rearRightCharacterName);
        }

        public void SwapCharacter(SpawnPosition lhs, SpawnPosition rhs)
        {
            var temp = rhs.Character;
            rhs.SetCharacter(lhs.Character);
            lhs.SetCharacter(temp);
        }

        public void RemoveCharacter(CharacterController character, CharacterAIType aiType)
        {
            character.BattleEventGroup.UnPublishAll(_battleManager);
            character.characterUI.OnCharacterRemove();
            var spawnInfo = aiType == CharacterAIType.Player ? playerSpawnInfo : enemySpawnInfo;
            var targetPosition = spawnInfo.spawnFormation.GetSpawnPosition(character);
            targetPosition.SetCharacter(null);
            _battleManager.GetCharacterList(targetPosition.SpawnAIType).Remove(character);
            Destroy(character.gameObject);
        }

        public SpawnPosition GetSpawnPosition(CharacterController character, CharacterAIType aiType)
        {
            var spawnInfo = aiType == CharacterAIType.Player ? playerSpawnInfo : enemySpawnInfo;
            return spawnInfo.spawnFormation.GetSpawnPosition(character);
        }
        
        private async UniTask SetCharacterSpawn(CharacterAIType aiType, SpawnPositionType spawnPositionType,
            string heroName)
        {
            if (string.IsNullOrEmpty(heroName) is false)
            {
                await SpawnCharacter(aiType, spawnPositionType, DataManager.Instance.GetData<CharacterData>(heroName));
            }
            else
            {
                var spawnInfo = aiType == CharacterAIType.Player ? playerSpawnInfo : enemySpawnInfo;
                var spawnPosition = spawnInfo.spawnFormation.GetSpawnPosition(spawnPositionType);
                spawnPosition.SetCharacter(null);
            }
        }

        private async UniTask SpawnCharacter(CharacterAIType aiType, SpawnPositionType spawnPositionType,
            CharacterData data)
        {
            var controller = await CreateCharacter(data, aiType);

            _battleManager.AddCharacterController(controller);

            SetCharacterPosition(controller, aiType, spawnPositionType);
        }

        private void SetCharacterPosition(CharacterController character, CharacterAIType aiType,
            SpawnPositionType spawnPositionType)
        {
            var spawnInfo = aiType == CharacterAIType.Player ? playerSpawnInfo : enemySpawnInfo;
            var spawnPosition = spawnInfo.spawnFormation.GetSpawnPosition(spawnPositionType);

            character.transform.SetParent(spawnInfo.spawnObject);
            spawnPosition.SetCharacter(character);
        }

        #region 캐릭터 생성 부문

        private async Task<CharacterController> CreateCharacter(CharacterData data, CharacterAIType aiType)
        {
            var controller = await AddressableManager.Instance.Controller.LoadAssetAsync<CharacterController>("Prefab/Character/Character.prefab");
            var characterInstance = Instantiate(controller);
            characterInstance.name = data.addressValue.characterName;

            SetModel(characterInstance, data);
            await SetPoolableObject(characterInstance, data);

            SetAnimation(characterInstance, data);
            SetStat(characterInstance, data);
            SetSkill(characterInstance, data);
            AddCharacterUI(characterInstance, data, aiType);
            AddCharacterAI(characterInstance, aiType);
            
            characterInstance.Initialized();
            return characterInstance;
        }


        private void SetStat(CharacterController controller, CharacterData data)
        {
            var statSystem = new StatSystem();
            statSystem.SetStatData(data.addressValue.characterName, data.stat);

            controller.StatSystem = statSystem;
        }

        private void SetModel(CharacterController controller, CharacterData data)
        {
            var modelObject = ResourceManager.Instance.GetPrefab(ResourceManager.CharacterModelLabelName,
                $"Model_{data.addressValue.characterName}");
            Instantiate(modelObject, controller.transform);
            var characterOffset = controller.gameObject.AddComponent<CharacterOffset>();
            characterOffset.Initialized();

            controller.offset = characterOffset;
        }

        private void SetAnimation(CharacterController controller, CharacterData data)
        {
            var animationController =
                ResourceManager.Instance.GetAsset<RuntimeAnimatorController>(data.addressValue.characterAnimationName);
            controller.AnimController = new CharacterBattleAnimationController(
                controller.GetComponentInChildren<Animator>(),
                controller.GetComponentInChildren<AnimationEventHandler>());
            controller.AnimController.SetAnimationController(animationController);
        }

        private async UniTask SetPoolableObject(CharacterController controller, CharacterData data)
        {
            controller.GetAttackHitEffect =
                await CreatePool<BattleEffect>(PoolableType.BattleEffect, data.addressValue.attackHitEffectAddress);
            controller.GetSkillHitEffect =
                await CreatePool<BattleEffect>(PoolableType.BattleEffect, data.addressValue.skillHitEffectAddress);
            controller.GetAttackProjectile = await CreatePool<BattleProjectile>(PoolableType.Projectile,
                data.addressValue.attackProjectileAddress);
            controller.GetSkillProjectile =
                await CreatePool<BattleProjectile>(PoolableType.Projectile, data.addressValue.skillProjectileAddress);
        }

        private void SetSkill(CharacterController controllerInstance, CharacterData data)
        {
            var skillName =
                $"{typeof(CharacterSkill).FullName}{data.addressValue.characterName}, {typeof(CharacterSkill).Assembly}";

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

        private void AddCharacterUI(CharacterController controller, CharacterData data, CharacterAIType aiType)
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

            await ResourceLoader.CreatePool(poolableType, address, GetBattleTransformParent(poolableType));
            return () => ResourceLoader.GetPoolableObject<T>(poolableType, address);
        }

        private Transform GetBattleTransformParent(PoolableType poolableType)
        {
            Transform parent = null;

            switch (poolableType)
            {
                case PoolableType.UI:
                    break;
                case PoolableType.BattleEffect:
                    parent = _battleManager.effectParent;
                    break;
                case PoolableType.Projectile:
                    parent = _battleManager.projectileParent;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(poolableType), poolableType, null);
            }

            return parent;
        }

        #endregion
    }
}