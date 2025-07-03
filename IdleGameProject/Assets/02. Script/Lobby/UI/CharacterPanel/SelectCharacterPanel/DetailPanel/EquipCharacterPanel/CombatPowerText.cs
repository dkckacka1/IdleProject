using DG.Tweening;
using IdleProject.Core.UI;
using UnityEngine;

namespace IdleProject.Lobby.UI.CharacterPanel
{
    public class CombatPowerText : UIText
    {
        [SerializeField] private float duration = 0.5f;

        private int _combatPower;
        private Tween _tween;

        public void SetCombatPower(int value, bool isTween = true)
        {
            if (_combatPower == value)
                return;

            _tween?.Kill();

            if (isTween)
            {
                var startValue = _combatPower;
                var endValue = value;

                _tween = DOTween.To(() => startValue, x =>
                    {
                        startValue = x;
                        Text.text = x.ToString("N0");
                    }, endValue, duration)
                    .OnComplete(() =>
                    {
                        _combatPower = endValue;
                        Text.text = _combatPower.ToString("N0");
                    })
                    .SetEase(Ease.OutQuad);
            }
            else
            {
                _combatPower = value;
                Text.text = _combatPower.ToString("N0");
            }
        }
    }
}