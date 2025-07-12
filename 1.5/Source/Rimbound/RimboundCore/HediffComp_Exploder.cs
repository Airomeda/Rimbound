using System.Collections.Generic;
using Verse;

namespace RimboundCore
{
    public class HediffComp_Exploder : HediffComp
    {
        public HediffCompProperties_Exploder Props => (HediffCompProperties_Exploder)props;

        public ThingDef weapon = null;

        public ThingDef projectile = null;

        public ThingDef preExplosionSpawnThingDef = null;

        public Thing target = null;

        public int postExplosionSpawnThingCount = 1;

        public int preExplosionSpawnThingCount = 1;

        public float preExplosionSpawnChance = 0f;

        public GasType? gasType = GasType.BlindSmoke;

        public bool applyDamageToNeighborCells = false;

        public bool damageFalloff = false;

        public float? flammabilityChanceCurve = null;

        public List<Thing> list = new List<Thing>();

        public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
        {
            if (this.parent.pawn?.Corpse != null && this.parent.pawn?.Corpse.Map != null)
            {
                if (!Props.damageUser)
                {
                    list.Add(this.parent.pawn);
                }
                GenExplosion.DoExplosion(this.parent.pawn.Corpse.Position, this.parent.pawn.Corpse.Map, Props.radius, Props.damageDef, this.parent.pawn, Props.damageAmount, Props.damagePenetration, Props.soundCreated, weapon, projectile, target, Props.thingCreated, Props.thingCreatedChance, postExplosionSpawnThingCount, gasType, applyDamageToNeighborCells, preExplosionSpawnThingDef, preExplosionSpawnChance, preExplosionSpawnThingCount, Props.chanceToStartFire, damageFalloff, flammabilityChanceCurve, list);
            }
        }
    }
}
