// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyCubeGridGroups
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Cube;
using System.Collections.Generic;
using VRage.Game.ModAPI;
using VRage.Groups;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities
{
  public class MyCubeGridGroups : IMySceneComponent
  {
    public static MyCubeGridGroups Static;
    private MyGroupsBase<MyCubeGrid>[] m_groupsByType;
    public MyGroups<MyCubeGrid, MyGridLogicalGroupData> Logical = new MyGroups<MyCubeGrid, MyGridLogicalGroupData>(true);
    public MyGroups<MyCubeGrid, MyGridPhysicalGroupData> Physical = new MyGroups<MyCubeGrid, MyGridPhysicalGroupData>(true, new MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.MajorGroupComparer(MyGridPhysicalGroupData.IsMajorGroup));
    public MyGroups<MyCubeGrid, MyGridNoDamageGroupData> NoContactDamage = new MyGroups<MyCubeGrid, MyGridNoDamageGroupData>(true);
    public MyGroups<MyCubeGrid, MyGridMechanicalGroupData> Mechanical = new MyGroups<MyCubeGrid, MyGridMechanicalGroupData>(true);
    public MyGroups<MySlimBlock, MyBlockGroupData> SmallToLargeBlockConnections = new MyGroups<MySlimBlock, MyBlockGroupData>();
    public MyGroups<MyCubeGrid, MyGridPhysicalDynamicGroupData> PhysicalDynamic = new MyGroups<MyCubeGrid, MyGridPhysicalDynamicGroupData>();
    private static readonly HashSet<object> m_tmpBlocksDebugHelper = new HashSet<object>();

    public MyCubeGridGroups()
    {
      this.m_groupsByType = new MyGroupsBase<MyCubeGrid>[4];
      this.m_groupsByType[0] = (MyGroupsBase<MyCubeGrid>) this.Logical;
      this.m_groupsByType[1] = (MyGroupsBase<MyCubeGrid>) this.Physical;
      this.m_groupsByType[2] = (MyGroupsBase<MyCubeGrid>) this.NoContactDamage;
      this.m_groupsByType[3] = (MyGroupsBase<MyCubeGrid>) this.Mechanical;
    }

    public void AddNode(GridLinkTypeEnum type, MyCubeGrid grid) => this.GetGroups(type).AddNode(grid);

    public void RemoveNode(GridLinkTypeEnum type, MyCubeGrid grid) => this.GetGroups(type).RemoveNode(grid);

    public void CreateLink(
      GridLinkTypeEnum type,
      long linkId,
      MyCubeGrid parent,
      MyCubeGrid child)
    {
      this.GetGroups(type).CreateLink(linkId, parent, child);
      if (type != GridLinkTypeEnum.Physical || parent.Physics.IsStatic || child.Physics.IsStatic)
        return;
      this.PhysicalDynamic.CreateLink(linkId, parent, child);
    }

    public bool BreakLink(GridLinkTypeEnum type, long linkId, MyCubeGrid parent, MyCubeGrid child = null)
    {
      if (type == GridLinkTypeEnum.Physical)
        this.PhysicalDynamic.BreakLink(linkId, parent, child);
      return this.GetGroups(type).BreakLink(linkId, parent, child);
    }

    public void UpdateDynamicState(MyCubeGrid grid)
    {
      bool flag1 = this.PhysicalDynamic.GetGroup(grid) != null;
      bool flag2 = !grid.IsStatic;
      if (flag1 && !flag2)
      {
        this.PhysicalDynamic.BreakAllLinks(grid);
      }
      else
      {
        if (!(!flag1 & flag2))
          return;
        MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node node = this.Physical.GetNode(grid);
        if (node == null)
          return;
        foreach (KeyValuePair<long, MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node> childLink in node.ChildLinks)
        {
          if (!childLink.Value.NodeData.IsStatic)
            this.PhysicalDynamic.CreateLink(childLink.Key, grid, childLink.Value.NodeData);
        }
        foreach (KeyValuePair<long, MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node> parentLink in node.ParentLinks)
        {
          if (!parentLink.Value.NodeData.IsStatic)
            this.PhysicalDynamic.CreateLink(parentLink.Key, parentLink.Value.NodeData, grid);
        }
      }
    }

    public MyGroupsBase<MyCubeGrid> GetGroups(GridLinkTypeEnum type) => this.m_groupsByType[(int) type];

    void IMySceneComponent.Load() => MyCubeGridGroups.Static = new MyCubeGridGroups();

    void IMySceneComponent.Unload() => MyCubeGridGroups.Static = (MyCubeGridGroups) null;

    internal static void DebugDrawBlockGroups<TNode, TGroupData>(MyGroups<TNode, TGroupData> groups)
      where TNode : MySlimBlock
      where TGroupData : IGroupData<TNode>, new()
    {
      int num1 = 0;
      foreach (MyGroups<TNode, TGroupData>.Group group in groups.Groups)
      {
        Color color1 = new Vector3((float) (num1++ % 15) / 15f, 1f, 1f).HSVtoColor();
        foreach (MyGroups<TNode, TGroupData>.Node node1 in group.Nodes)
        {
          try
          {
            BoundingBoxD aabb1;
            node1.NodeData.GetWorldBoundingBox(out aabb1, false);
            foreach (MyGroups<TNode, TGroupData>.Node child in node1.Children)
              MyCubeGridGroups.m_tmpBlocksDebugHelper.Add((object) child);
            foreach (object obj in MyCubeGridGroups.m_tmpBlocksDebugHelper)
            {
              MyGroups<TNode, TGroupData>.Node node2 = (MyGroups<TNode, TGroupData>.Node) null;
              int num2 = 0;
              foreach (MyGroups<TNode, TGroupData>.Node child in node1.Children)
              {
                if (obj == child)
                {
                  node2 = child;
                  ++num2;
                }
              }
              BoundingBoxD aabb2;
              node2.NodeData.GetWorldBoundingBox(out aabb2, false);
              MyRenderProxy.DebugDrawLine3D(aabb1.Center, aabb2.Center, color1, color1, false);
              MyRenderProxy.DebugDrawText3D((aabb1.Center + aabb2.Center) * 0.5, num2.ToString(), color1, 1f, false);
            }
            Color color2 = new Color(color1.ToVector3() + 0.25f);
            MyRenderProxy.DebugDrawSphere(aabb1.Center, 0.2f, (Color) color2.ToVector3(), 0.5f, false, true);
            MyRenderProxy.DebugDrawText3D(aabb1.Center, node1.LinkCount.ToString(), color2, 1f, false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
          }
          finally
          {
            MyCubeGridGroups.m_tmpBlocksDebugHelper.Clear();
          }
        }
      }
    }
  }
}
