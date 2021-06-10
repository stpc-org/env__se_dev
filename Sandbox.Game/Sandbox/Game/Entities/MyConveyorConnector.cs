// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyConveyorConnector
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems.Conveyors;
using VRage.Game;
using VRage.Game.Components;
using VRage.ModAPI;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Entities
{
  [MyCubeBlockType(typeof (MyObjectBuilder_ConveyorConnector))]
  public class MyConveyorConnector : MyCubeBlock, IMyConveyorSegmentBlock, Sandbox.ModAPI.IMyConveyorTube, VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyConveyorTube
  {
    private readonly MyConveyorSegment m_segment = new MyConveyorSegment();
    private bool m_working;
    private bool m_emissivitySet;
    private MyResourceStateEnum m_state;

    public override float MaxGlassDistSq => 22500f;

    public MyConveyorSegment ConveyorSegment => this.m_segment;

    public override void Init(MyObjectBuilder_CubeBlock builder, MyCubeGrid cubeGrid)
    {
      base.Init(builder, cubeGrid);
      this.NeedsUpdate = MyEntityUpdateEnum.EACH_100TH_FRAME;
      this.m_emissivitySet = false;
      this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyDebugRenderComponentDrawConveyorSegment(this.m_segment));
    }

    public void InitializeConveyorSegment()
    {
      MyConveyorLine.BlockLinePositionInformation[] blockLinePositions = MyConveyorLine.GetBlockLinePositions((MyCubeBlock) this);
      if (blockLinePositions.Length == 0)
        return;
      this.m_segment.Init((MyCubeBlock) this, this.PositionToGridCoords(blockLinePositions[0].Position).GetConnectingPosition(), this.PositionToGridCoords(blockLinePositions[1].Position).GetConnectingPosition(), blockLinePositions[0].LineType);
    }

    private ConveyorLinePosition PositionToGridCoords(
      ConveyorLinePosition position)
    {
      ConveyorLinePosition conveyorLinePosition = new ConveyorLinePosition();
      Matrix result = new Matrix();
      this.Orientation.GetMatrix(out result);
      Vector3 vector3 = Vector3.Transform(new Vector3(position.LocalGridPosition), result);
      conveyorLinePosition.LocalGridPosition = Vector3I.Round(vector3) + this.Position;
      conveyorLinePosition.Direction = this.Orientation.TransformDirection(position.Direction);
      return conveyorLinePosition;
    }

    public override void UpdateBeforeSimulation100()
    {
      if (this.m_segment.ConveyorLine == null)
        return;
      MyResourceStateEnum resourceStateEnum = this.m_segment.ConveyorLine.UpdateIsWorking();
      if (this.m_emissivitySet && this.m_working == this.m_segment.ConveyorLine.IsWorking && this.m_state == resourceStateEnum)
        return;
      this.m_working = this.m_segment.ConveyorLine.IsWorking;
      this.m_state = resourceStateEnum;
      this.SetEmissiveStateWorking();
    }

    public override bool SetEmissiveStateWorking()
    {
      if (!this.IsWorking)
        return false;
      if (this.m_state == MyResourceStateEnum.OverloadBlackout)
        return this.SetEmissiveState(MyCubeBlock.m_emissiveNames.Disabled, this.Render.RenderObjectIDs[0]);
      this.m_emissivitySet = true;
      if (this.m_working)
        return this.SetEmissiveState(MyCubeBlock.m_emissiveNames.Working, this.Render.RenderObjectIDs[0]);
      return this.m_state == MyResourceStateEnum.NoPower ? this.SetEmissiveState(MyCubeBlock.m_emissiveNames.Disabled, this.Render.RenderObjectIDs[0]) : this.SetEmissiveState(MyCubeBlock.m_emissiveNames.Warning, this.Render.RenderObjectIDs[0]);
    }

    public override bool SetEmissiveStateDisabled() => this.IsFunctional && !this.m_working ? this.SetEmissiveState(MyCubeBlock.m_emissiveNames.Warning, this.Render.RenderObjectIDs[0]) : this.SetEmissiveState(MyCubeBlock.m_emissiveNames.Disabled, this.Render.RenderObjectIDs[0]);

    public override void OnRemovedFromScene(object source)
    {
      base.OnRemovedFromScene(source);
      this.m_emissivitySet = false;
    }

    private class Sandbox_Game_Entities_MyConveyorConnector\u003C\u003EActor : IActivator, IActivator<MyConveyorConnector>
    {
      object IActivator.CreateInstance() => (object) new MyConveyorConnector();

      MyConveyorConnector IActivator<MyConveyorConnector>.CreateInstance() => new MyConveyorConnector();
    }
  }
}
