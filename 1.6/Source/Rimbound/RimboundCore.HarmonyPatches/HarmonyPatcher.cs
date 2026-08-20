using HarmonyLib;
using RimWorld;
using System;
using Verse;

namespace RimboundCore.HarmonyPatches
{
    [StaticConstructorOnStartup]
    public static class HarmonyPatcher
    {
        static HarmonyPatcher()
        {
            var stellar = new Harmony("com.airo.rimbound");

            // HarmonyPatch_HeridityPatches
            stellar.Patch(
                AccessTools.Method(typeof(PregnancyUtility), "GetInheritedGenes", new Type[]
                {
                    typeof(Pawn),
                    typeof(Pawn),
                    typeof(bool).MakeByRefType()
                }), postfix: new HarmonyMethod(typeof(HarmonyPatch_HeridityPatches), nameof(HarmonyPatch_HeridityPatches.HarmonyPatchPostfix_PregnancyUtilityGetInheritedGenes))
            );
            stellar.Patch(AccessTools.Method(typeof(PregnancyUtility), "TryGetInheritedXenotype"),
                postfix: new HarmonyMethod(typeof(HarmonyPatch_HeridityPatches), nameof(HarmonyPatch_HeridityPatches.HarmonyPatchPosfix_PregnancyUtilityTryGetInheritedXenotype))
            );
            stellar.Patch(AccessTools.Method(typeof(PregnancyUtility), "ShouldByHybrid"),
                postfix: new HarmonyMethod(typeof(HarmonyPatch_HeridityPatches), nameof(HarmonyPatch_HeridityPatches.HarmonyPatchPosfix_PregnancyUtilityShouldByHybrid))
            );

            stellar.Patch(
                AccessTools.Method(typeof(HeadTypeDef), "GetGraphic"), postfix: new HarmonyMethod(typeof(HarmonyPatch_HeadTypeDefGetGraphic), nameof(HarmonyPatch_HeadTypeDefGetGraphic.HarmonyPatchPostfix_HeadTypeDefGetGraphic))
            );

            // HarmonyPatch_PawnGeneGraphics
            stellar.Patch(
                AccessTools.Method(typeof(Gene), "PostAdd"), postfix: new HarmonyMethod(typeof(HarmonyPatch_GenePostAdd), nameof(HarmonyPatch_GenePostAdd.HarmonyPatchPostfix_GenePostAdd))
            );
            stellar.Patch(
                AccessTools.Method(typeof(PawnGenerator), "GenerateGenes"), postfix: new HarmonyMethod(typeof(HarmonyPatch_PawnGeneratorGenerateGenes), nameof(HarmonyPatch_PawnGeneratorGenerateGenes.HarmonyPatchPostfix_PawnGeneratorGenerateGenes))
            );

            // HarmonyPatch_StatPartFertilityByGenderAge
            stellar.Patch(
                AccessTools.Method(typeof(StatPart_FertilityByGenderAge), "AgeFactor"), postfix: new HarmonyMethod(typeof(HarmonyPatch_StatPartFertilityByGenderAgeAgeFactor), nameof(HarmonyPatch_StatPartFertilityByGenderAgeAgeFactor.HarmonyPatchPostfix_FertilityByGenderAgeAgeFactor))
            );

            // HarmonyPatch_ThoughtMemoryDuration
            stellar.Patch(
                AccessTools.PropertyGetter(typeof(Thought), "DurationTicks"), postfix: new HarmonyMethod(typeof(HarmonyPatch_ThoughtDuration), nameof(HarmonyPatch_ThoughtDuration.HarmonyPatchPostfix_ThoughtDuration))
            );
            stellar.Patch(
                AccessTools.PropertyGetter(typeof(Thought_Memory), "DurationTicks"), postfix: new HarmonyMethod(typeof(HarmonyPatch_ThoughtDuration), nameof(HarmonyPatch_ThoughtDuration.HarmonyPatchPostfix_ThoughtDuration))
            );

            Log.Message("[Rimbound - Core] Constellation formed, commencing harmony patches...");
        }
    }
}
