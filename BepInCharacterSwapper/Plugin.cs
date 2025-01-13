using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;

namespace BepInCharacterSwapper;

[BepInPlugin("BepInCharacterSwapper", "BepInCharacterSwapper", "1.0.0")]
internal class Plugin : BasePlugin
{
    internal static new ManualLogSource Log;
    private Harmony harmony;

    public override void Load()
    {
        Log = base.Log;

        harmony = new Harmony("BepInCharacterSwapper");
        harmony.PatchAll();

        AddComponent<Hook>();
    }
}