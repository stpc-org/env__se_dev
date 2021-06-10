// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.Generator.MyCompositeShapeOreDeposit
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using VRage.Game;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.World.Generator
{
  internal class MyCompositeShapeOreDeposit
  {
    public readonly MyCsgShapeBase Shape;
    protected readonly MyVoxelMaterialDefinition m_material;

    public virtual void DebugDraw(ref MatrixD translation, Color materialColor)
    {
      if (!MyDebugDrawSettings.DEBUG_DRAW_ASTEROID_ORES)
        return;
      this.Shape.DebugDraw(ref translation, materialColor);
      MyRenderProxy.DebugDrawText3D((Matrix.CreateTranslation(this.Shape.Center()) * translation).Translation, this.m_material.Id.SubtypeName, Color.White, 1f, false);
    }

    public MyCompositeShapeOreDeposit(MyCsgShapeBase shape, MyVoxelMaterialDefinition material)
    {
      this.Shape = shape;
      this.m_material = material;
    }

    public virtual MyVoxelMaterialDefinition GetMaterialForPosition(
      ref Vector3 pos,
      float lodSize)
    {
      return this.m_material;
    }
  }
}
