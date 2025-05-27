using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace IdleProject.Core.Loading
{
    public static class LoadingChecker
    {
        private const float MaxLoadingTime = 30f;

        private static Dictionary<string, LoadingSystem> currentLoadingDic = new Dictionary<string, LoadingSystem>();

        public static void StartLoading(string loadingName, Func<UniTask> loading)
        {
            if (currentLoadingDic.ContainsKey(loadingName) is false)
            {
                currentLoadingDic.Add(loadingName, new());
                currentLoadingDic[loadingName].AddLoading(loading);
                CheckLoading(loadingName).Forget();
            }
            else
            {
                currentLoadingDic[loadingName].AddLoading(loading);
            }
        }

        private static async UniTaskVoid CheckLoading(string loadingName)
        {
            float loadingTime = 0f;
            while (currentLoadingDic[loadingName].IsNowLoading)
            {
                await UniTask.WaitForEndOfFrame();
                loadingTime += Time.deltaTime;

                if (loadingTime >= MaxLoadingTime)
                {
                    Debug.Log("최대 로딩 시간 초과");
                    return;
                }
            }

            Debug.Log($"{loadingName} Loading is End");
            currentLoadingDic.Remove(loadingName);
        }
    }
}

