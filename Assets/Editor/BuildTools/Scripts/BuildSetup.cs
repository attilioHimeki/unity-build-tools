using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace Himeki.Build
{

    [Serializable]
    public class BuildSetup : ScriptableObject
    {
        public string rootDirectory = "";
        public bool abortBatchOnFailure = false;
        public List<BuildSetupEntry> entriesList;

        [MenuItem("Builds/Create/BuildSetup")]
        public static BuildSetup Create()
        {
            BuildSetup asset = ScriptableObject.CreateInstance<BuildSetup>();

            AssetDatabase.CreateAsset(asset, BuildUtils.SETUPS_REL_DIRECTORY + "buildSetup.asset");
            AssetDatabase.SaveAssets();
            return asset;
        }

        public void addBuildSetupEntry()
        {
            BuildSetupEntry buildEntry = new BuildSetupEntry();

            var currentBuildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;

            buildEntry.buildName = Application.productName;
            buildEntry.target = EditorUserBuildSettings.activeBuildTarget;
            buildEntry.customScenes = new List<string>();
            buildEntry.scriptingBackend = PlayerSettings.GetScriptingBackend(currentBuildTargetGroup);

            entriesList.Add(buildEntry);
        }

        public void deleteBuildSetupEntry(BuildSetupEntry entry)
        {
            entriesList.Remove(entry);
        }

        public bool isReady()
        {
            var hasPath = !string.IsNullOrEmpty(rootDirectory);
            var hasEntries = entriesList.Any(e => e.enabled);

            return hasPath && hasEntries;
        }
    }

}