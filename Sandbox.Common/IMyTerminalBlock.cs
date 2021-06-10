// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.IMyTerminalBlock
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using System;
using System.Text;

namespace Sandbox.ModAPI
{
  public interface IMyTerminalBlock : VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyTerminalBlock
  {
    event Action<IMyTerminalBlock> CustomDataChanged;

    event Action<IMyTerminalBlock> CustomNameChanged;

    event Action<IMyTerminalBlock> OwnershipChanged;

    event Action<IMyTerminalBlock> PropertiesChanged;

    event Action<IMyTerminalBlock> ShowOnHUDChanged;

    event Action<IMyTerminalBlock> VisibilityChanged;

    event Action<IMyTerminalBlock, StringBuilder> AppendingCustomInfo;

    void RefreshCustomInfo();

    bool IsInSameLogicalGroupAs(IMyTerminalBlock other);

    bool IsSameConstructAs(IMyTerminalBlock other);
  }
}
