// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.CoordinateSystem.MyCoordinateSystem
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Components.Session;
using VRage.Game.Definitions.SessionComponents;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.Components;
using VRage.Network;
using VRage.Serialization;
using VRageMath;

namespace Sandbox.Game.GameSystems.CoordinateSystem
{
  [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate, 1000, typeof (MyObjectBuilder_CoordinateSystem), null, false)]
  [StaticEventOwner]
  public class MyCoordinateSystem : MySessionComponentBase
  {
    public static MyCoordinateSystem Static;
    private double m_angleTolerance = 0.0001;
    private double m_positionTolerance = 0.001;
    private int m_coorsSystemSize = 1000;
    private int m_coorsSystemSizeSq = 1000000;
    private Dictionary<long, MyLocalCoordSys> m_localCoordSystems = new Dictionary<long, MyLocalCoordSys>();
    private long m_lastCoordSysId = 1;
    private bool m_drawBoundingBox;
    private long m_selectedCoordSys;
    private long m_lastSelectedCoordSys;
    private bool m_localCoordExist;
    private bool m_selectionChanged;
    private bool m_visible;

    public static event Action OnCoordinateChange;

    public long SelectedCoordSys => this.m_selectedCoordSys;

    public long LastSelectedCoordSys => this.m_lastSelectedCoordSys;

    public bool LocalCoordExist => this.m_localCoordExist;

    public bool Visible
    {
      get => this.m_visible;
      set => this.m_visible = value;
    }

    public int CoordSystemSize => this.m_coorsSystemSize;

    public int CoordSystemSizeSquared => this.m_coorsSystemSizeSq;

    public MyCoordinateSystem()
    {
      MyCoordinateSystem.Static = this;
      if (!Sync.IsServer)
        return;
      MyEntities.OnEntityAdd += new Action<MyEntity>(this.MyEntities_OnEntityCreate);
    }

    public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
    {
      base.Init(sessionComponent);
      MyObjectBuilder_CoordinateSystem coordinateSystem = sessionComponent as MyObjectBuilder_CoordinateSystem;
      this.m_lastCoordSysId = coordinateSystem.LastCoordSysId;
      foreach (MyObjectBuilder_CoordinateSystem.CoordSysInfo coordSystem in coordinateSystem.CoordSystems)
        this.m_localCoordSystems.Add(coordSystem.Id, new MyLocalCoordSys(new MyTransformD()
        {
          Position = (Vector3D) coordSystem.Position,
          Rotation = (Quaternion) coordSystem.Rotation
        }, this.m_coorsSystemSize)
        {
          Id = coordSystem.Id
        });
    }

    public override void InitFromDefinition(MySessionComponentDefinition definition)
    {
      base.InitFromDefinition(definition);
      MyCoordinateSystemDefinition systemDefinition = definition as MyCoordinateSystemDefinition;
      this.m_coorsSystemSize = systemDefinition.CoordSystemSize;
      this.m_coorsSystemSizeSq = this.m_coorsSystemSize * this.m_coorsSystemSize;
      this.m_angleTolerance = systemDefinition.AngleTolerance;
      this.m_positionTolerance = systemDefinition.PositionTolerance;
    }

    public override void LoadData() => base.LoadData();

    protected override void UnloadData()
    {
      base.UnloadData();
      this.m_lastCoordSysId = 1L;
      this.m_localCoordSystems.Clear();
      this.m_drawBoundingBox = false;
      this.m_selectedCoordSys = 0L;
      this.m_lastSelectedCoordSys = 0L;
      MyCoordinateSystem.Static = (MyCoordinateSystem) null;
    }

    public override MyObjectBuilder_SessionComponent GetObjectBuilder()
    {
      MyObjectBuilder_CoordinateSystem objectBuilder = base.GetObjectBuilder() as MyObjectBuilder_CoordinateSystem;
      objectBuilder.LastCoordSysId = this.m_lastCoordSysId;
      foreach (KeyValuePair<long, MyLocalCoordSys> localCoordSystem in this.m_localCoordSystems)
        objectBuilder.CoordSystems.Add(new MyObjectBuilder_CoordinateSystem.CoordSysInfo()
        {
          Id = localCoordSystem.Value.Id,
          EntityCount = localCoordSystem.Value.EntityCounter,
          Position = (SerializableVector3D) localCoordSystem.Value.Origin.Position,
          Rotation = (SerializableQuaternion) localCoordSystem.Value.Origin.Rotation
        });
      return (MyObjectBuilder_SessionComponent) objectBuilder;
    }

    private MyLocalCoordSys GetClosestCoordSys(
      ref Vector3D position,
      bool checkContain = true)
    {
      MyLocalCoordSys myLocalCoordSys1 = (MyLocalCoordSys) null;
      double num1 = double.MaxValue;
      foreach (MyLocalCoordSys myLocalCoordSys2 in this.m_localCoordSystems.Values)
      {
        if (!checkContain || myLocalCoordSys2.Contains(ref position))
        {
          double num2 = (myLocalCoordSys2.Origin.Position - position).LengthSquared();
          if (num2 < num1)
          {
            myLocalCoordSys1 = myLocalCoordSys2;
            num1 = num2;
          }
        }
      }
      return myLocalCoordSys1;
    }

    [Event(null, 244)]
    [Reliable]
    [BroadcastExcept]
    private static void CoordSysCreated_Client(
      MyCoordinateSystem.MyCreateCoordSysBuffer createBuffer)
    {
      MyCoordinateSystem.Static.CreateCoordSys_ClientInternal(ref new MyTransformD()
      {
        Position = createBuffer.Position,
        Rotation = createBuffer.Rotation
      }, createBuffer.Id);
    }

    private void CreateCoordSys_ClientInternal(ref MyTransformD transform, long coordSysId) => this.m_localCoordSystems.Add(coordSysId, new MyLocalCoordSys(transform, this.m_coorsSystemSize)
    {
      Id = coordSysId
    });

    public void CreateCoordSys(MyCubeGrid cubeGrid, bool staticGridAlignToCenter, bool sync = false)
    {
      MyTransformD origin = new MyTransformD(cubeGrid.PositionComp.WorldMatrixRef);
      origin.Rotation.Normalize();
      float gridSize = cubeGrid.GridSize;
      if (!staticGridAlignToCenter)
        origin.Position -= (origin.Rotation.Forward + origin.Rotation.Right + origin.Rotation.Up) * gridSize * 0.5f;
      MyLocalCoordSys coordSys = new MyLocalCoordSys(origin, this.m_coorsSystemSize);
      long key;
      do
      {
        key = this.m_lastCoordSysId++;
      }
      while (this.m_localCoordSystems.ContainsKey(key));
      coordSys.Id = key;
      this.m_localCoordSystems.Add(key, coordSys);
      if (cubeGrid.LocalCoordSystem != 0L)
        this.UnregisterCubeGrid(cubeGrid);
      this.RegisterCubeGrid(cubeGrid, coordSys);
      MyCoordinateSystem.MyCreateCoordSysBuffer createCoordSysBuffer = new MyCoordinateSystem.MyCreateCoordSysBuffer();
      createCoordSysBuffer.Position = origin.Position;
      createCoordSysBuffer.Rotation = origin.Rotation;
      createCoordSysBuffer.Id = key;
      if (!sync)
        return;
      MyMultiplayer.RaiseStaticEvent<MyCoordinateSystem.MyCreateCoordSysBuffer>((Func<IMyEventOwner, Action<MyCoordinateSystem.MyCreateCoordSysBuffer>>) (x => new Action<MyCoordinateSystem.MyCreateCoordSysBuffer>(MyCoordinateSystem.CoordSysCreated_Client)), createCoordSysBuffer);
    }

    public static void GetPosRoundedToGrid(
      ref Vector3D vecToRound,
      double gridSize,
      bool isStaticGridAlignToCenter)
    {
      if (isStaticGridAlignToCenter)
        vecToRound = Vector3L.Round(vecToRound / gridSize) * gridSize;
      else
        vecToRound = Vector3L.Round(vecToRound / gridSize + 0.5) * gridSize - 0.5 * gridSize;
    }

    [Event(null, 326)]
    [Reliable]
    [Broadcast]
    private static void CoorSysRemoved_Client(long coordSysId) => MyCoordinateSystem.Static.RemoveCoordSys(coordSysId);

    private void RemoveCoordSys(long coordSysId) => this.m_localCoordSystems.Remove(coordSysId);

    private void MyEntities_OnEntityCreate(MyEntity obj)
    {
      if (!(obj is MyCubeGrid cubeGrid) || cubeGrid.LocalCoordSystem == 0L)
        return;
      MyLocalCoordSys coordSysById = this.GetCoordSysById(cubeGrid.LocalCoordSystem);
      if (coordSysById == null)
        return;
      this.RegisterCubeGrid(cubeGrid, coordSysById);
    }

    public void RegisterCubeGrid(MyCubeGrid cubeGrid)
    {
      if (cubeGrid.LocalCoordSystem != 0L)
        return;
      Vector3D position = cubeGrid.PositionComp.GetPosition();
      MyLocalCoordSys closestCoordSys = this.GetClosestCoordSys(ref position);
      if (closestCoordSys == null)
        return;
      this.RegisterCubeGrid(cubeGrid, closestCoordSys);
    }

    private void RegisterCubeGrid(MyCubeGrid cubeGrid, MyLocalCoordSys coordSys)
    {
      cubeGrid.OnClose += new Action<MyEntity>(this.CubeGrid_OnClose);
      cubeGrid.OnPhysicsChanged += new Action<MyEntity>(this.CubeGrid_OnPhysicsChanged);
      cubeGrid.LocalCoordSystem = coordSys.Id;
      ++coordSys.EntityCounter;
    }

    private void UnregisterCubeGrid(MyCubeGrid cubeGrid)
    {
      cubeGrid.OnClose -= new Action<MyEntity>(this.CubeGrid_OnClose);
      cubeGrid.OnPhysicsChanged -= new Action<MyEntity>(this.CubeGrid_OnPhysicsChanged);
      long localCoordSystem = cubeGrid.LocalCoordSystem;
      MyLocalCoordSys coordSysById = this.GetCoordSysById(localCoordSystem);
      cubeGrid.LocalCoordSystem = 0L;
      if (coordSysById == null)
        return;
      --coordSysById.EntityCounter;
      if (coordSysById.EntityCounter > 0L)
        return;
      this.RemoveCoordSys(coordSysById.Id);
      MyMultiplayer.RaiseStaticEvent<long>((Func<IMyEventOwner, Action<long>>) (x => new Action<long>(MyCoordinateSystem.CoorSysRemoved_Client)), localCoordSystem);
    }

    private void CubeGrid_OnPhysicsChanged(MyEntity obj)
    {
      if (!(obj is MyCubeGrid cubeGrid) || cubeGrid.IsStatic)
        return;
      this.UnregisterCubeGrid(cubeGrid);
    }

    private void CubeGrid_OnClose(MyEntity obj)
    {
      if (!(obj is MyCubeGrid cubeGrid))
        return;
      this.UnregisterCubeGrid(cubeGrid);
    }

    private MyLocalCoordSys GetCoordSysById(long id) => this.m_localCoordSystems.ContainsKey(id) ? this.m_localCoordSystems[id] : (MyLocalCoordSys) null;

    public MyCoordinateSystem.CoordSystemData SnapWorldPosToClosestGrid(
      ref Vector3D worldPos,
      double gridSize,
      bool staticGridAlignToCenter,
      long? id = null)
    {
      this.m_lastSelectedCoordSys = this.m_selectedCoordSys;
      MyLocalCoordSys myLocalCoordSys = !id.HasValue || id.Value == 0L ? this.GetClosestCoordSys(ref worldPos) : this.GetCoordSysById(id.Value);
      if (myLocalCoordSys == null)
      {
        myLocalCoordSys = new MyLocalCoordSys(new MyTransformD(Vector3L.Round(worldPos / gridSize) * gridSize), this.m_coorsSystemSize);
        this.m_selectedCoordSys = 0L;
      }
      else
        this.m_selectedCoordSys = myLocalCoordSys.Id;
      this.m_localCoordExist = this.m_selectedCoordSys != 0L;
      if (this.m_selectedCoordSys != this.m_lastSelectedCoordSys)
      {
        this.m_selectionChanged = true;
        if (MyCoordinateSystem.OnCoordinateChange != null)
          MyCoordinateSystem.OnCoordinateChange();
      }
      else
        this.m_selectionChanged = false;
      MyCoordinateSystem.CoordSystemData coordSystemData = new MyCoordinateSystem.CoordSystemData();
      Quaternion rotation1 = myLocalCoordSys.Origin.Rotation;
      Quaternion rotation2 = Quaternion.Inverse(rotation1);
      Vector3D position = myLocalCoordSys.Origin.Position;
      Vector3D vecToRound = Vector3D.Transform(worldPos - position, rotation2);
      MyCoordinateSystem.GetPosRoundedToGrid(ref vecToRound, gridSize, staticGridAlignToCenter);
      coordSystemData.Id = this.m_selectedCoordSys;
      coordSystemData.LocalSnappedPos = vecToRound;
      Vector3D vector3D = Vector3D.Transform(vecToRound, rotation1);
      coordSystemData.SnappedTransform = new MyTransformD()
      {
        Position = position + vector3D,
        Rotation = rotation1
      };
      coordSystemData.Origin = myLocalCoordSys.Origin;
      return coordSystemData;
    }

    public bool IsAnyLocalCoordSysExist(ref Vector3D worldPos)
    {
      foreach (MyLocalCoordSys myLocalCoordSys in this.m_localCoordSystems.Values)
      {
        if (myLocalCoordSys.Contains(ref worldPos))
          return true;
      }
      return false;
    }

    public bool IsLocalCoordSysExist(ref MatrixD tranform, double gridSize)
    {
      foreach (MyLocalCoordSys myLocalCoordSys in this.m_localCoordSystems.Values)
      {
        Vector3D translation = tranform.Translation;
        if (myLocalCoordSys.Contains(ref translation))
        {
          MyTransformD origin = myLocalCoordSys.Origin;
          double num1 = Math.Abs(Vector3D.Dot((Vector3D) origin.Rotation.Forward, tranform.Forward));
          origin = myLocalCoordSys.Origin;
          double num2 = Math.Abs(Vector3D.Dot((Vector3D) origin.Rotation.Up, tranform.Up));
          if ((num1 < this.m_angleTolerance || num1 > 1.0 - this.m_angleTolerance) && (num2 < this.m_angleTolerance || num2 > 1.0 - this.m_angleTolerance))
          {
            Vector3D vector3D = Vector3D.Transform(translation - myLocalCoordSys.Origin.Position, Quaternion.Inverse(myLocalCoordSys.Origin.Rotation));
            double num3 = gridSize / 2.0;
            double num4 = Math.Abs(vector3D.X % num3);
            double num5 = Math.Abs(vector3D.Y % num3);
            double num6 = Math.Abs(vector3D.Z % num3);
            if ((num4 < this.m_positionTolerance || num4 > num3 - this.m_positionTolerance) && (num5 < this.m_positionTolerance || num5 > num3 - this.m_positionTolerance) && (num6 < this.m_positionTolerance || num6 > num3 - this.m_positionTolerance))
              return true;
          }
        }
      }
      return false;
    }

    public void ResetSelection()
    {
      this.m_lastSelectedCoordSys = 0L;
      this.m_selectedCoordSys = 0L;
      this.m_drawBoundingBox = false;
    }

    public override void Draw()
    {
      if (!this.m_visible)
        return;
      if (this.m_selectedCoordSys == 0L)
        this.m_drawBoundingBox = false;
      else if (this.m_selectedCoordSys != 0L)
        this.m_drawBoundingBox = true;
      if (MyFakes.ENABLE_DEBUG_DRAW_COORD_SYS)
      {
        foreach (MyLocalCoordSys myLocalCoordSys in this.m_localCoordSystems.Values)
          myLocalCoordSys.Draw();
      }
      else if (this.m_drawBoundingBox)
        this.GetCoordSysById(this.m_selectedCoordSys)?.Draw();
      base.Draw();
    }

    public Color GetCoordSysColor(long coordSysId) => this.m_localCoordSystems.ContainsKey(coordSysId) ? this.m_localCoordSystems[coordSysId].RenderColor : Color.White;

    [Serializable]
    private struct MyCreateCoordSysBuffer
    {
      public long Id;
      public Vector3D Position;
      [Serialize(MyPrimitiveFlags.Normalized)]
      public Quaternion Rotation;

      protected class Sandbox_Game_GameSystems_CoordinateSystem_MyCoordinateSystem\u003C\u003EMyCreateCoordSysBuffer\u003C\u003EId\u003C\u003EAccessor : IMemberAccessor<MyCoordinateSystem.MyCreateCoordSysBuffer, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyCoordinateSystem.MyCreateCoordSysBuffer owner,
          in long value)
        {
          owner.Id = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyCoordinateSystem.MyCreateCoordSysBuffer owner,
          out long value)
        {
          value = owner.Id;
        }
      }

      protected class Sandbox_Game_GameSystems_CoordinateSystem_MyCoordinateSystem\u003C\u003EMyCreateCoordSysBuffer\u003C\u003EPosition\u003C\u003EAccessor : IMemberAccessor<MyCoordinateSystem.MyCreateCoordSysBuffer, Vector3D>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyCoordinateSystem.MyCreateCoordSysBuffer owner,
          in Vector3D value)
        {
          owner.Position = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyCoordinateSystem.MyCreateCoordSysBuffer owner,
          out Vector3D value)
        {
          value = owner.Position;
        }
      }

      protected class Sandbox_Game_GameSystems_CoordinateSystem_MyCoordinateSystem\u003C\u003EMyCreateCoordSysBuffer\u003C\u003ERotation\u003C\u003EAccessor : IMemberAccessor<MyCoordinateSystem.MyCreateCoordSysBuffer, Quaternion>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyCoordinateSystem.MyCreateCoordSysBuffer owner,
          in Quaternion value)
        {
          owner.Rotation = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyCoordinateSystem.MyCreateCoordSysBuffer owner,
          out Quaternion value)
        {
          value = owner.Rotation;
        }
      }
    }

    public struct CoordSystemData
    {
      public long Id;
      public MyTransformD SnappedTransform;
      public MyTransformD Origin;
      public Vector3D LocalSnappedPos;
    }

    protected sealed class CoordSysCreated_Client\u003C\u003ESandbox_Game_GameSystems_CoordinateSystem_MyCoordinateSystem\u003C\u003EMyCreateCoordSysBuffer : ICallSite<IMyEventOwner, MyCoordinateSystem.MyCreateCoordSysBuffer, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MyCoordinateSystem.MyCreateCoordSysBuffer createBuffer,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyCoordinateSystem.CoordSysCreated_Client(createBuffer);
      }
    }

    protected sealed class CoorSysRemoved_Client\u003C\u003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long coordSysId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyCoordinateSystem.CoorSysRemoved_Client(coordSysId);
      }
    }
  }
}
