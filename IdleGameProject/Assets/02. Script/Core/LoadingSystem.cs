using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace IdleProject.Core.Loading
{
    public class LoadingSystem
    {
        private readonly List<Func<UniTask>> _loadingList = new();

        public bool IsNowLoading => _loadingList.Count > 0;

        public void AddLoading(Func<UniTask> method)
        {
            _loadingList.Add(method);
            StartLoading(method).Forget();
        }

        private async UniTask StartLoading(Func<UniTask> method)
        {
            await method.Invoke();
            _loadingList.Remove(method);
        }
    }
}

