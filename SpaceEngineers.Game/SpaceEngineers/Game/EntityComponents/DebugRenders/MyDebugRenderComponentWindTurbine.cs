// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.EntityComponents.DebugRenders.MyDebugRenderComponentWindTurbine
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Game.Components;
using SpaceEngineers.Game.Entities.Blocks;
using VRage.ModAPI;
using VRageMath;
using VRageRender;

namespace SpaceEngineers.Game.EntityComponents.DebugRenders
{
  public class MyDebugRenderComponentWindTurbine : MyDebugRenderComponent
  {
    public MyWindTurbine Entity => (MyWindTurbine) this.Entity;

    public MyDebugRenderComponentWindTurbine(MyWindTurbine turbine)
      : base((IMyEntity) turbine)
    {
    }

    public override void DebugDraw()
    {
      base.DebugDraw();
      float[] rayEffectivities = this.Entity.RayEffectivities;
      for (int id = 0; id < rayEffectivities.Length; ++id)
      {
        Vector3D start;
        Vector3D end;
        this.Entity.GetRaycaster(id, out start, out end);
        Vector3D vector3D1 = Vector3D.Lerp(start, end, (double) this.Entity.BlockDefinition.MinRaycasterClearance);
        Vector3D vector3D2 = Vector3D.Lerp(vector3D1, end, (double) rayEffectivities[id]);
        MyRenderProxy.DebugDrawText3D(end, rayEffectivities[id].ToString("F2"), Color.Green, 0.7f, false);
        MyRenderProxy.DebugDrawLine3D(start, vector3D1, Color.Black, Color.Black, false);
        MyRenderProxy.DebugDrawLine3D(vector3D1, vector3D2, Color.Green, Color.Green, false);
        MyRenderProxy.DebugDrawLine3D(vector3D2, end, Color.Red, Color.Red, false);
      }
    }
  }
}
