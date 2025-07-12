using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Engine.Util.Extension;
using IdleProject.Battle.AI;
using IdleProject.Battle.Character;
using IdleProject.Battle.Character.Skill;
using IdleProject.Battle.Character.Skill.SkillAction;
using IdleProject.Battle.UI;
using IdleProject.Core;
using IdleProject.Core.GameData;
using IdleProject.Core.Resource;
using IdleProject.Data.DynamicData;
using IdleProject.Data.StaticData;
using UnityEngine;
using CharacterController = IdleProject.Battle.Character.CharacterController;

namespace IdleProject.Battle.Spawn
{
    [Serializable]
    public class SpawnInfo
    {
        public SpawnFormation spawnFormation;
        public Transform spawnObject;
    }

    public class SpawnController : MonoBehaviour
    {
        [SerializeField] private SpawnInfo playerSpawnInfo;
        [SerializeField] private SpawnInfo enemySpawnInfo;

        private BattleManager _battleManager;

        public void Initialize()
        {
            _battleManager = BattleManager.Instance<BattleManager>();

            playerSpawnInfo.spawnFormation.SetDefaultSpawn(CharacterAIType.Ally);
            enemySpawnInfo.spawnFormation.SetDefaultSpawn(CharacterAIType.Enemy);
        }

        public async UniTaskVoid SpawnCharacterBySpawnPosition(DynamicCharacterData data, SpawnPosition position)
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
            await SetCharacterSpawn(aiType, SpawnPositionType.FrontLeft, formationInfo.frontLeftPositionInfo);
            await SetCharacterSpawn(aiType, SpawnPositionType.FrontMiddle, formationInfo.frontMiddlePositionInfo);
            await SetCharacterSpawn(aiType, SpawnPositionType.FrontRight, formationInfo.frontRightPositionInfo);
            await SetCharacterSpawn(aiType, SpawnPositionType.RearLeft, formationInfo.rearLeftPositionInfo);
            await SetCharacterSpawn(aiType, SpawnPositionType.RearRight, formationInfo.rearRightPositionInfo);
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
            var spawnInfo = aiType == CharacterAIType.Ally ? playerSpawnInfo : enemySpawnInfo;
            var targetPosition = spawnInfo.spawnFormation.GetSpawnPosition(character);
            targetPosition.SetCharacter(null);
            _battleManager.GetCharacterList(targetPosition.SpawnAIType).Remove(character);
            Destroy(character.gameObject);
        }

        public SpawnPosition GetSpawnPosition(CharacterController character, CharacterAIType aiType)
        {
            var spawnInfo = aiType == CharacterAIType.Ally ? playerSpawnInfo : enemySpawnInfo;
            return spawnInfo.spawnFormation.GetSpawnPosition(character);
        }

        public List<(SpawnPositionType, string)> GetPlayerFormation()
        {
            var result = new List<(SpawnPositionType, string)>();

            EnumExtension.Foreach<SpawnPositionType>(type => { result.Add((type, GetPlayerPosition(type))); });

            return result;
        }

        private string GetPlayerPosition(SpawnPositionType positionType)
        {
            return playerSpawnInfo.spawnFormation.GetSpawnPosition(positionType).Character is not null
                ? playerSpawnInfo.spawnFormation.GetSpawnPosition(positionType).Character.name
                : string.Empty;
        }


        private async UniTask SetCharacterSpawn(CharacterAIType aiType, SpawnPositionType spawnPositionType,
            PositionInfo info)
        {
            if (string.IsNullOrEmpty(info.characterName) is false)
            {
                await SpawnCharacter(aiType, spawnPositionType, DynamicCharacterData.GetInstance(info));
            }
            else
            {
                var spawnInfo = aiType == CharacterAIType.Ally ? playerSpawnInfo : enemySpawnInfo;
                var spawnPosition = spawnInfo.spawnFormation.GetSpawnPosition(spawnPositionType);
                spawnPosition.SetCharacter(null);
            }
        }

        private async UniTask SpawnCharacter(CharacterAIType aiType, SpawnPositionType spawnPositionType,
            DynamicCharacterData data)
        {
            var controller = await CreateCharacter(data, aiType);

            _battleManager.AddCharacterController(controller);

            SetCharacterPosition(controller, aiType, spawnPositionType);
        }

