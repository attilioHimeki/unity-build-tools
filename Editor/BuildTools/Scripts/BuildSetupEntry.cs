using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[Serializable]
public class BuildSetupEntry
{
	[SerializeField] public string buildName = "";
	[SerializeField] public BuildTarget target;
	[SerializeField] public bool debugBuild = false;
	[SerializeField] public string scriptingDefineSymbols = "";
	[SerializeField] public bool useDefaultBuildScenes = true;
	[SerializeField] public List<string> customScenes;

	// Advanced Options
	[SerializeField] public string assetBundleManifestPath = "";
	[SerializeField] public bool strictMode = false;

	// GUI status

	public bool guiShowCustomScenes = false;
	public bool guiShowAdvancedOptions = false;
}
