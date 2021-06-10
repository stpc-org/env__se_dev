// Decompiled with JetBrains decompiler
// Type: VRage.Utils.MyEventArgs
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace VRage.Utils
{
  public class MyEventArgs : EventArgs
  {
    private Dictionary<MyStringId, object> m_args = new Dictionary<MyStringId, object>((IEqualityComparer<MyStringId>) MyStringId.Comparer);

    public Dictionary<MyStringId, object>.KeyCollection ArgNames => this.m_args.Keys;

    public MyEventArgs()
    {
    }

    public MyEventArgs(KeyValuePair<MyStringId, object> arg) => this.SetArg(arg.Key, arg.Value);

    public MyEventArgs(KeyValuePair<MyStringId, object>[] args)
    {
      foreach (KeyValuePair<MyStringId, object> keyValuePair in args)
        this.SetArg(keyValuePair.Key, keyValuePair.Value);
    }

    public object GetArg(MyStringId argName) => !this.ArgNames.Contains<MyStringId>(argName) ? (object) null : this.m_args[argName];

    public void SetArg(MyStringId argName, object value)
    {
      this.m_args.Remove(argName);
      this.m_args.Add(argName, value);
    }
  }
}
