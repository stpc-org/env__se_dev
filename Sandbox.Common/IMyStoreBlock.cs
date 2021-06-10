// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.IMyStoreBlock
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI;

namespace Sandbox.ModAPI
{
  public interface IMyStoreBlock : IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyStoreBlock
  {
    MyStoreInsertResults InsertOffer(MyStoreItemData item, out long id);

    MyStoreInsertResults InsertOrder(MyStoreItemData item, out long id);
  }
}
