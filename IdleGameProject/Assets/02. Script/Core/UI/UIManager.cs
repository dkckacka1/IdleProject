using Engine.Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace IdleProject.Core.UI
{
    public class UIManager : SingletonMonoBehaviour<UIManager>
    {
        private readonly Dictionary<Type, UIController> _uiControllerDic = new Dictionary<Type, UIController>();
        private readonly Dictionary<string, UIBase> _uiBaseDic = new();

        [HideInInspector] public LoadingUI loadingUI;

        public bool IsShowingLoading => loadingUI.isLoading;

        protected override void Initialized()
        {
            base.Initialized();

            loadingUI = GetComponentInChildren<LoadingUI>();
        }

        public void AddUIController(UIController controller)
        {
            var uiControllerType = controller.GetType();
            _uiControllerDic.Add(uiControllerType, controller);
        }

        public void RemoveUIController(UIController controller)
        {
            var uiControllerType = controller.GetType();
            _uiControllerDic.Remove(uiControllerType);
        }

        public T GetUIController<T>() where T : UIController
        {
            if (_uiControllerDic.TryGetValue(typeof(T), out var ctrl))
                return ctrl as T;

            return null;
        }

        public void AddUI(string uiName, UIBase ui)
        {
            _uiBaseDic.Add(uiName,ui);
        }

        public T GetUI<T>() where T : UIBase
        {
            var uiName = typeof(T).Name;
            _uiBaseDic.TryGetValue(uiName, out var result);

            if (result is null)
            {
                Debug.LogError($"{uiName} UI is null");
            }

            var ui = result as T;
            if (ui is null)
            {
                Debug.LogError($"{uiName} is not {typeof(T).Name}");
            }            
            
            return ui;
        }
        
        public T GetUI<T>(string uiName) where T : UIBase
        {
            _uiBaseDic.TryGetValue(uiName, out var result);

            if (result is null)
            {
                Debug.LogError($"{uiName} UI is null");
            }

            var ui = result as T;
            if (ui is null)
            {
                Debug.LogError($"{uiName} is not {typeof(T).Name}");
            }            
            
            return ui;
        }

        public void RemoveUI(string uiName)
        {
            _uiBaseDic.Remove(uiName);
        }

        public static Vector3 GetUIInScreen(Vector3 position)
        {
            return new Vector3(Mathf.Clamp(position.x, 0, Screen.width), Mathf.Clamp(position.y, 0, Screen.height));
        }
    }
}