using System.Reflection;
using UnityEngine.AddressableAssets;

using Engine.Core.Addressable;
using Engine.Util;
using IdleProject.Data;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace IdleProject.Core
{
    public class AddressableManager : SingletonMonoBehaviour<AddressableManager>
    {
        public readonly AddressableController Controller = new();
    }
}