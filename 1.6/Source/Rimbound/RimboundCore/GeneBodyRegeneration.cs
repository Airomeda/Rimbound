using RimWorld;
using System.Collections.Generic;
using Verse;

namespace RimboundCore
{
    public class GeneBodyRegeneration : Gene
    {
        private int ticksToRegen = 2500;

        public GeneBodyRegenerationExtension modExtension;

        public HediffComp_BodyRegeneration hediffCompBodyRegen = new HediffComp_BodyRegeneration();

        public static List<BodyPartDef> validParts = new List<BodyPartDef>
        {
            RimboundBodyDefOf.Heart,
            RimboundBodyDefOf.Lung,
            RimboundBodyDefOf.Liver,
            RimboundBodyDefOf.Stomach,
            RimboundBodyDefOf.Kidney,
            RimboundBodyDefOf.Eye,
            RimboundBodyDefOf.Ear,
            RimboundBodyDefOf.Nose,
            RimboundBodyDefOf.Jaw,
            RimboundBodyDefOf.Tongue,
            RimboundBodyDefOf.Neck,
            RimboundBodyDefOf.Shoulder,
            RimboundBodyDefOf.Arm,
            RimboundBodyDefOf.Hand,
            RimboundBodyDefOf.Finger,
            RimboundBodyDefOf.Torso,
            RimboundBodyDefOf.Spine,
            RimboundBodyDefOf.Leg,
            RimboundBodyDefOf.Foot,
            RimboundBodyDefOf.Toe
        };

        public override void PostAdd()
        {
            base.PostAdd();
            modExtension = def.GetModExtension<GeneBodyRegenerationExtension>();
            ticksToRegen = modExtension.rateInTicks.min;
            ResetRegenInterval();
        }

        public override void Tick()
        {
            base.Tick();
            ticksToRegen--;

            if (ticksToRegen <= 0)
            {
                hediffCompBodyRegen.TryRegenerateBodyPart(pawn, LabelCap, modExtension.healAmount);
                ResetRegenInterval();
            }
        }

        private void ResetRegenInterval()
        {
            ticksToRegen = modExtension.rateInTicks.RandomInRange;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            modExtension = def.GetModExtension<GeneBodyRegenerationExtension>();
            Scribe_Values.Look(ref ticksToRegen, "ticksToRegen", 0);
        }
    }
}
