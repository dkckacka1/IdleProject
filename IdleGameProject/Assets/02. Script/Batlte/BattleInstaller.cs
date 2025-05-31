using System.Threading.Tasks;
using IdleProject.Battle.AI;
using IdleProject.Battle.Character;
using IdleProject.Battle.Spawn;
using IdleProject.Battle.UI;
using IdleProject.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

using CharacterController = IdleProject.Battle.Character.CharacterController;

namespace IdleProject.Battle
{
    public class BattleInstaller : MonoInstaller
    {
        [BoxGroup("Spawn"), SerializeField] private SpawnOffset playerSpawnOffset;
        [BoxGroup("Spawn"), SerializeField] private SpawnOffset enemySpawnOffset;

        [BoxGroup("BattleOffset"), SerializeField]
        private Transform effectTransformOffset;
        
        [BoxGroup("BattleOffset"), SerializeField]
        private Transform projectileTransformOffset;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<BattleManager>().FromComponentsInHierarchy().AsSingle();
            Container.Bind<SpawnController>().FromComponentsInHierarchy().AsSingle();
            Container.Bind<BattleResourceLoader>().AsSingle();

            Container.Bind<SpawnOffset>().WithId("PlayerSpawnOffset").FromInstance(playerSpawnOffset).AsCached();
            Container.Bind<SpawnOffset>().WithId("EnemySpawnOffset").FromInstance(enemySpawnOffset).AsCached();

            Container.Bind<Transform>().WithId("EffectTransformOffset").FromInstance(effectTransformOffset).AsCached();
            Container.Bind<Transform>().WithId("ProjectileTransformOffset").FromInstance(projectileTransformOffset).AsCached();
            
            Container.Bind<IFactory<string, CharacterData, CharacterAIType, Task<CharacterController>>>().To<CharacterFactory>().AsCached();
            Container.Bind<IFactory<CharacterController, CharacterData, CharacterAIType, CharacterUIController>>().To<CharacterFactory>().AsCached();
            Container.Bind<IFactory<CharacterController, CharacterAIType, CharacterAIController>>().To<CharacterFactory>().AsCached();
        }
    }
}