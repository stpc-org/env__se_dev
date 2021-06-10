// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyGridPhysicalGroupData
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Game.GameSystems;
using System;
using System.Buffers;
using System.Diagnostics;
using System.Threading;
using VRage;
using VRage.Collections;
using VRage.Groups;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities
{
  public class MyGridPhysicalGroupData : IGroupData<MyCubeGrid>
  {
    private volatile Ref<MyGridPhysicalGroupData.GroupSharedPxProperties> m_groupPropertiesCache;
    private MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Group m_group;
    internal readonly MyGroupControlSystem ControlSystem = new MyGroupControlSystem();

    public static MyGridPhysicalGroupData.GroupSharedPxProperties GetGroupSharedProperties(
      MyCubeGrid localGrid,
      bool checkMultithreading = true)
    {
      MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Group group = MyCubeGridGroups.Static.Physical.GetGroup(localGrid);
      if (group != null)
        return group.GroupData.GetSharedPxProperties(localGrid);
      HkMassProperties valueOrDefault = MyGridPhysicalGroupData.GetGridMassProperties(localGrid).GetValueOrDefault();
      return new MyGridPhysicalGroupData.GroupSharedPxProperties(localGrid, valueOrDefault, 1);
    }

    public static void InvalidateSharedMassPropertiesCache(MyCubeGrid groupRepresentative) => MyCubeGridGroups.Static.Physical.GetGroup(groupRepresentative)?.GroupData.InvalidateCoMCache();

    private MyGridPhysicalGroupData.GroupSharedPxProperties GetSharedPxProperties(
      MyCubeGrid referenceGrid)
    {
      Ref<MyGridPhysicalGroupData.GroupSharedPxProperties> groupPropertiesCache = this.m_groupPropertiesCache;
      if (groupPropertiesCache == null)
      {
        HashSetReader<MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node> nodes = this.m_group.Nodes;
        MatrixD matrixD1 = referenceGrid.PositionComp.WorldMatrixNormalizedInv;
        int count = nodes.Count;
        HkMassElement[] array = ArrayPool<HkMassElement>.Shared.Rent(count);
        Span<HkMassElement> span = new Span<HkMassElement>(array, 0, count);
        int length = 0;
        foreach (MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node node in nodes)
        {
          MyCubeGrid nodeData = node.NodeData;
          HkMassProperties? gridMassProperties = MyGridPhysicalGroupData.GetGridMassProperties(nodeData);
          if (gridMassProperties.HasValue)
          {
            MatrixD matrixD2 = nodeData.PositionComp.WorldMatrixRef * matrixD1;
            span[length++] = new HkMassElement()
            {
              Tranform = (Matrix) ref matrixD2,
              Properties = gridMassProperties.Value
            };
          }
        }
        HkMassProperties massProperties;
        HkInertiaTensorComputer.CombineMassProperties(span.Slice(0, length), out massProperties);
        ArrayPool<HkMassElement>.Shared.Return(array);
        groupPropertiesCache = Ref.Create<MyGridPhysicalGroupData.GroupSharedPxProperties>(new MyGridPhysicalGroupData.GroupSharedPxProperties(referenceGrid, massProperties, nodes.Count));
        this.m_groupPropertiesCache = groupPropertiesCache;
      }
      return groupPropertiesCache.Value;
    }

    private static void DrawDebugSphere(
      MyCubeGrid referenceGrid,
      Color color,
      Vector3 localPosition,
      double radius)
    {
      MyRenderProxy.DebugDrawSphere(Vector3D.Transform(localPosition, referenceGrid.PositionComp.WorldMatrixRef), (float) radius, color, depthRead: false);
    }

    private void InvalidateCoMCache() => this.m_groupPropertiesCache = (Ref<MyGridPhysicalGroupData.GroupSharedPxProperties>) null;

    private static HkMassProperties? GetGridMassProperties(MyCubeGrid grid) => grid.Physics == null ? new HkMassProperties?() : grid.Physics.Shape.MassProperties;

    public void OnNodeAdded(MyCubeGrid entity)
    {
      this.InvalidateCoMCache();
      entity.OnAddedToGroup(this);
    }

    public void OnNodeRemoved(MyCubeGrid entity)
    {
      this.InvalidateCoMCache();
      entity.OnRemovedFromGroup(this);
    }

    internal static bool IsMajorGroup(
      MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Group a,
      MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Group b)
    {
      float num = 0.0f;
      foreach (MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node node in a.Nodes)
      {
        if (node.NodeData.Physics != null)
          num += node.NodeData.PositionComp.LocalVolume.Radius;
      }
      foreach (MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node node in b.Nodes)
      {
        if (node.NodeData.Physics != null)
          num -= node.NodeData.PositionComp.LocalVolume.Radius;
      }
      return (double) num > 0.0;
    }

    public void OnCreate<TGroupData>(MyGroups<MyCubeGrid, TGroupData>.Group group) where TGroupData : IGroupData<MyCubeGrid>, new() => this.m_group = group as MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Group;

    public void OnRelease()
    {
      this.m_group = (MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Group) null;
      this.m_groupPropertiesCache = (Ref<MyGridPhysicalGroupData.GroupSharedPxProperties>) null;
      this.ControlSystem.Clear();
    }

    [DebuggerStepThrough]
    [Conditional("DEBUG")]
    private static void AssertThread()
    {
      Thread updateThread = MySandboxGame.Static.UpdateThread;
      Thread currentThread = Thread.CurrentThread;
    }

    public struct GroupSharedPxProperties
    {
      public readonly int GridCount;
      public readonly MyCubeGrid ReferenceGrid;
      public readonly HkMassProperties PxProperties;

      public Matrix InertiaTensor => this.PxProperties.InertiaTensor;

      public float Mass => this.PxProperties.Mass;

      public Vector3D CoMWorld
      {
        get
        {
          Vector3 centerOfMass = this.PxProperties.CenterOfMass;
          MatrixD worldMatrix = this.ReferenceGrid.WorldMatrix;
          Vector3D result;
          Vector3D.Transform(ref centerOfMass, ref worldMatrix, out result);
          return result;
        }
      }

      public GroupSharedPxProperties(
        MyCubeGrid referenceGrid,
        HkMassProperties sharedProperties,
        int gridCount)
      {
        this.GridCount = gridCount;
        this.ReferenceGrid = referenceGrid;
        this.PxProperties = sharedProperties;
      }

      public Matrix GetInertiaTensorLocalToGrid(MyCubeGrid localGrid)
      {
        Matrix inertiaTensor = this.InertiaTensor;
        MatrixD result = (MatrixD) ref inertiaTensor;
        if (this.ReferenceGrid != localGrid)
        {
          MatrixD matrix2 = this.ReferenceGrid.WorldMatrix * localGrid.PositionComp.WorldMatrixNormalizedInv;
          MatrixD.Multiply(ref result, ref matrix2, out result);
        }
        return (Matrix) ref result;
      }
    }
  }
}
