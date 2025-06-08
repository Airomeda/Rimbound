using RimWorld.Planet;
using RimWorld;
using RimWorld.QuestGen;
using System.Collections.Generic;
using Verse;

namespace RimboundCore
{
    public class QuestNode_GenerateXenotype : QuestNode
    {
        [NoTranslate]
        public SlateRef<string> storeAs;

        [NoTranslate]
        public SlateRef<string> addToList;

        [NoTranslate]
        public SlateRef<IEnumerable<string>> addToLists;

        public SlateRef<PawnKindDef> kindDef;

        public SlateRef<XenotypeDef> xenotypeDef;

        public SlateRef<Faction> faction;

        public SlateRef<bool> forbidAnyTitle;

        public SlateRef<bool> ensureNonNumericName;

        public SlateRef<IEnumerable<TraitDef>> forcedTraits;

        public SlateRef<IEnumerable<TraitDef>> prohibitedTraits;

        public SlateRef<Pawn> extraPawnForExtraRelationChance;

        public SlateRef<float> relationWithExtraPawnChanceFactor;

        public SlateRef<bool?> allowAddictions;

        public SlateRef<float> biocodeWeaponChance;

        public SlateRef<float> biocodeApparelChance;

        public SlateRef<bool> mustBeCapableOfViolence;

        public SlateRef<bool> isChild;

        public SlateRef<bool> allowPregnant;

        public SlateRef<Gender?> fixedGender;

        private const int MinExpertSkill = 11;

        protected override bool TestRunInt(Slate slate)
        {
            return true;
        }

        protected virtual DevelopmentalStage GetDevelopmentalStage(Slate slate)
        {
            if (!Find.Storyteller.difficulty.ChildrenAllowed || !isChild.GetValue(slate))
            {
                return DevelopmentalStage.Adult;
            }
            return DevelopmentalStage.Child;
        }

        protected override void RunInt()
        {
            Slate slate = QuestGen.slate;
            PawnKindDef value = kindDef.GetValue(slate);
            Faction ofPlayer = Faction.OfPlayer;
            bool flag = allowAddictions.GetValue(slate) ?? true;
            bool value2 = allowPregnant.GetValue(slate);
            IEnumerable<TraitDef> value3 = forcedTraits.GetValue(slate);
            IEnumerable<TraitDef> value4 = prohibitedTraits.GetValue(slate);
            float value5 = biocodeWeaponChance.GetValue(slate);
            bool value6 = mustBeCapableOfViolence.GetValue(slate);
            bool flag2 = value2;
            bool flag3 = flag;
            float num = value5;
            Pawn value7 = extraPawnForExtraRelationChance.GetValue(slate);
            float value8 = relationWithExtraPawnChanceFactor.GetValue(slate);
            Gender? value9 = fixedGender.GetValue(slate);
            PawnGenerationRequest request = new PawnGenerationRequest(value, ofPlayer, PawnGenerationContext.NonPlayer, -1, forceGenerateNewPawn: false, allowDead: false, allowDowned: false, canGeneratePawnRelations: true, value6, 1f, forceAddFreeWarmLayerIfNeeded: false, allowGay: true, flag2, allowFood: true, flag3, inhabitant: false, certainlyBeenInCryptosleep: false, forceRedressWorldPawnIfFormerColonist: false, worldPawnFactionDoesntMatter: false, num, biocodeApparelChance.GetValue(slate), value7, value8, null, null, value3, value4, null, null, null, value9, null, null, null, null, forceNoIdeo: false, forceNoBackstory: false, forbidAnyTitle: false, forceDead: false, null, null, null, null, null, 0f, GetDevelopmentalStage(slate));
            request.BiocodeApparelChance = biocodeApparelChance.GetValue(slate);
            request.ForbidAnyTitle = forbidAnyTitle.GetValue(slate);
            request.ForcedXenotype = xenotypeDef.GetValue(slate);
            Pawn pawn = PawnGenerator.GeneratePawn(request);
            if (ensureNonNumericName.GetValue(slate) && (pawn.Name == null || pawn.Name.Numerical))
            {
                pawn.Name = PawnBioAndNameGenerator.GeneratePawnName(pawn);
            }
            if (storeAs.GetValue(slate) != null)
            {
                QuestGen.slate.Set(storeAs.GetValue(slate), pawn);
            }
            if (addToList.GetValue(slate) != null)
            {
                QuestGenUtility.AddToOrMakeList(QuestGen.slate, addToList.GetValue(slate), pawn);
            }
            if (addToLists.GetValue(slate) != null)
            {
                foreach (string item in addToLists.GetValue(slate))
                {
                    QuestGenUtility.AddToOrMakeList(QuestGen.slate, item, pawn);
                }
            }
            QuestGen.AddToGeneratedPawns(pawn);
            if (!pawn.IsWorldPawn())
            {
                Find.WorldPawns.PassToWorld(pawn);
            }
        }
    }
}
