using System;
using Engine.Util.Extension;
using UnityEngine;

namespace IdleProject.Lobby.Character
{
    public class LobbyCharacter : MonoBehaviour
    {
        private Animator _animator;
        private GameObject _model;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void SetModel(GameObject model)
        {
            if (_model is not null)
            {
                Destroy(_model.gameObject);
            }

            if (model is null) return;
            
            model.SetLayerRecursively(LayerMask.NameToLayer("UIObject"));
            _model = model;
        }

        public void SetAnimation(RuntimeAnimatorController animatorController)
        {
            _animator.runtimeAnimatorController = animatorController;
            _animator.Rebind();
        }
    }
}
