// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyEdgeInfo
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
  public class MyEdgeInfo
  {
    public Vector4 LocalOrthoMatrix;
    private Color m_packedColor;
    public MyStringHash EdgeModel;
    public Base27Directions.Direction PackedNormal0;
    public Base27Directions.Direction PackedNormal1;

    public Color Color
    {
      get
      {
        Color packedColor = this.m_packedColor;
        packedColor.A = (byte) 0;
        return packedColor;
      }
      set
      {
        byte a = this.m_packedColor.A;
        this.m_packedColor = value;
        this.m_packedColor.A = a;
      }
    }

    public MyCubeEdgeType EdgeType
    {
      get => (MyCubeEdgeType) this.m_packedColor.A;
      set => this.m_packedColor.A = (byte) value;
    }

    public MyEdgeInfo()
    {
    }

    public MyEdgeInfo(
      ref Vector3 pos,
      ref Vector3I edgeDirection,
      ref Vector3 normal0,
      ref Vector3 normal1,
      ref Color color,
      MyStringHash edgeModel)
    {
      if (!MyCubeGridDefinitions.EdgeOrientations.ContainsKey(edgeDirection))
        edgeDirection = new Vector3I(0, 0, 1);
      MyEdgeOrientationInfo edgeOrientation = MyCubeGridDefinitions.EdgeOrientations[edgeDirection];
      this.PackedNormal0 = Base27Directions.GetDirection(normal0);
      this.PackedNormal1 = Base27Directions.GetDirection(normal1);
      this.m_packedColor = color;
      this.EdgeType = edgeOrientation.EdgeType;
      this.LocalOrthoMatrix = Vector4.PackOrthoMatrix(pos, edgeOrientation.Orientation.Forward, edgeOrientation.Orientation.Up);
      this.EdgeModel = edgeModel;
    }
  }
}
