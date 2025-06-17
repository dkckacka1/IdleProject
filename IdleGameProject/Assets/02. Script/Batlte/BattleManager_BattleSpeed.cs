using UnityEngine.Events;

using Cysharp.Threading.Tasks;
using Engine.Core.Time;

namespace IdleProject.Battle
{
    public partial class BattleManager
    {
        public float currentBattleSpeed = 1f;

        private const float MIN_BATTLE_SPEED = 1f;
        private const float MAX_BATTLE_SPEED = 3f;
        private const string BATTLE_SPEED_TIME_KEY = "BattleSpeed";

        public float GetCurrentBattleSpeed => TimeManager.Instance.GetTimeScaleFactor(BATTLE_SPEED_TIME_KEY);

        public float GetCurrentBattleDeltaTime => TimeManager.Instance.GetDeltaTimeScale(BATTLE_SPEED_TIME_KEY);

        public UnityEvent<float> GetChangeBattleSpeedEvent =>
            TimeManager.Instance.GetFactorChangeEvent(BATTLE_SPEED_TIME_KEY);

        public UniTask GetBattleTimer(float waitTime) =>
            TimeManager.Instance.StartTimer(waitTime, BATTLE_SPEED_TIME_KEY);
        
        
        public void NextBattleSpeed()
        {
            currentBattleSpeed += 1f;
            if (currentBattleSpeed > MAX_BATTLE_SPEED)
                currentBattleSpeed = MIN_BATTLE_SPEED;
            
            TimeManager.Instance.SetTimeScaleFactor(BATTLE_SPEED_TIME_KEY, currentBattleSpeed);
        }
    }
}