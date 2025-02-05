using UnityEngine;

namespace IdleProject.Battle.AI
{
    public interface BattleAIAble
    {
        public void UpdateAI();
        public void PauseAI();
        public void PlayAI();
    }
}