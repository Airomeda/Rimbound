using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

namespace RimboundCore.HarmonyPatches
{
    [StaticConstructorOnStartup]
    public static class HarmonyPatch_HeridityPatches
    {
        private static readonly Type patchType;

        private static bool motherXenotype = true;
        private static bool fatherXenotype = true;

        static HarmonyPatch_HeridityPatches()
        {
            var harmony = new Harmony("com.airo.rimbound");

            patchType = typeof(HarmonyPatch_HeridityPatches);
            harmony.Patch(
                AccessTools.Method(typeof(PregnancyUtility), "GetInheritedGenes", new Type[]
                {
                    typeof(Pawn),
                    typeof(Pawn),
                    typeof(bool).MakeByRefType()
                }), postfix: new HarmonyMethod(patchType, nameof(HarmonyPatchPostfix_PregnancyUtilityGetInheritedGenes))
            );
            harmony.Patch(AccessTools.Method(typeof(PregnancyUtility), "TryGetInheritedXenotype"),
                postfix: new HarmonyMethod(patchType, nameof(HarmonyPatchPosfix_PregnancyUtilityTryGetInheritedXenotype))
            );
            harmony.Patch(AccessTools.Method(typeof(PregnancyUtility), "ShouldByHybrid"),
                postfix: new HarmonyMethod(patchType, nameof(HarmonyPatchPosfix_PregnancyUtilityShouldByHybrid))
            );
        }

        public static void HarmonyPatchPostfix_PregnancyUtilityGetInheritedGenes(Pawn mother, Pawn father, ref List<GeneDef> __result)
        {
            GeneInheritanceExtension extensionA = null;
            GeneInheritanceExtension extensionB = null;

            XenotypeDef xenotypeA = null;
            XenotypeDef xenotypeB = null;

            if (mother != null && mother.genes != null)
            {
                extensionA = CheckForModExtension(mother.genes.GenesListForReading);
                xenotypeA = mother.genes.Xenotype;
            }
            if (father != null && father.genes != null)
            {
                extensionB = CheckForModExtension(father.genes.GenesListForReading);
                xenotypeB = father.genes.Xenotype;
            }

            if (extensionA != null || extensionB != null)
            {
                bool parentA = extensionA != null && extensionA.passXenotypeGenes == true;
                bool parentB = extensionB != null && extensionB.passXenotypeGenes == true;
                bool sameXenotype = xenotypeA == xenotypeB;

                if (parentA && !parentB && !sameXenotype)
                {
                    List<GeneDef> list = new List<GeneDef>();
                    if (Rand.Chance(extensionA.xenotypeGenesChance))
                    {
                        foreach (GeneDef item in mother.genes.Xenotype.AllGenes)
                        {
                            list.Add(item);
                        }
                        motherXenotype = true;
                        fatherXenotype = false;
                    }
                    else
                    {
                        list = __result;
                        motherXenotype = false;
                        fatherXenotype = false;
                    }
                    __result = list;
                }
                else if (!parentA && parentB && !sameXenotype)
                {
                    List<GeneDef> list = new List<GeneDef>();
                    if (Rand.Chance(extensionB.xenotypeGenesChance))
                    {
                        foreach (GeneDef item in father.genes.Xenotype.AllGenes)
                        {
                            list.Add(item);
                        }
                        motherXenotype = false;
                        fatherXenotype = true;
                    }
                    else
                    {
                        list = __result;
                        motherXenotype = false;
                        fatherXenotype = false;
                    }
                    __result = list;
                }
                else if (parentA && parentB && !sameXenotype)
                {
                    List<GeneDef> list = new List<GeneDef>();
                    if (Rand.Chance(extensionA.xenotypeGenesChance + extensionB.xenotypeGenesChance))
                    {
                        if (Rand.Chance(0.5f))
                        {
                            foreach (GeneDef item in mother.genes.Xenotype.AllGenes)
                            {
                                list.Add(item);
                            }
                            motherXenotype = true;
                            fatherXenotype = false;
                        }
                        else
                        {
                            foreach (GeneDef item in father.genes.Xenotype.AllGenes)
                            {
                                list.Add(item);
                            }
                            motherXenotype = false;
                            fatherXenotype = true;
                        }
                    }
                    else
                    {
                        list = __result;
                        motherXenotype = false;
                        fatherXenotype = false;
                    }
                    __result = list;
                }
                else if (parentA && parentB && sameXenotype)
                {
                    List<GeneDef> list = new List<GeneDef>();
                    foreach (GeneDef item in mother.genes.Xenotype.AllGenes)
                    {
                        list.Add(item);
                    }
                    motherXenotype = true;
                    fatherXenotype = true;
                    __result = list;
                }
            }
        }

        public static void HarmonyPatchPosfix_PregnancyUtilityTryGetInheritedXenotype(Pawn mother, Pawn father, ref XenotypeDef xenotype, ref bool __result)
        {
            GeneInheritanceExtension extensionA = null;
            GeneInheritanceExtension extensionB = null;

            if (mother != null && mother.genes != null)
            {
                extensionA = CheckForModExtension(mother.genes.GenesListForReading);
            }
            if (father != null && father.genes != null)
            {
                extensionB = CheckForModExtension(father.genes.GenesListForReading);
            }

            if (extensionA != null || extensionB != null)
            {
                bool xenotypeMother = motherXenotype && extensionA != null;
                bool xenotypeFather = fatherXenotype && extensionB != null;

                if (xenotypeMother == true && xenotypeFather == false)
                {
                    xenotype = mother.genes.Xenotype;
                    __result = true;
                }
                if (xenotypeMother == false && xenotypeFather == true)
                {
                    xenotype = father.genes.Xenotype;
                    __result = true;
                }
                if (xenotypeMother == true && xenotypeFather == true)
                {
                    xenotype = mother.genes.Xenotype;
                    __result = true;
                }
            }
        }

        public static void HarmonyPatchPosfix_PregnancyUtilityShouldByHybrid(Pawn mother, Pawn father, ref bool __result)
        {
            GeneInheritanceExtension extensionA = null;
            GeneInheritanceExtension extensionB = null;

            if (mother != null && mother.genes != null)
            {
                extensionA = CheckForModExtension(mother.genes.GenesListForReading);
            }
            if (father != null && father.genes != null)
            {
                extensionB = CheckForModExtension(father.genes.GenesListForReading);
            }

            if (extensionA != null || extensionB != null)
            {
                bool xenotypeMother = motherXenotype && extensionA != null;
                bool xenotypeFather = fatherXenotype && extensionB != null;

                if (xenotypeMother == true || xenotypeFather == true)
                {
                    __result = false;
                }
            }
        }

        public static GeneInheritanceExtension CheckForModExtension(List<Gene> genes)
        {
            GeneInheritanceExtension extension = null;
            foreach (Gene gene in genes)
            {
                if (gene.Active)
                {
                    var extensions = gene.def.GetModExtension<GeneInheritanceExtension>();

                    if (extensions != null)
                    {
                        extension = extensions;
                    }
                }
            }
            return extension;
        }
    }
}