using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace RimboundCore.HarmonyPatches
{
    [HarmonyPatch(typeof(HeadTypeDef), "GetGraphic")]
    public static class HarmonyPatch_HeadTypeDefGetGraphic
    {
        [HarmonyPostfix]
        public static void HarmonyPatchPostfix_HeadTypeDefGetGraphic(HeadTypeDef __instance, Pawn pawn, Color color, ref Graphic_Multi __result)
        {
            HeadTypeDefExtension modExtension = __instance.GetModExtension<HeadTypeDefExtension>();

            if (modExtension != null)
            {
                Shader shader = ShaderUtility.GetSkinShader(pawn);

                if (!modExtension.useSkinShader)
                {
                    shader = modExtension.shaderType?.Shader ?? ShaderDatabase.Cutout;
                }

                for (int i = 0; i < __instance.graphics.Count; i++)
                {
                    if (color.IndistinguishableFrom(__instance.graphics[i].Key) && __instance.graphics[i].Value.Shader == shader)
                    {
                        __result = __instance.graphics[i].Value;
                    }
                }

                Graphic_Multi graphic_Multi = (Graphic_Multi)GraphicDatabase.Get<Graphic_Multi>(__instance.graphicPath, shader, Vector2.one, color);
                __instance.graphics.Add(new KeyValuePair<Color, Graphic_Multi>(color, graphic_Multi));
                __result = graphic_Multi;
            }
        }
    }
}
