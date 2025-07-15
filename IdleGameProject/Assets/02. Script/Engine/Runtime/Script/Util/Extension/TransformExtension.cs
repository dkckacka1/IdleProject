using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Engine.Util.Extension
{
    public static class TransformExtension
    {
        public static Transform GetChildByName(this Transform transform, string name)
        {
            return transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name.Equals(name));
        }
        
        public static Vector3 GetCenterPoint(List<Transform> transforms)
        {
            if (transforms == null || transforms.Count == 0)
                return Vector3.zero;

            var sum = Vector3.zero;
            foreach (var t in transforms)
            {
                sum.x += t.position.x;
                sum.y += t.position.y;
                sum.z += t.position.z;
            }

            var centerX = sum.x / transforms.Count;
            var centerY = sum.y / transforms.Count;
            var centerZ = sum.z / transforms.Count;

            return new Vector3(centerX, centerY, centerZ);
        }
    }
}