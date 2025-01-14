using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BepInCharacterSwapper.Helpers
{
    internal static class ModelHelper
    {
        private static readonly string PATH = BepInEx.Paths.GameRootPath + "/vrm";
        public static List<string> files = new List<string>(); // files to be displayed
       
        public static void TryGetNewModels()
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
                    ButtonHelper.RemoveButton(item);
                }

                foreach (string vrmModel in Directory.GetFiles(PATH).Where(x => x.EndsWith(".vrm")))
                {
                    if (!files.Contains(vrmModel))
                        files.Add(vrmModel);
                }

                ButtonHelper.UpdateModelList(ref files);
            }
        }

    }
}
