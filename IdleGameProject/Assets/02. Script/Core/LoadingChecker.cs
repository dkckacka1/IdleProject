using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace IdleProject.Core.Loading
{
    public static class LoadingChecker
    {
        private const float MAX_LOADING_TIME = 30f;

        private static readonly Dictionary<string, LoadingSystem> CURRENT_LOADING_DIC = new Dictionary<string, LoadingSystem>();

        public static void StartLoading(string loadingName, Func<UniTask> loading)
        {
            if (CURRENT_LOADING_DIC.TryGetValue(loadingName, out var loadingSystem) is false)
            {
                CURRENT_LOADING_DIC.Add(loadingName, new());
                CURRENT_LOADING_DIC[loadingName].AddLoading(loading);
                CheckLoading(loadingName).Forget();
            }
            else
            {
                loadingSystem.AddLoading(loading);
            }
        }

        private static async UniTaskVoid CheckLoading(string loadingName)
        {
            var loadingTime = 0f;
            while (CURRENT_LOADING_DIC[loadingName].IsNowLoading)
            {
                await UniTask.WaitForEndOfFrame();
                loadingTime += Time.deltaTime;

                if (loadingTime >= MAX_LOADING_TIME)
                {
                    Debug.Log("최대 로딩 시간 초과");
                    return;
                }
            }

            Debug.Log($"{loadingName} Loading is End");
            CURRENT_LOADING_DIC.Remove(loadingName);
        }
    }
}

