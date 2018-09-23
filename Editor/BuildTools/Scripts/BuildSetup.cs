using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using UnityEditor.Build.Reporting;
 
[Serializable]
public class BuildSetup : ScriptableObject 
{
    [SerializeField] public string rootDirectory = "";

    [SerializeField] public List<BuildSetupEntry> itemList;

	[MenuItem("Assets/Create/BuildSetup")]
    public static BuildSetup Create()
    {
        BuildSetup asset = ScriptableObject.CreateInstance<BuildSetup>();

        AssetDatabase.CreateAsset(asset, BuildUtils.SETUPS_REL_DIRECTORY + "buildSetup.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }
}
