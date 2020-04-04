#!/bin/bash
set -e

UNITY_PATH=$1
PROJECT_PATH=$2
PACKAGE_FILENAME=$3
EXPORT_PATH=$4
ASSETS_REL_PATH=$5

EXTENSION=".unitypackage"

$UNITY_PATH -quit -batchmode -nographics -projectPath $PROJECT_PATH -exportPackage "$ASSET_SREL_PATH" "$EXPORT_PATH/$PACKAGE_FILENAME$EXTENSION" -logFile 

#EXAMPLE
# /Applications/Unity/Hub/Editor/2018.4.6f1/Unity.app/Contents/MacOS/Unity 
# -quit -batchmode -nographics 
# -projectPath /Users/attiliocarotenuto/Documents/GitHub/unity-build-tools 
# -exportPackage Assets/Editor/BuildTools /Users/attiliocarotenuto/Documents/Dev/BuildTools.unitypackage
# -logFile