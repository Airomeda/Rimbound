using HarmonyLib;
using RimWorld;

namespace RimboundCore.HarmonyPatches
{
    [HarmonyPatch(typeof(Thought), "DurationTicks", MethodType.Getter)]
    public static class HarmonyPatch_ThoughtDuration
    {
        [HarmonyPostfix]
        public static int HarmonyPatchPostfix_ThoughtDuration(int __result, Thought __instance)
        {
            return (int)(__result * GeneMemory.GetThoughtDurationMultiplierForPawn(__instance.pawn));
        }
    }

    [HarmonyPatch(typeof(Thought_Memory), "DurationTicks", MethodType.Getter)]
    public static class HarmonyPatch_ThoughtMemoryDuration
    {
        [HarmonyPostfix]
        public static int HarmonyPatchPostfix_ThoughtMemoryDuration(int __result, Thought __instance)
        {
            return (int)(__result * GeneMemory.GetThoughtDurationMultiplierForPawn(__instance.pawn));
        }
    }
}
