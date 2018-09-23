using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
 
[Serializable]
public class BuildSetup : ScriptableObject 
{
    public string rootDirectory = "";

    public List<BuildSetupEntry> entriesList;

	[MenuItem("Assets/Create/BuildSetup")]
    public static BuildSetup Create()
    {
        BuildSetup asset = ScriptableObject.CreateInstance<BuildSetup>();

        AssetDatabase.CreateAsset(asset, BuildUtils.SETUPS_REL_DIRECTORY + "buildSetup.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }
}
