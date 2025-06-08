using Verse;

namespace RimboundCore
{
    public class GeneBodyRegenerationExtension : DefModExtension
    {
        public IntRange rateInTicks = new IntRange(60000, 1800000);

        public float healAmount = 1f;
    }
}
