// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.EntityComponents.Renders.MyRenderComponentWindTurbine
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.EntityComponents.Renders;
using Sandbox.RenderDirect.ActorComponents;
using SpaceEngineers.Game.Entities.Blocks;
using System;
using VRage.Game.Entity;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace SpaceEngineers.Game.EntityComponents.Renders
{
  public class MyRenderComponentWindTurbine : MyRenderComponentCubeBlockWithParentedSubpart
  {
    public class TurbineRenderComponent : MyParentedSubpartRenderComponent
    {
      private float m_speed;
      private Color m_color;
      private bool m_animatorInitialized;

      protected MyWindTurbineDefinition Definition => ((MyWindTurbine) this.Entity.Parent).BlockDefinition;

      public void SetSpeed(float speed)
      {
        if ((double) this.m_speed == (double) speed)
          return;
        this.m_speed = speed;
        this.SendSpeed();
      }

      public void SetColor(Color color)
      {
        if (!(this.m_color != color))
          return;
        this.m_color = color;
        this.SendColor();
      }

      private void SendSpeed()
      {
        if (!this.m_animatorInitialized)
          return;
        uint renderObjectId = this.GetRenderObjectID();
        if (renderObjectId == uint.MaxValue)
          return;
        FloatData.Update<MyRotationAnimator>(renderObjectId, this.m_speed);
      }

      private void SendColor()
      {
        if (this.GetRenderObjectID() == uint.MaxValue)
          return;
        this.Entity.SetEmissiveParts("Emissive", this.m_color, 1f);
      }

      public override void OnParented()
      {
        if (((MyEntity) this.Entity.Parent.Parent).IsPreview)
          return;
        MyRenderProxy.UpdateRenderComponent<MyRotationAnimatorInitData, MyRenderComponentWindTurbine.TurbineRenderComponent>(this.GetRenderObjectID(), this, (Action<MyRotationAnimatorInitData, MyRenderComponentWindTurbine.TurbineRenderComponent>) ((message, thiz) =>
        {
          MyWindTurbineDefinition definition = this.Definition;
          message.SpinUpSpeed = definition.TurbineSpinUpSpeed;
          message.SpinDownSpeed = definition.TurbineSpinDownSpeed;
          message.RotationAxis = MyRotationAnimator.RotationAxis.AxisY;
        }));
        base.OnParented();
        this.m_animatorInitialized = true;
        this.SendSpeed();
        this.SendColor();
      }
    }
  }
}
