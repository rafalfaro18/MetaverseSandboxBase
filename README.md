# MetaverseSandboxBase

Base Unity Project for MetaverseSandbox. "Base Game" without DLCs.

## Setup Steps

1. Grab a legal copy of Quantum Console from the Unity Asset Store and import it to the project.
2. Build the project's Asset Bundles.
3. Using NPM (which comes with NODEJS) in a terminal window navigate to ``ServerData`` folder an run:
``sh
npx http-server
``
This command will start a web server in port 8080 in your local machine.
4. Start the game. The game build will automatically load the Lobby adressable scene which comes bundled with the base game. Press ``Esc`` to load the QUantum Console.
5. In the Quantum Console type: ``UnloadCurrentScene`` and hit enter.
6. In the Quantum Console type: ``DownloadRemoteCatalog http://127.0.0.1:8080 catalog_2023.06.05.01.56.56.json`` and hit enter (replace catalog_2023.06.05.01.56.56.json for the name of the catalog .json file you have in your ``ServerData/[NameOfYourPlatform]/`` folder).
7. In the Quantum Console type: ``DownloadSceneAt 1`` and hit enter. This will downloadt and load a scene from the web server for demo purposes.
8. To run this whole demo again go to ``C:\Users\YourUser\AppData\LocalLow\Rafalfaro\MetaverseSandboxBase`` and delete everything to purge the downloaded catalog.

## Dependencies

- com.unity.addressables 1.19.19
- Unity 2021.3.25f1
- Quantum Console
- com.rafalfaro.metaversesandbox 1.0.3

## Creating Remote Catalogs
1. You only need same versions of: com.rafalfaro.metaversesandbox , com.unity.addressables and Unity.
2. After installing same versions of everything create a folder for your environments bundle and mark it as addressable this will make everythig inside also addressable.
3. Inside the bundle folder create an EnvironmentsScriptableObject and all of the Environments you'll create in step 5.
4. You need to create a folder per environment.
5. Inside of each environment folder create at minimum this: A Scene and an EnvironmentScriptableObject (in this one paste the Addressable path of the Environment scene into the field called ``Environment Addessable Scene`` ).
6. After creating all of your environments build your addressables catalog and host it somewhere in the web.
7. Run the base game (you don't have to recompile) and do the Set Up Steps 4-6 again but replace `` http://127.0.0.1:8080`` for your web hosting address where you previously uploaded the contents of your new Unity Project's Addressables bake/build result (folder ``ServerData``) and the name of the catalo.json file for yours.
8. In the quantum Console (press Esc) you should see log messages that you downloaded succesfully the new catalog.

## To Do
- Refactor to allow any scene url of any bundle. Right now the app only loads 2 possible scenes for demo purposes from an Array of only 2 items [0-1].