// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Planet.MyPlanetSectorId
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage;
using VRageMath;

namespace Sandbox.Game.Entities.Planet
{
  public class MyPlanetSectorId
  {
    private const long CoordMask = 16777215;
    private const int CoordBits = 24;
    private const int FaceOffset = 48;
    private const long FaceMask = 7;
    private const int FaceBits = 3;
    private const long LodMask = 255;
    private const int LodBits = 8;
    private const int LodOffset = 51;

    private MyPlanetSectorId()
    {
    }

    public static long MakeSectorEntityId(int x, int y, int lod, int face, long parentId) => MyEntityIdentifier.ConstructIdFromString(MyEntityIdentifier.ID_OBJECT_TYPE.PLANET_ENVIRONMENT_SECTOR, string.Format("P({0})S(x{1}, y{2}, f{3}, l{4})", (object) parentId, (object) x, (object) y, (object) face, (object) lod));

    public static long MakeSectorId(int x, int y, int face, int lod = 0) => (long) x & 16777215L | ((long) y & 16777215L) << 24 | ((long) face & 7L) << 48 | ((long) lod & (long) byte.MaxValue) << 51;

    public static Vector3I DecomposeSectorId(long sectorID) => new Vector3I((float) (int) (sectorID & 16777215L), (float) (sectorID >> 24 & 16777215L), (float) (sectorID >> 48 & 7L));

    public static int GetFace(long packedSectorId) => (int) (packedSectorId >> 48 & 7L);
  }
}
