#if UNITY_EDITOR
using IdleProject.Data.StaticData;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace IdleProject.EditorClass
{
    public static class StaticDataCreator
    {
        
        
        public static T CreateStaticData<T>(string name, UnityAction<T> setDataAction = null) where T : StaticData
        {
            var asset = ScriptableObject.CreateInstance<T>();
            setDataAction?.Invoke(asset);
            
            AssetDatabase.CreateAsset(asset,$"Assets/{name}.asset");
            AssetDatabase.SaveAssets();

            return asset;
        }
    }
}
#endif