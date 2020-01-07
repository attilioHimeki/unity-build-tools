using UnityEditor;

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

        vrSupported = PlayerSettings.GetVirtualRealitySupported(targetGroup);
        vrSdks = PlayerSettings.GetVirtualRealitySDKs(targetGroup);

        androidAppBundleEnabled = EditorUserBuildSettings.buildAppBundle;
    }

    public void applySnapshot()
    {
        PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, scriptingDefineSymbols);
        PlayerSettings.SetScriptingBackend(buildTargetGroup, scriptingBackend);

#if UNITY_2018_3_OR_NEWER
        PlayerSettings.SetManagedStrippingLevel(buildTargetGroup, strippingLevel);
#endif

        EditorUserBuildSettings.buildAppBundle = androidAppBundleEnabled;

        PlayerSettings.SetVirtualRealitySupported(buildTargetGroup, vrSupported);
        PlayerSettings.SetVirtualRealitySDKs(buildTargetGroup, vrSdks);

    }

}