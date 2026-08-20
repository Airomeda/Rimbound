using Verse;

namespace RimboundCore
{
    public class GeneRandomize : Gene
    {
        public override void PostAdd()
        {
            base.PostAdd();
            pawn.genes.AddGene(def.GetModExtension<GeneRandomizeExtension>().geneDefs.RandomElement(), pawn.genes.IsXenogene(this));
            pawn.genes.RemoveGene(this);
        }
    }
}
