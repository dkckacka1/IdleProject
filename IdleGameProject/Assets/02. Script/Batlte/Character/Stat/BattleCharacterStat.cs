using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IdleProject.Battle.Character
{
    public class BattleCharacterStat
    {
        private float _statValue = 0f;

        private readonly Dictionary<string, float> _statValueChanger = new();
        
        public float Value
        {
            get => _statValue + _statValueChanger.Values.Sum();
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

        public void AddStatChanger(string key, float value)
        {
            if (!_statValueChanger.TryAdd(key, value))
            {
                Debug.Log($"{key} is already added");
            }
        }

        public void RemoveStatChanger(string key)
        {
            _statValueChanger.Remove(key);
        }
    }
}