// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.EntityComponents.MyAreaTriggerComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.EntityComponents
{
  [MyComponentBuilder(typeof (MyObjectBuilder_AreaTrigger), true)]
  public class MyAreaTriggerComponent : MyTriggerComponent
  {
    private readonly HashSet<MyEntity> m_prevEntities = new HashSet<MyEntity>();
    private readonly List<MyEntity> m_entitiesToRemove = new List<MyEntity>();
    private readonly HashSet<MyCharacter> m_prevPlayers = new HashSet<MyCharacter>();
    private readonly HashSet<MyCharacter> m_currentPlayers = new HashSet<MyCharacter>();
    private readonly List<MyCharacter> m_playersToRemove = new List<MyCharacter>();
    public Action<long, string> EntityEntered;

    public string Name { get; set; }

    public double Radius
    {
      get => this.m_boundingSphere.Radius;
      set
      {
        this.m_boundingSphere.Radius = value;
        this.m_AABB.Min = new Vector3D(-value / 2.0);
        this.m_AABB.Max = new Vector3D(value / 2.0);
        this.m_orientedBoundingBox.HalfExtent = new Vector3D(value / 2.0);
      }
    }

    public double SizeX
    {
      get => this.m_orientedBoundingBox.HalfExtent.X * 2.0;
      set
      {
        this.m_boundingSphere.Radius = value / 2.0;
        this.m_AABB.Min.X = -value / 2.0;
        this.m_AABB.Max.X = value / 2.0;
        this.m_orientedBoundingBox.HalfExtent.X = value / 2.0;
      }
    }

    public double SizeY
    {
      get => this.m_orientedBoundingBox.HalfExtent.Y * 2.0;
      set
      {
        this.m_boundingSphere.Radius = value / 2.0;
        this.m_AABB.Min.Y = -value / 2.0;
        this.m_AABB.Max.Y = value / 2.0;
        this.m_orientedBoundingBox.HalfExtent.Y = value / 2.0;
      }
    }

    public double SizeZ
    {
      get => this.m_orientedBoundingBox.HalfExtent.Z * 2.0;
      set
      {
        this.m_boundingSphere.Radius = value / 2.0;
        this.m_AABB.Min.Z = -value / 2.0;
        this.m_AABB.Max.Z = value / 2.0;
        this.m_orientedBoundingBox.HalfExtent.Z = value / 2.0;
      }
    }

    public MyAreaTriggerComponent()
      : this(string.Empty)
    {
    }

    public MyAreaTriggerComponent(string name)
      : base(MyTriggerComponent.TriggerType.Sphere, 20U)
      => this.Name = name;

    protected override void UpdateInternal()
    {
      base.UpdateInternal();
      this.m_currentPlayers.Clear();
      foreach (MyEntity myEntity in this.QueryResult)
      {
        if (myEntity is MyCharacter myCharacter && !myCharacter.IsBot && myCharacter.ControlSteamId != 0UL)
          this.m_currentPlayers.Add(myCharacter);
        if (myEntity is MyCubeGrid myCubeGrid && MySession.Static != null)
        {
          foreach (MyPlayer onlinePlayer in (IEnumerable<MyPlayer>) MySession.Static.Players.GetOnlinePlayers())
          {
            if (onlinePlayer.Character != null && onlinePlayer.Character.Parent is MyCockpit && ((MyCubeBlock) onlinePlayer.Character.Parent).CubeGrid.EntityId == myCubeGrid.EntityId)
              this.m_currentPlayers.Add(onlinePlayer.Character);
          }
        }
      }
      foreach (MyEntity prevEntity in this.m_prevEntities)
      {
        if (!this.QueryResult.Contains(prevEntity))
        {
          if (MyVisualScriptLogicProvider.AreaTrigger_EntityLeft != null)
            MyVisualScriptLogicProvider.AreaTrigger_EntityLeft(this.Name, prevEntity.EntityId, prevEntity.Name);
          this.m_entitiesToRemove.Add(prevEntity);
        }
      }
      foreach (MyEntity myEntity in this.m_entitiesToRemove)
        this.m_prevEntities.Remove(myEntity);
      foreach (MyCharacter prevPlayer in this.m_prevPlayers)
      {
        if (!this.m_currentPlayers.Contains(prevPlayer))
        {
          if (MyVisualScriptLogicProvider.AreaTrigger_Left != null && !prevPlayer.Closed)
          {
            MyIdentity identity = prevPlayer.GetIdentity();
            if (identity != null)
              MyVisualScriptLogicProvider.AreaTrigger_Left(this.Name, identity.IdentityId);
          }
          this.m_playersToRemove.Add(prevPlayer);
        }
      }
      foreach (MyCharacter myCharacter in this.m_playersToRemove)
        this.m_prevPlayers.Remove(myCharacter);
      this.m_entitiesToRemove.Clear();
      this.m_playersToRemove.Clear();
      foreach (MyEntity myEntity in this.QueryResult)
      {
        if (this.m_prevEntities.Add(myEntity))
        {
          if (MyVisualScriptLogicProvider.AreaTrigger_EntityEntered != null)
            MyVisualScriptLogicProvider.AreaTrigger_EntityEntered(this.Name, myEntity.EntityId, myEntity.Name);
          if (this.EntityEntered != null)
            this.EntityEntered(myEntity.EntityId, myEntity.Name);
        }
      }
      foreach (MyCharacter currentPlayer in this.m_currentPlayers)
      {
        if (this.m_prevPlayers.Add(currentPlayer) && MyVisualScriptLogicProvider.AreaTrigger_Entered != null)
        {
          MyIdentity identity = currentPlayer.GetIdentity();
          if (identity != null)
            MyVisualScriptLogicProvider.AreaTrigger_Entered(this.Name, identity.IdentityId);
        }
      }
    }

    protected override bool QueryEvaluator(MyEntity entity)
    {
      switch (entity)
      {
        case MyCharacter _:
          return true;
        case MyCubeGrid _:
          return true;
        case MyFloatingObject _:
          return true;
        default:
          return false;
      }
    }

    public override MyObjectBuilder_ComponentBase Serialize(bool copy)
    {
      MyObjectBuilder_AreaTrigger builderAreaTrigger = base.Serialize(copy) as MyObjectBuilder_AreaTrigger;
      builderAreaTrigger.Name = this.Name;
      return (MyObjectBuilder_ComponentBase) builderAreaTrigger;
    }

    public override void Deserialize(MyObjectBuilder_ComponentBase builder)
    {
      base.Deserialize(builder);
      this.Name = ((MyObjectBuilder_AreaTrigger) builder).Name;
    }

    public override bool IsSerialized() => true;

    public override void DebugDraw() => base.DebugDraw();

    private class Sandbox_Game_EntityComponents_MyAreaTriggerComponent\u003C\u003EActor : IActivator, IActivator<MyAreaTriggerComponent>
    {
      object IActivator.CreateInstance() => (object) new MyAreaTriggerComponent();

      MyAreaTriggerComponent IActivator<MyAreaTriggerComponent>.CreateInstance() => new MyAreaTriggerComponent();
    }
  }
}
