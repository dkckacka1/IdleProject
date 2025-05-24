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

        public HealthBar CharacterHealthBar => characterHealthBar;

        public void Initialized(CharacterData data, StatSystem characterStat)
        {
            characterIconImage.sprite = null;
            characterNameText.text = data.addressValue.characterName;
            characterHealthBar.Initialized(characterStat.GetStatValue(CharacterStatType.HealthPoint, true));
            characterStat.PublishValueChangedEvent(CharacterStatType.HealthPoint, characterHealthBar.ChangeCurrentHealthPoint);
        }
    }
}
