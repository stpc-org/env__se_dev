// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.EntityComponents.Renders.MyRenderComponentShipGrinder
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.RenderDirect.ActorComponents;
using System;
using VRage.Network;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Game.EntityComponents.Renders
{
  public class MyRenderComponentShipGrinder : MyRenderComponentCubeBlockWithParentedSubpart
  {
    public class MyRenderComponentShipGrinderBlade : MyParentedSubpartRenderComponent
    {
      private float m_speed;

      public override void OnParented()
      {
        base.OnParented();
        this.m_speed = 0.0f;
        MyRenderProxy.UpdateRenderComponent<MyRotationAnimatorInitData, object>(this.GetRenderObjectID(), (object) null, (Action<MyRotationAnimatorInitData, object>) ((d, _) =>
        {
          d.RotationAxis = MyRotationAnimator.RotationAxis.AxisX;
          d.SpinUpSpeed = 41.8879f;
          d.SpinDownSpeed = 20.94395f;
        }));
      }

      public void UpdateBladeSpeed(float speed)
      {
        if ((double) this.m_speed == (double) speed)
          return;
        this.m_speed = speed;
        uint renderObjectId = this.GetRenderObjectID();
        if (renderObjectId == uint.MaxValue)
          return;
        FloatData.Update<MyRotationAnimator>(renderObjectId, this.m_speed);
      }

      private class Sandbox_Game_EntityComponents_Renders_MyRenderComponentShipGrinder\u003C\u003EMyRenderComponentShipGrinderBlade\u003C\u003EActor : IActivator, IActivator<MyRenderComponentShipGrinder.MyRenderComponentShipGrinderBlade>
      {
        object IActivator.CreateInstance() => (object) new MyRenderComponentShipGrinder.MyRenderComponentShipGrinderBlade();

        MyRenderComponentShipGrinder.MyRenderComponentShipGrinderBlade IActivator<MyRenderComponentShipGrinder.MyRenderComponentShipGrinderBlade>.CreateInstance() => new MyRenderComponentShipGrinder.MyRenderComponentShipGrinderBlade();
      }
    }

    private class Sandbox_Game_EntityComponents_Renders_MyRenderComponentShipGrinder\u003C\u003EActor : IActivator, IActivator<MyRenderComponentShipGrinder>
    {
      object IActivator.CreateInstance() => (object) new MyRenderComponentShipGrinder();

      MyRenderComponentShipGrinder IActivator<MyRenderComponentShipGrinder>.CreateInstance() => new MyRenderComponentShipGrinder();
    }
  }
}
