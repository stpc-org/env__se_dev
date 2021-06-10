// Decompiled with JetBrains decompiler
// Type: VRage.Game.VisualScripting.VisualScriptingEvent
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Linq;

namespace VRage.Game.VisualScripting
{
  [AttributeUsage(AttributeTargets.Delegate, AllowMultiple = true)]
  public class VisualScriptingEvent : Attribute
  {
    public readonly bool[] IsKey;
    public readonly KeyTypeEnum[] KeyTypes;

    public bool HasKeys
    {
      get
      {
        if (this.IsKey == null)
          return false;
        foreach (bool flag in this.IsKey)
        {
          if (flag)
            return true;
        }
        return false;
      }
    }

    public VisualScriptingEvent()
      : this((bool[]) null)
    {
    }

    public VisualScriptingEvent(bool[] @params, KeyTypeEnum[] keyTypes = null)
    {
      this.IsKey = @params;
      if (keyTypes == null && this.IsKey != null)
        keyTypes = Enumerable.Repeat<KeyTypeEnum>(KeyTypeEnum.Unknown, this.IsKey.Length).ToArray<KeyTypeEnum>();
      this.KeyTypes = keyTypes;
    }
  }
}
