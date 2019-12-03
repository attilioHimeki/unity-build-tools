using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Diagnostics;
#if UNITY_2018_1_OR_NEWER
using UnityEditor.Build.Reporting;
#endif

public static class BuildProcess
{

    public static void Build(BuildSetup buildSetup)
    {
        var defaultScenes = ScenesUtils.getDefaultScenesAsArray();

        string path = buildSetup.rootDirectory;

        var playerSettingsSnapshot = new PlayerSettingsSnapshot();

        var setupList = buildSetup.entriesList;
        for (var i = 0; i < setupList.Count; i++)
        {
            var setup = setupList[i];
            if (setup.enabled)
            {
                var target = setup.target;
                var targetGroup = BuildPipeline.GetBuildTargetGroup(target);

                playerSettingsSnapshot.takeSnapshot(targetGroup);

                PlayerSettings.SetScriptingBackend(targetGroup, setup.scriptingBackend);
                PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, setup.scriptingDefineSymbols);

#if UNITY_2018_3_OR_NEWER
                PlayerSettings.SetManagedStrippingLevel(targetGroup, setup.strippingLevel);
#endif

                PlayerSettings.SetVirtualRealitySupported(targetGroup, setup.supportsVR);
                if (setup.supportsVR)
                {
                    var vrSdks = VRUtils.getSelectedVRSdksFromFlags(targetGroup, setup.vrSdkFlags);
                    PlayerSettings.SetVirtualRealitySDKs(targetGroup, vrSdks);
                }

                var buildPlayerOptions = BuildUtils.getBuildPlayerOptionsFromBuildSetupEntry(setup, path, defaultScenes);

#if UNITY_2018_1_OR_NEWER
                BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
                BuildSummary buildSummary = report.summary;
                var success = (buildSummary.result == BuildResult.Succeeded);
                UnityEngine.Debug.Log("Build " + setup.buildName + " ended with Status: " + buildSummary.result);
#else
                var result = BuildPipeline.BuildPlayer(buildPlayerOptions);
                var success = string.IsNullOrEmpty(result);
                UnityEngine.Debug.Log("Build " + setup.buildName + " ended with Success: " + success);
#endif

                // Revert group build player settings after building
                playerSettingsSnapshot.applySnapshot();

                if (!success && buildSetup.abortBatchOnFailure)
                {
                    UnityEngine.Debug.Log("Aborting remaining Builds from batch");
                    break;
                }
            }
            else
            {
                UnityEngine.Debug.Log("Skipping Build " + setup.buildName);
            }
        }
    }
}