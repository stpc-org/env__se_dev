// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.IMyGui
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using VRage.ModAPI;

namespace VRage.Game.ModAPI
{
  public interface IMyGui
  {
    event Action<object> GuiControlCreated;

    event Action<object> GuiControlRemoved;

    string ActiveGamePlayScreen { get; }

    IMyEntity InteractedEntity { get; }

    MyTerminalPageEnum GetCurrentScreen { get; }

    bool ChatEntryVisible { get; }

    bool IsCursorVisible { get; }
  }
}
