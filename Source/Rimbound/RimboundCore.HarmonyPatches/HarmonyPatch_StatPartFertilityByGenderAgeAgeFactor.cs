using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace RimboundCore.HarmonyPatches
{
    [HarmonyPatch(typeof(StatPart_FertilityByGenderAge), "AgeFactor")]
    public static class HarmonyPatch_StatPartFertilityByGenderAgeAgeFactor
    {
        [HarmonyPostfix]
        public static float HarmonyPatchPostfix_FertilityByGenderAgeAgeFactor(float __result, Pawn pawn)
        {
            if (pawn != null && pawn.Spawned && pawn.RaceProps.Humanlike && !pawn.genes.GenesListForReading.NullOrEmpty())
            {
                List<Gene> currentGenes = pawn.genes.GenesListForReading;
                    
                foreach (Gene gene in currentGenes)
                {
                    GeneFertilityByAgeExtension modExtensions = gene.def.GetModExtension<GeneFertilityByAgeExtension>();

                    if (modExtensions != null)
                    {
                        if (modExtensions.maleFertilityAgeFactor != null && pawn.gender == Gender.Male)
                        {
                            return modExtensions.maleFertilityAgeFactor.Evaluate(pawn.ageTracker.AgeBiologicalYearsFloat);
                        }
                        else if (modExtensions.femaleFertilityAgeFactor != null && pawn.gender == Gender.Female)
                        {
                            return modExtensions.femaleFertilityAgeFactor.Evaluate(pawn.ageTracker.AgeBiologicalYearsFloat);
                        }
                        else if (modExtensions.fertilityAgeFactor != null)
                        {
                            return modExtensions.fertilityAgeFactor.Evaluate(pawn.ageTracker.AgeBiologicalYearsFloat);
                        }
                    }
                }
            }
            return __result;
        }
    }
}
