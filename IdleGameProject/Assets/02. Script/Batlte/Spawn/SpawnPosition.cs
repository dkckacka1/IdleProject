using System;
using Engine.Core.EventBus;
using IdleProject.Core;
using UnityEngine;
using CharacterController = IdleProject.Battle.Character.CharacterController;

namespace IdleProject.Battle.Spawn
{
    public class SpawnPosition : MonoBehaviour, IEnumEvent<BattleStateType>
    {
        public SpawnPositionType positionType;
        
        [SerializeField] private ParticleSystem spawnParticle;

        public CharacterController Character { get; private set; }
        public CharacterAIType SpawnAIType { get; private set; }

        private void Start()
        {
            GameManager.GetCurrentSceneManager<BattleManager>().BattleStateEventBus.PublishEvent(this);
        }

        public void Initialize(CharacterAIType aiType)
        {
            SpawnAIType = aiType;
            spawnParticle.gameObject.SetActive(aiType == CharacterAIType.Player);
        }

        public void SetCharacter(CharacterController character)
        {
            Character = character;
            if (Character is not null)
            {
                Character.transform.position = transform.position;
                Character.transform.Rotate(transform.rotation.eulerAngles);
            }
        }

        public void OnEnumChange(BattleStateType type)
        {
            switch (type)
            {
                case BattleStateType.Ready:
                    spawnParticle.gameObject.SetActive(SpawnAIType == CharacterAIType.Player);
                    break;
                case BattleStateType.Battle:
                    spawnParticle.gameObject.SetActive(false);
                    break;
                case BattleStateType.Skill:
                case BattleStateType.Win:
                case BattleStateType.Defeat:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}

