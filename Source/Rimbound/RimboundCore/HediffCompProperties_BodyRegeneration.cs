using Verse;

namespace RimboundCore
{
    public class HediffCompProperties_BodyRegeneration : HediffCompProperties
    {
        public IntRange rateInTicks;

        public float healAmount = 1f;

        public HediffCompProperties_BodyRegeneration()
        {
            compClass = typeof(HediffComp_BodyRegeneration);
        }
    }
}
