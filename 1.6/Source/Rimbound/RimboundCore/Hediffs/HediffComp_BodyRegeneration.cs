using RimWorld;
using System.Collections.Generic;
using Verse;

namespace RimboundCore
{
    public class HediffComp_BodyRegeneration : HediffComp
    {
        public HediffCompProperties_BodyRegeneration Props => (HediffCompProperties_BodyRegeneration)props;

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

        private int tickCounter = 15000;

        public override void CompPostMake()
        {
            base.CompPostMake();

            ResetRegenInterval();
        }

        public override void CompPostPostAdd(DamageInfo? dinfo)
        {
            base.CompPostPostAdd(dinfo);

            ResetRegenInterval();
        }

        public override void CompExposeData()
        {
            base.CompExposeData();
            Scribe_Values.Look(ref tickCounter, "tickCounterBodyRegen", 0);
        }

        public override void CompPostTickInterval(ref float severityAdjustment, int delta)
        {
            base.CompPostTickInterval(ref severityAdjustment, delta);

            tickCounter -= delta;
            if (tickCounter <= 0)
            {
                Pawn pawn = this.parent.pawn;

                TryRegenerateBodyPart(pawn, parent.LabelCap, Props.healAmount, Props.damagedRestoredPart);
                ResetRegenInterval();
            }
        }

        private void ResetRegenInterval()
        {
            tickCounter = Props.rateInTicks.RandomInRange;
        }

        public List<Hediff_Injury> GetInjuries(Pawn pawn)
        {
            List<Hediff_Injury> list = new List<Hediff_Injury>();
            for (int i = 0; i < pawn.health.hediffSet.hediffs.Count; i++)
            {
                if (pawn.health.hediffSet.hediffs[i] is Hediff_Injury hediff_Injury && validParts.Contains(hediff_Injury.Part.def))
                {
                    list.Add(hediff_Injury);
                }
            }
            return list;
        }

        public static BodyPartRecord FindFirstMissingBodyPart(Pawn pawn)
        {
            BodyPartRecord bodyPartRecord = null;
            foreach (Hediff_MissingPart missingPartsCommonAncestor in pawn.health.hediffSet.GetMissingPartsCommonAncestors())
            {
                if (!pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(missingPartsCommonAncestor.Part) && (bodyPartRecord == null || missingPartsCommonAncestor.Part.coverageAbsWithChildren > bodyPartRecord.coverageAbsWithChildren) && validParts.Contains(missingPartsCommonAncestor.Part.def))
                {
                    bodyPartRecord = missingPartsCommonAncestor.Part;
                }
            }
            return bodyPartRecord;
        }

        public void TryRegenerateBodyPart(Pawn pawn, string cause, float heal, bool damagedRegen)
        {
            if (pawn.health != null)
            {
                BodyPartRecord bodyPartRecord = FindFirstMissingBodyPart(pawn);
                if (bodyPartRecord != null)
                {
                    pawn.health.RestorePart(bodyPartRecord);

                    if (damagedRegen)
                    {
                        int num = (int)pawn.health.hediffSet.GetPartHealth(bodyPartRecord) - 1;
                        DamageInfo dinfo = new DamageInfo(DamageDefOf.Cut, num, 999f, -1f, null, bodyPartRecord);
                        dinfo.SetAllowDamagePropagation(val: false);
                        pawn.TakeDamage(dinfo);
                    }

                    if (PawnUtility.ShouldSendNotificationAbout(pawn))
                    {
                        Messages.Message("MessagePermanentWoundHealed".Translate(cause, pawn.LabelShort, bodyPartRecord.Label, pawn.Named("PAWN")), pawn, MessageTypeDefOf.PositiveEvent);
                    }
                }
                else
                {
                    List<Hediff_Injury> injuries = GetInjuries(pawn);
                    if (injuries.Count > 0)
                    {
                        injuries.RandomElement().Severity -= heal;
                    }
                }
            }
        }
    }
}
