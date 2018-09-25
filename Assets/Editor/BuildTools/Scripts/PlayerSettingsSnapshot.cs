using UnityEditor;

public class PlayerSettingsSnapshot
{

    private BuildTargetGroup buildTargetGroup;
    private ScriptingImplementation scriptingBackend;
    private string scriptingDefineSymbols;

    public void takeSnapshot(BuildTargetGroup targetGroup)
    {
        buildTargetGroup = targetGroup;

        scriptingBackend = PlayerSettings.GetScriptingBackend(targetGroup);
        scriptingDefineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
    }

    public void applySnapshot()
    {
        PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, scriptingDefineSymbols);
        PlayerSettings.SetScriptingBackend(buildTargetGroup, scriptingBackend);
    }

}