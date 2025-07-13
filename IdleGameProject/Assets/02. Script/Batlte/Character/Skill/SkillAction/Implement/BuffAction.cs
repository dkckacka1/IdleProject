using Cysharp.Threading.Tasks;
using IdleProject.Core;
using IdleProject.Data.SkillData;

namespace IdleProject.Battle.Character.Skill.SkillAction.Implement
{
    public class BuffAction : BehaviourAction
    {
        private readonly CharacterStatType _characterStatType;
        private readonly float _buffDuration;
        private readonly float _buffValue;

        private readonly EffectAction _buffLoopEffect;

        private readonly string _buffName; 
        
        public BuffAction(BuffSkillActionData skillActionData, CharacterController controller) : base(skillActionData, controller)
        {
            _characterStatType = skillActionData.buffStatType;
            _buffValue = skillActionData.value;
            _buffDuration = skillActionData.duration;

            _buffName = controller.name;
            
            if (skillActionData.buffEffect is not null)
            {
                _buffLoopEffect = new EffectAction(skillActionData.buffEffect, controller);
            }
        }

        public override void ActionExecute()
        {
            foreach (var target in GetTargetList.Invoke())
            {
                AddBuff(target).Forget();
                
                if (_buffLoopEffect is not null)
                {
                    _buffLoopEffect.SetTarget(target);
                    _buffLoopEffect.ActionExecute();
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