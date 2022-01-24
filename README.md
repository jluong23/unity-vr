# unity-vr
Dissertation project

## Installation
As of 24/01/22, there are problems with conflicting Newtonsoft packages with Unity (12.0.0.0) and the installed version (13.0.0.1). The steps to fix this issue is as follows:

- Before opening the project, move Assets/Plugins/Newtonsoft.json.dll somewhere and delete its meta file. 
- Open the project. There will be errors as the google plugins require the Newtonsoft plugin.
- Add the Newtonsoft package back in and clear the console. 
- There will be further errors with conflicting versions of Newtonsoft. Delete the .dll for the unity version.
- Run the project as normal with the new warnings with the Oculus Quest plugged in.

Note: The project can now be ran as many times as needed for the session, but closing the unity editor will require this process to be repeated sometimes. 