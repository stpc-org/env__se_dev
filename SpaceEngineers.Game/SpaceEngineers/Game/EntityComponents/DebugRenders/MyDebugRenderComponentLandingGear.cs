// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.EntityComponents.DebugRenders.MyDebugRenderComponentLandingGear
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using SpaceEngineers.Game.Entities.Blocks;
using VRage.ModAPI;
using VRageMath;
using VRageRender;

namespace SpaceEngineers.Game.EntityComponents.DebugRenders
{
  public class MyDebugRenderComponentLandingGear : MyDebugRenderComponent
  {
    private MyLandingGear m_langingGear;

    public MyDebugRenderComponentLandingGear(MyLandingGear landingGear)
      : base((IMyEntity) landingGear)
      => this.m_langingGear = landingGear;

    public override void DebugDraw()
    {
      if (!MyDebugDrawSettings.DEBUG_DRAW_MODEL_DUMMIES)
        return;
      foreach (Matrix lockPosition in this.m_langingGear.LockPositions)
      {
        Vector3 halfExtents;
        Vector3D position;
        Quaternion orientation;
        this.m_langingGear.GetBoxFromMatrix((MatrixD) ref lockPosition, out halfExtents, out position, out orientation);
        Matrix fromQuaternion = Matrix.CreateFromQuaternion(orientation);
        fromQuaternion.Translation = (Vector3) position;
        Matrix matrix = Matrix.CreateScale(halfExtents * 2f * new Vector3(2f, 1f, 2f)) * fromQuaternion;
        MyRenderProxy.DebugDrawOBB((MatrixD) ref matrix, (Color) Color.Yellow.ToVector3(), 1f, false, false);
      }
    }
  }
}
