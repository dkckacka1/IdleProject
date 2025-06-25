using Cysharp.Threading.Tasks;
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

        public async UniTask SetAnimation(RuntimeAnimatorController animatorController)
        {
            _animator.runtimeAnimatorController = animatorController;
            await BindAnimation();
        }
        
        private async UniTask BindAnimation()
            // 상태전이 유예를 위해 1프레임 대기 후 리바인드
        {
            _model.gameObject.SetActive(false);
            await UniTask.Yield();
            _animator.Update(0f);
            _animator.Rebind();
            _model.gameObject.SetActive(true);
        }
    }
}
