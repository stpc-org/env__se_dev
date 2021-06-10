// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyAdvancedDoor
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender.Import;

namespace Sandbox.Game.Entities
{
  [MyCubeBlockType(typeof (MyObjectBuilder_AdvancedDoor))]
  [MyTerminalInterface(new System.Type[] {typeof (Sandbox.ModAPI.IMyAdvancedDoor), typeof (Sandbox.ModAPI.Ingame.IMyAdvancedDoor)})]
  public class MyAdvancedDoor : MyDoorBase, Sandbox.ModAPI.IMyAdvancedDoor, Sandbox.ModAPI.IMyDoor, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyDoor, Sandbox.ModAPI.Ingame.IMyAdvancedDoor
  {
    private const float CLOSED_DISSASEMBLE_RATIO = 3.3f;
    private static readonly float EPSILON = 1E-09f;
    private int m_lastUpdateTime;
    private float m_time;
    private float m_totalTime = 99999f;
    private bool m_stateChange;
    private readonly List<MyEntitySubpart> m_subparts = new List<MyEntitySubpart>();
    private readonly List<int> m_subpartIDs = new List<int>();
    private readonly List<float> m_currentOpening = new List<float>();
    private readonly List<bool> m_currentlyAtLimit = new List<bool>();
    private readonly List<float> m_currentSpeed = new List<float>();
    private readonly List<MyEntity3DSoundEmitter> m_emitter = new List<MyEntity3DSoundEmitter>();
    private readonly List<Vector3> m_hingePosition = new List<Vector3>();
    private readonly List<MyObjectBuilder_AdvancedDoorDefinition.Opening> m_openingSequence = new List<MyObjectBuilder_AdvancedDoorDefinition.Opening>();
    private Matrix[] transMat = new Matrix[1];
    private Matrix[] rotMat = new Matrix[1];
    private int m_sequenceCount;
    private int m_subpartCount;
    private bool[] m_isSubpartAtLimits;

    public event Action<bool> DoorStateChanged;

    public event Action<Sandbox.ModAPI.IMyDoor, bool> OnDoorStateChanged;

    protected override bool CheckIsWorking() => this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) && base.CheckIsWorking();

    public override float DisassembleRatio => base.DisassembleRatio * (this.Open ? 1f : 3.3f);

    public MyAdvancedDoor()
    {
      this.m_subparts.Clear();
      this.m_subpartIDs.Clear();
      this.m_currentOpening.Clear();
      this.m_currentlyAtLimit.Clear();
      this.m_currentSpeed.Clear();
      this.m_emitter.Clear();
      this.m_hingePosition.Clear();
      this.m_openingSequence.Clear();
      this.m_open.ValueChanged += (Action<SyncBase>) (x => this.OnStateChange());
    }

    public override void UpdateVisual()
    {
      base.UpdateVisual();
      this.UpdateEmissivity();
    }

