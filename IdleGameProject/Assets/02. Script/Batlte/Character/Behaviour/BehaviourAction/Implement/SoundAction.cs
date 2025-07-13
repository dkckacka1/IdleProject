using IdleProject.Core.Sound;

namespace IdleProject.Battle.Character.Behaviour.SkillAction.Implement
{
    public class SoundAction : IBehaviourAction
    {
        private readonly string _soundName;
        private readonly float _soundVolume;

        public SoundAction(string soundName, float soundVolume)
        {
            _soundName = soundName;
            _soundVolume = soundVolume;
        }

        public void ActionExecute(bool isSkillBehaviour)
        {
            SoundManager.Instance.PlaySfx(_soundName, _soundVolume);
        }
    }
}