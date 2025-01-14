using System.IO;
using UnityEngine;
using BepInCharacterSwapper.Helpers;

namespace BepInCharacterSwapper
{
    internal class Hook : MonoBehaviour
    {
        public static Hook? Instance;
        private VRMLoader? VRMLoader;

        private bool _init = false; // autoloader hook

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            VRMLoader = new();
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Plugin.Log.LogWarning("CharacterSwapper Hook loaded!");
        }

        private void Update()
        {
            ModelHelper.TryGetNewModels(); // update model list each frame

            if (!_init && GameObject.Find("/CharactersRoot").transform.GetChild(0) != null) // debouncer for the model autoloader
            {
                _init = true;
                if (Plugin.loadedCharacter != null)
                {
                    Plugin.Log.Log(BepInEx.Logging.LogLevel.Message, $"Loading vrm: {Plugin.loadedCharacter.Value}");

                    if (File.Exists(Plugin.loadedCharacter.Value))
                        LoadCharacter(Plugin.loadedCharacter.Value);
                }
            }
        }

        public void LoadCharacter(string path)
        {
            if (!File.Exists(path))
            {
                Plugin.Log.LogError("VRM no longer exists at path: " + path);
                return;
            }

            GameObject root = GameObject.Find("/CharactersRoot");
            GameObject chara = root.transform.GetChild(0).gameObject;
            CharaData oldData = chara.GetComponent<CharaData>();
            RuntimeAnimatorController oldAnimator = chara.GetComponent<Animator>().runtimeAnimatorController;
            Destroy(chara);

            GameObject? newChara = VRMLoader.LoadVRM(path);
            if (newChara == null)
            {
                Plugin.Log.LogError("Failed to load VRM: " + path);
                return;
            }

            newChara.transform.parent = root.transform;
            CharaData newData = newChara.AddComponent<CharaData>();
            CopyCharaData(oldData, newData);

            GameObject.Find("MainManager").GetComponent<MainManager>().charaData = newData;

            Animator animator = newChara.GetComponent<Animator>();
            animator.applyRootMotion = true;
            animator.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
            animator.runtimeAnimatorController = oldAnimator;
            Plugin.Log.LogWarning("Character loaded sucessfully from " + path);
        }

        private void CopyCharaData(CharaData source, CharaData target)
        {
            target.alarmAnim = source.alarmAnim;
            target.draggedAnims = source.draggedAnims;
            target.hideLeftAnims = source.hideLeftAnims;
            target.hideRightAnims = source.hideRightAnims;
            target.jumpInAnim = source.jumpInAnim;
            target.jumpOutAnim = source.jumpOutAnim;
            target.pickedSittingAnim = source.pickedSittingAnim;
            target.pickedStandingAnim = source.pickedStandingAnim;
            target.sittingOneShotAnims = source.sittingOneShotAnims;
            target.sittingRandomAnims = source.sittingRandomAnims;
            target.standingOneShotAnims = source.standingOneShotAnims;
            target.standingRandomAnims = source.standingRandomAnims;
            target.strokedSittingAnim = source.strokedSittingAnim;
            target.strokedStandingAnim = source.strokedStandingAnim;
        }
    }
}
