// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.IngameObjectiveAttribute
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;

namespace Sandbox.Game.SessionComponents
{
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
  public class IngameObjectiveAttribute : Attribute
  {
    private string m_id;
    private int m_priority;

    public IngameObjectiveAttribute(string id, int priority)
    {
      this.m_id = id;
      this.m_priority = priority;
    }

    public string Id => this.m_id;

    public int Priority => this.m_priority;
  }
}
