// Decompiled with JetBrains decompiler
// Type: VRage.Game.Definitions.MyDefinitionTypeAttribute
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using VRage.Game.Common;

namespace VRage.Game.Definitions
{
  public class MyDefinitionTypeAttribute : MyFactoryTagAttribute
  {
    public readonly Type PostProcessor;

    public MyDefinitionTypeAttribute(Type objectBuilderType, Type postProcessor = null)
      : base(objectBuilderType)
    {
      if (postProcessor == (Type) null)
        postProcessor = typeof (NullDefinitionPostprocessor);
      else if (!typeof (MyDefinitionPostprocessor).IsAssignableFrom(postProcessor))
        throw new ArgumentException("postProcessor processor must be a subclass of MyDefinitionPostprocessor.", nameof (postProcessor));
      this.PostProcessor = postProcessor;
    }
  }
}
