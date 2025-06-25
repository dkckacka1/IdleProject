using System;
using IdleProject.Battle.Character;
using IdleProject.Core;
using IdleProject.Core.UI;
using TMPro;
using UnityEngine;

namespace IdleProject.Lobby.UI.CharacterPopup
{
    public class CharacterStatBar : UIBase
    {
        [SerializeField] private TextMeshProUGUI statNameText;
        [SerializeField] private TextMeshProUGUI statValueText;

        public void ShowStat(CharacterStatType statType, float value)
        {
            switch (statType)
            {
                case CharacterStatType.HealthPoint:
                    statNameText.text = "체력";
                    break;
                case CharacterStatType.ManaPoint:
                    statNameText.text = "마나";
                    break;
                case CharacterStatType.MovementSpeed:
                    statNameText.text = "이동속도";
                    break;
                case CharacterStatType.AttackDamage:
                    statNameText.text = "공격력";
                    break;
                case CharacterStatType.AttackRange:
                    statNameText.text = "공격 사거리";
                    break;
                case CharacterStatType.AttackCoolTime:
                    statNameText.text = "공격 쿨타임";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(statType), statType, null);
            }

            statValueText.text = value.ToString("0");
        }
    }
}