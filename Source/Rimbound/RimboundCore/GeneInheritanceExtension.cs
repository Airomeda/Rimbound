using RimWorld;
using Verse;

namespace RimboundCore
{
    public class GeneInheritanceExtension : DefModExtension
    {
        public bool passXenotypeGenes = false;

        public XenotypeDef xenotypeDefName;

        public float xenotypeGenesChance = 0.5f;
    }
}
