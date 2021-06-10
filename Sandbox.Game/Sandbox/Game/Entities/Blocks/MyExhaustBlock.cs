// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Blocks.MyExhaustBlock
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using VRage.Game;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender.Import;

namespace Sandbox.Game.Entities.Blocks
{
  [MyCubeBlockType(typeof (MyObjectBuilder_ExhaustBlock))]
  public class MyExhaustBlock : MyFunctionalBlock, IMyExhaustBlock
  {
    private List<MyExhaustBlock.MyExhaustPipe> m_exhaustPipes = new List<MyExhaustBlock.MyExhaustPipe>();
    private Dictionary<string, MatrixD> m_exhaustDummies = new Dictionary<string, MatrixD>();
    private readonly VRage.Sync.Sync<string, SyncDirection.BothWays> m_exhaustEffect;
    private readonly VRage.Sync.Sync<float, SyncDirection.BothWays> m_powerDependency;
    private float m_currentGridPower;
    private float m_lastGridPower;

    public MyExhaustBlockDefinition BlockDefinition => (MyExhaustBlockDefinition) base.BlockDefinition;

    public MyExhaustBlock()
    {
      this.CreateTerminalControls();
      this.m_exhaustEffect.SetLocalValue("");
      this.m_exhaustEffect.ValueChanged += new Action<SyncBase>(this.exhaustEffect_ValueChanged);
      this.m_powerDependency.ValueChanged += (Action<SyncBase>) (x => this.UpdateEffectValues());
    }

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      MyResourceSinkComponent resourceSinkComponent = new MyResourceSinkComponent();
      resourceSinkComponent.Init(MyStringHash.GetOrCompute("Utility"), this.BlockDefinition.RequiredPowerInput, (Func<float>) (() => !this.Enabled || !this.IsFunctional ? 0.0f : this.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId)));
      resourceSinkComponent.IsPoweredChanged += new Action(this.Receiver_IsPoweredChanged);
      this.ResourceSink = resourceSinkComponent;
      this.m_exhaustPipes.Clear();
      base.Init(objectBuilder, cubeGrid);
      this.SlimBlock.ComponentStack.IsFunctionalChanged += new Action(this.ComponentStack_IsFunctionalChanged);
      this.IsWorkingChanged += new Action<MyCubeBlock>(this.CubeBlock_OnWorkingChanged);
      MyObjectBuilder_ExhaustBlock builderExhaustBlock = (MyObjectBuilder_ExhaustBlock) objectBuilder;
      string newValue = builderExhaustBlock.ExhaustEffect;
      if (string.IsNullOrEmpty(newValue))
      {
        IEnumerable<MyExhaustEffectDefinition> allDefinitions = MyDefinitionManager.Static.GetAllDefinitions<MyExhaustEffectDefinition>();
        if (allDefinitions != null)
          newValue = allDefinitions.First<MyExhaustEffectDefinition>().Id.SubtypeName;
      }
      this.m_exhaustEffect.SetLocalValue(newValue);
      this.m_powerDependency.SetLocalValue(builderExhaustBlock.PowerDependency);
      this.UpdatePipes();
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
    }

    public override void UpdateOnceBeforeFrame()
    {
      base.UpdateOnceBeforeFrame();
      this.StopEffects();
      this.StartEffects();
    }

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MyExhaustBlock>())
        return;
      base.CreateTerminalControls();
      MyTerminalControlCombobox<MyExhaustBlock> terminalControlCombobox = new MyTerminalControlCombobox<MyExhaustBlock>("EffectsCombo", MySpaceTexts.BlockPropertyTitle_ExhaustEffect, MySpaceTexts.Blank);
      terminalControlCombobox.ComboBoxContent = new Action<List<MyTerminalControlComboBoxItem>>(this.FillEffectsCombo);
      terminalControlCombobox.Getter = (MyTerminalValueControl<MyExhaustBlock, long>.GetterDelegate) (x => (long) (int) MyStringHash.GetOrCompute((string) x.m_exhaustEffect));
      terminalControlCombobox.Setter = (MyTerminalValueControl<MyExhaustBlock, long>.SetterDelegate) ((x, v) => x.m_exhaustEffect.Value = MyStringHash.TryGet((int) v).ToString());
      MyTerminalControlFactory.AddControl<MyExhaustBlock>((MyTerminalControl<MyExhaustBlock>) terminalControlCombobox);
      MyTerminalControlSlider<MyExhaustBlock> slider = new MyTerminalControlSlider<MyExhaustBlock>("PowerDependency", MySpaceTexts.BlockPropertyTitle_PowerDependency, MySpaceTexts.Blank);
      slider.SetLimits((MyTerminalValueControl<MyExhaustBlock, float>.GetterDelegate) (x => 0.0f), (MyTerminalValueControl<MyExhaustBlock, float>.GetterDelegate) (x => 1f));
      slider.DefaultValue = new float?(0.0f);
      slider.Getter = (MyTerminalValueControl<MyExhaustBlock, float>.GetterDelegate) (x => (float) x.m_powerDependency);
      slider.Setter = (MyTerminalValueControl<MyExhaustBlock, float>.SetterDelegate) ((x, v) => x.m_powerDependency.Value = v);
      slider.Writer = (MyTerminalControl<MyExhaustBlock>.WriterDelegate) ((x, result) => result.Append(MyValueFormatter.GetFormatedFloat(x.m_powerDependency.Value * 100f, 2)).Append(" %"));
      slider.EnableActions<MyExhaustBlock>();
      MyTerminalControlFactory.AddControl<MyExhaustBlock>((MyTerminalControl<MyExhaustBlock>) slider);
    }

    private void FillEffectsCombo(List<MyTerminalControlComboBoxItem> list)
    {
      IEnumerable<MyExhaustEffectDefinition> allDefinitions = MyDefinitionManager.Static.GetAllDefinitions<MyExhaustEffectDefinition>();
      if (allDefinitions == null)
        return;
      foreach (MyExhaustEffectDefinition effectDefinition in allDefinitions)
        list.Add(new MyTerminalControlComboBoxItem()
        {
          Key = (long) MyStringHash.GetOrCompute(effectDefinition.Id.SubtypeName).GetHashCode(),
          Value = MyStringId.GetOrCompute(effectDefinition.Id.SubtypeName)
        });
    }

    private void exhaustEffect_ValueChanged(SyncBase obj)
    {
      this.StopEffects();
      this.UpdatePipes();
      this.StartEffects();
    }

    public void SelectEffect(string name) => this.m_exhaustEffect.Value = name;

    private void UpdatePipes()
    {
      this.m_exhaustPipes.Clear();
      if (string.IsNullOrEmpty(this.m_exhaustEffect.Value))
        return;
      MyExhaustEffectDefinition definition = MyDefinitionManager.Static.GetDefinition<MyExhaustEffectDefinition>(this.m_exhaustEffect.Value);
      if (definition == null)
        return;
      foreach (MyObjectBuilder_ExhaustEffectDefinition.Pipe exhaustPipe in definition.ExhaustPipes)
        this.m_exhaustPipes.Add(new MyExhaustBlock.MyExhaustPipe()
        {
          Data = exhaustPipe
        });
    }

    public void StopEffects()
    {
      foreach (MyExhaustBlock.MyExhaustPipe exhaustPipe in this.m_exhaustPipes)
      {
        if (exhaustPipe.Effect != null)
        {
          exhaustPipe.Effect.Stop(false);
          exhaustPipe.Effect = (MyParticleEffect) null;
        }
      }
    }

    public void StartEffects()
    {
      if (!this.IsWorking || !this.Enabled || (this.IsPreview || this.CubeGrid.IsPreview) || Sandbox.Game.Multiplayer.Sync.IsDedicated)
        return;
      Vector3D position = this.PositionComp.GetPosition();
      foreach (MyExhaustBlock.MyExhaustPipe exhaustPipe in this.m_exhaustPipes)
      {
        if (this.m_exhaustDummies.ContainsKey(exhaustPipe.Data.Dummy) && !string.IsNullOrEmpty(exhaustPipe.Data.Effect))
        {
          MatrixD effectMatrix = this.m_exhaustDummies[exhaustPipe.Data.Dummy] * this.PositionComp.LocalMatrix;
          MyParticlesManager.TryCreateParticleEffect(exhaustPipe.Data.Effect, ref effectMatrix, ref position, this.Render.ParentIDs[0], out exhaustPipe.Effect);
          if (exhaustPipe.Effect != null)
            this.UpdateEffectValues(exhaustPipe);
        }
      }
    }

    private void UpdateEffectValues(MyExhaustBlock.MyExhaustPipe pipe)
    {
      if (pipe.Effect == null)
        return;
      MyObjectBuilder_ExhaustEffectDefinition.Pipe data = pipe.Data;
      pipe.Effect.UserScale = (double) data.EffectIntensity > 0.0 ? data.EffectIntensity : 1f;
      float num = (double) this.m_powerDependency.Value > 0.0 ? MathHelper.Clamp(this.m_currentGridPower / this.m_powerDependency.Value, 0.0f, 1f) : 1f;
      pipe.Effect.UserFadeMultiplier = num;
      pipe.Effect.UserRadiusMultiplier = (double) data.PowerToRadius != 0.0 ? num * data.PowerToRadius : num;
      pipe.Effect.UserBirthMultiplier = (double) data.PowerToBirth != 0.0 ? num * data.PowerToBirth : num;
      if ((double) data.PowerToVelocity == 0.0)
        pipe.Effect.UserVelocityMultiplier = num;
      else
        pipe.Effect.UserVelocityMultiplier = num * data.PowerToVelocity;
    }

    public override void OnModelChange()
    {
      base.OnModelChange();
      this.StopEffects();
      this.m_exhaustDummies.Clear();
      if (this.IsBuilt)
      {
        foreach (KeyValuePair<string, MyModelDummy> dummy in this.Model.Dummies)
          this.m_exhaustDummies.Add(dummy.Key, MatrixD.Normalize((MatrixD) ref dummy.Value.Matrix));
      }
      this.StartEffects();
    }

    protected override bool CheckIsWorking() => this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) && base.CheckIsWorking();

    private void Receiver_IsPoweredChanged()
    {
      this.UpdateIsWorking();
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    private void ComponentStack_IsFunctionalChanged()
    {
      this.ResourceSink.Update();
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    private void CubeBlock_OnWorkingChanged(MyCubeBlock block) => this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;

    protected override void OnEnabledChanged()
    {
      this.ResourceSink.Update();
      base.OnEnabledChanged();
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    protected override void Closing()
    {
      base.Closing();
      this.StopEffects();
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_ExhaustBlock builderCubeBlock = (MyObjectBuilder_ExhaustBlock) base.GetObjectBuilderCubeBlock(copy);
      builderCubeBlock.ExhaustEffect = (string) this.m_exhaustEffect;
      builderCubeBlock.PowerDependency = (float) this.m_powerDependency;
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    private void UpdateEffectValues()
    {
      foreach (MyExhaustBlock.MyExhaustPipe exhaustPipe in this.m_exhaustPipes)
        this.UpdateEffectValues(exhaustPipe);
    }

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      this.m_currentGridPower = 0.0f;
      MyGridResourceDistributorSystem resourceDistributor = this.CubeGrid.GridSystems.ResourceDistributor;
      if (resourceDistributor != null)
      {
        float max = resourceDistributor.MaxAvailableResourceByType(MyResourceDistributorComponent.ElectricityId);
        float num = MyMath.Clamp(resourceDistributor.TotalRequiredInputByType(MyResourceDistributorComponent.ElectricityId), 0.0f, max);
        if ((double) max > 0.0)
          this.m_currentGridPower = num / max;
      }
      foreach (MyExhaustBlock.MyExhaustPipe exhaustPipe in this.m_exhaustPipes)
      {
        if (exhaustPipe.Effect != null && exhaustPipe.Effect.GetName() != exhaustPipe.Data.Effect)
          this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      }
      if ((double) this.m_lastGridPower != (double) this.m_currentGridPower)
      {
        this.UpdateEffectValues();
        this.m_lastGridPower = this.m_currentGridPower;
      }
      int count = this.m_exhaustPipes.Count;
    }

    private class MyExhaustPipe
    {
      public MyParticleEffect Effect;
      public MyObjectBuilder_ExhaustEffectDefinition.Pipe Data;
    }

    protected class m_exhaustEffect\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<string, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<string, SyncDirection.BothWays>(obj1, obj2));
        ((MyExhaustBlock) obj0).m_exhaustEffect = (VRage.Sync.Sync<string, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_powerDependency\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.BothWays>(obj1, obj2));
        ((MyExhaustBlock) obj0).m_powerDependency = (VRage.Sync.Sync<float, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    private class Sandbox_Game_Entities_Blocks_MyExhaustBlock\u003C\u003EActor : IActivator, IActivator<MyExhaustBlock>
    {
      object IActivator.CreateInstance() => (object) new MyExhaustBlock();

      MyExhaustBlock IActivator<MyExhaustBlock>.CreateInstance() => new MyExhaustBlock();
    }
  }
}
