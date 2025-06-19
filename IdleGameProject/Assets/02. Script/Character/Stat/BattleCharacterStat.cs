using System;
using UnityEngine;

namespace IdleProject.Character.Stat
{
    public class BattleCharacterStat
    {
        private float _statValue = 0f;

        public float Value
        {
            get => _statValue;
            set
            {
                _statValue = value;
                OnValueChanged?.Invoke(_statValue);
            }
        }

        public float DefaultStatValue { get; private set; } = 0f;


        public Action<float> OnValueChanged = null;

        public void SetStat(float stat)
        {
            DefaultStatValue = stat;
            _statValue = stat;
            OnValueChanged?.Invoke(stat);
        }
    }
}