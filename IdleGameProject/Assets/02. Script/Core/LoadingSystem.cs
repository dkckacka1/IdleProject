using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace IdleProject.Core.Loading
{
    public class LoadingSystem
    {
        List<Func<UniTask>> loadingList;

        public bool IsNowLoading => loadingList.Count > 0;

        public LoadingSystem()
        {
            loadingList = new List<Func<UniTask>>();
        }

        public void AddLoading(Func<UniTask> method)
        {
            loadingList.Add(method);
            StartLoading(method);
        }

        private async void StartLoading(Func<UniTask> method)
        {
            await method.Invoke();
            loadingList.Remove(method);
        }
    }
}

