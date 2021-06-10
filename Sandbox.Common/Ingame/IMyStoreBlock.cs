// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Ingame.IMyStoreBlock
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using System.Collections.Generic;
using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
  public interface IMyStoreBlock : IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
  {
    MyStoreInsertResults InsertOffer(MyStoreItemDataSimple item, out long id);

    MyStoreInsertResults InsertOrder(MyStoreItemDataSimple item, out long id);

    bool CancelStoreItem(long id);

    void GetPlayerStoreItems(List<MyStoreQueryItem> storeItems);
  }
}
