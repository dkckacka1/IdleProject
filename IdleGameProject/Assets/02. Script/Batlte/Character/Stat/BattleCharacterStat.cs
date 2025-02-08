using System;
using UnityEngine;

namespace IdleProject.Battle.Character
{
    public class BattleCharacterStat
    {
        private float statValue = 0f;

        public float Value
        {
            get
            {
                return statValue;
            }
            set
            {
                statValue = value;
                OnValueChanged?.Invoke(statValue);
            }
        }

        public float defaultStatValue { get; private set; } = 0f;


        public Action<float> OnValueChanged = null;

        public void SetStat(float stat)
        {
            defaultStatValue = stat;
            statValue = stat;
        }
    }
}