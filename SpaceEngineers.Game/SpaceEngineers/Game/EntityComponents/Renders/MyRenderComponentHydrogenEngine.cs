// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.EntityComponents.Renders.MyRenderComponentHydrogenEngine
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.EntityComponents.Renders;
using Sandbox.RenderDirect.ActorComponents;
using SpaceEngineers.Game.Entities.Blocks;
using System;
using VRage.Game.Entity;
using VRage.Render.Scene.Components;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace SpaceEngineers.Game.EntityComponents.Renders
{
  public class MyRenderComponentHydrogenEngine : MyRenderComponentCubeBlockWithParentedSubpart
  {
    public class MyHydrogenEngineSubpartRenderComponent<TComponent> : MyParentedSubpartRenderComponent
      where TComponent : MyRenderDirectComponent
    {
      private float m_speed;
      private bool m_animatorInitialized;

      protected MyHydrogenEngineDefinition Definition => ((MyHydrogenEngine) this.Entity.Parent).BlockDefinition;

      public void SetSpeed(float speed)
      {
        if ((double) speed == (double) this.m_speed)
          return;
        this.m_speed = speed;
        this.SendSpeed();
      }

      private void SendSpeed()
      {
        if (!this.m_animatorInitialized)
          return;
        uint renderObjectId = this.GetRenderObjectID();
        if (renderObjectId == uint.MaxValue)
          return;
        FloatData.Update<TComponent>(renderObjectId, this.m_speed);
      }

      public override void OnParented()
      {
        base.OnParented();
        this.m_animatorInitialized = true;
        this.SendSpeed();
      }

      protected void FillAnimationParams(MySpinupAnimatorInitData data)
      {
        MyHydrogenEngineDefinition definition = this.Definition;
        data.SpinUpSpeed = definition.AnimationSpinUpSpeed;
        data.SpinDownSpeed = definition.AnimationSpinDownSpeed;
      }
    }

    public class MyRotatingSubpartRenderComponent : MyRenderComponentHydrogenEngine.MyHydrogenEngineSubpartRenderComponent<MyRotationAnimator>
    {
      public override void OnParented()
      {
        if (((MyEntity) this.Entity.Parent.Parent).IsPreview)
          return;
        MyRenderProxy.UpdateRenderComponent<MyRotationAnimatorInitData, MyRenderComponentHydrogenEngine.MyRotatingSubpartRenderComponent>(this.GetRenderObjectID(), this, (Action<MyRotationAnimatorInitData, MyRenderComponentHydrogenEngine.MyRotatingSubpartRenderComponent>) ((message, thiz) =>
        {
          thiz.FillAnimationParams((MySpinupAnimatorInitData) message);
          message.RotationAxis = MyRotationAnimator.RotationAxis.AxisZ;
        }));
        base.OnParented();
      }
    }

    public class MyPistonRenderComponent : MyRenderComponentHydrogenEngine.MyHydrogenEngineSubpartRenderComponent<MyTranslationAnimator>
    {
      public float AnimationOffset { get; set; }

      public override void OnParented()
      {
        if (((MyEntity) this.Entity.Parent.Parent).IsPreview)
          return;
        MyRenderProxy.UpdateRenderComponent<MyTranslationAnimatorInitData, MyRenderComponentHydrogenEngine.MyPistonRenderComponent>(this.GetRenderObjectID(), this, (Action<MyTranslationAnimatorInitData, MyRenderComponentHydrogenEngine.MyPistonRenderComponent>) ((message, thiz) =>
        {
          thiz.FillAnimationParams((MySpinupAnimatorInitData) message);
          message.AnimationOffset = this.AnimationOffset;
          message.TranslationAxis = Base6Directions.Direction.Up;
          message.MinPosition = thiz.Definition.PistonAnimationMin;
          message.MaxPosition = thiz.Definition.PistonAnimationMax;
          thiz.GetCullObjectRelativeMatrix(out message.BaseRelativeTransform);
        }));
        base.OnParented();
      }
    }
  }
}
