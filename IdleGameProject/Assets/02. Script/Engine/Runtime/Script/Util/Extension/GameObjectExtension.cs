﻿using UnityEngine;

namespace Engine.Util.Extension
{
    public static class GameObjectExtension
    {
        public static void SetLayerRecursively(this GameObject obj, int layer)
        {
            obj.layer = layer;

            foreach (Transform child in obj.transform)
            {
                SetLayerRecursively(child.gameObject, layer);
            }
        }
    }
}