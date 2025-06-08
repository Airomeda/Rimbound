using RimWorld;
using Verse;

namespace RimboundCore
{
    public class CompProperties_GiveHediff : CompProperties_AbilityEffect
    {
        public HediffDef hediffDef;

        public bool applyToCaster = true;

        public bool applyToRadius = false;

        public CompProperties_GiveHediff()
        {
            compClass = typeof(CompGiveHediff);
        }
    }
}
