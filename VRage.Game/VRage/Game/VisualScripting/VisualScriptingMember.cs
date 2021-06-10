// Decompiled with JetBrains decompiler
// Type: VRage.Game.VisualScripting.VisualScriptingMember
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;

namespace VRage.Game.VisualScripting
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
  public class VisualScriptingMember : Attribute
  {
    public readonly bool Sequential;
    public readonly bool Reserved;

    public VisualScriptingMember(bool isSequenceDependent = false, bool reserved = false)
    {
      this.Sequential = isSequenceDependent;
      this.Reserved = reserved;
    }
  }
}
