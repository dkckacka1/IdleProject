using System;
using System.Threading.Tasks;
using Engine.Core.Addressable;
using IdleProject.Battle.AI;
using IdleProject.Battle.UI;
using UnityEngine;
using Zenject;

namespace IdleProject.Battle.Character
{
    public class CharacterFactory :
        IFactory<string, CharacterData, CharacterAIType, Task<CharacterController>>,
        IFactory<CharacterController, CharacterData, CharacterAIType, CharacterUIController>,
        IFactory<CharacterController, CharacterAIType, CharacterAIController>
    {
        private readonly DiContainer _container;

        public CharacterFactory(DiContainer container)
        {
            _container = container;
        }

        public async Task<CharacterController> Create(string address, CharacterData data, CharacterAIType aiType)
        {
            var prefab = await AddressableManager.Instance.LoadAssetAsync<GameObject>(address);
            if (!prefab)
            {
                return null;
            }
            
            var instance = GameObject.Instantiate(prefab);

            var animator = instance.GetComponentInChildren<Animator>();
            var eventHandler = instance.GetComponentInChildren<AnimationEventHandler>();
            var animController = new AnimationController(animator, eventHandler);

            var character = _container.InjectGameObjectForComponent<CharacterController>(
                instance,
                new object[] { data, aiType, animController }
            );

            return character;
        }

        public CharacterUIController Create(CharacterController targetCharacter, CharacterData data,
            CharacterAIType aiType)
        {
            CharacterUIController ui = null;

            var statSystem = targetCharacter.StatSystem;
            var offset = targetCharacter.GetComponent<CharacterOffset>();

            ui = aiType switch
            {
                CharacterAIType.Player => _container.InstantiateComponent<PlayerCharacterUIController>(
                    gameObject: targetCharacter.gameObject, extraArgs: new object[] { data, statSystem, offset }),
                CharacterAIType.Enemy => _container.InstantiateComponent<CharacterUIController>(
                    gameObject: targetCharacter.gameObject, extraArgs: new object[] { data, statSystem, offset }),
                _ => throw new ArgumentOutOfRangeException(nameof(aiType), aiType, null)
            };

            return ui;
        }

        public CharacterAIController Create(CharacterController controller, CharacterAIType aiType)
        {
            var ai = _container.InstantiateComponent<CharacterAIController>(
                gameObject: controller.gameObject,
                extraArgs: new object[] { controller, aiType });

            return ai;
        }
    }
}