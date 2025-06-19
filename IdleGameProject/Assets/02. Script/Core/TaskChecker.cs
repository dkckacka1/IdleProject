using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace IdleProject.Core.Loading
{
    public class CheckList
    {
        public readonly Checker Checker = new();
        public readonly UnityEvent OnInitComplete = new();
    }
    
    public static class TaskChecker
    {
        private const float MAX_LOADING_TIME = 30f;

        private static readonly Dictionary<string, CheckList> CURRENT_TASK_DIC = new Dictionary<string, CheckList>();

        public static bool IsTasking(string taskName) => CURRENT_TASK_DIC.ContainsKey(taskName);

        public static async UniTask WaitTasking(string taskName)
        {
            await UniTask.WaitUntil(() => IsTasking(taskName) is false);
        }
        
        public static void StartLoading(string taskName, Func<UniTask> task, UnityAction onTaskComplete = null)
        {
            if (CURRENT_TASK_DIC.TryGetValue(taskName, out var checkList) is false)
            {
                CURRENT_TASK_DIC.Add(taskName, new CheckList());
                CURRENT_TASK_DIC[taskName].Checker.AddLoading(task);

                CheckLoading(taskName).Forget();
            }
            else
            {
                checkList.Checker.AddLoading(task);
            }
            
            if(onTaskComplete is not null)
                CURRENT_TASK_DIC[taskName].OnInitComplete.AddListener(onTaskComplete);
        }

        public static void AddOnCompleteCallback(string taskName, UnityAction onTaskComplete)
        {
            if (CURRENT_TASK_DIC.TryGetValue(taskName, out var checkList))
            {
                checkList.OnInitComplete.AddListener(onTaskComplete);
            }
            else
            {
                Debug.LogError($"{taskName} Task is not Tasking");
            }
        }
        
        private static async UniTaskVoid CheckLoading(string taskName)
        {
            var taskTime = 0f;
            while (CURRENT_TASK_DIC[taskName].Checker.IsNowLoading)
            {
                await UniTask.WaitForEndOfFrame();
                taskTime += Time.deltaTime;

                if (taskTime >= MAX_LOADING_TIME)
                {
                    Debug.Log("최대 로딩 시간 초과");
                    return;
                }
            }

            CURRENT_TASK_DIC[taskName].OnInitComplete?.Invoke();
            CURRENT_TASK_DIC.Remove(taskName);
        }
    }
}

