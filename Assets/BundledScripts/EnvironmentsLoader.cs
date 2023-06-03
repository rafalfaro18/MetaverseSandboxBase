using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

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
        public List<EnvironmentsReference> environmentBundles;
        // Start is called before the first frame update
        void Start()
        {
            if (environmentBundles == null) { return; }

            if (environmentBundles.Count == 0) { return; }

            var Testenv = environmentBundles[0].LoadAssetAsync(); // TODO: Get all bundles.
            Testenv.Completed += Testenv_Completed;

        }

        private void Testenv_Completed(AsyncOperationHandle<Environments> obj)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log(obj.Result.name); // Gets the Environments Bundles
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
