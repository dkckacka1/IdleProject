using Engine.Core.Addressable;
using Engine.Util;

namespace IdleProject.Core
{
    public class AddressableManager : SingletonMonoBehaviour<AddressableManager>
    {
        public readonly AddressableController Controller = new();
    }
}