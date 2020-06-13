# unity-build-tools
Tools to automate and streamline Unity builds

## Enhanced Build Tools

The enhanced build tool lets you setup multiple builds at once, save them for later use, and batch them.

Normally, for each build you'd need to switch platform, change the scripting define symbols manually, wait for compilation, and repeat. For a medium-large project that supports multiple platforms and stores, such as Oath to the Stars, that means a long time wasted waiting for Unity to do its work.

Using this tool, all the setup work will be done just once, and the necessary builds will be generated just by pressing a button, with no further input required.

<div style="float: right">
<img src="https://user-images.githubusercontent.com/4165016/77833524-52d7b200-713e-11ea-930c-073ccda68001.png" width="50%" max-width:400px>
</div>

## Advanced Features

### Addressables
The tool includes support to rebuild addressables, per-build. Since Unity doesn't provide a reliable way to detect if the Addressables package is installed in your editor, in order to show this option this you'll need to add ADDRESSABLES to your Scripting Define Symbols. Then, you can enable it in the Advanced Options of each build.

### Command-Line

The tool includes Shell and Bash scripts to create builds via CLI, allowing easy integration to Jenkins, Travis and other CI/CD build systems. You can take a look in the CommandLine folder to get started.
There are scripts to install your preferred Unity version in the build server, run your batched builds, and optionally create UnityPackages if you're working on an extension or library rather than an executable.

## Usage and license
While I'm mostly using this for internal development, you're more than welcome to use this in your Unity project, following the conditions and limitations specified in the LICENSE. 
Also, while not necessary, please make sure to credit me and send your feedback.
