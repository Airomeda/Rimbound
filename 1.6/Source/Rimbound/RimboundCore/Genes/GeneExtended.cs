using RimWorld;
using System.Collections.Generic;
using Verse;
using static HarmonyLib.Code;

namespace RimboundCore
{
    public class GeneExtended : Gene
    {
        public GeneExtension_Exploder exploderExtension;

        public GeneExtension_Incident incidentExtension;

        public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
        {
            base.Notify_PawnDied(dinfo, culprit);

            if (exploderExtension != null)
            {
                List<Thing> list = new List<Thing>();

                if (this.pawn?.Corpse != null && this.pawn?.Corpse.Map != null)
                {
                    if (!exploderExtension.damageUser)
                    {
                        list.Add(this.pawn);
                    }
                    GenExplosion.DoExplosion(this.pawn.Corpse.Position, this.pawn.Corpse.Map, exploderExtension.radius, exploderExtension.damageDef, this.pawn.Corpse, exploderExtension.damageAmount, exploderExtension.damagePenetration, exploderExtension.soundCreated, null, null, null, exploderExtension.thingCreated, exploderExtension.thingCreatedChance, 1, null, null, 255, false, null, 0f, 1, exploderExtension.chanceToStartFire, false, null, list, null, true, 1, exploderExtension.excludeRadius, true, null, exploderExtension.screenShakeFactor, null, null, exploderExtension.postExplosionSpawnSingleThingDef, null);
                }
            }

            if (incidentExtension != null)
            {
                IncidentDef incident = incidentExtension.incidentDef;

                if (this.pawn?.Corpse != null && this.pawn?.Corpse.Map != null)
                {
                    IncidentParms parms = StorytellerUtility.DefaultParmsNow(incident.category, this.pawn.Corpse.Map);

                    if (Rand.Chance(incidentExtension.triggerChance))
                    {
                        if (incident.gameCondition != null)
                        {
                            if (parms.target.GameConditionManager.ConditionIsActive(incident.gameCondition) && !incidentExtension.allowOverlap)
                            {
                                return;
                            }
                            incident.Worker.TryExecute(parms);
                        }
                        else
                        {
                            incident.Worker.TryExecute(parms);
                        }
                    }
                }
            }
        }

        public override void PostAdd()
        {
            base.PostAdd();
            exploderExtension = def.GetModExtension<GeneExtension_Exploder>();
            incidentExtension = def.GetModExtension<GeneExtension_Incident>();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            exploderExtension = def.GetModExtension<GeneExtension_Exploder>();
            incidentExtension = def.GetModExtension<GeneExtension_Incident>();
        }
    }
}
