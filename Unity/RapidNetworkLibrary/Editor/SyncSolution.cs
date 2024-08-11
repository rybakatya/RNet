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


            UnityEditor.PlayerSettings.GetScriptingDefineSymbols(UnityEditor.Build.NamedBuildTarget.Server, out strings);

            l = strings.ToList();
            if (l.Contains("CLIENT"))
            {
                l.Remove("CLIENT");
            }
            l.Add("SERVER");

            newSymbols = l.ToArray();
            UnityEditor.PlayerSettings.SetScriptingDefineSymbols(UnityEditor.Build.NamedBuildTarget.Server, newSymbols);
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


            UnityEditor.PlayerSettings.GetScriptingDefineSymbols(UnityEditor.Build.NamedBuildTarget.Server, out strings);

            l = strings.ToList();
            if (l.Contains("SERVER"))
            {
                l.Remove("SERVER");
            }
            l.Add("CLIENT");

            newSymbols = l.ToArray();
            UnityEditor.PlayerSettings.SetScriptingDefineSymbols(UnityEditor.Build.NamedBuildTarget.Server, newSymbols);
            Menu.SetChecked(menuName + "Server", false);
            Menu.SetChecked(menuName + "Client", true);

        }

    }

    [InitializeOnLoad]
    internal class MemoryTrackingEnabler
    {


        const string menuName = "RNet/MemoryTracker/";

        [UnityEditor.MenuItem(menuName + "Off")]
        public static void ToggleToOff()
        {
            string[] strings = new string[128];
            UnityEditor.PlayerSettings.GetScriptingDefineSymbols(UnityEditor.Build.NamedBuildTarget.Standalone, out strings);

            var l = strings.ToList();
            if (l.Contains("TRACK"))
            {
                l.Remove("TRACK");
            }
            

            var newSymbols = l.ToArray();
            UnityEditor.PlayerSettings.SetScriptingDefineSymbols(UnityEditor.Build.NamedBuildTarget.Standalone, newSymbols);


            UnityEditor.PlayerSettings.GetScriptingDefineSymbols(UnityEditor.Build.NamedBuildTarget.Server, out strings);

            l = strings.ToList();
            if (l.Contains("TRACK"))
            {
                l.Remove("TRACK");
            }
            

            newSymbols = l.ToArray();
            UnityEditor.PlayerSettings.SetScriptingDefineSymbols(UnityEditor.Build.NamedBuildTarget.Server, newSymbols);
            Menu.SetChecked(menuName + "Off", true);
            Menu.SetChecked(menuName + "On", false);

        }


        [UnityEditor.MenuItem(menuName + "On")]
        public static void ToggleToClient()
        {
            string[] strings = new string[128];
            UnityEditor.PlayerSettings.GetScriptingDefineSymbols(UnityEditor.Build.NamedBuildTarget.Standalone, out strings);

            var l = strings.ToList();
            if (l.Contains("TRACK") == false)
            {
                l.Add("TRACK");
            }
            

            var newSymbols = l.ToArray();
            UnityEditor.PlayerSettings.SetScriptingDefineSymbols(UnityEditor.Build.NamedBuildTarget.Standalone, newSymbols);


            UnityEditor.PlayerSettings.GetScriptingDefineSymbols(UnityEditor.Build.NamedBuildTarget.Server, out strings);

            l = strings.ToList();
            if (l.Contains("TRACK") == false)
            {
                l.Add("TRACK");
            }


            newSymbols = l.ToArray();
            UnityEditor.PlayerSettings.SetScriptingDefineSymbols(UnityEditor.Build.NamedBuildTarget.Server, newSymbols);
            Menu.SetChecked(menuName + "Off", false);
            Menu.SetChecked(menuName + "On", true);

        }

    }
}
#endif