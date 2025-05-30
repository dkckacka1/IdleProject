using Engine.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace IdleProject.Core.UI
{
    public class UIManager : SingletonMonoBehaviour<UIManager>
    {
        private readonly Dictionary<Type, UIController> _uiControllerDic = new Dictionary<Type, UIController>();
        private readonly Dictionary<string, UIBase> _uiBaseDic = new();

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

        public void AddUI(string name, UIBase ui)
        {
            _uiBaseDic.Add(name,ui);
        }

        public T GetUI<T>(string name) where T : UIBase
        {
            _uiBaseDic.TryGetValue(name, out var result);

            if (result is null)
            {
                Debug.LogError($"{name} UI is null");
            }

            var ui = result as T;
            if (ui is null)
            {
                Debug.LogError($"{name} is not {typeof(T).Name}");
            }            
            
            return ui;
        }

        public void RemoveUI(string name)
        {
            _uiBaseDic.Remove(name);
        }

        public static Vector3 GetUIInScreen(Vector3 position)
        {
            return new Vector3(Mathf.Clamp(position.x, 0, Screen.width), Mathf.Clamp(position.y, 0, Screen.height));
        }
    }
}