    private void UpdateEmissivity()
    {
      if (this.Enabled && this.ResourceSink != null && this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
      {
        MyCubeBlock.UpdateEmissiveParts(this.Render.RenderObjectIDs[0], 1f, Color.Green, Color.White);
        this.OnStateChange();
      }
      else
        MyCubeBlock.UpdateEmissiveParts(this.Render.RenderObjectIDs[0], 0.0f, Color.Red, Color.White);
    }

    DoorStatus Sandbox.ModAPI.Ingame.IMyDoor.Status
    {
      get
      {
        float openRatio = this.OpenRatio;
        return (bool) this.m_open ? (1.0 - (double) openRatio >= (double) MyAdvancedDoor.EPSILON ? DoorStatus.Opening : DoorStatus.Open) : ((double) openRatio >= (double) MyAdvancedDoor.EPSILON ? DoorStatus.Closing : DoorStatus.Closed);
      }
    }

    bool Sandbox.ModAPI.IMyDoor.IsFullyClosed => this.FullyClosed;

    [Obsolete("Use Sandbox.ModAPI.IMyDoor.IsFullyClosed")]
    public bool FullyClosed
    {
      get
      {
        for (int index = 0; index < this.m_currentOpening.Count; ++index)
        {
          if ((double) this.m_currentOpening[index] != 0.0)
            return false;
        }
        return true;
      }
    }

    public bool FullyOpen
    {
      get
      {
        for (int index = 0; index < this.m_currentOpening.Count; ++index)
        {
          if ((double) this.m_openingSequence[index].MaxOpen != (double) this.m_currentOpening[index])
            return false;
        }
        return true;
      }
    }

    public float OpenRatio
    {
      get
      {
        for (int index = 0; index < this.m_currentOpening.Count; ++index)
        {
          if ((double) this.m_currentOpening[index] > 0.0)
            return this.m_currentOpening[index];
        }
        return 0.0f;
      }
    }

    void Sandbox.ModAPI.Ingame.IMyDoor.OpenDoor()
    {
      if (!this.IsWorking)
        return;
      switch (((Sandbox.ModAPI.Ingame.IMyDoor) this).Status)
      {
        case DoorStatus.Opening:
        case DoorStatus.Open:
          break;
        default:
          ((Sandbox.ModAPI.Ingame.IMyDoor) this).ToggleDoor();
          break;
      }
    }

    void Sandbox.ModAPI.Ingame.IMyDoor.CloseDoor()
    {
      if (!this.IsWorking)
        return;
      switch (((Sandbox.ModAPI.Ingame.IMyDoor) this).Status)
      {
        case DoorStatus.Closing:
        case DoorStatus.Closed:
          break;
        default:
          ((Sandbox.ModAPI.Ingame.IMyDoor) this).ToggleDoor();
          break;
      }
    }

    void Sandbox.ModAPI.Ingame.IMyDoor.ToggleDoor()
    {
      if (!this.IsWorking)
        return;
      this.SetOpenRequest(!this.Open, this.OwnerId);
    }

    public float OpeningSpeed
    {
      get
      {
        for (int index = 0; index < this.m_currentSpeed.Count; ++index)
        {
          if ((double) this.m_currentSpeed[index] > 0.0)
            return this.m_currentSpeed[index];
        }
        return 0.0f;
      }
    }

    private MyAdvancedDoorDefinition BlockDefinition => (MyAdvancedDoorDefinition) base.BlockDefinition;

    private static void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MyAdvancedDoor>())
        return;
      MyTerminalControlOnOffSwitch<MyAdvancedDoor> onOff = new MyTerminalControlOnOffSwitch<MyAdvancedDoor>("Open", MySpaceTexts.Blank, on: new MyStringId?(MySpaceTexts.BlockAction_DoorOpen), off: new MyStringId?(MySpaceTexts.BlockAction_DoorClosed));
      onOff.Getter = (MyTerminalValueControl<MyAdvancedDoor, bool>.GetterDelegate) (x => x.Open);
      onOff.Setter = (MyTerminalValueControl<MyAdvancedDoor, bool>.SetterDelegate) ((x, v) => x.SetOpenRequest(v, x.OwnerId));
      onOff.EnableToggleAction<MyAdvancedDoor>();
      onOff.EnableOnOffActions<MyAdvancedDoor>();
      MyTerminalControlFactory.AddControl<MyAdvancedDoor>((MyTerminalControl<MyAdvancedDoor>) onOff);
    }

    private void GetOpenCloseStatus(out bool fullyOpen, out bool fullyClosed)
    {
      fullyOpen = true;
      fullyClosed = true;
      for (int index = 0; index < this.m_currentOpening.Count; ++index)
      {
        if ((double) this.m_currentOpening[index] > 0.0)
          fullyClosed = false;
        if ((double) this.m_currentOpening[index] < (double) this.m_openingSequence[index].MaxOpen)
          fullyOpen = false;
      }
    }

    private void OnStateChange()
    {
      for (int index = 0; index < this.m_openingSequence.Count; ++index)
      {
        float speed = this.m_openingSequence[index].Speed;
        this.m_currentSpeed[index] = (bool) this.m_open ? speed : -speed;
      }
      this.ResourceSink.Update();
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME;
      this.m_lastUpdateTime = MySandboxGame.TotalGamePlayTimeInMilliseconds - 1;
      bool isPowered = this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId);
      this.UpdateCurrentOpening(isPowered);
      this.UpdateDoorPosition(isPowered, true);
      if ((bool) this.m_open)
      {
        this.DoorStateChanged.InvokeIfNotNull<bool>((bool) this.m_open);
        this.OnDoorStateChanged.InvokeIfNotNull<Sandbox.ModAPI.IMyDoor, bool>((Sandbox.ModAPI.IMyDoor) this, (bool) this.m_open);
      }
      this.m_stateChange = true;
    }

    protected override void OnEnabledChanged()
    {
      this.ResourceSink.Update();
      base.OnEnabledChanged();
    }

    public override void OnBuildSuccess(long builtBy, bool instantBuild)
    {
      this.ResourceSink.Update();
      this.UpdateHavokCollisionSystemID(this.CubeGrid.GetPhysicsBody().HavokCollisionSystemID);
      base.OnBuildSuccess(builtBy, instantBuild);
    }

    public override void Init(MyObjectBuilder_CubeBlock builder, MyCubeGrid cubeGrid)
    {
      MyResourceSinkComponent resourceSinkComponent = new MyResourceSinkComponent();
      resourceSinkComponent.Init(this.BlockDefinition.ResourceSinkGroup, this.BlockDefinition.PowerConsumptionMoving, new Func<float>(this.UpdatePowerInput));
      this.ResourceSink = resourceSinkComponent;
      base.Init(builder, cubeGrid);
      this.m_open.SetLocalValue(((MyObjectBuilder_AdvancedDoor) builder).Open);
      resourceSinkComponent.IsPoweredChanged += new Action(this.Receiver_IsPoweredChanged);
      resourceSinkComponent.Update();
      bool isPowered = this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId);
      if (!this.Enabled || !isPowered)
        this.UpdateDoorPosition(isPowered, true);
      this.OnStateChange();
      if ((bool) this.m_open)
        this.UpdateDoorPosition(isPowered, true);
      this.SlimBlock.ComponentStack.IsFunctionalChanged += new Action(this.ComponentStack_IsFunctionalChanged);
      this.ResourceSink.Update();
    }

    private MyEntitySubpart LoadSubpartFromName(string name)
    {
      MyEntitySubpart myEntitySubpart1;
      this.Subparts.TryGetValue(name, out myEntitySubpart1);
      if (myEntitySubpart1 != null)
        return myEntitySubpart1;
      MyEntitySubpart myEntitySubpart2 = new MyEntitySubpart();
      string model = Path.Combine(Path.GetDirectoryName(this.Model.AssetName), name) + ".mwm";
      myEntitySubpart2.Render.EnableColorMaskHsv = this.Render.EnableColorMaskHsv;
      myEntitySubpart2.Render.ColorMaskHsv = this.Render.ColorMaskHsv;
      myEntitySubpart2.Render.TextureChanges = this.Render.TextureChanges;
      myEntitySubpart2.Render.MetalnessColorable = this.Render.MetalnessColorable;
      myEntitySubpart2.Init((StringBuilder) null, model, (MyEntity) this, new float?());
      this.Subparts[name] = myEntitySubpart2;
      if (this.InScene)
        myEntitySubpart2.OnAddedToScene((object) this);
      return myEntitySubpart2;
    }

    private void InitSubparts()
    {
      if (!this.CubeGrid.CreatePhysics)
        return;
      this.m_subparts.Clear();
      this.m_subpartIDs.Clear();
      this.m_currentOpening.Clear();
      this.m_currentlyAtLimit.Clear();
      this.m_currentSpeed.Clear();
      this.m_emitter.Clear();
      this.m_hingePosition.Clear();
      this.m_openingSequence.Clear();
      for (int index = 0; index < this.BlockDefinition.Subparts.Length; ++index)
      {
        MyEntitySubpart myEntitySubpart = this.LoadSubpartFromName(this.BlockDefinition.Subparts[index].Name);
        if (myEntitySubpart != null)
        {
          this.m_subparts.Add(myEntitySubpart);
          if (!this.BlockDefinition.Subparts[index].PivotPosition.HasValue)
          {
            MyModelBone myModelBone = ((IEnumerable<MyModelBone>) myEntitySubpart.Model.Bones).First<MyModelBone>((Func<MyModelBone, bool>) (b => !b.Name.Contains("Root")));
            if (myModelBone != null)
              this.m_hingePosition.Add(myModelBone.Transform.Translation);
          }
          else
            this.m_hingePosition.Add((Vector3) this.BlockDefinition.Subparts[index].PivotPosition.Value);
        }
      }
      int length = this.BlockDefinition.OpeningSequence.Length;
      for (int index1 = 0; index1 < length; ++index1)
      {
        if (!string.IsNullOrEmpty(this.BlockDefinition.OpeningSequence[index1].IDs))
        {
          string[] strArray1 = this.BlockDefinition.OpeningSequence[index1].IDs.Split(',');
          for (int index2 = 0; index2 < strArray1.Length; ++index2)
          {
            string[] strArray2 = strArray1[index2].Split('-');
            if (strArray2.Length == 2)
            {
              for (int int32 = Convert.ToInt32(strArray2[0]); int32 <= Convert.ToInt32(strArray2[1]); ++int32)
              {
                this.m_openingSequence.Add(this.BlockDefinition.OpeningSequence[index1]);
                this.m_subpartIDs.Add(int32);
              }
            }
            else
            {
              this.m_openingSequence.Add(this.BlockDefinition.OpeningSequence[index1]);
              this.m_subpartIDs.Add(Convert.ToInt32(strArray1[index2]));
            }
          }
        }
        else
        {
          this.m_openingSequence.Add(this.BlockDefinition.OpeningSequence[index1]);
          this.m_subpartIDs.Add(this.BlockDefinition.OpeningSequence[index1].ID);
        }
      }
      for (int index = 0; index < this.m_openingSequence.Count; ++index)
      {
        this.m_currentOpening.Add(0.0f);
        this.m_currentlyAtLimit.Add(false);
        this.m_currentSpeed.Add(0.0f);
        this.m_emitter.Add(new MyEntity3DSoundEmitter((MyEntity) this, true));
        if ((double) this.m_openingSequence[index].MaxOpen < 0.0)
        {
          this.m_openingSequence[index].MaxOpen *= -1f;
          this.m_openingSequence[index].InvertRotation = !this.m_openingSequence[index].InvertRotation;
        }
      }
      this.m_sequenceCount = this.m_openingSequence.Count;
      this.m_subpartCount = this.m_subparts.Count;
      this.m_isSubpartAtLimits = new bool[this.m_subpartCount];
      for (int index = 0; index > this.m_isSubpartAtLimits.Length; ++index)
        this.m_isSubpartAtLimits[index] = false;
      Array.Resize<Matrix>(ref this.transMat, this.m_subpartCount);
      Array.Resize<Matrix>(ref this.rotMat, this.m_subpartCount);
      this.UpdateDoorPosition(this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId), true);
      if (this.CubeGrid.Projector != null)
        return;
      foreach (MyEntitySubpart subpart in this.m_subparts)
      {
        if (subpart.Physics != null)
        {
          subpart.Physics.Close();
          subpart.Physics = (MyPhysicsComponentBase) null;
        }
        if (subpart != null && subpart.Physics == null && (subpart.ModelCollision.HavokCollisionShapes != null && subpart.ModelCollision.HavokCollisionShapes.Length != 0))
        {
          HkShape[] havokCollisionShapes = subpart.ModelCollision.HavokCollisionShapes;
          HkListShape hkListShape = new HkListShape(havokCollisionShapes, havokCollisionShapes.Length, HkReferencePolicy.None);
          subpart.Physics = (MyPhysicsComponentBase) new MyPhysicsBody((VRage.ModAPI.IMyEntity) subpart, RigidBodyFlag.RBF_KINEMATIC | RigidBodyFlag.RBF_UNLOCKED_SPEEDS);
          subpart.Physics.IsPhantom = false;
          (subpart.Physics as MyPhysicsBody).CreateFromCollisionObject((HkShape) hkListShape, Vector3.Zero, this.WorldMatrix);
          subpart.Physics.Enabled = true;
          hkListShape.Base.RemoveReference();
        }
      }
      this.CubeGrid.OnHavokSystemIDChanged -= new Action<int>(this.CubeGrid_HavokSystemIDChanged);
      this.CubeGrid.OnHavokSystemIDChanged += new Action<int>(this.CubeGrid_HavokSystemIDChanged);
      this.CubeGrid.OnPhysicsChanged -= new Action<MyEntity>(this.CubeGrid_OnPhysicsChanged);
      this.CubeGrid.OnPhysicsChanged += new Action<MyEntity>(this.CubeGrid_OnPhysicsChanged);
      if (this.CubeGrid.Physics == null)
        return;
      this.UpdateHavokCollisionSystemID(this.CubeGrid.GetPhysicsBody().HavokCollisionSystemID);
    }

    private void CubeGrid_OnPhysicsChanged(MyEntity obj)
    {
      if (this.m_subparts == null || this.m_subparts.Count == 0 || (obj.Physics == null || this.m_subparts[0].Physics == null))
        return;
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    public override void UpdateOnceBeforeFrame()
    {
      base.UpdateOnceBeforeFrame();
      this.UpdateHavokCollisionSystemID((MyEntity) this.CubeGrid);
    }

    private void UpdateHavokCollisionSystemID(MyEntity obj)
    {
      if (obj == null || obj.MarkedForClose || (obj.GetPhysicsBody() == null || this.m_subparts[0].GetPhysicsBody() == null) || obj.GetPhysicsBody().HavokCollisionSystemID == this.m_subparts[0].GetPhysicsBody().HavokCollisionSystemID)
        return;
      this.UpdateHavokCollisionSystemID(obj.GetPhysicsBody().HavokCollisionSystemID);
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_AdvancedDoor builderCubeBlock = (MyObjectBuilder_AdvancedDoor) base.GetObjectBuilderCubeBlock(copy);
      builderCubeBlock.Open = (bool) this.m_open;
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    protected float UpdatePowerInput()
    {
      if (!this.Enabled || !this.IsFunctional)
        return 0.0f;
      return (double) this.OpeningSpeed == 0.0 ? this.BlockDefinition.PowerConsumptionIdle : this.BlockDefinition.PowerConsumptionMoving;
    }

    private void StartSound(int emitterId, MySoundPair cuePair)
    {
      if (this.m_emitter[emitterId].Sound != null && this.m_emitter[emitterId].Sound.IsPlaying && (this.m_emitter[emitterId].SoundId == cuePair.Arcade || this.m_emitter[emitterId].SoundId == cuePair.Realistic))
        return;
      this.m_emitter[emitterId].StopSound(true);
      this.m_emitter[emitterId].PlaySingleSound(cuePair);
    }

    public override void UpdateSoundEmitters()
    {
      for (int index = 0; index < this.m_emitter.Count; ++index)
      {
        if (this.m_emitter[index] != null)
          this.m_emitter[index].Update();
      }
    }

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      bool isPowered = this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId);
      this.SequencePreparation(isPowered);
      if (this.CubeGrid.Physics == null)
        return;
      this.UpdateDoorPosition(isPowered);
    }

    private void SequencePreparation(bool isPowered)
    {
      bool fullyOpen;
      bool fullyClosed;
      this.GetOpenCloseStatus(out fullyOpen, out fullyClosed);
      bool flag = false;
      if (fullyClosed)
      {
        this.m_time = 0.0f;
        if (!(bool) this.m_open)
          flag = true;
      }
      else if (fullyOpen)
      {
        if ((double) this.m_totalTime != (double) this.m_time)
          this.m_totalTime = this.m_time;
        if ((bool) this.m_open)
          flag = true;
      }
      for (int index = 0; index < this.m_openingSequence.Count; ++index)
      {
        float maxOpen = this.m_openingSequence[index].MaxOpen;
        if (this.Open && (double) this.m_currentOpening[index] == (double) maxOpen || !this.Open && (double) this.m_currentOpening[index] == 0.0)
        {
          if (!Sandbox.Game.Multiplayer.Sync.IsDedicated && this.m_emitter[index] != null && (this.m_emitter[index].IsPlaying && this.m_emitter[index].Loop))
            this.m_emitter[index].StopSound(false);
          this.m_currentSpeed[index] = 0.0f;
        }
        if (!Sandbox.Game.Multiplayer.Sync.IsDedicated)
          this.UpdateSounds(index, isPowered);
      }
      if (this.m_stateChange && ((bool) this.m_open & fullyOpen || !(bool) this.m_open & fullyClosed))
      {
        this.ResourceSink.Update();
        this.RaisePropertiesChanged();
        if (!(bool) this.m_open)
        {
          this.DoorStateChanged.InvokeIfNotNull<bool>((bool) this.m_open);
          this.OnDoorStateChanged.InvokeIfNotNull<Sandbox.ModAPI.IMyDoor, bool>((Sandbox.ModAPI.IMyDoor) this, (bool) this.m_open);
        }
        this.m_stateChange = false;
      }
      if (!flag)
      {
        this.UpdateCurrentOpening(isPowered);
      }
      else
      {
        for (int index = 0; index < this.m_currentlyAtLimit.Count; ++index)
          this.m_currentlyAtLimit[index] = true;
      }
      this.m_lastUpdateTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
    }

    private void UpdateSounds(int index, bool isPowered)
    {
      if (((!this.Enabled ? 0 : (this.ResourceSink != null ? 1 : 0)) & (isPowered ? 1 : 0)) != 0 && (double) this.m_currentSpeed[index] != 0.0)
      {
        string cueName = !this.Open ? this.m_openingSequence[index].CloseSound : this.m_openingSequence[index].OpenSound;
        if (string.IsNullOrEmpty(cueName))
          return;
        this.StartSound(index, new MySoundPair(cueName));
      }
      else
      {
        if (this.m_emitter[index] == null)
          return;
        this.m_emitter[index].StopSound(false);
      }
    }

    private void UpdateCurrentOpening(bool isPowered)
    {
      if (((!this.Enabled ? 0 : (this.ResourceSink != null ? 1 : 0)) & (isPowered ? 1 : 0)) == 0)
        return;
      float num1 = (float) (MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_lastUpdateTime) / 1000f;
      this.m_time += (float) ((double) (MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_lastUpdateTime) / 1000.0 * ((bool) this.m_open ? 1.0 : -1.0));
      this.m_time = MathHelper.Clamp(this.m_time, 0.0f, this.m_totalTime);
      for (int index = 0; index < this.m_openingSequence.Count; ++index)
      {
        float num2 = (bool) this.m_open ? this.m_openingSequence[index].OpenDelay : this.m_openingSequence[index].CloseDelay;
        if ((bool) this.m_open && (double) this.m_time > (double) num2 || !(bool) this.m_open && (double) this.m_time < (double) this.m_totalTime - (double) num2)
        {
          float num3 = this.m_currentSpeed[index] * num1;
          float maxOpen = this.m_openingSequence[index].MaxOpen;
          if (this.m_openingSequence[index].SequenceType == MyObjectBuilder_AdvancedDoorDefinition.Opening.Sequence.Linear)
          {
            float num4 = MathHelper.Clamp(this.m_currentOpening[index] + num3, 0.0f, maxOpen);
            this.m_currentlyAtLimit[index] = (double) Math.Abs(num4 - this.m_currentOpening[index]) < 1.0 / 1000.0;
            this.m_currentOpening[index] = num4;
          }
        }
      }
    }

    private void UpdateDoorPosition(bool isPowered, bool simplicicationOverride = false)
    {
      if (this.CubeGrid.Physics == null)
        return;
      if ((!this.Enabled || this.ResourceSink == null || !isPowered) && !simplicicationOverride)
      {
        this.UpdateLinearVelocities();
      }
      else
      {
        this.GetSubpartLimits(ref this.m_isSubpartAtLimits);
        for (int index = 0; index < this.m_subpartCount; ++index)
        {
          if (!this.m_isSubpartAtLimits[index] || simplicicationOverride)
          {
            this.transMat[index] = Matrix.Identity;
            this.rotMat[index] = Matrix.Identity;
          }
        }
        for (int index = 0; index < this.m_sequenceCount; ++index)
        {
          int subpartId = this.m_subpartIDs[index];
          if (!this.m_currentlyAtLimit[index] || !this.m_isSubpartAtLimits[subpartId] || simplicicationOverride)
          {
            MyObjectBuilder_AdvancedDoorDefinition.Opening.MoveType move = this.m_openingSequence[index].Move;
            float num1 = this.m_currentOpening[index];
            if (this.m_subparts.Count != 0 && subpartId >= 0)
            {
              if (this.m_subparts[subpartId] != null && this.m_subparts[subpartId].Physics != null)
              {
                switch (move)
                {
                  case MyObjectBuilder_AdvancedDoorDefinition.Opening.MoveType.Slide:
                    this.transMat[subpartId].Translation += (Vector3) this.m_openingSequence[index].SlideDirection * new Vector3(num1);
                    continue;
                  case MyObjectBuilder_AdvancedDoorDefinition.Opening.MoveType.Rotate:
                    float num2 = this.m_openingSequence[index].InvertRotation ? -1f : 1f;
                    float radians1 = 0.0f;
                    float radians2 = 0.0f;
                    float radians3 = 0.0f;
                    if (this.m_openingSequence[index].RotationAxis == MyObjectBuilder_AdvancedDoorDefinition.Opening.Rotation.X)
                      radians1 = MathHelper.ToRadians(num1 * num2);
                    else if (this.m_openingSequence[index].RotationAxis == MyObjectBuilder_AdvancedDoorDefinition.Opening.Rotation.Y)
                      radians2 = MathHelper.ToRadians(num1 * num2);
                    else if (this.m_openingSequence[index].RotationAxis == MyObjectBuilder_AdvancedDoorDefinition.Opening.Rotation.Z)
                      radians3 = MathHelper.ToRadians(num1 * num2);
                    Vector3 vector3 = !this.m_openingSequence[index].PivotPosition.HasValue ? this.m_hingePosition[subpartId] : (Vector3) this.m_openingSequence[index].PivotPosition.Value;
                    int num3 = Vector3.IsZero(vector3) ? 1 : 0;
                    if (num3 == 0)
                      this.rotMat[subpartId].Translation -= vector3;
                    if ((double) radians1 != 0.0)
                      this.rotMat[subpartId] *= Matrix.CreateRotationX(radians1);
                    else if ((double) radians2 != 0.0)
                      this.rotMat[subpartId] *= Matrix.CreateRotationY(radians2);
                    else if ((double) radians3 != 0.0)
                      this.rotMat[subpartId] *= Matrix.CreateRotationZ(radians3);
                    if (num3 == 0)
                    {
                      this.rotMat[subpartId].Translation += vector3;
                      continue;
                    }
                    continue;
                  default:
                    continue;
                }
              }
            }
            else
              break;
          }
        }
        this.UpdateLinearVelocities();
        for (int index = 0; index < this.m_subpartCount; ++index)
        {
          if (!this.m_isSubpartAtLimits[index] || simplicicationOverride)
          {
            Matrix localMatrix = this.rotMat[index] * this.transMat[index];
            this.m_subparts[index].PositionComp.SetLocalMatrix(ref localMatrix);
          }
        }
      }
    }

    private void UpdateLinearVelocities()
    {
      for (int index = 0; index < this.m_subpartCount; ++index)
      {
        if (this.m_subparts[index].Physics != null)
        {
          if (this.m_subparts[index].Physics.LinearVelocity != this.CubeGrid.Physics.LinearVelocity)
            this.m_subparts[index].Physics.LinearVelocity = this.CubeGrid.Physics.LinearVelocity;
          if (this.m_subparts[index].Physics.AngularVelocity != this.CubeGrid.Physics.AngularVelocity)
            this.m_subparts[index].Physics.AngularVelocity = this.CubeGrid.Physics.AngularVelocity;
        }
      }
    }

    private bool IsSubpartAtLimit(int subpartId)
    {
      bool flag = true;
      for (int index = 0; index < this.m_openingSequence.Count; ++index)
      {
        if (this.m_subpartIDs[index] == subpartId)
          flag &= this.m_currentlyAtLimit[index];
      }
      return flag;
    }

    private void GetSubpartLimits(ref bool[] limits)
    {
      if (limits == null || limits.Length < this.m_subparts.Count)
        limits = new bool[this.m_subparts.Count];
      for (int index = 0; index < limits.Length; ++index)
        limits[index] = true;
      for (int index = 0; index < this.m_openingSequence.Count; ++index)
      {
        if (!this.m_currentlyAtLimit[index])
          limits[this.m_subpartIDs[index]] = this.m_currentlyAtLimit[index];
      }
    }

    public override void OnCubeGridChanged(MyCubeGrid oldGrid)
    {
      oldGrid.OnHavokSystemIDChanged -= new Action<int>(this.CubeGrid_HavokSystemIDChanged);
      this.CubeGrid.OnHavokSystemIDChanged += new Action<int>(this.CubeGrid_HavokSystemIDChanged);
      if (this.CubeGrid.Physics != null)
        this.UpdateHavokCollisionSystemID(this.CubeGrid.GetPhysicsBody().HavokCollisionSystemID);
      base.OnCubeGridChanged(oldGrid);
    }

    private void CubeGrid_HavokSystemIDChanged(int id)
    {
      bool flag = true;
      foreach (MyEntitySubpart subpart in this.m_subparts)
      {
        int num1 = flag ? 1 : 0;
        MyPhysicsComponentBase physics = subpart.Physics;
        int num2 = physics != null ? (physics.IsInWorld ? 1 : 0) : 0;
        flag = (num1 & num2) != 0;
      }
      if (!flag)
        return;
      this.UpdateHavokCollisionSystemID(id);
    }

    internal void UpdateHavokCollisionSystemID(int HavokCollisionSystemID)
    {
      foreach (MyEntitySubpart subpart in this.m_subparts)
      {
        if (subpart != null && subpart.Physics != null && (subpart.ModelCollision.HavokCollisionShapes != null && subpart.ModelCollision.HavokCollisionShapes.Length != 0))
        {
          uint info = HkGroupFilter.CalcFilterInfo(15, HavokCollisionSystemID, 1, 1);
          subpart.Physics.RigidBody.SetCollisionFilterInfo(info);
          if (subpart.GetPhysicsBody().HavokWorld != null)
            MyPhysics.RefreshCollisionFilter(subpart.GetPhysicsBody());
        }
      }
    }

    protected override void Closing()
    {
      for (int index = 0; index < this.m_emitter.Count; ++index)
      {
        if (this.m_emitter[index] != null)
          this.m_emitter[index].StopSound(true);
      }
      this.CubeGrid.OnHavokSystemIDChanged -= new Action<int>(this.CubeGrid_HavokSystemIDChanged);
      base.Closing();
    }

    public override void OnModelChange()
    {
      base.OnModelChange();
      this.InitSubparts();
    }

    private void Receiver_IsPoweredChanged()
    {
      this.UpdateIsWorking();
      this.UpdateEmissivity();
    }

    private void ComponentStack_IsFunctionalChanged() => this.ResourceSink.Update();

    public override void ContactCallbackInternal() => base.ContactCallbackInternal();

    public override bool EnableContactCallbacks() => false;

    public override bool IsClosing() => !(bool) this.m_open && (double) this.OpenRatio > 0.0;

    private class Sandbox_Game_Entities_MyAdvancedDoor\u003C\u003EActor : IActivator, IActivator<MyAdvancedDoor>
    {
      object IActivator.CreateInstance() => (object) new MyAdvancedDoor();

      MyAdvancedDoor IActivator<MyAdvancedDoor>.CreateInstance() => new MyAdvancedDoor();
    }
  }
}
