using UniGLTF;
using UnityEngine;
using UniVRM10;

namespace BepInCharacterSwapper
{
    internal class VRMLoader
    {
        public GameObject? LoadVRM(string path)
        {
            GltfData data = new GlbFileParser(path).Parse();
            Vrm10Data? vrm = Vrm10Data.Parse(data);
            if (vrm == null)
            {
                vrm = Migrate(data);
                if (vrm == null)
                {
                    return null;
                }
                Plugin.Log.LogWarning("VRM migrated! Continuing...");            
            }

            RuntimeGltfInstance loaded = new Vrm10Importer(vrm).Load();
            loaded.EnableUpdateWhenOffscreen();
            loaded.ShowMeshes();
            loaded.gameObject.name = "CustomCharacter";

            return loaded.gameObject;
        }

        private Vrm10Data? Migrate(GltfData data)
        {
            Vrm10Data.Migrate(data, out Vrm10Data newData, out _);
            if (newData == null)
            {
                Plugin.Log.LogError("Failed to migrate VRM.");
            }
            return newData;
        }
    }
}
