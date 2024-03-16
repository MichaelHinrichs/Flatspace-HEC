//Written for Flatspace. https://store.steampowered.com/app/1210780/
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace Flatspace_HEC
{
    public class HEC
    {
        private string textureA = new();
        private string textureB = new();
        private string textureC = new();//I haven't seen a model that has 3 textures, but there's exactly enough space for one in between textureB and the point cloud.
        private readonly List<Vector3> points = new();
        private readonly List<Vector3> normals = new();
        private readonly List<Vector2> uvs = new();
        private readonly List<Vector3> faces = new();

        private static HEC Read(string HECFile)
        {
            BinaryReader br = new(File.OpenRead(HECFile));

            if (new string(br.ReadChars(4)) != "QCEH")
                throw new System.Exception("Wrong file. Input a hec file from Flatspace.");

            br.ReadInt32();//number of pointcounts? Always* 1.
            int pointCount = br.ReadInt32();
            br.ReadInt32();//Unknown
            br.ReadInt32();//Unknown
            br.ReadInt32();//Unknown. Always* 4.
            int faceCount = br.ReadInt32();
            br.ReadInt32();//Unknown. Always* 0x3F800000.
            br.ReadInt32();//Unknown
            br.ReadInt32();//Unknown
            br.ReadInt32();//Unknown
            br.ReadInt32();//Unknown
            br.ReadInt32();//Unknown
            br.ReadInt32();//Unknown. Always* 6.
            br.ReadInt32();//Unknown
            br.ReadInt32();//Unknown
            br.ReadInt32();//Unknown
            br.ReadInt32();//Unknown
            br.ReadInt32();//Unknown
            HEC hec = new()
            {
                textureA = new string(br.ReadChars(0x40)).TrimEnd((char)0x00),
                textureB = new string(br.ReadChars(0x40)).TrimEnd((char)0x00),
                textureC = new string(br.ReadChars(0x40)).TrimEnd((char)0x00)
            };

            for (int i = 0; i < pointCount; i++)
            {
                hec.points.Add(new Vector3(br.ReadInt32(), br.ReadInt32(), br.ReadInt32()));
                hec.normals.Add(new Vector3(br.ReadInt32(), br.ReadInt32(), br.ReadInt32()));
                hec.uvs.Add(new Vector2(br.ReadInt32(), br.ReadInt32()));
            }

            for (int i = 0; i < faceCount; i++)
            {
                hec.faces.Add(new Vector3(i * 3, i * 3 + 1, i * 3 + 2));
            }
            return hec;
        }
    }
}
//"Always", meaning "in all of the hec files that i checked".
