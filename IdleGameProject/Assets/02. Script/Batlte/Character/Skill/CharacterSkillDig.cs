using Cysharp.Threading.Tasks;
using IdleProject.Core;

namespace IdleProject.Battle.Character.Skill
{
    public class CharacterSkillDig : CharacterSkill
    {
        private const float BUFF_DURATION = 10f;
        private const float VALUE = 3f;

        protected override void SkillAction(int skillNumber)
        {
            var battleManager = GameManager.GetCurrentSceneManager<BattleManager>();
            var allyList = battleManager.GetCharacterList(Controller.characterAI.GetAllyType);

            foreach (var allyCharacter in allyList)
            {
                var skillEffect = Controller.GetSkillHitEffect();
                skillEffect.transform.position = allyCharacter.HitEffectOffset;
                AddBuff(allyCharacter).Forget();
            }
        }

        private async UniTaskVoid AddBuff(CharacterController targetController)
        {
            targetController.StatSystem.AddStatChanger(CharacterStatType.DefensePoint, nameof(GetType), VALUE);
            await GameManager.GetCurrentSceneManager<BattleManager>().GetBattleTimer(BUFF_DURATION);
            targetController.StatSystem.AddStatChanger(CharacterStatType.DefensePoint, nameof(GetType), VALUE);
        }
    }
}