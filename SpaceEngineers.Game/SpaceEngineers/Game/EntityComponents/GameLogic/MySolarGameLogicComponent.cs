// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.EntityComponents.GameLogic.MySolarGameLogicComponent
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using ParallelTasks;
using Sandbox;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Game.Components;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRageMath;

namespace SpaceEngineers.Game.EntityComponents.GameLogic
{
  public class MySolarGameLogicComponent : MyGameLogicComponent
  {
    private const int NUMBER_OF_PIVOTS = 8;
    private float m_maxOutput;
    private float m_solarModifier = 1f;
    private Vector3 m_panelOrientation;
    private float m_panelOffset;
    private bool m_isTwoSided;
    private MyFunctionalBlock m_solarBlock;
    private bool m_initialized;
    private byte m_debugCurrentPivot;
    private bool[] m_debugIsPivotInSun = new bool[8];
    private bool m_isBackgroundProcessing;
    private byte m_currentPivot;
    private float m_angleToSun;
    private int m_pivotsInSun;
    private bool[] m_isPivotInSun = new bool[8];
    private List<MyPhysics.HitInfo> m_hitList = new List<MyPhysics.HitInfo>();
    private Vector3D m_to;
    private Vector3D m_from;
    private Action ComputeSunAngleFunc;
    private Action<List<MyPhysics.HitInfo>> OnRayCastCompletedFunc;
    private Action OnSunAngleComputedFunc;

    public event Action OnProductionChanged;

    public float MaxOutput
    {
      get => this.m_maxOutput;
      set
      {
        if ((double) this.m_maxOutput == (double) value)
          return;
        this.m_maxOutput = value;
        this.OnProductionChanged.InvokeIfNotNull();
      }
    }

    public Vector3 PanelOrientation => this.m_panelOrientation;

    public float PanelOffset => this.m_panelOffset;

    public byte DebugCurrentPivot => this.m_debugCurrentPivot;

    public bool[] DebugIsPivotInSun => this.m_debugIsPivotInSun;

    public MySolarGameLogicComponent()
    {
      this.ComputeSunAngleFunc = new Action(this.ComputeSunAngle);
      this.OnSunAngleComputedFunc = new Action(this.OnSunAngleComputed);
      this.OnRayCastCompletedFunc = new Action<List<MyPhysics.HitInfo>>(this.OnRayCastCompleted);
    }

    public void Initialize(
      Vector3 panelOrientation,
      bool isTwoSided,
      float panelOffset,
      MyFunctionalBlock solarBlock)
    {
      this.m_initialized = true;
      this.m_panelOrientation = panelOrientation;
      this.m_isTwoSided = isTwoSided;
      this.m_panelOffset = panelOffset;
      this.m_solarBlock = solarBlock;
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
    }

    public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false) => (MyObjectBuilder_EntityBase) null;

    public override void UpdateBeforeSimulation100()
    {
      base.UpdateBeforeSimulation100();
      if (this.m_solarBlock.CubeGrid.Physics == null)
        return;
      if (!this.m_solarBlock.IsWorking)
      {
        this.MaxOutput = 0.0f;
      }
      else
      {
        if (!this.m_isBackgroundProcessing)
        {
          this.m_isBackgroundProcessing = true;
          this.m_currentPivot = this.m_debugCurrentPivot;
          for (int index = 0; index < 8; ++index)
            this.m_isPivotInSun[index] = this.m_debugIsPivotInSun[index];
          Parallel.Start(this.ComputeSunAngleFunc);
        }
        this.m_solarModifier = MySession.Static.GetComponent<MySectorWeatherComponent>().GetSolarMultiplier(this.Entity.PositionComp.GetPosition());
      }
    }

