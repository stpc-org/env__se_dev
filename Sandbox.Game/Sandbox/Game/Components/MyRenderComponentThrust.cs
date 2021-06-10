// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Components.MyRenderComponentThrust
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.EntityComponents.Renders;
using Sandbox.Game.Multiplayer;
using Sandbox.RenderDirect.ActorComponents;
using System;
using VRage.Network;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Game.Components
{
  public class MyRenderComponentThrust : MyRenderComponentCubeBlockWithParentedSubpart
  {
    private float m_strength;
    private bool m_flamesEnabled;
    private float m_propellerSpeed;
    private MyThrust m_thrust;
    private bool m_flameAnimatorInitialized;

    public override void OnAddedToContainer()
    {
      base.OnAddedToContainer();
      this.m_thrust = this.Container.Entity as MyThrust;
    }

    public override void AddRenderObjects()
    {
      base.AddRenderObjects();
      float strength = this.m_strength;
      bool flamesEnabled = this.m_flamesEnabled;
      this.m_strength = 0.0f;
      this.m_propellerSpeed = 0.0f;
      this.m_flamesEnabled = false;
      this.m_flameAnimatorInitialized = false;
      this.UpdateFlameAnimatorData();
      if (!this.m_flameAnimatorInitialized)
        return;
      this.UpdateFlameProperties(flamesEnabled, strength);
    }

    public void UpdateFlameProperties(bool enabled, float strength)
    {
      if (this.m_thrust.CubeGrid.Physics == null || !this.m_flameAnimatorInitialized)
        return;
      bool flag = false;
      if ((double) this.m_strength != (double) strength)
      {
        flag = true;
        this.m_strength = strength;
      }
      if (this.m_flamesEnabled != enabled)
      {
        flag = true;
        this.m_flamesEnabled = enabled;
      }
      if (!flag)
        return;
      FloatData.Update<MyThrustFlameAnimator>(this.GetRenderObjectID(), this.m_flamesEnabled ? this.m_strength : -1f);
    }

    public void UpdateFlameAnimatorData()
    {
      if (this.m_thrust.CubeGrid.Physics == null || Sync.IsDedicated)
        return;
      uint renderObjectId = this.GetRenderObjectID();
      if (renderObjectId == uint.MaxValue)
        return;
      if (this.m_thrust.Flames.Count == 0)
      {
        if (!this.m_flameAnimatorInitialized)
          return;
        this.UpdateFlameProperties(false, 0.0f);
        this.m_flameAnimatorInitialized = false;
        MyRenderProxy.RemoveRenderComponent<MyThrustFlameAnimator>(renderObjectId);
      }
      else
      {
        this.m_flameAnimatorInitialized = true;
        MyRenderProxy.UpdateRenderComponent<FlameData, MyThrust>(renderObjectId, this.m_thrust, (Action<FlameData, MyThrust>) ((d, t) =>
        {
          MatrixD matrix = (MatrixD) ref t.PositionComp.LocalMatrixRef;
          d.LightPosition = Vector3D.TransformNormal(t.Flames[0].Position, matrix) + matrix.Translation;
          d.Flames = t.Flames;
          d.FlareSize = t.Flares.Size;
          d.Glares = t.Flares.SubGlares;
          d.GridScale = t.CubeGrid.GridScale;
          d.FlareIntensity = t.Flares.Intensity;
          d.FlamePointMaterial = t.FlamePointMaterial;
          d.FlameLengthMaterial = t.FlameLengthMaterial;
          d.GlareQuerySize = t.CubeGrid.GridSize / 2.5f;
          d.IdleColor = t.BlockDefinition.FlameIdleColor;
          d.FullColor = t.BlockDefinition.FlameFullColor;
          d.FlameLengthScale = t.BlockDefinition.FlameLengthScale;
        }));
      }
    }

    public void UpdatePropellerSpeed(float propellerSpeed)
    {
      if ((double) this.m_propellerSpeed == (double) propellerSpeed)
        return;
      this.m_propellerSpeed = propellerSpeed;
      ((MyRenderComponentThrust.MyPropellerRenderComponent) this.m_thrust.Propeller.Render).SendPropellerSpeed(propellerSpeed);
    }

    public class MyPropellerRenderComponent : MyParentedSubpartRenderComponent
    {
      public override void OnParented()
      {
        base.OnParented();
        MyThrust parent = (MyThrust) this.Entity.Parent;
        MyRenderProxy.UpdateRenderComponent<MyRotationAnimatorInitData, MyThrust>(this.GetRenderObjectID(), parent, (Action<MyRotationAnimatorInitData, MyThrust>) ((d, t) =>
        {
          MyThrustDefinition blockDefinition = t.BlockDefinition;
          float num = blockDefinition.PropellerFullSpeed * 6.283185f;
          d.SpinUpSpeed = num / blockDefinition.PropellerAcceleration;
          d.SpinDownSpeed = num / blockDefinition.PropellerDeceleration;
          d.RotationAxis = MyRotationAnimator.RotationAxis.AxisZ;
        }));
        this.SendPropellerSpeed(parent.Render.m_propellerSpeed);
      }

      public void SendPropellerSpeed(float speed) => FloatData.Update<MyRotationAnimator>(this.GetRenderObjectID(), speed);

      private class Sandbox_Game_Components_MyRenderComponentThrust\u003C\u003EMyPropellerRenderComponent\u003C\u003EActor : IActivator, IActivator<MyRenderComponentThrust.MyPropellerRenderComponent>
      {
        object IActivator.CreateInstance() => (object) new MyRenderComponentThrust.MyPropellerRenderComponent();

        MyRenderComponentThrust.MyPropellerRenderComponent IActivator<MyRenderComponentThrust.MyPropellerRenderComponent>.CreateInstance() => new MyRenderComponentThrust.MyPropellerRenderComponent();
      }
    }

    private class Sandbox_Game_Components_MyRenderComponentThrust\u003C\u003EActor : IActivator, IActivator<MyRenderComponentThrust>
    {
      object IActivator.CreateInstance() => (object) new MyRenderComponentThrust();

      MyRenderComponentThrust IActivator<MyRenderComponentThrust>.CreateInstance() => new MyRenderComponentThrust();
    }
  }
}
