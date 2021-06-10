// Decompiled with JetBrains decompiler
// Type: VRage.ObjectBuilders.MyObjectBuilderDefinitionAttribute
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using VRage.Game.Common;

namespace VRage.ObjectBuilders
{
  public class MyObjectBuilderDefinitionAttribute : MyFactoryTagAttribute
  {
    private Type ObsoleteBy;
    public readonly string LegacyName;

    public MyObjectBuilderDefinitionAttribute(Type obsoleteBy = null, string LegacyName = null)
      : base((Type) null)
    {
      this.ObsoleteBy = obsoleteBy;
      this.LegacyName = LegacyName;
    }
  }
}
