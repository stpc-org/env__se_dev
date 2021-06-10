// Decompiled with JetBrains decompiler
// Type: VRage.Game.Graphics.MyTrailProperties
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRage.Utils;
using VRageMath;

namespace VRage.Game.Graphics
{
  public class MyTrailProperties
  {
    public Vector3D Position = Vector3D.Zero;
    public Vector3D Normal = Vector3D.Zero;
    public Vector3D ForwardDirection = Vector3D.Zero;
    public long EntityId;
    public MyStringHash PhysicalMaterial;
    public MyStringHash VoxelMaterial;
  }
}
