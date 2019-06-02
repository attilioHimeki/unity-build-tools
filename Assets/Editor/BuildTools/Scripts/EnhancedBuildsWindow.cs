using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Diagnostics;
#if UNITY_2018_1_OR_NEWER
using UnityEditor.Build.Reporting;
#endif

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
        Undo.undoRedoPerformed += onUndoRedo;

        if(EditorPrefs.HasKey(EDITOR_PREFS_KEY)) 
        {
            string objectPath = EditorPrefs.GetString(EDITOR_PREFS_KEY);
            buildSetup = AssetDatabase.LoadAssetAtPath (objectPath, typeof(BuildSetup)) as BuildSetup;
        }
    }

    void OnDisable () 
    {
        Undo.undoRedoPerformed -= onUndoRedo;
    }

    private void onUndoRedo()
    {
        if(buildSetup)
        {
            EditorUtility.SetDirty(buildSetup);
            Repaint();
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
                Undo.RecordObject(buildSetup, "Set Build Setup Root Directory");
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
                    EditorGUILayout.BeginHorizontal();
                    b.enabled = EditorGUILayout.Toggle("", b.enabled, GUILayout.MaxWidth(15.0f));
                    b.guiShowOptions = EditorGUILayout.Foldout(b.guiShowOptions, b.buildName, EditorStyles.foldout);
                    EditorGUILayout.EndHorizontal();
                    if(b.guiShowOptions)
                    {
                        EditorGUI.indentLevel++;
                        drawBuildEntryGUI(b);
                        EditorGUI.indentLevel--;
                    }   
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
                Undo.RecordObject(buildSetup, "Add Build Setup Entry");
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
                            Undo.RecordObject(buildSetup, "Remove Build Setup Entry Custom scene");
                            b.customScenes.RemoveAt(i);
                            i--;
                        }
                        GUILayout.EndHorizontal ();
                    }

                }
                if (GUILayout.Button("Add Scene", GUILayout.ExpandWidth(false))) 
                {
                    Undo.RecordObject(buildSetup, "Add Build Setup Entry Custom scene");
                    b.customScenes.Add(string.Empty);
                }

                EditorGUI.indentLevel--;
            }
        }

        b.guiShowAdvancedOptions = EditorGUILayout.Foldout(b.guiShowAdvancedOptions, "Advanced Options");
        if(b.guiShowAdvancedOptions)
        {
            EditorGUI.indentLevel++;

            #if UNITY_2018_3_OR_NEWER
            b.strippingLevel = (ManagedStrippingLevel)EditorGUILayout.EnumPopup ("Stripping Level", b.strippingLevel);
            #endif

            b.strictMode = EditorGUILayout.Toggle(new GUIContent("Strict Mode", 
                                                "Do not allow the build to succeed if any errors are reported."), 
                                                b.strictMode);
            b.assetBundleManifestPath = EditorGUILayout.TextField("AssetBundle Manifest Path", b.assetBundleManifestPath);
            if(b.target == BuildTarget.iOS)
            {
                b.iosSymlinkLibraries = EditorGUILayout.Toggle("XCode - Symlink Library", b.iosSymlinkLibraries);
            }
            b.scriptingBackend = (ScriptingImplementation)EditorGUILayout.EnumPopup ("Scripting Backend", b.scriptingBackend);
            EditorGUI.indentLevel--;
        }

        if (GUILayout.Button("Remove Entry", GUILayout.ExpandWidth(false))) 
        {
            Undo.RecordObject(buildSetup, "Removed Build Setup Entry");
            buildSetup.deleteBuildSetupEntry(b);
        }
        
        GUILayout.Space(10);
    }

    private void buildGame ()
    {
        var defaultScenes = ScenesUtils.getDefaultScenesAsArray();

        string path = buildSetup.rootDirectory;

        var playerSettingsSnapshot = new PlayerSettingsSnapshot();

        var setupList = buildSetup.entriesList;
        for(var i = 0; i < setupList.Count; i++)
        {
            var setup = setupList[i];
            if(setup.enabled)
            {
                var target = setup.target;
                var targetGroup = BuildPipeline.GetBuildTargetGroup(target);

                playerSettingsSnapshot.takeSnapshot(targetGroup);

                PlayerSettings.SetScriptingBackend(targetGroup, setup.scriptingBackend);
                PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, setup.scriptingDefineSymbols);
                
                #if UNITY_2018_3_OR_NEWER
                PlayerSettings.SetManagedStrippingLevel(targetGroup, setup.strippingLevel);
                #endif

                var buildPlayerOptions = BuildUtils.getBuildPlayerOptionsFromBuildSetupEntry(setup, path, defaultScenes);

                #if UNITY_2018_1_OR_NEWER
                BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
                BuildSummary buildSummary = report.summary;
                UnityEngine.Debug.Log("Build " + setup.buildName + " ended with Status: " + buildSummary.result);
                #else
                var result = BuildPipeline.BuildPlayer(buildPlayerOptions);
                var success = string.IsNullOrEmpty(result);
                UnityEngine.Debug.Log("Build " + setup.buildName + " ended with Success: " + success);
                #endif

                // Revert group build player settings after building
                playerSettingsSnapshot.applySnapshot();
            }
            else
            {
                UnityEngine.Debug.Log("Skipping Build " + setup.buildName);
            }
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
            var loadedBuildAsset = AssetDatabase.LoadAssetAtPath (relPath, typeof(BuildSetup)) as BuildSetup;

            if (loadedBuildAsset) 
            {
                buildSetup = loadedBuildAsset;
                EditorPrefs.SetString(EDITOR_PREFS_KEY, relPath);
            }
        }
    }


}