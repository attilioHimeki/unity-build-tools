#! /bin/sh
set -e

SCRIPTPATH=$(dirname $0)

# Used to compose the Unity download URL
BASE_URL="https://download.unity3d.com/download_unity/"
EDITOR_PACKAGE_SUFFIX="/MacEditorInstaller/Unity-"
WINDOWS_TARGET_PACKAGE_SUFFIX="/MacEditorTargetInstaller/UnitySetup-Windows-Mono-Support-for-Editor-"
MAC_TARGET_PACKAGE_SUFFIX="/MacEditorTargetInstaller/UnitySetup-Mac-Mono-Support-for-Editor-"
LINUX_TARGET_PACKAGE_SUFFIX="/MacEditorTargetInstaller/UnitySetup-Linux-Support-for-Editor-"
IOS_TARGET_PACKAGE_SUFFIX="/MacEditorTargetInstaller/UnitySetup-iOS-Support-for-Editor-"
ANDROID_TARGET_PACKAGE_SUFFIX="/MacEditorTargetInstaller/UnitySetup-Android-Support-for-Editor-"
EXTENSION=".pkg"

UNITY_DOWNLOAD_DIR="$HOME/Downloads/unity_downloads"
DEFAULT_UNITY_INSTALL_FOLDER="/Applications/Unity"
CHANGESETS_TEXT_FILE_PATH="$SCRIPTPATH/unity-download-changesets.txt"

install_unity() 
{
	VERSION=$1
    PACKAGE_SUFFIX=$2
    
    CHANGESET_SEARCH_RESULT="$(grep $VERSION $CHANGESETS_TEXT_FILE_PATH)"
    ENTRIES="$(echo $CHANGESET_SEARCH_RESULT | grep -o ':' | wc -l)"
    
    if [ $ENTRIES != 1 ] ; then
        echo "Unity version not found, or ambiguous. Please specify a valid version from the changesets file. Aborting..."
        exit 1
    fi
        
    #Set version from file, just in case the user forgot to add the last part, for example f1
    VERSION="$(cut -f1 -d ":" <<< $CHANGESET_SEARCH_RESULT)"
    CHANGESET="$(cut -d ":" -f2- <<< $CHANGESET_SEARCH_RESULT)"
    
    URL=$BASE_URL$CHANGESET$PACKAGE_SUFFIX$VERSION$EXTENSION
	download_package $URL
    
    FILENAME=`basename "$URL"`
    FILEPATH="$UNITY_DOWNLOAD_DIR/$FILENAME"
    
	echo "Installing $FILENAME"
	sudo installer -dumplog -verbose -package $FILEPATH -target /
    
    #TODO: rename folder with unity version
}

download_package() 
{
	URL=$1
    
    #Example URL: https://download.unity3d.com/download_unity/787658998520/MacEditorInstaller/Unity-2018.2.0f2.pkg
    
    FILENAME=`basename "$URL"`
    FILEPATH="$UNITY_DOWNLOAD_DIR/$FILENAME"
	
	if [ ! -e $FILEPATH ] ; then
		echo "$FILENAME not found. Downloading from $URL"
		mkdir -p "$UNITY_DOWNLOAD_DIR"
        curl -o $FILEPATH "$URL"
        echo "$FILENAME downloaded in folder $UNITY_DOWNLOAD_DIR"
	else
		echo "$FILENAME Already downloaded. Skipping..."
	fi
}


#TODO: Allow choosing platform packages
echo "Installing Unity v. $1..."
install_unity $1 $EDITOR_PACKAGE_SUFFIX
echo "Installing Android support for Unity v. $1..."
install_unity $1 $ANDROID_TARGET_PACKAGE_SUFFIX
