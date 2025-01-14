using CharacterSwapper.Patches;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;

namespace CharacterSwapper;

[BepInPlugin("CharacterSwapper", "CharacterSwapper", "1.0.0")]
internal class Plugin : BasePlugin
{
    internal static new ManualLogSource Log;
    private Harmony harmony;

    public static ConfigEntry<string>? loadedCharacter;

    public override void Load()
    {
        Log = base.Log;

        loadedCharacter = Config.Bind<string>(new ConfigDefinition("Character", "Loaded Character"), "Default");

        harmony = new Harmony("CharacterSwapper");
        harmony.PatchAll();

        AddComponent<Hook>();
    }
}