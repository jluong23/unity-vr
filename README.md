# unity-vr
Dissertation project

## Installation
As of 24/01/22, there are problems with conflicting Newtonsoft packages with Unity (12.0.0.0) and the installed version (13.0.0.1). The steps to fix this issue is as follows:

1. Before opening the project, move Assets/Plugins/Newtonsoft.json.dll somewhere and delete its meta file. 
    - This is to ensure the project opens without errors. Ignoring the errors and opening without safe mode can cause game assets to disappear?
2. Open the project as normal. There will be errors as the google plugins require the Newtonsoft plugin.
3. Add the Newtonsoft package back in and clear the console. 
4. There will be further errors with conflicting versions of Newtonsoft. Delete the .dll for the unity version.
5. Run the project as normal with the new warnings with the Oculus Quest plugged in.

Note: The project can now be ran as many times as needed for the session, but closing the unity editor will require this process to be repeated sometimes. 