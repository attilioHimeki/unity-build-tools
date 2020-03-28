using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Diagnostics;
#if UNITY_2018_1_OR_NEWER
using UnityEditor.Build.Reporting;
#endif

namespace Himeki.Build
{
    public static class BuildProcess
    {

        private const string BUILD_FILE_RELATIVE_PATH_ARG = "-buildSetupRelPath";

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

                    if(VRUtils.targetGroupSupportsVirtualReality(targetGroup))
                    {
                        PlayerSettings.SetVirtualRealitySupported(targetGroup, setup.supportsVR);
                        if (setup.supportsVR)
                        {
                            var vrSdks = VRUtils.getSelectedVRSdksFromFlags(targetGroup, setup.vrSdkFlags);
                            PlayerSettings.SetVirtualRealitySDKs(targetGroup, vrSdks);
                        }
                    }
                    else
                    {
                        PlayerSettings.SetVirtualRealitySupported(targetGroup, false);
                    }

                    if (target == BuildTarget.Android)
                    {
                        EditorUserBuildSettings.buildAppBundle = setup.androidAppBundle;
                        PlayerSettings.Android.targetArchitectures = setup.androidArchitecture;
                    }

                    if(target == BuildTarget.PS4)
                    {
                        EditorUserBuildSettings.ps4HardwareTarget = setup.ps4HardwareTarget;
                        EditorUserBuildSettings.ps4BuildSubtarget = setup.ps4BuildSubtarget;
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
                        UnityEngine.Debug.LogError("Failure - Aborting remaining builds from batch");
                        break;
                    }
                }
                else
                {
                    UnityEngine.Debug.Log("Skipping Build " + setup.buildName);
                }
            }
        }

        public static void Build(string buildSetupRelativePath)
        {
            var buildSetup = AssetDatabase.LoadAssetAtPath(buildSetupRelativePath, typeof(BuildSetup)) as BuildSetup;
            if (buildSetup != null)
            {
                Build(buildSetup);
            }
            else
            {
                UnityEngine.Debug.LogError("Cannot find build setup in path: " + buildSetupRelativePath);
            }
        }

        public static void BuildWithArgs()
        {
            var args = System.Environment.GetCommandLineArgs();

            string buildFilePath = string.Empty;
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == BUILD_FILE_RELATIVE_PATH_ARG)
                {
                    buildFilePath = args[i + 1];
                    break;
                }
            }

            if (!string.IsNullOrEmpty(buildFilePath))
            {
                Build(buildFilePath);
            }
            else
            {
                UnityEngine.Debug.LogError("Cannot find build setup path, make sure to specify using " + BUILD_FILE_RELATIVE_PATH_ARG);
            }
        }
    }
}