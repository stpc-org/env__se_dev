// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Ingame.IMyMechanicalConnectionBlock
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using System;
using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
  public interface IMyMechanicalConnectionBlock : IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
  {
    IMyCubeGrid TopGrid { get; }

    IMyAttachableTopBlock Top { get; }

    [Obsolete("SafetyLock is no longer supported. This is property dummy property only, for backwards compatibility.")]
    float SafetyLockSpeed { get; set; }

    [Obsolete("SafetyLock is no longer supported. This is property dummy property only, for backwards compatibility.")]
    bool SafetyLock { get; set; }

    bool IsAttached { get; }

    [Obsolete("SafetyLock is no longer supported. This is property dummy property only, for backwards compatibility.")]
    bool IsLocked { get; }

    bool PendingAttachment { get; }

    void Attach();

    void Detach();
  }
}
