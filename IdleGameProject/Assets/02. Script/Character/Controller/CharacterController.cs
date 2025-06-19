using UnityEngine;

namespace IdleProject.Character
{
    public class CharacterController : MonoBehaviour
    {
        private GameObject _model;
        
        public void SetModel(GameObject model)
        {
            if (_model is not null)
            {
                Destroy(_model.gameObject);
            }
            
            _model = model;
        }
    }
}