        private void SetCharacterPosition(CharacterController character, CharacterAIType aiType,
            SpawnPositionType spawnPositionType)
        {
            var spawnInfo = aiType == CharacterAIType.Ally ? playerSpawnInfo : enemySpawnInfo;
            var spawnPosition = spawnInfo.spawnFormation.GetSpawnPosition(spawnPositionType);

            character.transform.SetParent(spawnInfo.spawnObject);
            spawnPosition.SetCharacter(character);
        }

        #region 캐릭터 생성 부문

        private async UniTask<CharacterController> CreateCharacter(DynamicCharacterData data, CharacterAIType aiType)
        {
            var controllerObj = ResourceManager.Instance.GetPrefab(ResourceManager.GamePrefab, "Character");
            var characterInstance = Instantiate(controllerObj).GetComponent<CharacterController>();
            characterInstance.name = data.StaticData.Index;

            await SetModel(characterInstance, data.StaticData);
            SetAnimation(characterInstance, data.StaticData);
            SetStat(characterInstance, data);

            characterInstance.CharacterAttack = GetSkill(characterInstance,
                DataManager.Instance.GetData<StaticSkillData>(data.StaticData.characterAttackName));

            if (string.IsNullOrEmpty(data.StaticData.characterSkillName) is false)
            {
                characterInstance.CharacterSkill = GetSkill(characterInstance,
                    DataManager.Instance.GetData<StaticSkillData>(data.StaticData.characterSkillName));
            }

            AddCharacterUI(characterInstance, data.StaticData, aiType);
            AddCharacterAI(characterInstance, aiType);

            characterInstance.Initialized();
            return characterInstance;
        }


        private void SetStat(CharacterController controller, DynamicCharacterData data)
        {
            var statSystem = new StatSystem();
            statSystem.SetStatData(data.StaticData.Index, data.GetStat());

            controller.StatSystem = statSystem;
        }

        private async UniTask SetModel(CharacterController controller, StaticCharacterData data)
        {
            var modelObject = ResourceManager.Instance.GetPrefab(ResourceManager.GamePrefab,
                $"Model_{data.Index}");
            await InstantiateAsync(modelObject, controller.transform).ToUniTask();

            var characterOffset = controller.gameObject.AddComponent<CharacterOffset>();
            characterOffset.Initialized();

            controller.offset = characterOffset;
        }

        private void SetAnimation(CharacterController controller, StaticCharacterData data)
        {
            var animationController =
                ResourceManager.Instance.GetAsset<RuntimeAnimatorController>(data.characterAnimationName);
            controller.AnimController = new CharacterBattleAnimationController(
                controller.GetComponentInChildren<Animator>(),
                controller.GetComponentInChildren<AnimationEventHandler>());
            controller.AnimController.SetAnimationController(animationController);
        }

        private CharacterSkill GetSkill(CharacterController controllerInstance, StaticSkillData data)
        {
            var executeSkillList = new List<ExecuteSkill>();
            foreach (var executeData in data.executeDataList)
            {
                var actionList = executeData.skillActionDataList.Select(action => SkillAction.GetSkillAction(action, controllerInstance)).ToList();
                var skillExecute = new ExecuteSkill(actionList);
                
                executeSkillList.Add(skillExecute);
            }
            
            return new CharacterSkill(executeSkillList);
        }

        private void AddCharacterAI(CharacterController controller, CharacterAIType aiType)
        {
            var aiController = controller.gameObject.AddComponent<CharacterAIController>();
            aiController.aiType = aiType;

            aiController.Initialized();
            controller.characterAI = aiController;
        }

        private void AddCharacterUI(CharacterController controller, StaticCharacterData data, CharacterAIType aiType)
        {
            CharacterUIController uiController = null;

            switch (aiType)
            {
                case CharacterAIType.Ally:
                    uiController = controller.gameObject.AddComponent<PlayerCharacterUIController>();
                    break;
                case CharacterAIType.Enemy:
                    uiController = controller.gameObject.AddComponent<CharacterUIController>();
                    break;
            }

            uiController.Initialized(data, controller.StatSystem);
            controller.characterUI = uiController;
        }

        #endregion
    }
}