// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Physics.MyWeldingGroups
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Game.Entities;
using System.Threading;
using VRage.Game.Entity;
using VRage.Groups;

namespace Sandbox.Engine.Physics
{
  public class MyWeldingGroups : MyGroups<MyEntity, MyWeldGroupData>, IMySceneComponent
  {
    private static MyWeldingGroups m_static;

    public static MyWeldingGroups Static => MyWeldingGroups.m_static;

    public void Load()
    {
      MyWeldingGroups.m_static = this;
      this.SupportsOphrans = true;
    }

    public void Unload() => MyWeldingGroups.m_static = (MyWeldingGroups) null;

    public static MyEntity ReplaceParent(
      MyGroups<MyEntity, MyWeldGroupData>.Group group,
      MyEntity oldParent,
      MyEntity newParent)
    {
      if (oldParent != null && oldParent.Physics != null)
      {
        oldParent.GetPhysicsBody().UnweldAll(false);
      }
      else
      {
        if (group == null)
          return oldParent;
        foreach (MyGroups<MyEntity, MyWeldGroupData>.Node node in group.Nodes)
        {
          if (!node.NodeData.MarkedForClose)
            node.NodeData.GetPhysicsBody().Unweld(false);
        }
      }
      if (group == null)
        return oldParent;
      if (newParent == null)
      {
        foreach (MyGroups<MyEntity, MyWeldGroupData>.Node node in group.Nodes)
        {
          if (!node.NodeData.MarkedForClose && node.NodeData != oldParent)
          {
            if (node.NodeData.Physics.IsStatic)
            {
              newParent = node.NodeData;
              break;
            }
            if ((HkReferenceObject) node.NodeData.Physics.RigidBody2 != (HkReferenceObject) null)
              newParent = node.NodeData;
          }
        }
      }
      foreach (MyGroups<MyEntity, MyWeldGroupData>.Node node in group.Nodes)
      {
        if (!node.NodeData.MarkedForClose && newParent != node.NodeData)
        {
          if (newParent == null)
            newParent = node.NodeData;
          else
            newParent.GetPhysicsBody().Weld(node.NodeData.Physics, false);
        }
      }
      if (newParent != null && !newParent.Physics.IsInWorld)
        newParent.Physics.Activate();
      return newParent;
    }

    public override void CreateLink(long linkId, MyEntity parentNode, MyEntity childNode)
    {
      if (MySandboxGame.Static.UpdateThread != Thread.CurrentThread)
        return;
      base.CreateLink(linkId, parentNode, childNode);
    }

    public bool IsEntityParent(MyEntity entity)
    {
      MyGroups<MyEntity, MyWeldGroupData>.Group group = this.GetGroup(entity);
      return group == null || entity == group.GroupData.Parent;
    }

    public MyWeldingGroups()
      : base()
    {
    }
  }
}
