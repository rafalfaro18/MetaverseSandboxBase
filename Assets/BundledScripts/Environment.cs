using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MetaverseSandbox {
    [CreateAssetMenu(fileName = "Environment", menuName = "ScriptableObjects/EnvironmentScriptableObject", order = 1)]
    public class Environment : ScriptableObject
    {
        public AssetReferenceGameObject environmentAddressablePrefab;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}