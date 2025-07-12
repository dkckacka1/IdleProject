using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace IdleProject.Core.Loading
{
    public class Checker
    {
        private readonly List<Func<UniTask>> _taskList = new();

        public bool IsNowLoading => _taskList.Count > 0;

        public void AddLoading(Func<UniTask> method)
        {
            _taskList.Add(method);
            StartTasking(method).Forget();
        }

        private async UniTask StartTasking(Func<UniTask> method)
        {
            await method.Invoke();
            _taskList.Remove(method);
        }
    }
}

