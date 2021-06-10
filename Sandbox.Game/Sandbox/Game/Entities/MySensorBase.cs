// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MySensorBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using System.Linq;
using VRage;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Entities
{
  internal class MySensorBase : MyEntity
  {
    private Stack<MySensorBase.DetectedEntityInfo> m_unusedInfos = new Stack<MySensorBase.DetectedEntityInfo>();
    private Dictionary<MyEntity, MySensorBase.DetectedEntityInfo> m_detectedEntities = new Dictionary<MyEntity, MySensorBase.DetectedEntityInfo>((IEqualityComparer<MyEntity>) new InstanceComparer<MyEntity>());
    private List<MyEntity> m_deleteList = new List<MyEntity>();
    private Action<MyPositionComponentBase> m_entityPositionChanged;
    private Action<MyEntity> m_entityClosed;

    public MySensorBase()
    {
      this.Save = false;
      this.m_entityPositionChanged = new Action<MyPositionComponentBase>(this.entity_OnPositionChanged);
      this.m_entityClosed = new Action<MyEntity>(this.entity_OnClose);
    }

    public MyEntity GetClosestEntity(Vector3 position)
    {
      MyEntity myEntity = (MyEntity) null;
      double num1 = double.MaxValue;
      foreach (KeyValuePair<MyEntity, MySensorBase.DetectedEntityInfo> detectedEntity in this.m_detectedEntities)
      {
        double num2 = (position - detectedEntity.Key.PositionComp.GetPosition()).LengthSquared();
        if (num2 < num1)
        {
          num1 = num2;
          myEntity = detectedEntity.Key;
        }
      }
      return myEntity;
    }

    public event SensorFilterHandler Filter;

    public event EntitySensorHandler EntityEntered;

    public event EntitySensorHandler EntityMoved;

    public event EntitySensorHandler EntityLeft;

    private MySensorBase.DetectedEntityInfo GetInfo() => this.m_unusedInfos.Count == 0 ? new MySensorBase.DetectedEntityInfo() : this.m_unusedInfos.Pop();

    protected void TrackEntity(MyEntity entity)
    {
      if (this.FilterEntity(entity))
        return;
      MySensorBase.DetectedEntityInfo info;
      if (!this.m_detectedEntities.TryGetValue(entity, out info))
      {
        entity.PositionComp.OnPositionChanged += this.m_entityPositionChanged;
        entity.OnClose += this.m_entityClosed;
        info = this.GetInfo();
        info.Moved = false;
        info.EventType = MySensorBase.EventType.Add;
        this.m_detectedEntities[entity] = info;
      }
      else
      {
        if (info.EventType != MySensorBase.EventType.Delete)
          return;
        info.EventType = MySensorBase.EventType.None;
      }
    }

    protected bool FilterEntity(MyEntity entity)
    {
      SensorFilterHandler filter = this.Filter;
      if (filter != null)
      {
        bool processEntity = true;
        filter(this, entity, ref processEntity);
        if (!processEntity)
          return true;
      }
      return false;
    }

    public bool AnyEntityWithState(MySensorBase.EventType type) => this.m_detectedEntities.Any<KeyValuePair<MyEntity, MySensorBase.DetectedEntityInfo>>((Func<KeyValuePair<MyEntity, MySensorBase.DetectedEntityInfo>, bool>) (s => s.Value.EventType == type));

    public bool HasAnyMoved() => this.m_detectedEntities.Any<KeyValuePair<MyEntity, MySensorBase.DetectedEntityInfo>>((Func<KeyValuePair<MyEntity, MySensorBase.DetectedEntityInfo>, bool>) (s => s.Value.Moved));

    private void UntrackEntity(MyEntity entity)
    {
      entity.PositionComp.OnPositionChanged -= this.m_entityPositionChanged;
      entity.OnClose -= this.m_entityClosed;
    }

    private void entity_OnClose(MyEntity obj)
    {
      MySensorBase.DetectedEntityInfo detectedEntityInfo;
      if (!this.m_detectedEntities.TryGetValue(obj, out detectedEntityInfo))
        return;
      detectedEntityInfo.EventType = MySensorBase.EventType.Delete;
    }

    private void entity_OnPositionChanged(MyPositionComponentBase entity)
    {
      MySensorBase.DetectedEntityInfo detectedEntityInfo;
      if (!this.m_detectedEntities.TryGetValue(entity.Container.Entity as MyEntity, out detectedEntityInfo))
        return;
      detectedEntityInfo.Moved = true;
    }

    private void raise_EntityEntered(MyEntity entity)
    {
      EntitySensorHandler entityEntered = this.EntityEntered;
      if (entityEntered == null)
        return;
      entityEntered(this, entity);
    }

    private void raise_EntityMoved(MyEntity entity)
    {
      EntitySensorHandler entityMoved = this.EntityMoved;
      if (entityMoved == null)
        return;
      entityMoved(this, entity);
    }

    private void raise_EntityLeft(MyEntity entity)
    {
      EntitySensorHandler entityLeft = this.EntityLeft;
      if (entityLeft == null)
        return;
      entityLeft(this, entity);
    }

    public void RaiseAllMove()
    {
      EntitySensorHandler entityMoved = this.EntityMoved;
      if (entityMoved == null)
        return;
      foreach (KeyValuePair<MyEntity, MySensorBase.DetectedEntityInfo> detectedEntity in this.m_detectedEntities)
        entityMoved(this, detectedEntity.Key);
    }

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      foreach (KeyValuePair<MyEntity, MySensorBase.DetectedEntityInfo> detectedEntity in this.m_detectedEntities)
      {
        if (detectedEntity.Value.EventType == MySensorBase.EventType.Delete)
        {
          this.UntrackEntity(detectedEntity.Key);
          this.raise_EntityLeft(detectedEntity.Key);
          this.m_deleteList.Add(detectedEntity.Key);
          this.m_unusedInfos.Push(detectedEntity.Value);
        }
        else
        {
          if (detectedEntity.Value.EventType == MySensorBase.EventType.Add)
            this.raise_EntityEntered(detectedEntity.Key);
          else if (detectedEntity.Value.Moved)
            this.raise_EntityMoved(detectedEntity.Key);
          detectedEntity.Value.Moved = false;
          detectedEntity.Value.EventType = MySensorBase.EventType.Delete;
        }
      }
      foreach (MyEntity delete in this.m_deleteList)
        this.m_detectedEntities.Remove(delete);
      this.m_deleteList.Clear();
    }

    public enum EventType : byte
    {
      None,
      Add,
      Delete,
    }

    private class DetectedEntityInfo
    {
      public bool Moved;
      public MySensorBase.EventType EventType;
    }

    private class Sandbox_Game_Entities_MySensorBase\u003C\u003EActor : IActivator, IActivator<MySensorBase>
    {
      object IActivator.CreateInstance() => (object) new MySensorBase();

      MySensorBase IActivator<MySensorBase>.CreateInstance() => new MySensorBase();
    }
  }
}
