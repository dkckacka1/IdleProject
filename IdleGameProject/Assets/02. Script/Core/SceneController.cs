using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace IdleProject.Core
{
    // 씬 매니저 클래스
    // 각 씬당 하나씩 존재
    public abstract class SceneController : MonoBehaviour
    {
        private void Awake()
        {
            GameManager.SetCurrentSceneManager(this);
        }

        public abstract void SceneInitialize();
    }
}
