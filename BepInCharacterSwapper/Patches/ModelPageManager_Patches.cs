using HarmonyLib;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace BepInCharacterSwapper.Patches
{
    [HarmonyPatch(typeof(ModelPageManager), "Start")]
    internal class ModelPageManager_Patches
    {
        static readonly string PATH = BepInEx.Paths.GameRootPath + "/vrm";

        private static void Postfix(ModelPageManager __instance)
        {
            if (!Directory.Exists(PATH))
            {
                Directory.CreateDirectory(PATH);
                Plugin.Log.LogWarning("vrm folder not found, creating a new one at " + PATH);
            }
            float offset = 0.32f;
            foreach (string path in Directory.GetFiles(PATH).Where(x => x.EndsWith(".vrm")))
            {
                string file = Path.GetFileName(path);
                string name = file.Split('.')[0];
                GameObject modButton = DefaultControls.CreateButton(new DefaultControls.Resources());
                modButton.transform.position = new Vector3(0.83f, offset, -1f);
                modButton.transform.localScale = new Vector3(1.2f, 1.2f, 1f);
                modButton.name = name + "_Button";
                modButton.GetComponentInChildren<Text>().text = name;
                System.Action action = () =>
                {
                    if (Hook.Instance == null)
                    {
                        Plugin.Log.LogError("Hook is not loaded, could not load character.");
                        return;
                    }
                    Hook.Instance.LoadCharacter(path);
                };
                modButton.GetComponent<Button>().onClick.AddListener(action);
                modButton.transform.SetParent(__instance.mikuButton.transform.parent.transform, false);
                Plugin.Log.LogWarning("Loaded VRM " + file);
                offset -= 0.32f;
            }
        }
    }
}
