// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.IMyBlockGroup
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using System;
using System.Collections.Generic;

namespace Sandbox.ModAPI
{
  public interface IMyBlockGroup : Sandbox.ModAPI.Ingame.IMyBlockGroup
  {
    void GetBlocks(List<IMyTerminalBlock> blocks, Func<IMyTerminalBlock, bool> collect = null);

    void GetBlocksOfType<T>(List<IMyTerminalBlock> blocks, Func<IMyTerminalBlock, bool> collect = null) where T : class;
  }
}
