﻿// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Ingame.IMyGridTerminalSystem
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using System;
using System.Collections.Generic;

namespace Sandbox.ModAPI.Ingame
{
  public interface IMyGridTerminalSystem
  {
    void GetBlocks(List<IMyTerminalBlock> blocks);

    void GetBlockGroups(List<IMyBlockGroup> blockGroups, Func<IMyBlockGroup, bool> collect = null);

    void GetBlocksOfType<T>(List<IMyTerminalBlock> blocks, Func<IMyTerminalBlock, bool> collect = null) where T : class;

    void GetBlocksOfType<T>(List<T> blocks, Func<T, bool> collect = null) where T : class;

    void SearchBlocksOfName(
      string name,
      List<IMyTerminalBlock> blocks,
      Func<IMyTerminalBlock, bool> collect = null);

    IMyTerminalBlock GetBlockWithName(string name);

    IMyBlockGroup GetBlockGroupWithName(string name);

    IMyTerminalBlock GetBlockWithId(long id);
  }
}
