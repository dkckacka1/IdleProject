using Engine.Util;
using System.Collections.Generic;
using System.Linq;
using IdleProject.Core.UI.Loading;
using IdleProject.Lobby.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace IdleProject.Core.UI
{
    public class UIManager : SingletonMonoBehaviour<UIManager>
    {
        private readonly Dictionary<string, UIBase> _uiBaseDic = new();
        private UIController _currentUIController;

        [HideInInspector] public LoadingUI loadingUI;
        
        [FormerlySerializedAs("confirmPopup")] [SerializeField] private ConfirmUIPopup confirmUIPopup;
        [FormerlySerializedAs("toastPopup")] [SerializeField] private ToastUIPopup toastUIPopup;        
        
        public bool IsShowingLoading => loadingUI.isLoading;
        
        protected override void Initialized()
        {
            base.Initialized();
            loadingUI = GetComponentInChildren<LoadingUI>();
        }
        
        public void SetUIController(UIController controller)
        {
            _currentUIController = controller;
        }

        public T GetUIController<T>() where T : UIController
        {
            return _currentUIController as T;
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

        public void InitializedUI()
        {
            _currentUIController.Initialized();
            foreach (var ui in _uiBaseDic.Values.OfType<IUIInit>())
            {
                ui.Initialized();
            }
        }

        public IEnumerable<T> GetUIsOfType<T>()
        {
            return _uiBaseDic.Values.OfType<T>();
        }

        public void OpenConfirmPopup(string description, UnityAction yesAction)
        {
            confirmUIPopup.SetConfirm(description, yesAction);
            confirmUIPopup.OpenPopup();
        }

        public void OpenToastPopup(string description)
        {
            toastUIPopup.SetToast(description);
            toastUIPopup.OpenPopup();
        }

        public static Vector3 GetUIInScreen(Vector3 position)
        {
            return new Vector3(Mathf.Clamp(position.x, 0, Screen.width), Mathf.Clamp(position.y, 0, Screen.height));
        }
    }
}