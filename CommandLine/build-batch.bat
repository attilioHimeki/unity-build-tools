TITLE Unity Build Tools - Batch

SET DEFAULTBUILDSETUPPATH="Assets/Editor/BuildTools/Setup/BuildSetup.asset"

SET UNITYPATH=%1
SET PROJECTPATH=%2
SET BUILDSETUPPATH=%DEFAULTBUILDSETUPPATH%

%UNITYPATH% -quit -batchmode -nographics -projectPath %PROJECTPATH% -executeMethod Himeki.Build.BuildProcess.BuildWithArgs -buildSetupRelPath %BUILDSETUPPATH% -logFile

:: EXAMPLE
:: "C:\Program Files\Unity\Hub\Editor\2019.1.4f1\Editor\Unity.exe"
:: -quit -batchmode -nographics 
:: -projectPath "C:\Users\Attilio\Documents\GitHub\unity-build-tools"
:: -executeMethod Himeki.Build.BuildProcess.BuildWithArgs
:: -buildSetupRelPath "Assets/Editor/BuildTools/Setup/BuildSetup.asset"
:: -logFile