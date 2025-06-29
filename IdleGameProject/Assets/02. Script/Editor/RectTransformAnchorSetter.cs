using UnityEditor;
using UnityEngine;

namespace IdleProject.EditorClass
{
    public static class RectTransformAnchorSetter
    {
        [MenuItem("Tools/UI/Set Anchors To Corners %&a")] // 단축키: Ctrl + Alt + A
        private static void SetAnchorsToCorners()
        {
            foreach (var obj in Selection.transforms)
            {
                var rectTransform = obj as RectTransform;
                if (!rectTransform || !rectTransform.parent)
                    continue;

                var parent = rectTransform.parent as RectTransform;
                if (!parent)
                    continue;

                var newAnchorMin = new Vector2(
                    rectTransform.anchorMin.x + rectTransform.offsetMin.x / parent.rect.width,
                    rectTransform.anchorMin.y + rectTransform.offsetMin.y / parent.rect.height);

                var newAnchorMax = new Vector2(
                    rectTransform.anchorMax.x + rectTransform.offsetMax.x / parent.rect.width,
                    rectTransform.anchorMax.y + rectTransform.offsetMax.y / parent.rect.height);

                Undo.RecordObject(rectTransform, "Set Anchors To Corners");

                rectTransform.anchorMin = newAnchorMin;
                rectTransform.anchorMax = newAnchorMax;
                rectTransform.offsetMin = Vector2.zero;
                rectTransform.offsetMax = Vector2.zero;
            }
        }

        [MenuItem("Tools/UI/Set Anchors To Center %&q")] // 단축키 Ctrl + Alt + Q
        static void SetAnchorsToCenter()
        {
            foreach (var obj in Selection.transforms)
            {
                var rectTransform = obj as RectTransform;
                if (!rectTransform || !rectTransform.parent)
                    continue;

                var parent = rectTransform.parent as RectTransform;
                if (!parent)
                    continue;

                var anchorMin = new Vector2(
                    rectTransform.anchorMin.x + rectTransform.offsetMin.x / parent.rect.width,
                    rectTransform.anchorMin.y + rectTransform.offsetMin.y / parent.rect.height);

                var anchorMax = new Vector2(
                    rectTransform.anchorMax.x + rectTransform.offsetMax.x / parent.rect.width,
                    rectTransform.anchorMax.y + rectTransform.offsetMax.y / parent.rect.height);

                var newAnchor = (anchorMin + anchorMax) * 0.5f;

                var size = rectTransform.rect.size;
                var pivot = rectTransform.pivot;
                var newOffsetMin = -size * pivot;
                var newOffsetMax = size * (Vector2.one - pivot);

                Undo.RecordObject(rectTransform, "Set Anchors To Corners");

                rectTransform.anchorMin = newAnchor;
                rectTransform.anchorMax = newAnchor;
                rectTransform.anchoredPosition = Vector2.zero;

                rectTransform.offsetMin = newOffsetMin;
                rectTransform.offsetMax = newOffsetMax;
            }
        }
    }
}