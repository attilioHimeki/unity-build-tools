using UnityEditor;

public class PlayerSettingsSnapshot
{

    private BuildTargetGroup buildTargetGroup;
    private ScriptingImplementation scriptingBackend;
    private string scriptingDefineSymbols;
    #if UNITY_2018_3_OR_NEWER
    private ManagedStrippingLevel strippingLevel;
    #endif

    public void takeSnapshot(BuildTargetGroup targetGroup)
    {
        buildTargetGroup = targetGroup;

        scriptingBackend = PlayerSettings.GetScriptingBackend(targetGroup);
        scriptingDefineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);

        #if UNITY_2018_3_OR_NEWER
        strippingLevel = PlayerSettings.GetManagedStrippingLevel(targetGroup);
        #endif
    }

    public void applySnapshot()
    {
        PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, scriptingDefineSymbols);
        PlayerSettings.SetScriptingBackend(buildTargetGroup, scriptingBackend);

        #if UNITY_2018_3_OR_NEWER
        PlayerSettings.SetManagedStrippingLevel(buildTargetGroup, strippingLevel);
        #endif
    }

}