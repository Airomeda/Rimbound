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

        private static XenotypeDef inheritedXenotype = new XenotypeDef();

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
        }
        public static void HarmonyPatchPostfix_PregnancyUtilityGetInheritedGenes(Pawn mother, Pawn father, ref List<GeneDef> __result)
        {
            List<Gene> genesA = new List<Gene>();
            List<Gene> genesB = new List<Gene>();

            GeneInheritanceExtension extensionA = new GeneInheritanceExtension();
            GeneInheritanceExtension extensionB = new GeneInheritanceExtension();

            if (mother?.genes != null)
            {
                genesA = mother.genes.GenesListForReading;
                extensionA = CheckForModExtension(genesA);
            }
            if (father?.genes != null)
            {
                genesB = father.genes.GenesListForReading;
                extensionB = CheckForModExtension(genesB);
            }

            if (extensionA != null || extensionB != null)
            {
                bool parentA = mother != null && extensionA?.passXenotypeGenes == true;
                bool parentB = father != null && extensionB?.passXenotypeGenes == true;
                bool bothParent = parentA & parentB;
                bool sameXenotype = mother.genes.Xenotype == father.genes.Xenotype;

                if (parentA && !parentB && !sameXenotype)
                {
                    List<GeneDef> list = new List<GeneDef>();
                    if (Rand.Chance(extensionA.xenotypeGenesChance))
                    {
                        foreach (GeneDef item in mother.genes.Xenotype.AllGenes)
                        {
                            list.Add(item);
                        }
                        inheritedXenotype = extensionA.xenotypeDefName;
                    }
                    else
                    {
                        list = __result;
                        inheritedXenotype = null;
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
                        inheritedXenotype = extensionB.xenotypeDefName;
                    }
                    else
                    {
                        list = __result;
                        inheritedXenotype = null;
                    }
                    __result = list;
                }
                else if (bothParent && !sameXenotype)
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
                            inheritedXenotype = extensionA.xenotypeDefName;
                        }
                        else
                        {
                            foreach (GeneDef item in father.genes.Xenotype.AllGenes)
                            {
                                list.Add(item);
                            }
                            inheritedXenotype = extensionB.xenotypeDefName;
                        }

                    }
                    else
                    {
                        list = __result;
                        inheritedXenotype = null;
                    }
                    __result = list;
                }
                else if (bothParent && sameXenotype)
                {
                    List<GeneDef> list = new List<GeneDef>();
                    foreach (GeneDef item in mother.genes.Xenotype.AllGenes)
                    {
                        list.Add(item);
                    }
                    inheritedXenotype = extensionA.xenotypeDefName;
                    __result = list;
                }
            }
        }

        public static void HarmonyPatchPosfix_PregnancyUtilityTryGetInheritedXenotype(Pawn mother, Pawn father, ref XenotypeDef xenotype, ref bool __result)
        {
            if (inheritedXenotype != null)
            {
                xenotype = inheritedXenotype;
                __result = true;
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