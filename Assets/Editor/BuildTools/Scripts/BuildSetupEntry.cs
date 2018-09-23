using System;
using System.Collections.Generic;
using UnityEditor;

[Serializable]
public class BuildSetupEntry
{
	public string buildName = "";
	public BuildTarget target;
	public bool debugBuild = false;
	public string scriptingDefineSymbols = "";
	public bool useDefaultBuildScenes = true;
	public List<string> customScenes;

	// Advanced Options
	public string assetBundleManifestPath = "";
	public bool strictMode = false;

	// GUI status
	[NonSerialized] public bool guiShowCustomScenes = false;
	[NonSerialized] public bool guiShowAdvancedOptions = false;
}
