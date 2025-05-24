
using Cysharp.Threading.Tasks;
using IdleProject.Battle.Character;

namespace IdleProject.Battle.UI
{
    public class PlayerCharacterUIController : CharacterUIController
    {
        private PlayerCharacterBanner banner;

        public override void Initialized(CharacterData data)
        {
            base.Initialized(data);
        }

        private async UniTaskVoid SetPlayerCharacterBanner(StatSystem stat)
        {
            banner = null;
            // TODO
        }
    }
}

