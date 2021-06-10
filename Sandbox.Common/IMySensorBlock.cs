// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.IMySensorBlock
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using System;
using VRageMath;

namespace Sandbox.ModAPI
{
  public interface IMySensorBlock : IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMySensorBlock
  {
    event Action<bool> StateChanged;

    Vector3 FieldMin { get; set; }

    Vector3 FieldMax { get; set; }
  }
}
