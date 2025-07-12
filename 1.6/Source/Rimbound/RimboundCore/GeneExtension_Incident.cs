using RimWorld;
using Verse;

namespace RimboundCore
{
    public class GeneExtension_Incident : DefModExtension
    {
        public IncidentDef incidentDef;

        public float triggerChance = 1.0f;

        public bool allowOverlap = false;
    }
}
