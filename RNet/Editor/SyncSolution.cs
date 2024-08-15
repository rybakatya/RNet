#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEditor.Build;

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
        private static NamedBuildTarget[] targets = new NamedBuildTarget[]
        {
            NamedBuildTarget.Android,
            NamedBuildTarget.EmbeddedLinux,
            NamedBuildTarget.iOS,
            NamedBuildTarget.LinuxHeadlessSimulation,
            NamedBuildTarget.NintendoSwitch,
            NamedBuildTarget.QNX,
            NamedBuildTarget.Server,
            NamedBuildTarget.Stadia,
            NamedBuildTarget.Standalone,
            NamedBuildTarget.tvOS,
            NamedBuildTarget.VisionOS,
            NamedBuildTarget.WebGL,
            NamedBuildTarget.WindowsStoreApps,
            NamedBuildTarget.XboxOne
        };

        [UnityEditor.MenuItem(menuName + "Server")]
        public static void ToggleToServer()
        {
            foreach (var target in targets)
            {
                string[] strings = new string[128];


                PlayerSettings.GetScriptingDefineSymbols(target, out strings);
                var l = strings.ToList();
                if (l.Contains("CLIENT"))
                {
                    l.Remove("CLIENT");
                }
                if(l.Contains("SERVER"))
                {
                    l.Remove("SERVER");
                }
                l.Add("SERVER");

                var newSymbols = l.ToArray();

                PlayerSettings.SetScriptingDefineSymbols(target, newSymbols);

               
            }

            Menu.SetChecked(menuName + "Server", true);
            Menu.SetChecked(menuName + "Client", false);

        }


        [UnityEditor.MenuItem(menuName + "Client")]
        public static void ToggleToClient()
        {
            foreach (var target in targets)
            {
                string[] strings = new string[128];
                PlayerSettings.GetScriptingDefineSymbols(target, out strings);

                var l = strings.ToList();
                if (l.Contains("SERVER"))
                {
                    l.Remove("SERVER");
                }
                if(l.Contains("CLIENT"))
                {
                    l.Remove("CLIENT");
                }    

                l.Add("CLIENT");

                var newSymbols = l.ToArray();
                PlayerSettings.SetScriptingDefineSymbols(target, newSymbols);

               
            }
            Menu.SetChecked(menuName + "Server", false);
            Menu.SetChecked(menuName + "Client", true);


        }

    }

    [InitializeOnLoad]
    internal class MemoryTrackingEnabler
    {
        private static NamedBuildTarget[] targets = new NamedBuildTarget[]
       {
            NamedBuildTarget.Android,
            NamedBuildTarget.EmbeddedLinux,
            NamedBuildTarget.iOS,
            NamedBuildTarget.LinuxHeadlessSimulation,
            NamedBuildTarget.NintendoSwitch,
            NamedBuildTarget.QNX,
            NamedBuildTarget.Server,
            NamedBuildTarget.Stadia,
            NamedBuildTarget.Standalone,
            NamedBuildTarget.tvOS,
            NamedBuildTarget.VisionOS,
            NamedBuildTarget.WebGL,
            NamedBuildTarget.WindowsStoreApps,
            NamedBuildTarget.XboxOne
       };

        const string menuName = "RNet/MemoryTracker/";

        [UnityEditor.MenuItem(menuName + "Off")]
        public static void ToggleToOff()
        {
            foreach (var target in targets)
            {
                string[] strings = new string[128];
                PlayerSettings.GetScriptingDefineSymbols(target, out strings);

                var l = strings.ToList();
                if (l.Contains("TRACK"))
                {
                    l.Remove("TRACK");
                }


                var newSymbols = l.ToArray();
                PlayerSettings.SetScriptingDefineSymbols(target, newSymbols);

            }

            Menu.SetChecked(menuName + "Off", true);
            Menu.SetChecked(menuName + "On", false);


        }


        [UnityEditor.MenuItem(menuName + "On")]
        public static void ToggleToClient()
        {
            foreach (var target in targets)
            {
                string[] strings = new string[128];
                PlayerSettings.GetScriptingDefineSymbols(target, out strings);

                var l = strings.ToList();
                if (l.Contains("TRACK") == false)
                {
                    l.Add("TRACK");
                }


                var newSymbols = l.ToArray();
                PlayerSettings.SetScriptingDefineSymbols(target, newSymbols);
            }
            Menu.SetChecked(menuName + "Off", false);
            Menu.SetChecked(menuName + "On", true);


        }

    }
}
#endif