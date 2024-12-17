using HarmonyLib;
using Verse;

namespace Rimbound
{
    public class RimboundMod : Mod
    {
        public RimboundMod(ModContentPack content) : base(content)
        {
            Log.Message("[Rimbound - Novakin Xenotype] Welcome to the Wild Rim, Space Cowboy!");

            Harmony stellar = new Harmony("com.airo.rimbound");

            stellar.PatchAll();
        }
    }
}
