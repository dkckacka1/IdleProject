using System;
using Engine.Util.Extension;
using IdleProject.Core.UI;
using UnityEngine;

namespace IdleProject.Lobby.Character
{
    public class LobbyCharacter : UIBase
    {
        private Animator _animator;
        private GameObject _model;

        protected override void Awake()
        {
            base.Awake();
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
            model.transform.position = transform.position;
            _model = model;
        }

        public void SetAnimation(RuntimeAnimatorController animatorController)
        {
            _animator.runtimeAnimatorController = animatorController;
            _animator.Rebind();
            _animator.Update(0f);
        }
    }
}
