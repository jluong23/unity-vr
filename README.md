# unity-vr
Accessing External Image Assets within a Virtual Reality System

UG Dissertation Project at the University of Sheffield  

Author: James Luong  

Supervisor: Dr Steve Maddock

## Installation
As of 09/05/22:

1. Ensure the unity version is 2020.3.29f1.
2. Create a Project on Google Cloud Platform https://console.developers.google.com/, enabling the Photos Library API. 
3. Under the credentials tab, create an Oauth 2.0 Client ID. Ensure the type is Desktop Application.
4. In the OAuth consent screen tab, add your email which will be used for Photos Library API as a test user (and subsequent emails to test) .
5. Download the credential json file of the created Oauth 2.0 ID, copying into the ```Assets/Resources``` directory with file name ```credentials.json```. 
6. Run the application in the Unity Editor (haven't got the Google Cloud and XR Interaction Toolkit to work when exporting the project on Desktop).

