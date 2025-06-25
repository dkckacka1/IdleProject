using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Engine.Util;
using Sirenix.OdinInspector;
using UnityEngine;

namespace IdleProject.Core.GameData
{
    public class DataManager : SingletonMonoBehaviour<DataManager>
    {
        [ShowInInspector]
        private readonly Dictionary<Type, Dictionary<string, Data.StaticData.StaticData>> _dataDictionary = new();

        private const string DATA_LABEL_REFERENCE_NAME = "Data";
        
        [ShowInInspector]
        public DataController DataController { get; private set; }

        public async UniTask LoadData()
        {
            var locateList =
                await AddressableManager.Instance.Controller.LoadAssetLabelLocationList(DATA_LABEL_REFERENCE_NAME);

            foreach (var locate in locateList)
            {
                var loadData =
                    await AddressableManager.Instance.Controller.LoadAssetAsync<Data.StaticData.StaticData>(locate.PrimaryKey);
                
                AddData(locate.ResourceType, loadData);                
            }
            
            DataController = TestManager.Instance.isTestPlay is false ? new DataController() : new DataController(TestManager.Instance.testPlayerData);
        }

        public void AddData(Type type, Data.StaticData.StaticData staticData)
        {
            if (_dataDictionary.ContainsKey(type) is false)
            {
                _dataDictionary.Add(type, new Dictionary<string, Data.StaticData.StaticData>());
            }

            var targetDic = _dataDictionary[type];
            targetDic.Add(staticData.Index, staticData);
        }

        public T GetData<T>(string dataIndex) where T : Data.StaticData.StaticData
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
        public List<T> GetDataList<T>() where T : Data.StaticData.StaticData
        {
            return _dataDictionary.TryGetValue(typeof(T), out var targetDic) ? targetDic.Values.Select(data => data as T).ToList() : null;
        }
    }
}