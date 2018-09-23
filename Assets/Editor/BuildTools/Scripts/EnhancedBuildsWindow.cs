using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor.Build.Reporting;

public class EnhancedBuildsWindow : EditorWindow
{
    private const string EDITOR_PREFS_KEY = "ObjectPath";
    private const string WINDOW_TITLE = "Enhanced Builds";
    public BuildSetup buildSetup;
    private Vector2 buildEntriesListScrollPos;

    [MenuItem("Builds/Open Enhanced Builds %#e")]
    static void Init()
    {
        EnhancedBuildsWindow window = (EnhancedBuildsWindow)EditorWindow.GetWindow(typeof(EnhancedBuildsWindow), false, WINDOW_TITLE, true);

        window.Show();
    }

    void OnEnable () 
    {
        if(EditorPrefs.HasKey(EDITOR_PREFS_KEY)) 
        {
            string objectPath = EditorPrefs.GetString(EDITOR_PREFS_KEY);
            buildSetup = AssetDatabase.LoadAssetAtPath (objectPath, typeof(BuildSetup)) as BuildSetup;
        }
    }

    void  OnGUI () 
    {
        EditorGUIUtility.labelWidth = 0f;

        GUILayout.Label ("Build Setup Editor", EditorStyles.boldLabel);
        GUILayout.Space(10);
        if (buildSetup != null) 
        {
            string objectPath = EditorPrefs.GetString(EDITOR_PREFS_KEY);
            EditorGUILayout.LabelField("Current Build File", objectPath);
        }

        GUILayout.BeginHorizontal ();

        if(buildSetup != null)
        {
            if (GUILayout.Button("Show in Library")) 
            {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = buildSetup;
            }
        }

        if (GUILayout.Button("Select Build File")) 
        {
            selectBuildFile();
        }

        if (GUILayout.Button("Create New Build File")) 
        {
            createNewBuildSetup();
        }

        GUILayout.EndHorizontal ();
            
        GUILayout.Space(20);

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            
        if (buildSetup != null) 
        {
            GUILayout.Label ("Loaded Build Setup", EditorStyles.boldLabel);

            GUILayout.Space(20);

            EditorGUIUtility.labelWidth = 200f;
            if (GUILayout.Button("Choose Root Directory", GUILayout.ExpandWidth(false))) 
            {
                buildSetup.rootDirectory = EditorUtility.SaveFolderPanel("Choose Location", "", "");
            }
            EditorGUILayout.LabelField("Root Directory", buildSetup.rootDirectory);
            
            int buildsAmount = buildSetup.entriesList.Count;
            
            GUILayout.Space(20);
            GUILayout.Label ("Builds (" + buildsAmount + ")", EditorStyles.label);
            GUILayout.Space(10);

            if (buildsAmount > 0) 
            {
                buildEntriesListScrollPos = EditorGUILayout.BeginScrollView(buildEntriesListScrollPos, false, false, GUILayout.Width(position.width), GUILayout.MaxHeight(500));
        
                var list = buildSetup.entriesList;
                for(var i = 0; i < list.Count; i++)
                {
                    var b = list[i];
                    drawBuildEntryGUI(b);
                }   

                EditorGUILayout.EndScrollView();
            
            } 
            else 
            {
                GUILayout.Label ("This Built List is Empty");
            }

            GUILayout.Space(10);

            if (GUILayout.Button("Add Entry", GUILayout.ExpandWidth(true))) 
            {
                buildSetup.addBuildSetupEntry();
            }

            GUILayout.Space(10);

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            if(buildSetup.isReady())
            {
                GUILayout.Space(10);

                if (GUILayout.Button("Build", GUILayout.ExpandWidth(true))) 
                {
                    buildGame();
                }
            }
            else
            {
                GUILayout.Label ("Define a Root directory and at least one build entry");
            }
        }
        else
        {
            GUILayout.Label ("Select or Create a new Build Setup", EditorStyles.boldLabel);
        }
        if (GUI.changed) 
        {
            EditorUtility.SetDirty(buildSetup);
        }
    }

