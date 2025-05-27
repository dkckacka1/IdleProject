using IdleProject.Battle.Character;
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

        public HealthBar CharacterHealthBar => characterHealthBar;
        public ManaBar CharacaterManabar => characterManaBar;


        public void Initialized(CharacterData data, StatSystem characterStat)
        {
            characterIconImage.sprite = null;
            characterNameText.text = data.addressValue.characterName;
            characterHealthBar.Initialized(characterStat.GetStatValue(CharacterStatType.HealthPoint, true));
            characterManaBar.Initialized(characterStat.GetStatValue(CharacterStatType.ManaPoint, true));
            characterStat.PublishValueChangedEvent(CharacterStatType.HealthPoint, characterHealthBar.OnChangeHealthPoint);
            characterStat.PublishValueChangedEvent(CharacterStatType.ManaPoint, characterManaBar.OnChangeManaPoint);
        }
    }
}
