using Cysharp.Threading.Tasks;
using IdleProject.Battle.Character;
using IdleProject.Core;
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

        private static string ICON_TYPE = "Banner";

        public HealthBar CharacterHealthBar => characterHealthBar;
        public ManaBar CharacaterManabar => characterManaBar;


        public async UniTaskVoid Initialized(CharacterData data, StatSystem characterStat)
        {
            characterIconImage.sprite = await ResourcesLoader.GetIcon(IconType.Character, data.addressValue.characterName,ICON_TYPE);
            characterNameText.text = data.addressValue.characterName;
            characterHealthBar.Initialized(characterStat.GetStatValue(CharacterStatType.HealthPoint, true));
            characterManaBar.Initialized(characterStat.GetStatValue(CharacterStatType.ManaPoint, true));
            characterStat.PublishValueChangedEvent(CharacterStatType.HealthPoint, characterHealthBar.OnChangeHealthPoint);
            characterStat.PublishValueChangedEvent(CharacterStatType.ManaPoint, characterManaBar.OnChangeManaPoint);
        }
    }
}
