// Decompiled with JetBrains decompiler
// Type: VRage.Game.GUI.TextPanel.ContentType
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;

namespace VRage.Game.GUI.TextPanel
{
  public enum ContentType : byte
  {
    NONE,
    TEXT_AND_IMAGE,
    [Obsolete("Use TEXT_AND_IMAGE instead.")] IMAGE,
    SCRIPT,
  }
}
