using HarmonyLib;
using Verse;

namespace Rimbound
{
    public class RimboundMod : Mod
    {
        public RimboundMod(ModContentPack content) : base(content)
        {
            Log.Message("[Rimbound - Core] The stars had aligned, commencing assembly...");

            Harmony stellar = new Harmony("com.airo.rimbound");

            stellar.PatchAll();
        }
    }
}
