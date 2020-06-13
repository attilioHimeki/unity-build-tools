using System;
using System.Collections.Generic;
using UnityEditor;

namespace Himeki.Build
{

    [Serializable]
    public class BuildSetupEntry
    {
        public bool enabled = true;
        public string buildName = "";
        public BuildTarget target = BuildTarget.NoTarget;
        public bool debugBuild = false;
        public string scriptingDefineSymbols = "";
        public bool useDefaultBuildScenes = true;
        public List<string> customScenes;

        //VR
        public bool supportsVR = false;
        public int vrSdkFlags;

        // Advanced Options
#if UNITY_2018_3_OR_NEWER
        public ManagedStrippingLevel strippingLevel;
#endif
        public ScriptingImplementation scriptingBackend = ScriptingImplementation.IL2CPP;
        public string assetBundleManifestPath = "";
        public bool strictMode = false;
#if UNITY_2020_1_OR_NEWER
        public bool detailedBuildReport = false;
#endif

        //iOS
        public bool iosSymlinkLibraries = false;

        //Android
#if UNITY_2017_4_OR_NEWER
        public bool androidAppBundle = false;
        public AndroidArchitecture androidArchitecture;
#endif

        //PS4
        public PS4BuildSubtarget ps4BuildSubtarget;
        public PS4HardwareTarget ps4HardwareTarget;

        // GUI status
        [NonSerialized] public bool guiShowOptions = true;
        [NonSerialized] public bool guiShowCustomScenes = false;
        [NonSerialized] public bool guiShowAdvancedOptions = false;
        [NonSerialized] public bool guiShowVROptions = false;

        public static BuildSetupEntry Clone(BuildSetupEntry source)
        {
            return source.MemberwiseClone() as BuildSetupEntry;
        }

    }

}