using UnityEditor;
using System.Collections.Generic;

public static class BuildUtils
{

    public const string SETUPS_REL_DIRECTORY = "Assets/Editor/BuildTools/Setups/";
    public static BuildPlayerOptions getBuildPlayerOptionsFromBuildSetupEntry(BuildSetupEntry setupEntry, string rootDirPath, string[] defaultScenes)
    {
        var buildPlayerOptions = new BuildPlayerOptions();

        buildPlayerOptions.target = setupEntry.target;

        if(setupEntry.useDefaultBuildScenes)
        {
            buildPlayerOptions.scenes = defaultScenes;
        }
        else
        {
            buildPlayerOptions.scenes = setupEntry.customScenes.ToArray();
        }

        buildPlayerOptions.locationPathName = rootDirPath + "/" + setupEntry.buildName;

        if(!string.IsNullOrEmpty(setupEntry.assetBundleManifestPath))
        {
            buildPlayerOptions.assetBundleManifestPath = setupEntry.assetBundleManifestPath;
        }

        BuildOptions buildOptions = BuildOptions.None;
        if(setupEntry.debugBuild)
        {
            buildOptions |= BuildOptions.Development;
        }
        if(setupEntry.strictMode)
        {
            buildOptions |= BuildOptions.StrictMode;
        }
        buildPlayerOptions.options = buildOptions;

        return buildPlayerOptions;
    }
}