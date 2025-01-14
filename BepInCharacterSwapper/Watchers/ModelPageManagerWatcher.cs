using UnityEngine;

namespace BepInCharacterSwapper.Watchers
{
    internal class ModelPageManagerWatcher : MonoBehaviour
    {
        public static ModelPageManager? Instance;

        private void OnEnable()
        {
            if (!Instance)
                Instance = FindObjectOfType<ModelPageManager>();
        }

        private void Update()
        {
            if(!Instance)
                Instance = FindObjectOfType<ModelPageManager>();
        }

    }
}
