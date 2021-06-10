// Decompiled with JetBrains decompiler
// Type: VRage.Game.VisualScripting.VisualScriptingMiscData
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;

namespace VRage.Game.VisualScripting
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
  public class VisualScriptingMiscData : Attribute
  {
    private const int m_cadetBlue = -10510688;
    public readonly string Group;
    public readonly string Comment;
    public readonly int BackgroundColor;

    public VisualScriptingMiscData(string group, string comment = null, int backgroundColor = -10510688)
    {
      this.Group = group;
      this.Comment = comment;
      this.BackgroundColor = backgroundColor;
    }
  }
}
