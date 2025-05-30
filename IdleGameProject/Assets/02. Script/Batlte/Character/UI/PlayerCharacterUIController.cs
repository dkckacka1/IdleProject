
using Cysharp.Threading.Tasks;
using IdleProject.Battle.Character;

namespace IdleProject.Battle.UI
{
    public class PlayerCharacterUIController : CharacterUIController
    {
        private PlayerCharacterBanner _banner;

        public override void Initialized(CharacterData data, StatSystem stat)
        {
            base.Initialized(data, stat);

            SetPlayerCharacterBanner(data, stat);
        }

        public override void OnBattleUIEvent()
        {
            base.OnBattleUIEvent();

            _banner.CharacterHealthBar.PlayDamageSlider();
        }

        private void SetPlayerCharacterBanner(CharacterData data, StatSystem stat)
        {
            _banner = GetBattleUI.GetPlayerCharacterBanner();
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

