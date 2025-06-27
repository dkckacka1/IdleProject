using System;
using IdleProject.Core;
using IdleProject.Core.UI;
using IdleProject.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IdleProject.Lobby.UI.CharacterPopup
{
    public class CharacterStatBar : UIBase
    {
        [SerializeField] private TextMeshProUGUI statNameText;
        [SerializeField] private TextMeshProUGUI statValueText;
        [SerializeField] private Image statTypeIconImage;

        public void ShowStat(CharacterStatType statType, float value)
        {
            statNameText.text = statType switch
            {
                CharacterStatType.HealthPoint => "체력",
                CharacterStatType.ManaPoint => "마나",
                CharacterStatType.MovementSpeed => "이동속도",
                CharacterStatType.AttackDamage => "공격력",
                CharacterStatType.AttackRange => "공격 사거리",
                CharacterStatType.AttackCoolTime => "공격 쿨타임",
                CharacterStatType.DefensePoint => "방어력",
                CharacterStatType.CriticalPercent => "치명타 확률",
                CharacterStatType.CriticalResistance => "치명타 저항력",
                _ => throw new ArgumentOutOfRangeException(nameof(statType), statType, null)
            };

            statTypeIconImage.sprite = GetSpriteExtension.GetCharacterStatTypeIconSprite(statType);
            statValueText.text = value.ToString("0");
        }
    }
}