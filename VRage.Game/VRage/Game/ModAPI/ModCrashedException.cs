// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.ModCrashedException
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;

namespace VRage.Game.ModAPI
{
  public class ModCrashedException : Exception
  {
    public IMyModContext ModContext { get; private set; }

    public ModCrashedException(Exception innerException, IMyModContext modContext)
      : base("Mod crashed!", innerException)
      => this.ModContext = modContext;
  }
}
