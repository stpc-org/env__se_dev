// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Entities.Blocks.MyHydrogenEngine
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using SpaceEngineers.Game.EntityComponents.Renders;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRageMath;
using VRageRender.Import;

namespace SpaceEngineers.Game.Entities.Blocks
{
  [MyCubeBlockType(typeof (MyObjectBuilder_HydrogenEngine))]
  public class MyHydrogenEngine : MyGasFueledPowerProducer
  {
    private bool m_renderAnimationEnabled = true;
    private List<MyHydrogenEngine.MyPistonSubpart> m_pistons = new List<MyHydrogenEngine.MyPistonSubpart>();
    private List<MyHydrogenEngine.MyRotatingSubpartSubpart> m_rotatingSubparts = new List<MyHydrogenEngine.MyRotatingSubpartSubpart>();

    public MyHydrogenEngineDefinition BlockDefinition => (MyHydrogenEngineDefinition) base.BlockDefinition;

    public MyRenderComponentHydrogenEngine Render => (MyRenderComponentHydrogenEngine) base.Render;

    protected override void OnStopWorking()
    {
      base.OnStopWorking();
      this.UpdateVisuals();
    }

    protected override void OnStartWorking()
    {
      base.OnStartWorking();
      this.UpdateVisuals();
    }

    public override void UpdateAfterSimulation100()
    {
      base.UpdateAfterSimulation100();
      if (Sync.IsDedicated)
        return;
      bool flag = Vector3D.DistanceSquared(MySector.MainCamera.Position, this.PositionComp.GetPosition()) < (double) this.BlockDefinition.AnimationVisibilityDistanceSq;
      if (flag == this.m_renderAnimationEnabled)
        return;
      this.m_renderAnimationEnabled = flag;
      this.UpdateVisuals();
    }

    private void UpdateVisuals()
    {
      float speed = 0.0f;
      if (this.m_renderAnimationEnabled && this.IsWorking)
        speed = this.BlockDefinition.AnimationSpeed;
      foreach (MyHydrogenEngine.MyPistonSubpart piston in this.m_pistons)
        piston.Render.SetSpeed(speed);
      foreach (MyHydrogenEngine.MyRotatingSubpartSubpart rotatingSubpart in this.m_rotatingSubparts)
        rotatingSubpart.Render.SetSpeed(speed);
    }

    protected override string GetDefaultEmissiveParts(byte index) => index != (byte) 0 ? (string) null : "Emissive2";

    public override void InitComponents()
    {
      this.Render = (MyRenderComponentBase) new MyRenderComponentHydrogenEngine();
      base.InitComponents();
    }

    public override void RefreshModels(string modelPath, string modelCollisionPath)
    {
      this.m_pistons.Clear();
      this.m_rotatingSubparts.Clear();
      base.RefreshModels(modelPath, modelCollisionPath);
      this.UpdateVisuals();
    }

    protected override MyEntitySubpart InstantiateSubpart(
      MyModelDummy subpartDummy,
      ref MyEntitySubpart.Data data)
    {
      string name = data.Name;
      if (name.Contains("Piston"))
      {
        MyHydrogenEngine.MyPistonSubpart myPistonSubpart = new MyHydrogenEngine.MyPistonSubpart();
        float num = 0.0f;
        float[] animationOffsets = this.BlockDefinition.PistonAnimationOffsets;
        if (animationOffsets != null && animationOffsets.Length != 0)
          num = animationOffsets[this.m_pistons.Count % animationOffsets.Length];
        myPistonSubpart.Render.AnimationOffset = num;
        this.m_pistons.Add(myPistonSubpart);
        return (MyEntitySubpart) myPistonSubpart;
      }
      if (!name.Contains("Propeller") && !name.Contains("Camshaft"))
        return base.InstantiateSubpart(subpartDummy, ref data);
      MyHydrogenEngine.MyRotatingSubpartSubpart rotatingSubpartSubpart = new MyHydrogenEngine.MyRotatingSubpartSubpart();
      this.m_rotatingSubparts.Add(rotatingSubpartSubpart);
      return (MyEntitySubpart) rotatingSubpartSubpart;
    }

    private class MyRotatingSubpartSubpart : MyEntitySubpart
    {
      public MyRenderComponentHydrogenEngine.MyRotatingSubpartRenderComponent Render => (MyRenderComponentHydrogenEngine.MyRotatingSubpartRenderComponent) base.Render;

      public override void InitComponents()
      {
        this.Render = (MyRenderComponentBase) new MyRenderComponentHydrogenEngine.MyRotatingSubpartRenderComponent();
        base.InitComponents();
      }
    }

    private class MyPistonSubpart : MyEntitySubpart
    {
      public MyRenderComponentHydrogenEngine.MyPistonRenderComponent Render => (MyRenderComponentHydrogenEngine.MyPistonRenderComponent) base.Render;

      public override void InitComponents()
      {
        this.Render = (MyRenderComponentBase) new MyRenderComponentHydrogenEngine.MyPistonRenderComponent();
        base.InitComponents();
      }
    }
  }
}
