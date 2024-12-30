using RimWorld;
using Verse;

namespace RimboundCore
{
    [DefOf]
    public static class RimboundBodyDefOf
    {
        public static BodyPartDef Heart;

        public static BodyPartDef Lung;

        public static BodyPartDef Liver;

        public static BodyPartDef Stomach;

        public static BodyPartDef Kidney;

        public static BodyPartDef Eye;

        public static BodyPartDef Ear;

        public static BodyPartDef Nose;

        public static BodyPartDef Jaw;

        public static BodyPartDef Tongue;

        public static BodyPartDef Neck;

        public static BodyPartDef Shoulder;

        public static BodyPartDef Arm;

        public static BodyPartDef Hand;

        public static BodyPartDef Torso;

        public static BodyPartDef Spine;

        public static BodyPartDef Leg;

        public static BodyPartDef Foot;

        static RimboundBodyDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(RimboundBodyDefOf));
        }
    }
}
