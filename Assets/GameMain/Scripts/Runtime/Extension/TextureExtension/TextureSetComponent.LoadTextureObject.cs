using System;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;

namespace UGFExtensions.Texture
{
    public partial class TextureSetComponent
    {
        [Serializable]
        public class LoadTextureObject
        {
#if ODIN_INSPECTOR
            [ShowInInspector]
#endif
            public ISetTexture2dObject Texture2dObject { get; }
#if ODIN_INSPECTOR
            [ShowInInspector]
#endif
            public Texture2D Texture2D { get; }
#if UNITY_EDITOR
            public bool IsSelect { get; set; }
#endif
            public LoadTextureObject(ISetTexture2dObject obj,Texture2D texture2D)
            {
                Texture2dObject = obj;
                Texture2D = texture2D;
            }
        }
    }
}