    private void ComputeSunAngle()
    {
      this.m_angleToSun = Vector3.Dot((Vector3) Vector3.Transform(this.m_panelOrientation, this.m_solarBlock.WorldMatrix.GetOrientation()), MySector.DirectionToSunNormalized);
      if ((double) this.m_angleToSun < 0.0 && !this.m_isTwoSided || !this.m_solarBlock.IsFunctional)
        MySandboxGame.Static.Invoke(this.OnSunAngleComputedFunc, "SolarGamelogic:OnSunAngleComputed");
      else if (MySectorWeatherComponent.IsOnDarkSide(this.m_solarBlock.WorldMatrix.Translation))
      {
        ((IEnumerable<bool>) this.m_isPivotInSun).ForEach<bool>((Action<bool>) (x => x = false));
        this.m_pivotsInSun = 0;
        MySandboxGame.Static.Invoke(this.OnSunAngleComputedFunc, "SolarGamelogic:OnSunAngleComputed");
      }
      else
      {
        this.m_currentPivot %= (byte) 8;
        MatrixD orientation = this.m_solarBlock.WorldMatrix.GetOrientation();
        float num1 = (float) this.m_solarBlock.WorldMatrix.Forward.Dot(Vector3.Transform(this.m_panelOrientation, orientation));
        float num2 = this.m_solarBlock.BlockDefinition.CubeSize == MyCubeSize.Large ? 2.5f : 0.5f;
        Vector3D translation = this.m_solarBlock.WorldMatrix.Translation;
        double num3 = ((double) ((int) this.m_currentPivot % 4) - 1.5) * (double) num2 * (double) num1 * ((double) this.m_solarBlock.BlockDefinition.Size.X / 4.0);
        MatrixD worldMatrix = this.m_solarBlock.WorldMatrix;
        Vector3D left = worldMatrix.Left;
        Vector3D vector3D1 = num3 * left;
        Vector3D vector3D2 = translation + vector3D1;
        double num4 = ((double) ((int) this.m_currentPivot / 4) - 0.5) * (double) num2 * (double) num1 * ((double) this.m_solarBlock.BlockDefinition.Size.Y / 2.0);
        worldMatrix = this.m_solarBlock.WorldMatrix;
        Vector3D up = worldMatrix.Up;
        Vector3D vector3D3 = num4 * up;
        Vector3D vector3D4 = vector3D2 + vector3D3 + (double) num2 * (double) num1 * ((double) this.m_solarBlock.BlockDefinition.Size.Z / 2.0) * Vector3.Transform(this.m_panelOrientation, orientation) * (double) this.m_panelOffset;
        this.m_from = vector3D4 + MySector.DirectionToSunNormalized * 100f;
        this.m_to = vector3D4 + MySector.DirectionToSunNormalized * this.m_solarBlock.CubeGrid.GridSize / 4f;
        MyPhysics.CastRayParallel(ref this.m_to, ref this.m_from, this.m_hitList, 15, this.OnRayCastCompletedFunc);
      }
    }

    private void OnRayCastCompleted(List<MyPhysics.HitInfo> hits)
    {
      this.m_isPivotInSun[(int) this.m_currentPivot] = true;
      foreach (MyPhysics.HitInfo hit in hits)
      {
        IMyEntity hitEntity = hit.HkHitInfo.GetHitEntity();
        if (hitEntity != this.m_solarBlock.CubeGrid)
        {
          this.m_isPivotInSun[(int) this.m_currentPivot] = false;
          break;
        }
        MyCubeGrid myCubeGrid = hitEntity as MyCubeGrid;
        Vector3I? nullable = myCubeGrid.RayCastBlocks(this.m_from, this.m_to);
        if (nullable.HasValue && myCubeGrid.GetCubeBlock(nullable.Value) != this.m_solarBlock.SlimBlock)
        {
          this.m_isPivotInSun[(int) this.m_currentPivot] = false;
          break;
        }
      }
      this.m_pivotsInSun = 0;
      foreach (bool flag in this.m_isPivotInSun)
      {
        if (flag)
          ++this.m_pivotsInSun;
      }
      MySandboxGame.Static.Invoke(this.OnSunAngleComputedFunc, "SolarGamelogic:OnSunAngleComputed");
    }

    private void OnSunAngleComputed()
    {
      this.m_isBackgroundProcessing = false;
      if ((double) this.m_angleToSun < 0.0 && !this.m_isTwoSided || !this.m_solarBlock.Enabled)
      {
        this.MaxOutput = 0.0f;
      }
      else
      {
        float num = this.m_angleToSun;
        if ((double) num < 0.0)
          num = !this.m_isTwoSided ? 0.0f : Math.Abs(num);
        this.MaxOutput = num * ((float) this.m_pivotsInSun / 8f) * this.m_solarModifier;
        this.m_debugCurrentPivot = this.m_currentPivot;
        ++this.m_debugCurrentPivot;
        for (int index = 0; index < 8; ++index)
          this.m_debugIsPivotInSun[index] = this.m_isPivotInSun[index];
      }
    }
  }
}
