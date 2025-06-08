using RimWorld;
using Verse;

namespace RimboundCore
{
    public class CompGiveHediff : CompAbilityEffect
    {
        public new CompProperties_GiveHediff Props => (CompProperties_GiveHediff)props;

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            if (Props.applyToCaster)
            {
                parent.pawn.health.AddHediff(Props.hediffDef);
            }
            if (!Props.applyToRadius)
            {
                return;
            }
            foreach (Pawn item in parent.pawn.Map.mapPawns.AllPawnsSpawned)
            {
                if (item.Spawned && item.Position.InHorDistOf(target.Cell, parent.def.EffectRadius))
                {
                    item.health.AddHediff(Props.hediffDef);
                }
            }
        }
    }
}
