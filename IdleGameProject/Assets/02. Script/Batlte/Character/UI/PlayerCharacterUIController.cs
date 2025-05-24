
using Cysharp.Threading.Tasks;
using IdleProject.Battle.Character;

namespace IdleProject.Battle.UI
{
    public class PlayerCharacterUIController : CharacterUIController
    {
        private PlayerCharacterBanner banner;

        public override void Initialized(CharacterData data, StatSystem stat)
        {
            base.Initialized(data, stat);

            SetPlayerCharacterBanner(data, stat);
        }

        public override void OnBattleUIEvent()
        {
            base.OnBattleUIEvent();

            banner.CharacterHealthBar.PlayDamageSlider();
        }

        private void SetPlayerCharacterBanner(CharacterData data, StatSystem stat)
        {
            banner = GetBattleUI.GetPlayerCharacterBanner();
            banner.Initialized(data, stat);
        }
    }
}

