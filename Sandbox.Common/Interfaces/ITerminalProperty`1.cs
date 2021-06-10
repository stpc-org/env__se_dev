﻿// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Interfaces.ITerminalProperty`1
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using System;
using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Interfaces
{
  public interface ITerminalProperty<TValue> : ITerminalProperty
  {
    TValue GetValue(IMyCubeBlock block);

    void SetValue(IMyCubeBlock block, TValue value);

    TValue GetDefaultValue(IMyCubeBlock block);

    [Obsolete("Use GetMinimum instead")]
    TValue GetMininum(IMyCubeBlock block);

    TValue GetMinimum(IMyCubeBlock block);

    TValue GetMaximum(IMyCubeBlock block);
  }
}
