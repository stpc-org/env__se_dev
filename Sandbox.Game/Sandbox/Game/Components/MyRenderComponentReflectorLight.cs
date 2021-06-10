// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Components.MyRenderComponentReflectorLight
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Lights;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Components
{
  internal class MyRenderComponentReflectorLight : MyRenderComponentLight
  {
    private const float RADIUS_TO_CONE_MULTIPLIER = 0.25f;
    private const float SMALL_LENGTH_MULTIPLIER = 0.5f;
    private MyReflectorLight m_reflectorLight;
    private List<MyLight> m_lights;

    public MyRenderComponentReflectorLight(List<MyLight> lights) => this.m_lights = lights;

    public override void OnAddedToContainer()
    {
      base.OnAddedToContainer();
      this.m_reflectorLight = this.Container.Entity as MyReflectorLight;
    }

    public override void AddRenderObjects()
    {
      base.AddRenderObjects();
      BoundingBox boundingBox = this.m_reflectorLight.PositionComp.LocalAABB;
      boundingBox.Inflate(this.m_reflectorLight.IsLargeLight ? 3f : 1f);
      float num = this.m_reflectorLight.ReflectorRadiusBounds.Max * 0.25f;
      if (!this.m_reflectorLight.IsLargeLight)
        num *= 0.5f;
      boundingBox = boundingBox.Include(new Vector3(0.0f, 0.0f, -num));
      MyRenderProxy.UpdateRenderObject(this.m_renderObjectIDs[0], new MatrixD?(), new BoundingBox?(boundingBox));
    }

    public override void Draw()
    {
      base.Draw();
      if (!this.m_reflectorLight.IsReflectorEnabled)
        return;
      this.DrawReflectorCone();
    }

    private void DrawReflectorCone()
    {
      if (string.IsNullOrEmpty(this.m_reflectorLight.ReflectorConeMaterial))
        return;
      foreach (MyLight light in this.m_lights)
      {
        Vector3 vector1 = Vector3.Normalize(MySector.MainCamera.Position - this.m_reflectorLight.PositionComp.GetPosition());
        Vector3.TransformNormal(light.ReflectorDirection, this.m_reflectorLight.PositionComp.WorldMatrixRef);
        Vector3 reflectorDirection1 = light.ReflectorDirection;
        float num = MathHelper.Saturate(1f - (float) Math.Pow((double) Math.Abs(Vector3.Dot(vector1, reflectorDirection1)), 30.0));
        uint parentId = light.ParentID;
        Vector3D position = light.Position;
        Vector3D reflectorDirection2 = (Vector3D) light.ReflectorDirection;
        float length = Math.Max(15f, this.m_reflectorLight.ReflectorRadius * 0.25f);
        if (!this.m_reflectorLight.IsLargeLight)
          length *= 0.5f;
        float reflectorThickness = this.m_reflectorLight.BlockDefinition.ReflectorThickness;
        Color color = this.m_reflectorLight.Color;
        float n = (float) ((double) this.m_reflectorLight.CurrentLightPower * (double) this.m_reflectorLight.Intensity * 0.800000011920929);
        MyTransparentGeometry.AddLocalLineBillboard(MyStringId.GetOrCompute(this.m_reflectorLight.ReflectorConeMaterial), color.ToVector4() * num * MathHelper.Saturate(n), position, parentId, (Vector3) reflectorDirection2, length, reflectorThickness, MyBillboard.BlendTypeEnum.AdditiveBottom);
      }
    }

    private class Sandbox_Game_Components_MyRenderComponentReflectorLight\u003C\u003EActor
    {
    }
  }
}
