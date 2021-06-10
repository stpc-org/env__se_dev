// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.UseObject.IMyUsableEntity
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game.Entity.UseObject;

namespace Sandbox.Game.Entities.UseObject
{
  public interface IMyUsableEntity
  {
    UseActionResult CanUse(UseActionEnum actionEnum, IMyControllableEntity user);

    void RemoveUsers(bool local);
  }
}
