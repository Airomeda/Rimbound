using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using Verse;

namespace RimboundCore
{
    public class RenderProperties_FaceExtension : DefModExtension
    {
        public GeneOffsets offsets;

        public class GeneOffsets
        {
            public RotationOffset GetOffset(Rot4 rotation) =>
                rotation == Rot4.South ? this.south :
                rotation == Rot4.North ? this.north :
                rotation == Rot4.East ? this.east ?? this.west : this.west;

            public RotationOffset south = new RotationOffset();
            public RotationOffset north = new RotationOffset();
            public RotationOffset east = new RotationOffset();
            public RotationOffset west;
        }

        public class RotationOffset
        {
            public Vector3 GetOffset(HeadTypeDef headType)
            {
                Vector3 headOffset = this.headTypes?.FirstOrDefault(predicate: to => to.headType == headType)?.offset ?? Vector3.zero;
                return new Vector3(headOffset.x, headOffset.y, headOffset.z);
            }

            public List<HeadTypeOffset> headTypes;
        }

        public class HeadTypeOffset
        {
            public HeadTypeDef headType;
            public Vector3 offset = Vector3.zero;

            public void LoadDataFromXmlCustom(XmlNode xmlRoot)
            {
                DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, nameof(this.headType), xmlRoot.Name);
                this.offset = (Vector3)ParseHelper.FromString(xmlRoot.FirstChild.Value, typeof(Vector3));
            }
        }
    }
}
