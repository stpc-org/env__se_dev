// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Voxels.Planet.MyHeightCubemap
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Voxels;

namespace Sandbox.Engine.Voxels.Planet
{
  public class MyHeightCubemap : MyWrappedCubemap<MyHeightmapFace>
  {
    public MyHeightCubemap(string name, MyHeightmapFace[] faces, int resolution)
      : base(name, resolution, faces)
    {
    }

    public unsafe VrPlanetShape.Mapset GetMapset() => new VrPlanetShape.Mapset()
    {
      Front = this.m_faces[0].Data,
      Back = this.m_faces[1].Data,
      Left = this.m_faces[2].Data,
      Right = this.m_faces[3].Data,
      Up = this.m_faces[4].Data,
      Down = this.m_faces[5].Data,
      Resolution = this.Resolution
    };
  }
}
