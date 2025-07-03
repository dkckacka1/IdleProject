using System;
using Cysharp.Threading.Tasks;
using IdleProject.Core;
using IdleProject.Core.GameData;
using IdleProject.Core.Loading;
using IdleProject.Core.Resource;
using IdleProject.Core.Sound;
using IdleProject.Core.UI;
using IdleProject.Title.UI;
using UnityEngine;

namespace IdleProject.Title
{
    public class TitleManager : SceneController
    {
        private const string DATA_INIT_TASK = "DataInit";

        private TitleUIController _titleUIController; 
        public override async UniTask Initialize()
        {
            _titleUIController = UIManager.Instance.GetUIController<TitleUIController>(); 
        }

        private async void Start()
        {
            TaskChecker.StartLoading(DATA_INIT_TASK, DataManager.Instance.LoadData);
            TaskChecker.StartLoading(DATA_INIT_TASK, ResourceManager.Instance.LoadAssets);
            _titleUIController.Loading(TaskChecker.WaitTasking(DATA_INIT_TASK));
            await TaskChecker.WaitTasking(DATA_INIT_TASK);
        }
    }
}
