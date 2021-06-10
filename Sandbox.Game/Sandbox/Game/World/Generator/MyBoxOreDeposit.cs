// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.Generator.MyBoxOreDeposit
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Voxels;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRageMath;

namespace Sandbox.Game.World.Generator
{
  internal class MyBoxOreDeposit : MyCompositeShapeOreDeposit
  {
    private MyCsgBox m_boxShape;

    public MyBoxOreDeposit(MyCsgShapeBase baseShape, MyVoxelMaterialDefinition material)
      : base(baseShape, material)
      => this.m_boxShape = (MyCsgBox) baseShape;

    public override MyVoxelMaterialDefinition GetMaterialForPosition(
      ref Vector3 pos,
      float lodSize)
    {
      List<MyVoxelMaterialDefinition> list = MyDefinitionManager.Static.GetVoxelMaterialDefinitions().ToList<MyVoxelMaterialDefinition>();
      float num = 2f * this.m_boxShape.HalfExtents;
      int index = (int) ((double) MathHelper.Clamp((pos - this.m_boxShape.Center() + this.m_boxShape.HalfExtents).X / num, 0.0f, 1f) * (double) (list.Count - 1));
      return list[index];
    }

    public override void DebugDraw(ref MatrixD translation, Color materialColor)
    {
    }
  }
}
