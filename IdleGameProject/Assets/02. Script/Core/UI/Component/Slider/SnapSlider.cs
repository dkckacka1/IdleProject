using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace IdleProject.Core.UI
{
    [RequireComponent(typeof(Slider))]
    public class SnapSlider : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        [Range(2, 100), SerializeField]
        private int snapCount = 10;

        private Slider _slider;
        private float[] _snapPoints;

        private void Awake()
        {
            _slider = GetComponent<Slider>();
            GenerateSnapPoints();
        }

        private void OnValidate()
        {
            if (_slider)
                GenerateSnapPoints();
        }

        private void GenerateSnapPoints()
        {
            // 최소 2개 이상의 스냅 포인트가 있어야 유효
            snapCount = Mathf.Max(2, snapCount);
            _snapPoints = new float[snapCount];

            var range = _slider.maxValue - _slider.minValue;
            var step = range / (snapCount - 1);

            for (int i = 0; i < snapCount; i++)
            {
                _snapPoints[i] = _slider.minValue + step * i;
            }
        }

        private float GetClosestSnapValue(float value)
        {
            var closest = _snapPoints[0];
            var minDiff = Mathf.Abs(value - closest);

            for (int i = 1; i < _snapPoints.Length; i++)
            {
                var diff = Mathf.Abs(value - _snapPoints[i]);
                if (diff < minDiff)
                {
                    minDiff = diff;
                    closest = _snapPoints[i];
                }
            }

            return closest;
        }

        public void OnDrag(PointerEventData eventData)
        {
            _slider.value = GetClosestSnapValue(_slider.value);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _slider.value = GetClosestSnapValue(_slider.value);
        }
    }
}
