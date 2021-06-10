// Decompiled with JetBrains decompiler
// Type: VRage.Game.Definitions.MyDefinitionPostprocessor
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using VRage.Utils;

namespace VRage.Game.Definitions
{
  public abstract class MyDefinitionPostprocessor
  {
    public Type DefinitionType;
    public static MyDefinitionPostprocessor.PostprocessorComparer Comparer = new MyDefinitionPostprocessor.PostprocessorComparer();

    public virtual int Priority => 500;

    public abstract void AfterLoaded(ref MyDefinitionPostprocessor.Bundle definitions);

    public abstract void AfterPostprocess(
      MyDefinitionSet set,
      Dictionary<MyStringHash, MyDefinitionBase> definitions);

    public virtual void OverrideBy(
      ref MyDefinitionPostprocessor.Bundle currentDefinitions,
      ref MyDefinitionPostprocessor.Bundle overrideBySet)
    {
      foreach (KeyValuePair<MyStringHash, MyDefinitionBase> definition in overrideBySet.Definitions)
      {
        if (definition.Value.Enabled)
          currentDefinitions.Definitions[definition.Key] = definition.Value;
        else
          currentDefinitions.Definitions.Remove(definition.Key);
      }
    }

    public struct Bundle
    {
      public MyModContext Context;
      public MyDefinitionSet Set;
      public Dictionary<MyStringHash, MyDefinitionBase> Definitions;
    }

    public class PostprocessorComparer : IComparer<MyDefinitionPostprocessor>
    {
      public int Compare(MyDefinitionPostprocessor x, MyDefinitionPostprocessor y) => y.Priority - x.Priority;
    }
  }
}
