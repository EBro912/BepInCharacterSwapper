using BepInCharacterSwapper.Watchers;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace BepInCharacterSwapper.Helpers
{
    internal class ButtonHelper
    {
        private static readonly string PATH = BepInEx.Paths.GameRootPath + "/vrm";
        private static Dictionary<string, Button> buttonKeypair = new Dictionary<string, Button>();

        public static void RemoveButton(string key)
        {
            GameObject.Destroy(buttonKeypair[key].gameObject);
            buttonKeypair.Remove(key);
        }

        public static void UpdateModelList(ref List<string> files)
        {
            if (ModelPageManagerWatcher.Instance == null)
            {
                Plugin.Log.LogError("ModelPageManagerWatcher failed to get ModelPageManager Instance");
                return;
            }

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

                    if (buttonKeypair.ContainsKey(path))
                        continue;

                    GameObject modButton = DefaultControls.CreateButton(new DefaultControls.Resources());

                    if (!File.Exists(path))
                    {
                        toRemove.Add(path);
                        RemoveButton(path);
                        continue;
                    }

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
                    modButton.transform.SetParent(ModelPageManagerWatcher.Instance.mikuButton.transform.parent.transform, false);
                    Plugin.Log.LogWarning("Loaded VRM " + file);
                    offset -= 0.32f;
                }

                foreach (string s in toRemove)
                {
                    files.Remove(s);
                    RemoveButton(s);
                }
            }
        }
    }
}
