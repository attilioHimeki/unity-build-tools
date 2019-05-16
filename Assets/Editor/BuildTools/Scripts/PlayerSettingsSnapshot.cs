using UnityEditor;

public class PlayerSettingsSnapshot
{

    private BuildTargetGroup buildTargetGroup;
    private ScriptingImplementation scriptingBackend;
    private ManagedStrippingLevel strippingLevel;
    private string scriptingDefineSymbols;

    public void takeSnapshot(BuildTargetGroup targetGroup)
    {
        buildTargetGroup = targetGroup;

        scriptingBackend = PlayerSettings.GetScriptingBackend(targetGroup);
        scriptingDefineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
        strippingLevel = PlayerSettings.GetManagedStrippingLevel(targetGroup);
    }

    public void applySnapshot()
    {
        PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, scriptingDefineSymbols);
        PlayerSettings.SetScriptingBackend(buildTargetGroup, scriptingBackend);
        PlayerSettings.SetManagedStrippingLevel(buildTargetGroup, strippingLevel);
    }

}