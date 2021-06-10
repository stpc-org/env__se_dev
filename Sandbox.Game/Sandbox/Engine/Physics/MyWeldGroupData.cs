// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Physics.MyWeldGroupData
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Game.Entities;
using System;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Groups;

namespace Sandbox.Engine.Physics
{
  public class MyWeldGroupData : IGroupData<MyEntity>
  {
    private MyGroups<MyEntity, MyWeldGroupData>.Group m_group;
    private MyEntity m_weldParent;

    public MyEntity Parent => this.m_weldParent;

    public void OnRelease()
    {
      this.m_group = (MyGroups<MyEntity, MyWeldGroupData>.Group) null;
      this.m_weldParent = (MyEntity) null;
    }

    public void OnNodeAdded(MyEntity entity)
    {
      if (entity.MarkedForClose)
        return;
      if (this.m_weldParent == null)
      {
        this.m_weldParent = entity;
      }
      else
      {
        MyPhysicsBody physics = this.m_weldParent.Physics as MyPhysicsBody;
        if (physics.IsStatic)
          physics.Weld(entity.Physics as MyPhysicsBody, true);
        else if (entity.Physics.IsStatic || (HkReferenceObject) physics.RigidBody2 == (HkReferenceObject) null && (HkReferenceObject) entity.Physics.RigidBody2 != (HkReferenceObject) null)
          this.ReplaceParent(entity);
        else
          physics.Weld(entity.Physics as MyPhysicsBody, true);
      }
      if (this.m_weldParent.Physics != null && (HkReferenceObject) this.m_weldParent.Physics.RigidBody != (HkReferenceObject) null)
        this.m_weldParent.Physics.RigidBody.Activate();
      this.m_weldParent.RaisePhysicsChanged();
    }

    public void OnNodeRemoved(MyEntity entity)
    {
      if (this.m_weldParent == null)
        return;
      if (this.m_weldParent == entity)
      {
        if ((this.m_group.Nodes.Count != 1 || !this.m_group.Nodes.First().NodeData.MarkedForClose) && this.m_group.Nodes.Count > 0)
          this.ReplaceParent((MyEntity) null);
      }
      else if (this.m_weldParent.Physics != null && !entity.MarkedForClose)
        (this.m_weldParent.Physics as MyPhysicsBody).Unweld(entity.Physics as MyPhysicsBody);
      if (this.m_weldParent != null && this.m_weldParent.Physics != null && (HkReferenceObject) this.m_weldParent.Physics.RigidBody != (HkReferenceObject) null)
      {
        this.m_weldParent.Physics.RigidBody.Activate();
        this.m_weldParent.RaisePhysicsChanged();
      }
      entity.RaisePhysicsChanged();
    }

    private void ReplaceParent(MyEntity newParent) => this.m_weldParent = MyWeldingGroups.ReplaceParent(this.m_group, this.m_weldParent, newParent);

    public void OnCreate<TGroupData>(MyGroups<MyEntity, TGroupData>.Group group) where TGroupData : IGroupData<MyEntity>, new() => this.m_group = group as MyGroups<MyEntity, MyWeldGroupData>.Group;

    public bool UpdateParent(MyEntity oldParent)
    {
      MyPhysicsBody physicsBody = oldParent.GetPhysicsBody();
      if (physicsBody.WeldedRigidBody.IsFixed)
        return false;
      MyPhysicsBody myPhysicsBody = physicsBody;
      foreach (MyPhysicsBody child in physicsBody.WeldInfo.Children)
      {
        if (child.WeldedRigidBody.IsFixed)
        {
          myPhysicsBody = child;
          break;
        }
        if (!myPhysicsBody.Flags.HasFlag((Enum) RigidBodyFlag.RBF_DOUBLED_KINEMATIC) && child.Flags.HasFlag((Enum) RigidBodyFlag.RBF_DOUBLED_KINEMATIC))
          myPhysicsBody = child;
      }
      if (myPhysicsBody == physicsBody)
        return false;
      this.ReplaceParent((MyEntity) myPhysicsBody.Entity);
      myPhysicsBody.Weld(physicsBody, true);
      return true;
    }
  }
}
