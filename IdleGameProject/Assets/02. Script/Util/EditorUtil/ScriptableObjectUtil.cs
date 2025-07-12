#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using IdleProject.Data.StaticData;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace IdleProject.EditorClass
{
    public static class ScriptableObjectUtil
    {
        public static T CreateStaticData<T>(string name, UnityAction<T> setDataAction = null) where T : StaticData
        {
            var asset = ScriptableObject.CreateInstance<T>();
            setDataAction?.Invoke(asset);
            
            AssetDatabase.CreateAsset(asset,$"Assets/{name}.asset");
            AssetDatabase.SaveAssets();

            return asset;
        }
        
        public static void AddChildObject<T>(ScriptableObject parent, List<T> childList, Func<int, string> setNameAction) where T : ScriptableObject
        {
            // 자식 인스턴스화
            var child = ScriptableObject.CreateInstance<T>();
            childList.Add(child);
            child.name = setNameAction.Invoke(childList.Count - 1);

            AssetDatabase.AddObjectToAsset(child, parent);
            EditorUtility.SetDirty(parent);
            AssetDatabase.SaveAssets();
        }

        public static void RefreshChildList<T>(ScriptableObject parent, List<T> childList, Func<int, string> setNameAction) where T : ScriptableObject
        {
            // 현재 리스트에 맞게 자식 오브젝트 삭제 및 정렬
            // 현재 이 ScriptableObject에 포함된 모든 서브 오브젝트 가져오기
            var subAssets = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(parent));

            // SkillAction 타입 중에서 현재 리스트에 없는 오브젝트들을 찾아 삭제
            foreach (var asset in subAssets)
            {
                if (asset is T action && !childList.Contains(action))
                {
                    Object.DestroyImmediate(action, true);
                }
            }

            // 이름 재정렬
            for (int i = 0; i < childList.Count; i++)
            {
                var action = childList[i];
                if (action)
                {
                    var newName = setNameAction.Invoke(i);
                    if (action.name != newName)
                    {
                        action.name = newName;
                        EditorUtility.SetDirty(action);
                    }
                }
            }

            // 변경 사항 저장
            EditorUtility.SetDirty(parent);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
#endif