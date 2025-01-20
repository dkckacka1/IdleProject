using UnityEngine;

using Engine.Util;
using Engine.Core.Addressable;

namespace IdleProject.Core
{
    public class AddressableManager : SingletonMonoBehaviour<AddressableManager>
    {
        public AddressableController Controller { get; private set; }

        private void Awake()
        {
            Controller = new AddressableController();
        }
    }
}