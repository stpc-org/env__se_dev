// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.EntityComponents.Renders.MyShipDrillRenderComponent
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
  public class MyShipDrillRenderComponent : MyRenderComponentCubeBlockWithParentedSubpart
  {
    public class MyDrillHeadRenderComponent : MyParentedSubpartRenderComponent
    {
      private float m_speed;

      public override void OnParented()
      {
        base.OnParented();
        MyRenderProxy.UpdateRenderComponent<MyRotationAnimatorInitData, float>(this.GetRenderObjectID(), 25.13274f, (Action<MyRotationAnimatorInitData, float>) ((d, s) =>
        {
          d.SpinUpSpeed = s;
          d.SpinDownSpeed = s;
          d.RotationAxis = MyRotationAnimator.RotationAxis.AxisZ;
        }));
      }

      public void UpdateSpeed(float speed)
      {
        if ((double) this.m_speed == (double) speed)
          return;
        this.m_speed = speed;
        FloatData.Update<MyRotationAnimator>(this.GetRenderObjectID(), speed);
      }

      private class Sandbox_Game_EntityComponents_Renders_MyShipDrillRenderComponent\u003C\u003EMyDrillHeadRenderComponent\u003C\u003EActor : IActivator, IActivator<MyShipDrillRenderComponent.MyDrillHeadRenderComponent>
      {
        object IActivator.CreateInstance() => (object) new MyShipDrillRenderComponent.MyDrillHeadRenderComponent();

        MyShipDrillRenderComponent.MyDrillHeadRenderComponent IActivator<MyShipDrillRenderComponent.MyDrillHeadRenderComponent>.CreateInstance() => new MyShipDrillRenderComponent.MyDrillHeadRenderComponent();
      }
    }

    private class Sandbox_Game_EntityComponents_Renders_MyShipDrillRenderComponent\u003C\u003EActor : IActivator, IActivator<MyShipDrillRenderComponent>
    {
      object IActivator.CreateInstance() => (object) new MyShipDrillRenderComponent();

      MyShipDrillRenderComponent IActivator<MyShipDrillRenderComponent>.CreateInstance() => new MyShipDrillRenderComponent();
    }
  }
}
