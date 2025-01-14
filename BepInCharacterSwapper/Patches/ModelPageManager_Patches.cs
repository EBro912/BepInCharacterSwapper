using BepInEx.Configuration;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace BepInCharacterSwapper.Patches
{
    [HarmonyPatch(typeof(ModelPageManager), nameof(ModelPageManager.Start))]
    internal class ModelPageManager_Patches
    {
        static readonly string PATH = BepInEx.Paths.GameRootPath + "/vrm";

        static List<string> files = new List<string>();
        private static List<string> created = new List<string>();

        private static ModelPageManager? instance;

        private static Dictionary<string, Button> buttonKeypair = new Dictionary<string, Button>();

        public static void tryGetNewModels()
        {
            if (!Directory.Exists(PATH))
            {
                Directory.CreateDirectory(PATH);
                Plugin.Log.LogWarning("vrm folder not found, creating a new one at " + PATH);
            }

            lock (files)
            {
                List<string> remove = new List<string>();

                foreach (string model in files)
                    if (!File.Exists(model))
                        remove.Add(model);

                foreach (var item in remove)
                {
                    files.Remove(item);
                    GameObject.Destroy(buttonKeypair[item].gameObject);
                }

                foreach (string vrmModel in Directory.GetFiles(PATH).Where(x => x.EndsWith(".vrm")))
                {
                    if(!files.Contains(vrmModel))
                        files.Add(vrmModel);
                }
            }

            UpdateModelList();
        }

        private static void Postfix(ModelPageManager __instance)
        {
            instance = __instance;
        }

        private static void UpdateModelList()
        {
            if (instance == null)
                return;

            if (!Directory.Exists(PATH))
            {
                Directory.CreateDirectory(PATH);
                Plugin.Log.LogWarning("vrm folder not found, creating a new one at " + PATH);
            }
            float offset = 0.32f;
            lock (files)
            {
                List<string> toRemove = new List<string>();

                foreach (string path in files)
                {
                    string file = Path.GetFileName(path);
                    string name = file.Split('.')[0];

                    if (created.Contains(path))
                        continue;

                    GameObject modButton = DefaultControls.CreateButton(new DefaultControls.Resources());

                    if (!File.Exists(path))
                    {
                        toRemove.Add(path);
                        GameObject.DestroyImmediate(modButton);
                        continue;
                    }

                    created.Add(path);
                    buttonKeypair.Add(path, modButton.GetComponent<Button>());

                    modButton.transform.position = new Vector3(0.83f, offset, -1f);
                    modButton.transform.localScale = new Vector3(1.5f, 1.2f, 1f);
                    modButton.name = name + "_Button";
                    modButton.GetComponentInChildren<Text>().text = name;
                    System.Action action = () =>
                    {
                        if (Hook.Instance == null)
                        {
                            Plugin.Log.LogError("Hook is not loaded, could not load character.");
                            return;
                        }

                        if (Plugin.loadedCharacter != null)
                        {
                            Plugin.loadedCharacter.Value = path;
                            Hook.Instance.LoadCharacter(path);
                        }
                    };
                    modButton.GetComponent<Button>().onClick.AddListener(action);
                    modButton.transform.SetParent(instance.mikuButton.transform.parent.transform, false);
                    Plugin.Log.LogWarning("Loaded VRM " + file);
                    offset -= 0.32f;
                }

                foreach(string s in toRemove)
                {
                    files.Remove(s);
                    created.Remove(s);
                    GameObject.Destroy(buttonKeypair[s].gameObject);
                }
            }
        }
    }
}
