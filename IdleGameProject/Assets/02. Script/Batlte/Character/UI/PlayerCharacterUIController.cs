
using System;
using Cysharp.Threading.Tasks;
using IdleProject.Battle.Character;
using IdleProject.Data;
using IdleProject.Data.StaticData;

namespace IdleProject.Battle.UI
{
    public class PlayerCharacterUIController : CharacterUIController
    {
        private PlayerCharacterBanner _banner;

        public override void Initialized(StaticCharacterData data, StatSystem stat)
        {
            base.Initialized(data, stat);

            _banner = GetBattleUI.GetPlayerCharacterBanner();
            _banner.Initialized(data, stat);
        }

        protected override void OnBattleUIEvent()
        {
            base.OnBattleUIEvent();

            _banner.CharacterHealthBar.PlayDamageSlider();
        }

        public override void OnCharacterRemove()
        {
            base.OnCharacterRemove();
            _banner.gameObject.SetActive(false);
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

