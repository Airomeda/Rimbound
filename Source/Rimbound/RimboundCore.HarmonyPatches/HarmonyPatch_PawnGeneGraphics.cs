using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;
using Verse;

namespace RimboundCore.HarmonyPatches
{
    [HarmonyPatch(typeof(Gene), "PostAdd")]
    public static class HarmonyPatch_GenePostAdd
    {
        [HarmonyPostfix]
        public static void HarmonyPatchPostfix_GenePostAdd(Gene __instance)
        {
            if (PawnGenerator.IsBeingGenerated(__instance.pawn) is false && __instance.Active)
            {
                ApplyGeneEffect(__instance);
            }
        }

        public static void ApplyGeneEffect(Gene gene)
        {
            GeneHairSkinColorExtension modExtension = gene.def.GetModExtension<GeneHairSkinColorExtension>();
            if (gene.Active)
            {
                if (modExtension != null)
                {
                    if (modExtension.disableSkinShader == true)
                    {
                        gene.pawn.story.skinColorOverride = gene.pawn.story.SkinColorBase;
                    }
                    if (modExtension.hairUsesSkinColor == true)
                    {
                        gene.pawn.story.HairColor = (Color)gene.pawn.story.skinColorOverride;
                    }
                    gene.pawn.Drawer.renderer.SetAllGraphicsDirty();
                }
            }
        }
    }

    [HarmonyPatch(typeof(PawnGenerator), "GenerateGenes")]
    public static class HarmonyPatch_PawnGeneratorGenerateGenes
    {
        [HarmonyPostfix]
        public static void HarmonyPatchPostfix_PawnGeneratorGenerateGenes(Pawn pawn)
        {
            if (pawn.genes != null)
            {
                List<Gene> genes = pawn.genes.GenesListForReading;
                foreach (Gene gene in genes)
                {
                    if (gene.Active)
                    {
                        HarmonyPatch_GenePostAdd.ApplyGeneEffect(gene);
                    }
                }
            }
        }
    }

}
