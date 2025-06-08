using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimboundCore
{
    public class PawnRenderNodeWorker_Face : PawnRenderNodeWorker_FlipWhenCrawling
    {
        public override Vector3 OffsetFor(PawnRenderNode node, PawnDrawParms parms, out Vector3 pivot)
        {
            RenderProperties_FaceExtension modExtensions = CheckForModExtension(parms.pawn.genes.GenesListForReading);
            HeadTypeDef headType = parms.pawn.story.headType;
            Vector3 result = base.OffsetFor(node, parms, out pivot);

            if (modExtensions != null)
            {
                if (parms.facing == Rot4.North)
                {
                    result.x += modExtensions.offsets.north.GetOffset(headType).x;
                    result.z += modExtensions.offsets.north.GetOffset(headType).z;
                }
                else if (parms.facing == Rot4.East)
                {
                    result.x += modExtensions.offsets.east.GetOffset(headType).x;
                    result.z += modExtensions.offsets.east.GetOffset(headType).z;
                }
                else if (parms.facing == Rot4.South)
                {
                    result.x += modExtensions.offsets.south.GetOffset(headType).x;
                    result.z += modExtensions.offsets.south.GetOffset(headType).z;
                }
                else if (parms.facing == Rot4.West)
                {
                    result.x += modExtensions.offsets.west.GetOffset(headType).x;
                    result.z += modExtensions.offsets.west.GetOffset(headType).z;
                }
            }
            return result;
        }

        public static RenderProperties_FaceExtension CheckForModExtension(List<Gene> genes)
        {
            RenderProperties_FaceExtension extension = null;
            foreach (Gene gene in genes)
            {
                if (gene.Active)
                {
                    var extensions = gene.def.GetModExtension<RenderProperties_FaceExtension>();

                    if (extensions != null)
                    {
                        extension = extensions;
                    }
                }
            }
            return extension;
        }
    }
}
