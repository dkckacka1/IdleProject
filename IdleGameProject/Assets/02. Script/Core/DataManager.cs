using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Engine.Util;
using IdleProject.Data;
using Sirenix.OdinInspector;
using UnityEngine;
using Data = IdleProject.Data.Data;

namespace IdleProject.Core
{
    public class DataManager : SingletonMonoBehaviour<DataManager>
    {
        [ShowInInspector]
        private readonly Dictionary<Type, Dictionary<string, Data.Data>> _dataDictionary =
            new Dictionary<Type, Dictionary<string, Data.Data>>();

        private const string DATA_LABEL_REFERENCE_NAME = "Data";

        protected override void Initialized()
        {
            base.Initialized();

            LoadData().Forget();
        }
        
        private async UniTaskVoid LoadData()
        {
            var locateList =
                await AddressableManager.Instance.Controller.LoadAssetLabelLocationList(DATA_LABEL_REFERENCE_NAME);

            foreach (var locate in locateList)
            {
                var loadData =
                    await AddressableManager.Instance.Controller.LoadAssetAsync<Data.Data>(locate.PrimaryKey);
                
                AddData(locate.ResourceType, loadData);                
            }
        }

        public void AddData(Type type, Data.Data data)
        {
            if (_dataDictionary.ContainsKey(type) is false)
            {
                _dataDictionary.Add(type, new Dictionary<string, Data.Data>());
            }

            var targetDic = _dataDictionary[type];
            targetDic.Add(data.Index, data);
        }

        public T GetData<T>(string dataIndex) where T : Data.Data
        {
            if (_dataDictionary.TryGetValue(typeof(T), out var targetDic))
            {
                if (targetDic.TryGetValue(dataIndex, out var result))
                {
                    return result as T;
                }
                else
                {
                    Debug.LogError($"{dataIndex} is invalid Key");
                }
            }
            else
            {
                Debug.LogError($"{typeof(T).Name} is nullDic");
            }
            
            return null;
        }
    }
}