    private void drawBuildEntryGUI(BuildSetupEntry b)
    {
        b.buildName = EditorGUILayout.TextField("Build Name", b.buildName);
        b.target = (BuildTarget)EditorGUILayout.EnumPopup ("Target", b.target);
        b.debugBuild = EditorGUILayout.Toggle("Debug Build", b.debugBuild);
        b.scriptingDefineSymbols = EditorGUILayout.TextField("Scripting Define Symbols", b.scriptingDefineSymbols);
        b.useDefaultBuildScenes = EditorGUILayout.Toggle("Use Default Build Scenes", b.useDefaultBuildScenes);
    
        if(!b.useDefaultBuildScenes)
        {
            b.guiShowCustomScenes = EditorGUILayout.Foldout(b.guiShowCustomScenes, "Custom Scenes");
            if (b.guiShowCustomScenes)
            {
                EditorGUI.indentLevel++;
                if (b.customScenes.Count > 0) 
                {   
                    var scenes = b.customScenes;

                    for(int i = 0; i < scenes.Count; i++)
                    {
                        GUILayout.BeginHorizontal ();
                        scenes[i] = EditorGUILayout.TextField("Scene " + i, scenes[i]);
                        if(GUILayout.Button("Select Scene", GUILayout.ExpandWidth(false)))
                        {
                            string absPath = EditorUtility.OpenFilePanel ("Select Scene file", "", "unity");
                            if (absPath.StartsWith(Application.dataPath)) 
                            {
                                string relPath = absPath.Substring(Application.dataPath.Length - "Assets".Length);
                                scenes[i] = relPath;
                            }
                        }
                        if (GUILayout.Button("Remove Scene", GUILayout.ExpandWidth(false))) 
                        {
                            b.customScenes.RemoveAt(i);
                            i--;
                        }
                        GUILayout.EndHorizontal ();
                    }

                }
                if (GUILayout.Button("Add Scene", GUILayout.ExpandWidth(false))) 
                {
                    b.customScenes.Add(string.Empty);
                }

                EditorGUI.indentLevel--;
            }
        }

        b.guiShowAdvancedOptions = EditorGUILayout.Foldout(b.guiShowAdvancedOptions, "Advanced Options");
        if(b.guiShowAdvancedOptions)
        {
            EditorGUI.indentLevel++;
            b.strictMode = EditorGUILayout.Toggle("Strict Mode", b.strictMode);
            b.assetBundleManifestPath = EditorGUILayout.TextField("AssetBundle Manifest Path", b.assetBundleManifestPath);
            if(b.target == BuildTarget.iOS)
            {
                b.iosSymlinkLibraries = EditorGUILayout.Toggle("XCode - Symlink Library", b.iosSymlinkLibraries);
            }
            EditorGUI.indentLevel--;
        }

        if (GUILayout.Button("Remove Entry", GUILayout.ExpandWidth(false))) 
        {
            buildSetup.deleteBuildSetupEntry(b);
        }
        
        GUILayout.Space(10);
    }

    private void buildGame ()
    {
        var defaultScenes = ScenesUtils.getDefaultScenesAsArray();

        string path = buildSetup.rootDirectory;

        var setupList = buildSetup.entriesList;
        for(var i = 0; i < setupList.Count; i++)
        {
            var setup = setupList[i];
            var target = setup.target;
            var targetGroup = BuildPipeline.GetBuildTargetGroup(target);

            var originalDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, setup.scriptingDefineSymbols);

            var buildPlayerOptions = BuildUtils.getBuildPlayerOptionsFromBuildSetupEntry(setup, path, defaultScenes);

            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary buildSummary = report.summary;

            UnityEngine.Debug.Log("Build " + setup.buildName + " ended with Status: " + buildSummary.result);

            PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, originalDefines);
        }

    }

    private void createNewBuildSetup () 
    {
        buildSetup = BuildSetup.Create();
        if (buildSetup) 
        {
            buildSetup.entriesList = new List<BuildSetupEntry>();
            string relPath = AssetDatabase.GetAssetPath(buildSetup);
            EditorPrefs.SetString(EDITOR_PREFS_KEY, relPath);
        }
    }
    private void selectBuildFile () 
    {
        string absPath = EditorUtility.OpenFilePanel ("Select Build Setup file", BuildUtils.SETUPS_REL_DIRECTORY, "asset");
        if (absPath.StartsWith(Application.dataPath)) 
        {
            string relPath = absPath.Substring(Application.dataPath.Length - "Assets".Length);
            buildSetup = AssetDatabase.LoadAssetAtPath (relPath, typeof(BuildSetup)) as BuildSetup;

            if (buildSetup) 
            {
                EditorPrefs.SetString(EDITOR_PREFS_KEY, relPath);
            }
        }
    }


}