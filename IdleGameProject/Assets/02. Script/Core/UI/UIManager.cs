using Engine.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace IdleProject.Core.UI
{
    public class UIManager : SingletonMonoBehaviour<UIManager>
    {
        Dictionary<Type, UIController> uiControllerDic = new Dictionary<Type, UIController>();

        public void AddUIController(UIController controller)
        {
            var uiControllerType = controller.GetType();
            uiControllerDic.Add(uiControllerType, controller);
        }

        public void RemoveUIController(UIController controller)
        {
            var uiControllerType = controller.GetType();
            uiControllerDic.Remove(uiControllerType);
        }

        public T GetUIController<T>() where T : UIController
        {
            if (uiControllerDic.TryGetValue(typeof(T), out var ctrl))
                return ctrl as T;

            return null;
        }

        public static Vector3 GetUIInScreen(Vector3 position)
        {
            return new Vector3(Mathf.Clamp(position.x, 0, Screen.width), Mathf.Clamp(position.y, 0, Screen.height));
        }
    }
}