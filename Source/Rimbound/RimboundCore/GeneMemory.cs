using RimWorld;
using System.Collections.Concurrent;
using UnityEngine;
using Verse;

namespace RimboundCore
{
    public class GeneMemory : Gene
    {
        public static ConcurrentDictionary<int, float> ThoughtDurationMultipliers = new ConcurrentDictionary<int, float>();

        public GeneMemoryExtension modExtension;

        public float ThoughtDurationMultiplier()
        {
            return modExtension?.thoughtDurationMultiplier ?? 1f;
        }

        public static float GetThoughtDurationMultiplierForPawn(Pawn pawn)
        {
            if (pawn != null)
            {
                return ThoughtDurationMultipliers.TryGetValue(pawn.thingIDNumber, 1f);
            }
            return 1f;
        }

        public void Initialize()
        {
            ThoughtDurationMultipliers.AddOrUpdate(pawn.thingIDNumber, ThoughtDurationMultiplier(), (int _, float oldValue) => oldValue * ThoughtDurationMultiplier());
        }

        public override void ExposeData()
        {
            base.ExposeData();
            if (modExtension != null || Scribe.mode != LoadSaveMode.LoadingVars || (Scribe.mode != LoadSaveMode.ResolvingCrossRefs && Scribe.mode != LoadSaveMode.PostLoadInit))
            {
                return;
            }
            modExtension = def.GetModExtension<GeneMemoryExtension>();
            if (modExtension == null)
            {
                return;
            }
            Initialize();
        }

        public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
        {
            base.Notify_PawnDied(dinfo, culprit);
            if (this.pawn?.Corpse != null && this.pawn?.Corpse.Map != null)
            {
                ThoughtDurationMultipliers.TryRemove(pawn.thingIDNumber, out var value);
            }
        }

        public override void PostRemove()
        {
            base.PostRemove();
            float value;
            if (ThoughtDurationMultipliers.ContainsKey(pawn.thingIDNumber) && !Mathf.Approximately(ThoughtDurationMultiplier(), ThoughtDurationMultipliers.TryGetValue(pawn.thingIDNumber, ThoughtDurationMultiplier())))
            {
                ThoughtDurationMultipliers.AddOrUpdate(pawn.thingIDNumber, ThoughtDurationMultiplier(), (int _, float oldValue) => oldValue / ThoughtDurationMultiplier());
            }
            else
            {
                ThoughtDurationMultipliers.TryRemove(pawn.thingIDNumber, out value);
            }
        }

        public override void PostAdd()
        {
            base.PostAdd();
            modExtension = def.GetModExtension<GeneMemoryExtension>();
            Initialize();
        }
    }
}
