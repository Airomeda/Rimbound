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

            if (Props.hediffDef != null)
            {
                Pawn casterPawn = parent.pawn;
                Pawn targetPawn = target.Pawn;

                if (Props.applyToCaster)
                {
                    casterPawn.health.AddHediff(Props.hediffDef);
                }
                if (targetPawn != null)
                {
                    targetPawn.health.AddHediff(Props.hediffDef);
                }
                if (Props.applyToRadius)
                {
                    foreach (Pawn affectedPawn in casterPawn.Map.mapPawns.AllPawnsSpawned)
                    {
                        if (affectedPawn.Spawned && affectedPawn.Position.InHorDistOf(target.Cell, parent.def.EffectRadius))
                        {
                            affectedPawn.health.AddHediff(Props.hediffDef);
                        }
                    }
                }
            }
        }
    }
}
