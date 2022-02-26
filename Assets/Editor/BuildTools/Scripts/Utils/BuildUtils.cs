using UnityEditor;

namespace Himeki.Build
{
    public static class BuildUtils
    {

        public const string SETUPS_REL_DIRECTORY = "Assets/Editor/BuildTools/";
        private const string WINDOWS_EXTENSION = ".exe";
        public static BuildPlayerOptions getBuildPlayerOptionsFromBuildSetupEntry(BuildSetupEntry setupEntry, string rootDirPath, string[] defaultScenes)
        {
            var buildPlayerOptions = new BuildPlayerOptions();

            buildPlayerOptions.target = setupEntry.target;

            if (setupEntry.useDefaultBuildScenes)
            {
                buildPlayerOptions.scenes = defaultScenes;
            }
            else
            {
                buildPlayerOptions.scenes = setupEntry.customScenes.ToArray();
            }

            var pathName = rootDirPath + "/" + setupEntry.buildName;
            if(setupEntry.target == BuildTarget.StandaloneWindows || setupEntry.target == BuildTarget.StandaloneWindows64)
            {
                if(!pathName.Contains(WINDOWS_EXTENSION))
                {
                    pathName += WINDOWS_EXTENSION;
                }
            }
            buildPlayerOptions.locationPathName = pathName;
            

            if (!string.IsNullOrEmpty(setupEntry.assetBundleManifestPath))
            {
                buildPlayerOptions.assetBundleManifestPath = setupEntry.assetBundleManifestPath;
            }

            BuildOptions buildOptions = BuildOptions.None;
            if (setupEntry.debugBuild)
            {
                buildOptions |= BuildOptions.Development;
            }
            
            if (setupEntry.strictMode)
            {
                buildOptions |= BuildOptions.StrictMode;
            }

            if (setupEntry.target == BuildTarget.iOS)
            {
                if (setupEntry.iosSymlinkLibraries)
                {
//Todo: Need to find specific version when this was changed
#if UNITY_2021_1_OR_NEWER
                    buildOptions |= BuildOptions.SymlinkSources;
#else
                    buildOptions |= BuildOptions.SymlinkLibraries;
#endif
                }
            }

#if UNITY_2020_1_OR_NEWER
            if(setupEntry.detailedBuildReport)
            {
                buildOptions |= BuildOptions.DetailedBuildReport;
            }
#endif

            buildPlayerOptions.options = buildOptions;

            return buildPlayerOptions;
        }
    }
}