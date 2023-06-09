using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.AsyncOperations;
using MetaverseSandbox.Core;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets.ResourceLocators;

namespace MetaverseSandbox {

    [Serializable]
    public class EnvironmentsReference : AssetReferenceT<Environments>
    {
        public EnvironmentsReference(string guid) : base(guid)
        {
        }
    }
    public class EnvironmentsLoader : MonoBehaviour
    {
        [SerializeField]
        private Transform envParent;
        public List<EnvironmentsReference> environmentBundles;
        private Scene loadedScene;
        private bool isSceneLoaded = false;
        // Start is called before the first frame update
        void Start()
        {
            if (environmentBundles == null) { return; }

            if (environmentBundles.Count == 0) { return; }

            var Testenv = Addressables.LoadAssetAsync<Environments>(environmentBundles[0]); // TODO: Get all bundles.
            Testenv.Completed += DownloadEnvsList_Completed;

        }

        private void DownloadEnvsList_Completed(AsyncOperationHandle<MetaverseSandbox.Core.Environments> obj)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log("- " + obj.Result.name); // Gets the Environments Bundles

                // Load first environment of the first bundle

                var handle = Addressables.LoadAssetAsync<MetaverseSandbox.Core.Environment>(obj.Result.environments[0]);
                handle.Completed += DownloadEnvRef_Completed;
                // Done loading Env.
            }
        }

        private void DownloadEnvRef_Completed(AsyncOperationHandle<MetaverseSandbox.Core.Environment> obj)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log("-- " + obj.Result.name);

                // Instantiate Environment Prefab

                if (!isSceneLoaded)
                {
                    var handle = Addressables.LoadSceneAsync(obj.Result.environmentAddressableScene, LoadSceneMode.Additive);
                    handle.Completed += DownloadSceneRef_Completed;
                }
                else {
                    Debug.Log("Scene already loaded. Unload the loaded scene first.");
                }

                // Done instantiating.
            }
        }

        private void DownloadSceneRef_Completed(AsyncOperationHandle<SceneInstance> obj)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                loadedScene = obj.Result.Scene;
                Debug.Log("--- " + loadedScene.name);
                isSceneLoaded = true;
            }
        }

        // Update is called once per frame
        void Update()
        {
           
        }

        [QFSW.QC.Command]
        public void UnloadCurrentScene() {
            var handle = SceneManager.UnloadSceneAsync(loadedScene);
            handle.completed += SceneUnloaded;
        }

        private void SceneUnloaded(AsyncOperation obj)
        {
            Debug.Log(" loadedScene unloaded");
            isSceneLoaded = false;
        }

        [QFSW.QC.Command]
        public void DownloadSceneAt(int i)
        {
            if (environmentBundles == null) { return; }

            if (environmentBundles.Count == 0) { return; }

            var Testenv = Addressables.LoadAssetAsync<MetaverseSandbox.Core.Environments>(environmentBundles[i]);
            Testenv.Completed += DownloadEnvsList_Completed;
        }

        [QFSW.QC.Command]
        public void DownloadRemoteCatalog(string rootFolderUrl, string catalogFileName) {
            string platformName = "StandaloneWindows64";
#if UNITY_ANDROID
            platformName = "Android";
#endif
            if (rootFolderUrl.EndsWith("/")) {
                rootFolderUrl = rootFolderUrl.Substring(0, rootFolderUrl.Length - 1);
            }

            if (catalogFileName.EndsWith("/"))
            {
                catalogFileName = catalogFileName.Substring(0, catalogFileName.Length - 1);
            }

            string catalogUrl = rootFolderUrl + "/" + platformName + "/" + catalogFileName;

            Debug.Log("Downloading " + catalogUrl );
            Addressables.LoadContentCatalogAsync(catalogUrl).Completed += CatalogDownloader_Completed;
        }

        private void CatalogDownloader_Completed(AsyncOperationHandle<IResourceLocator> obj)
        {
            if(obj.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log("Downloaded");
            }
        }

        [QFSW.QC.Command]
        public void TestDownloadSpecificEnvironmentScene(string environmentAddressableUrl)
        { // Test with: Assets/DLC/DLCEnvironmentsPack2/Spheres/Spheres.asset
            var envHandle = Addressables.LoadAssetAsync<MetaverseSandbox.Core.Environment>(environmentAddressableUrl);
            envHandle.Completed += EnvHandleDirectDownload_Completed;
        }

        
        private void EnvHandleDirectDownload_Completed(AsyncOperationHandle<Core.Environment> obj)
        {
            if(obj.Status == AsyncOperationStatus.Succeeded)
            {
                string scenePath = obj.Result.environmentAddressableScene;

                Addressables.LoadSceneAsync(scenePath, LoadSceneMode.Additive).Completed += DownloadSceneRef_Completed;
            }
        }
    }
}
