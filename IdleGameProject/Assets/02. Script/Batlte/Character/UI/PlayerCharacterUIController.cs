using IdleProject.Battle.Character;
using Zenject;

namespace IdleProject.Battle.UI
{
    public class PlayerCharacterUIController : CharacterUIController
    {
        private PlayerCharacterBanner _banner;

        public override void Initialized(CharacterData data, StatSystem stat, CharacterOffset offset)
        {
            base.Initialized(data, stat, offset);
            
            SetPlayerCharacterBanner(data, stat);
            
        }

        public override void OnBattleUIEvent()
        {
            base.OnBattleUIEvent();

            _banner.CharacterHealthBar.PlayDamageSlider();
        }

        private void SetPlayerCharacterBanner(CharacterData data, StatSystem stat)
        {
            _banner = BattleUIController.GetPlayerCharacterBanner();
            _banner.Initialized(data, stat).Forget();
        }

        public void StartSkill()
        {
            _banner.SetSkillBanner(true);
        }

        public void EndSkill()
        {
            _banner.SetSkillBanner(false);
        }
    }
}

