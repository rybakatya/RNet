#if ENABLE_MONO || ENABLE_IL2CPP
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RapidNetworkLibrary.Editor
{
    public class ExtensionWindowTabAttribute : System.Attribute
    {
        public int order { get; set; }
        public string name { get; set; }
    }

    public abstract class ExtensionWindowTab 
    {
        protected readonly RNetWindow _window;
        public ExtensionWindowTab(RNetWindow window)
        {
            _window = window;
        }

        public abstract void OnEnable();
        public abstract void OnDestroy();
        public abstract void OnGUI();
    }
}
#endif