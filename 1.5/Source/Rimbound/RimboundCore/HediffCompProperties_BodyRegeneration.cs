using Verse;

namespace RimboundCore
{
    public class HediffCompProperties_BodyRegeneration : HediffCompProperties
    {
        public IntRange rateInTicks = new IntRange(900000, 1800000);

        public float healAmount = 1f;

        public HediffCompProperties_BodyRegeneration()
        {
            compClass = typeof(HediffComp_BodyRegeneration);
        }
    }
}
