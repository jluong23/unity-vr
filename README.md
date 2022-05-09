# unity-vr
Accessing External Image Assets within a Virtual Reality System

UG Dissertation Project at the University of Sheffield  

Author: James Luong  

Supervisor: Dr Steve Maddock

## Description

This project is a VR interface which interacts with a user's Google Photos library via the Photos Library API in the Google Cloud Platform. Therefore, it requires an internet connection to run.

Developed with Unity using the XR Interaction Toolkit 2.0.0-pre.5. 

Also uses the following NuGet packages:

| NuGet Package                                   | Version |
|-------------------------------------------------|---------|
| Google.Apis, Google.Apis.Core, Google.Apis.Auth | 1.55.0  |
| Newtonsoft.Json                                 | 13.0.1  |

## Installation

A test set was constructed from the researcher's personal photos and used in the dissertation report. This test set is unavailable to use so you will need to use your own Google Photos Library.

1. Ensure the unity version is 2020.3.29f1.
2. Create a Project on Google Cloud Platform https://console.developers.google.com/, enabling the Photos Library API. 
3. Under the credentials tab, create an Oauth 2.0 Client ID. Ensure the type is Desktop Application.
4. In the OAuth consent screen tab, add your email which will be used for Photos Library API as a test user (and subsequent emails to test) .
5. Download the credential json file of the created Oauth 2.0 ID, copying into the ```Assets/Resources``` directory with file name ```credentials.json```. 
6. Ensure the Oculus Quest is plugged in to your computer with Oculus Link enabled.
7. Run the application in the Unity Editor and wear the Oculus Quest headset 

I have not got the application desktop export working with using Google Cloud methods and XR Interaction Toolkit... Let me know of any other methods of running the system without the Unity Editor.
