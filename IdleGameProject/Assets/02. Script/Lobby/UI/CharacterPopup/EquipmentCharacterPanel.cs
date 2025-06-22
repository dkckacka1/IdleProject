using Cysharp.Threading.Tasks;
using IdleProject.Battle.Character;
using IdleProject.Core;
using IdleProject.Core.GameData;
using IdleProject.Core.Resource;
using IdleProject.Core.UI;
using IdleProject.Core.UI.Loading;
using IdleProject.Data;
using IdleProject.Lobby.Character;
using UnityEngine;

namespace IdleProject.Lobby.UI.EquipmentPanel
{
    public class EquipmentCharacterPanel : UIPanel
    {
        [SerializeField] private LobbyCharacter equipCharacter;
        [SerializeField] private LoadingRotateUI characterLoadingRotate;
        
        public override void Initialized()
        {
            SetCharacter(DataManager.Instance.DataController.userData.UserHeroList[0]);
        }

        private void SetCharacter(string heroName)
        {
            var data = DataManager.Instance.GetData<CharacterData>(heroName);
            
            characterLoadingRotate.StartLoading(LoadCharacter(data)).Forget();

            UIManager.Instance.GetUI<CharacterStatBar>("CharacterStatBar_AttackDamage")
                .ShowStat(CharacterStatType.AttackDamage, data.stat.attackDamage);
            UIManager.Instance.GetUI<CharacterStatBar>("CharacterStatBar_Health")
                .ShowStat(CharacterStatType.HealthPoint, data.stat.healthPoint);
            UIManager.Instance.GetUI<CharacterStatBar>("CharacterStatBar_MovementSpeed")
                .ShowStat(CharacterStatType.MovementSpeed, data.stat.movementSpeed);
        }

        private async UniTask LoadCharacter(CharacterData characterData)
        {
            var modelObject = ResourceManager.Instance.GetPrefab(ResourceManager.CharacterModelLabelName,
                $"Model_{characterData.addressValue.characterName}");
            equipCharacter.SetModel(Instantiate(modelObject, equipCharacter.transform));

            var animatorController = ResourceManager.Instance.GetAsset<RuntimeAnimatorController>(characterData.addressValue.characterAnimationName);
            equipCharacter.SetAnimation(animatorController);
        }
    }
}