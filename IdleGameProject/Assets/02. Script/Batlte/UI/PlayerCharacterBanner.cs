using Cysharp.Threading.Tasks;
using IdleProject.Battle.Character;
using IdleProject.Core;
using IdleProject.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IdleProject.Battle.UI
{
    public class PlayerCharacterBanner : MonoBehaviour
    {
        [SerializeField] private Image characterIconImage;
        [SerializeField] private TextMeshProUGUI characterNameText;
        [SerializeField] private HealthBar characterHealthBar;
        [SerializeField] private ManaBar characterManaBar;
        
        [SerializeField] private GameObject characterSkillBannerObj;
        [SerializeField] private Image characterSkillBannerImage;

        private const string ICON_TYPE = "Banner";
        private const string SKILL_ICON_TYPE = "SkillBanner";

        public HealthBar CharacterHealthBar => characterHealthBar;

        public async UniTaskVoid Initialized(CharacterData data, StatSystem characterStat)
        {
            characterIconImage.sprite = await ResourceLoader.GetIcon(IconType.Character, data.addressValue.characterName,ICON_TYPE);
            characterSkillBannerImage.sprite = await ResourceLoader.GetIcon(IconType.Character,
                data.addressValue.characterName, SKILL_ICON_TYPE);
            characterNameText.text = data.addressValue.characterName;
            characterHealthBar.Initialized(characterStat.GetStatValue(CharacterStatType.HealthPoint, true));
            characterManaBar.Initialized(characterStat.GetStatValue(CharacterStatType.ManaPoint, true));
            characterStat.PublishValueChangedEvent(CharacterStatType.HealthPoint, characterHealthBar.OnChangeHealthPoint);
            characterStat.PublishValueChangedEvent(CharacterStatType.ManaPoint, characterManaBar.OnChangeManaPoint);
            
            characterSkillBannerObj.SetActive(false);
        }

        public void SetSkillBanner(bool active)
        {
            characterSkillBannerObj.SetActive(active);
        }
    }
}
