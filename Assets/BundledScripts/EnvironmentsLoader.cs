using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.AsyncOperations;
using MetaverseSandbox.Core;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

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
        // Start is called before the first frame update
        void Start()
        {
            if (environmentBundles == null) { return; }

            if (environmentBundles.Count == 0) { return; }

            var Testenv = environmentBundles[0].LoadAssetAsync(); // TODO: Get all bundles.
            Testenv.Completed += Testenv_Completed;

        }

        private void Testenv_Completed(AsyncOperationHandle<MetaverseSandbox.Core.Environments> obj)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log(obj.Result.name); // Gets the Environments Bundles

                // Load first environment of the first bundle

                var handle = obj.Result.environments[0].LoadAssetAsync();
                handle.Completed += Handle_Completed;
                // Done loading Env.
            }
        }

        private void Handle_Completed(AsyncOperationHandle<MetaverseSandbox.Core.Environment> obj)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log(obj.Result.name);

                // Instantiate Environment Prefab

                var handle = Addressables.LoadSceneAsync(obj.Result.environmentAddressableScene, LoadSceneMode.Additive);
                handle.Completed += EnvLoad_Completed;

                // Done instantiating.
            }
        }

        private void EnvLoad_Completed(AsyncOperationHandle<SceneInstance> obj)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                loadedScene = obj.Result.Scene;
                Debug.Log(loadedScene);
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
        }

        [QFSW.QC.Command]
        public void LoadSceneAt(int i)
        {
            if (environmentBundles == null) { return; }

            if (environmentBundles.Count == 0) { return; }

            var Testenv = environmentBundles[i].LoadAssetAsync();
            Testenv.Completed += Testenv_Completed;
        }

    }
}
