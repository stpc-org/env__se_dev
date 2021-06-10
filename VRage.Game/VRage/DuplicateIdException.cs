// Decompiled with JetBrains decompiler
// Type: VRage.DuplicateIdException
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using VRage.ModAPI;

namespace VRage
{
  public class DuplicateIdException : Exception
  {
    public IMyEntity NewEntity;
    public IMyEntity OldEntity;

    public DuplicateIdException(IMyEntity newEntity, IMyEntity oldEntity)
    {
      this.NewEntity = newEntity;
      this.OldEntity = oldEntity;
    }

    public override string ToString() => "newEntity: " + (object) this.OldEntity.GetType() + ", oldEntity: " + (object) this.NewEntity.GetType() + base.ToString();
  }
}
