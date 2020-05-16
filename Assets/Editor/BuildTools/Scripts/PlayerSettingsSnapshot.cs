using UnityEditor;

namespace Himeki.Build
{
    public class PlayerSettingsSnapshot
    {

        private BuildTargetGroup buildTargetGroup;
        private ScriptingImplementation scriptingBackend;
        private string scriptingDefineSymbols;
#if UNITY_2018_3_OR_NEWER
        private ManagedStrippingLevel strippingLevel;
#endif
        public bool androidAppBundleEnabled;
        private bool vrSupported;
        private string[] vrSdks;

        public void takeSnapshot(BuildTargetGroup targetGroup)
        {
            buildTargetGroup = targetGroup;

            scriptingBackend = PlayerSettings.GetScriptingBackend(targetGroup);
            scriptingDefineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);

#if UNITY_2018_3_OR_NEWER
            strippingLevel = PlayerSettings.GetManagedStrippingLevel(targetGroup);
#endif

#if UNITY_2017_2_OR_NEWER
            vrSupported = PlayerSettings.GetVirtualRealitySupported(targetGroup);
            vrSdks = PlayerSettings.GetVirtualRealitySDKs(targetGroup);
#endif

#if UNITY_2017_4_OR_NEWER
            androidAppBundleEnabled = EditorUserBuildSettings.buildAppBundle;
#endif
        }

        public void applySnapshot()
        {
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, scriptingDefineSymbols);
            PlayerSettings.SetScriptingBackend(buildTargetGroup, scriptingBackend);

#if UNITY_2018_3_OR_NEWER
            PlayerSettings.SetManagedStrippingLevel(buildTargetGroup, strippingLevel);
#endif

#if UNITY_2017_4_OR_NEWER
            EditorUserBuildSettings.buildAppBundle = androidAppBundleEnabled;
#endif

#if UNITY_2017_2_OR_NEWER
            PlayerSettings.SetVirtualRealitySupported(buildTargetGroup, vrSupported);
            PlayerSettings.SetVirtualRealitySDKs(buildTargetGroup, vrSdks);
#endif

        }

    }
}