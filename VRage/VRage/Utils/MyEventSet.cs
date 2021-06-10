// Decompiled with JetBrains decompiler
// Type: VRage.Utils.MyEventSet
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;

namespace VRage.Utils
{
  public sealed class MyEventSet
  {
    private readonly Dictionary<MyStringId, Delegate> m_events = new Dictionary<MyStringId, Delegate>((IEqualityComparer<MyStringId>) MyStringId.Comparer);

    public void Add(MyStringId eventKey, Delegate handler)
    {
      Delegate a;
      this.m_events.TryGetValue(eventKey, out a);
      this.m_events[eventKey] = Delegate.Combine(a, handler);
    }

    public void Remove(MyStringId eventKey, Delegate handler)
    {
      Delegate source;
      if (!this.m_events.TryGetValue(eventKey, out source))
        return;
      Delegate @delegate = Delegate.Remove(source, handler);
      if ((object) @delegate != null)
        this.m_events[eventKey] = @delegate;
      else
        this.m_events.Remove(eventKey);
    }

    public void Raise(MyStringId eventKey, object sender, EventArgs e)
    {
      Delegate @delegate;
      this.m_events.TryGetValue(eventKey, out @delegate);
      if ((object) @delegate == null)
        return;
      @delegate.DynamicInvoke(sender, (object) e);
    }
  }
}
