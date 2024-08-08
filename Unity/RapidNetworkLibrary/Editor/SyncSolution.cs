#if UNITY_EDITOR
using System.Linq;
using UnityEditor;


namespace RapidNetworkLibrary.Editor
{
    internal class SyncSolution
    {

        [UnityEditor.MenuItem("Assets/Regen Sln", false, 1000)]
        public static void RegenerateSolution()
        {
            Unity.CodeEditor.CodeEditor.Editor.CurrentCodeEditor.SyncAll();
        }
    }

    [InitializeOnLoad]
    internal class SwitchTargets
    {

        static SwitchTargets()
        {

        }

        const string menuName = "RNet/SwitchTarget/";

        [UnityEditor.MenuItem(menuName + "Server")]
        public static void ToggleToServer()
        {
            string[] strings = new string[128];
            UnityEditor.PlayerSettings.GetScriptingDefineSymbols(UnityEditor.Build.NamedBuildTarget.Standalone, out strings);

            var l = strings.ToList();
            if (l.Contains("CLIENT"))
            {
                l.Remove("CLIENT");
            }
            l.Add("SERVER");

            var newSymbols = l.ToArray();
            UnityEditor.PlayerSettings.SetScriptingDefineSymbols(UnityEditor.Build.NamedBuildTarget.Standalone, newSymbols);
            Menu.SetChecked(menuName + "Server", true);
            Menu.SetChecked(menuName + "Client", false);

        }


        [UnityEditor.MenuItem(menuName + "Client")]
        public static void ToggleToClient()
        {
            string[] strings = new string[128];
            UnityEditor.PlayerSettings.GetScriptingDefineSymbols(UnityEditor.Build.NamedBuildTarget.Standalone, out strings);

            var l = strings.ToList();
            if (l.Contains("SERVER"))
            {
                l.Remove("SERVER");
            }
            l.Add("CLIENT");

            var newSymbols = l.ToArray();
            UnityEditor.PlayerSettings.SetScriptingDefineSymbols(UnityEditor.Build.NamedBuildTarget.Standalone, newSymbols);
            Menu.SetChecked(menuName + "Server", false);
            Menu.SetChecked(menuName + "Client", true);

        }

    }
}
#endif