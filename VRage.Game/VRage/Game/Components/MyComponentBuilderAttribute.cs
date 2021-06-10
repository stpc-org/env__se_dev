// Decompiled with JetBrains decompiler
// Type: VRage.Game.Components.MyComponentBuilderAttribute
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using VRage.Game.Common;

namespace VRage.Game.Components
{
  public class MyComponentBuilderAttribute : MyFactoryTagAttribute
  {
    public MyComponentBuilderAttribute(Type objectBuilderType, bool mainBuilder = true)
      : base(objectBuilderType, mainBuilder)
    {
    }
  }
}
