using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.AsyncOperations;
using MetaverseSandbox.Core;

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

                var handle = obj.Result.environmentAddressablePrefab.InstantiateAsync(envParent);
                handle.Completed += EnvLoad_Completed;

                // Done instantiating.
            }
        }

        private void EnvLoad_Completed(AsyncOperationHandle<GameObject> obj)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded && obj.Result == null) {
                Debug.Log(obj.Result.name);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.T)) {
                foreach (Transform child in envParent)
                {
                    Addressables.Release(child.gameObject);
                    GameObject.Destroy(child.gameObject);
                }

                if (environmentBundles == null) { return; }

                if (environmentBundles.Count == 0) { return; }

                var Testenv = environmentBundles[1].LoadAssetAsync(); // This one is remote.
                Testenv.Completed += Testenv_Completed;
            }

            if (Input.GetKeyDown(KeyCode.Y))
            {
                foreach (Transform child in envParent)
                {
                    Addressables.Release(child.gameObject);
                    GameObject.Destroy(child.gameObject);
                };

                if (environmentBundles == null) { return; }

                if (environmentBundles.Count == 0) { return; }

                var Testenv = environmentBundles[0].LoadAssetAsync(); // This one is remote.
                Testenv.Completed += Testenv_Completed;
            }
        }
    }
}
