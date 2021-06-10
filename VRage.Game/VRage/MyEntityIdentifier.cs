// Decompiled with JetBrains decompiler
// Type: VRage.MyEntityIdentifier
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Utils;

namespace VRage
{
  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct MyEntityIdentifier
  {
    private const int DEFAULT_DICTIONARY_SIZE = 32768;
    [ThreadStatic]
    private static bool m_inEntityCreationBlock;
    [ThreadStatic]
    private static MyEntityIdentifier.PerThreadData m_perThreadData;
    private static MyEntityIdentifier.PerThreadData m_mainData;
    private static bool m_isSwapPrepared = false;
    private static bool m_isSwapped = false;
    private static MyEntityIdentifier.PerThreadData m_perThreadData_Swap = (MyEntityIdentifier.PerThreadData) null;
    private static long[] m_lastGeneratedIds = new long[(int) (MyEnum<MyEntityIdentifier.ID_OBJECT_TYPE>.Range.Max + (byte) 1)];

    public static bool InEntityCreationBlock
    {
      get => MyEntityIdentifier.m_inEntityCreationBlock;
      set
      {
        if (value)
        {
          Thread mainThread = MyUtils.MainThread;
          Thread currentThread = Thread.CurrentThread;
        }
        MyEntityIdentifier.m_inEntityCreationBlock = value;
      }
    }

    private static Dictionary<long, IMyEntity> EntityList => (MyEntityIdentifier.m_perThreadData ?? MyEntityIdentifier.m_mainData).EntityList;

    public static bool SwapPerThreadData()
    {
      MyEntityIdentifier.PerThreadData perThreadData = MyEntityIdentifier.m_perThreadData;
      MyEntityIdentifier.m_perThreadData = MyEntityIdentifier.m_perThreadData_Swap;
      MyEntityIdentifier.m_perThreadData_Swap = perThreadData;
      MyEntityIdentifier.m_isSwapped = !MyEntityIdentifier.m_isSwapped;
      return MyEntityIdentifier.m_isSwapped;
    }

    public static void PrepareSwapData()
    {
      MyEntityIdentifier.m_isSwapPrepared = true;
      MyEntityIdentifier.m_perThreadData_Swap = new MyEntityIdentifier.PerThreadData(32768);
    }

    public static void ClearSwapDataAndRestore()
    {
      if (MyEntityIdentifier.m_isSwapped)
      {
        MyEntityIdentifier.m_perThreadData = MyEntityIdentifier.m_perThreadData_Swap;
        MyEntityIdentifier.m_perThreadData_Swap = (MyEntityIdentifier.PerThreadData) null;
        MyEntityIdentifier.m_isSwapped = false;
        MyEntityIdentifier.m_isSwapPrepared = false;
      }
      else
      {
        MyEntityIdentifier.m_perThreadData_Swap = (MyEntityIdentifier.PerThreadData) null;
        MyEntityIdentifier.m_isSwapPrepared = false;
      }
    }

    public static bool AllocationSuspended
    {
      get => (MyEntityIdentifier.m_perThreadData ?? MyEntityIdentifier.m_mainData).AllocationSuspended;
      set => (MyEntityIdentifier.m_perThreadData ?? MyEntityIdentifier.m_mainData).AllocationSuspended = value;
    }

    static MyEntityIdentifier()
    {
      MyEntityIdentifier.m_mainData = new MyEntityIdentifier.PerThreadData(32768);
      MyEntityIdentifier.m_perThreadData = MyEntityIdentifier.m_mainData;
    }

    public static void InitPerThreadStorage(int defaultCapacity) => MyEntityIdentifier.m_perThreadData = new MyEntityIdentifier.PerThreadData(defaultCapacity);

    public static void LazyInitPerThreadStorage(int defaultCapacity)
    {
      if (MyEntityIdentifier.m_perThreadData != null && MyEntityIdentifier.m_perThreadData != MyEntityIdentifier.m_mainData)
        return;
      MyEntityIdentifier.m_perThreadData = new MyEntityIdentifier.PerThreadData(defaultCapacity);
    }

    public static void DestroyPerThreadStorage() => MyEntityIdentifier.m_perThreadData = (MyEntityIdentifier.PerThreadData) null;

    public static void GetPerThreadEntities(List<IMyEntity> result)
    {
      foreach (KeyValuePair<long, IMyEntity> entity in MyEntityIdentifier.m_perThreadData.EntityList)
        result.Add(entity.Value);
    }

    public static void ClearPerThreadEntities() => MyEntityIdentifier.m_perThreadData.EntityList.Clear();

    public static void Reset() => Array.Clear((Array) MyEntityIdentifier.m_lastGeneratedIds, 0, MyEntityIdentifier.m_lastGeneratedIds.Length);

    public static void MarkIdUsed(long id)
    {
      long idUniqueNumber = MyEntityIdentifier.GetIdUniqueNumber(id);
      MyEntityIdentifier.ID_OBJECT_TYPE idObjectType = MyEntityIdentifier.GetIdObjectType(id);
      MyUtils.InterlockedMax(ref MyEntityIdentifier.m_lastGeneratedIds[(int) idObjectType], idUniqueNumber);
    }

    public static void AddEntityWithId(IMyEntity entity)
    {
      if (MyEntityIdentifier.EntityList.ContainsKey(entity.EntityId))
        throw new DuplicateIdException(entity, MyEntityIdentifier.EntityList[entity.EntityId]);
      MyEntityIdentifier.EntityList.Add(entity.EntityId, entity);
    }

    public static long AllocateId(
      MyEntityIdentifier.ID_OBJECT_TYPE objectType = MyEntityIdentifier.ID_OBJECT_TYPE.ENTITY,
      MyEntityIdentifier.ID_ALLOCATION_METHOD generationMethod = MyEntityIdentifier.ID_ALLOCATION_METHOD.RANDOM)
    {
      long uniqueNumber = generationMethod != MyEntityIdentifier.ID_ALLOCATION_METHOD.RANDOM ? Interlocked.Increment(ref MyEntityIdentifier.m_lastGeneratedIds[(int) objectType]) : MyRandom.Instance.NextLong() & 72057594037927935L;
      return MyEntityIdentifier.ConstructId(objectType, uniqueNumber);
    }

    public static MyEntityIdentifier.ID_OBJECT_TYPE GetIdObjectType(long id) => (MyEntityIdentifier.ID_OBJECT_TYPE) (id >> 56);

    public static long GetIdUniqueNumber(long id) => id & 72057594037927935L;

    public static long ConstructIdFromString(
      MyEntityIdentifier.ID_OBJECT_TYPE type,
      string uniqueString)
    {
      long hashCode64 = uniqueString.GetHashCode64();
      return (hashCode64 >> 8) + hashCode64 + (hashCode64 << 13) & 72057594037927935L | (long) type << 56;
    }

    public static long ConstructId(MyEntityIdentifier.ID_OBJECT_TYPE type, long uniqueNumber) => uniqueNumber & 72057594037927935L | (long) type << 56;

    public static long FixObsoleteIdentityType(long id)
    {
      if (MyEntityIdentifier.GetIdObjectType(id) == MyEntityIdentifier.ID_OBJECT_TYPE.NPC || MyEntityIdentifier.GetIdObjectType(id) == MyEntityIdentifier.ID_OBJECT_TYPE.SPAWN_GROUP)
        id = MyEntityIdentifier.ConstructId(MyEntityIdentifier.ID_OBJECT_TYPE.IDENTITY, MyEntityIdentifier.GetIdUniqueNumber(id));
      return id;
    }

    public static void RemoveEntity(long entityId) => MyEntityIdentifier.EntityList.Remove(entityId);

    public static IMyEntity GetEntityById(long entityId, bool allowClosed = false)
    {
      IMyEntity myEntity;
      if (!MyEntityIdentifier.EntityList.TryGetValue(entityId, out myEntity) && MyEntityIdentifier.m_perThreadData != null)
        MyEntityIdentifier.m_mainData.EntityList.TryGetValue(entityId, out myEntity);
      return myEntity != null && !allowClosed && myEntity.GetTopMostParent().Closed ? (IMyEntity) null : myEntity;
    }

    public static bool TryGetEntity(long entityId, out IMyEntity entity, bool allowClosed = false)
    {
      bool flag = MyEntityIdentifier.EntityList.TryGetValue(entityId, out entity);
      if (!flag && MyEntityIdentifier.m_perThreadData != null)
        flag = MyEntityIdentifier.m_mainData.EntityList.TryGetValue(entityId, out entity);
      if (entity != null && !allowClosed && entity.GetTopMostParent().Closed)
      {
        entity = (IMyEntity) null;
        flag = false;
      }
      return flag;
    }

    public static bool TryGetEntity<T>(long entityId, out T entity, bool allowClosed = false) where T : class, IMyEntity
    {
      IMyEntity entity1;
      int num = MyEntityIdentifier.TryGetEntity(entityId, out entity1, allowClosed) ? 1 : 0;
      entity = entity1 as T;
      return num != 0 && (object) entity != null;
    }

    public static bool ExistsById(long entityId)
    {
      if (MyEntityIdentifier.EntityList.ContainsKey(entityId))
        return true;
      return MyEntityIdentifier.m_perThreadData != null && MyEntityIdentifier.m_mainData.EntityList.ContainsKey(entityId);
    }

    public static void SwapRegisteredEntityId(IMyEntity entity, long oldId, long newId)
    {
      if (MyEntityIdentifier.EntityList.TryGetValue(oldId, out IMyEntity _))
        MyEntityIdentifier.EntityList.Remove(oldId);
      MyEntityIdentifier.EntityList[newId] = entity;
    }

    public static void Clear() => MyEntityIdentifier.EntityList.Clear();

    private class PerThreadData
    {
      public bool AllocationSuspended;
      public Dictionary<long, IMyEntity> EntityList;

      public PerThreadData(int defaultCapacity) => this.EntityList = new Dictionary<long, IMyEntity>(defaultCapacity);
    }

    public enum ID_OBJECT_TYPE : byte
    {
      UNKNOWN,
      ENTITY,
      IDENTITY,
      FACTION,
      NPC,
      SPAWN_GROUP,
      ASTEROID,
      PLANET,
      VOXEL_PHYSICS,
      PLANET_ENVIRONMENT_SECTOR,
      PLANET_ENVIRONMENT_ITEM,
      PLANET_VOXEL_DETAIL,
      STATION,
      CONTRACT,
      CONTRACT_CONDITION,
      STORE_ITEM,
    }

    public enum ID_ALLOCATION_METHOD : byte
    {
      RANDOM,
      SERIAL_START_WITH_1,
    }
  }
}
