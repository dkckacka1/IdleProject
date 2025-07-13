using Cysharp.Threading.Tasks;
using IdleProject.Core;
using IdleProject.Data.BehaviourData;

namespace IdleProject.Battle.Character.Behaviour.SkillAction.Implement
{
    public class BuffAction : BehaviourAction
    {
        private readonly CharacterStatType _characterStatType;
        private readonly float _buffDuration;
        private readonly float _buffValue;

        private readonly EffectAction _buffLoopEffect;

        private readonly string _buffName; 
        
        public BuffAction(BuffBehaviourActionData behaviourActionData, CharacterController controller) : base(behaviourActionData, controller)
        {
            _characterStatType = behaviourActionData.buffStatType;
            _buffValue = behaviourActionData.value;
            _buffDuration = behaviourActionData.duration;

            _buffName = controller.name;
            
            if (behaviourActionData.buffEffect is not null)
            {
                _buffLoopEffect = new EffectAction(behaviourActionData.buffEffect, controller);
            }
        }

        public override void ActionExecute(bool isSkillBehaviour)
        {
            foreach (var target in GetTargetList.Invoke())
            {
                AddBuff(target).Forget();
                
                if (_buffLoopEffect is not null)
                {
                    _buffLoopEffect.SetTarget(target);
                    _buffLoopEffect.ActionExecute(isSkillBehaviour);
                }
            }
        }

        private async UniTaskVoid AddBuff(CharacterController target)
        {
            target.StatSystem.AddStatChanger(_characterStatType, _buffName, _buffValue);
            await GameManager.GetCurrentSceneManager<BattleManager>().GetBattleTimer(_buffDuration);
            target.StatSystem.RemoveStatChanger(_characterStatType,_buffName);
        }
    }
}