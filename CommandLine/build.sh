#!/bin/bash
set -e

DEFAULTBUILDSETUPPATH="Assets/Editor/BuildTools/Setup/BuildSetup.asset"

UNITYPATH=$1
PROJECTPATH=$2
BUILDSETUPPATH=$DEFAULTBUILDSETUPPATH

function set_build_setup_path()
{
    if [ "$1" != "" ]; then
        BUILDSETUPPATH=$1
    fi
}

set_build_setup_path $3
$UNITYPATH -quit -batchmode -nographics -projectPath $PROJECTPATH -executeMethod Himeki.Build.BuildProcess.BuildWithArgs -buildSetupRelPath $BUILDSETUPPATH -logFile 

#EXAMPLE
# /Applications/Unity/Hub/Editor/2018.4.6f1/Unity.app/Contents/MacOS/Unity 
# -quit -batchmode -nographics 
# -projectPath /Users/attiliocarotenuto/Documents/GitHub/unity-build-tools 
# -executeMethod Himeki.Build.BuildProcess.BuildWithArgs
# -buildSetupRelPath Assets/Editor/BuildTools/Setup/BuildSetup.asset 
# -logFile