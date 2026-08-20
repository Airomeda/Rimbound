using RimWorld;
using System.Collections.Generic;
using Verse;

namespace RimboundCore
{
    public class GeneBodyRegeneration : Gene
    {
        private int ticksToRegen = 15000;

        public GeneBodyRegenerationExtension modExtension;

        public HediffComp_BodyRegeneration hediffCompBodyRegen = new HediffComp_BodyRegeneration();

        public override void PostAdd()
        {
            base.PostAdd();

            modExtension = def.GetModExtension<GeneBodyRegenerationExtension>();
            ResetRegenInterval();
        }

        public override void TickInterval(int delta)
        {
            base.TickInterval(delta);

            ticksToRegen -= delta;

            if (ticksToRegen <= 0)
            {
                hediffCompBodyRegen.TryRegenerateBodyPart(pawn, LabelCap, modExtension.healAmount, modExtension.damagedRestoredPart);
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
