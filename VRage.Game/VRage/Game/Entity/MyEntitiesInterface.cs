// Decompiled with JetBrains decompiler
// Type: VRage.Game.Entity.MyEntitiesInterface
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;

namespace VRage.Game.Entity
{
  public class MyEntitiesInterface
  {
    public static Action<MyEntity> RegisterUpdate;
    public static Action<MyEntity, bool> UnregisterUpdate;
    public static Action<MyEntity> RegisterDraw;
    public static Action<MyEntity> UnregisterDraw;
    public static Action<MyEntity, bool> SetEntityName;
    public static Func<bool> IsUpdateInProgress;
    public static Func<bool> IsCloseAllowed;
    public static Action<MyEntity> RemoveName;
    public static Action<MyEntity> RemoveFromClosedEntities;
    public static Func<MyEntity, bool> Remove;
    public static Action<MyEntity> RaiseEntityRemove;
    public static Action<MyEntity> Close;
  }
}
