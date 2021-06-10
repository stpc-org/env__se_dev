// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyMaterialEnvironmentItem
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRageMath;

namespace Sandbox.Definitions
{
  public class MyMaterialEnvironmentItem
  {
    public MyDefinitionId Definition;
    public string GroupId;
    public int GroupIndex;
    public string ModifierId;
    public int ModifierIndex;
    public float Frequency;
    private bool m_detail;
    public bool IsBot;
    public bool IsVoxel;
    public bool IsEnvironemntItem;
    public Vector3 BaseColor;
    public Vector2 ColorSpread;
    public float Offset;
    public float MaxRoll;

    public bool IsDetail
    {
      set => this.m_detail = value;
      get => this.m_detail || this.IsBot || this.IsVoxel;
    }
  }
}
