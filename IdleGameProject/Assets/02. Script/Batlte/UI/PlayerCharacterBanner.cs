using Cysharp.Threading.Tasks;
using IdleProject.Battle.Character;
using IdleProject.Core;
using IdleProject.Core.Resource;
using IdleProject.Data;
using IdleProject.Data.StaticData;
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

        public void Initialized(CharacterData data, StatSystem characterStat)
        {
            characterIconImage.sprite = ResourceManager.Instance.GetAsset<Sprite>(data.GetCharacterBannerIconName);
            characterSkillBannerImage.sprite = ResourceManager.Instance.GetAsset<Sprite>(data.GetCharacterSkillBannerIconName);
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
