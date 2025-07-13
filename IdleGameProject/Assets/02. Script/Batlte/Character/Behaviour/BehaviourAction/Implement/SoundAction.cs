using IdleProject.Core.Sound;
using IdleProject.Data.BehaviourData;

namespace IdleProject.Battle.Character.Behaviour.SkillAction.Implement
{
    public class SoundAction : IBehaviourAction
    {
        private readonly string _soundName;
        private readonly float _soundVolume;

        public SoundAction(SoundBehaviourActionData actionData)
        {
            _soundName = actionData.soundName;
            _soundVolume = actionData.volume;
        }

        public void ActionExecute(bool isSkillBehaviour)
        {
            SoundManager.Instance.PlaySfx(_soundName, _soundVolume);
        }
    }
}