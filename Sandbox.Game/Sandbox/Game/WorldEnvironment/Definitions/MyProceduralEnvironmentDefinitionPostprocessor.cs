// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.WorldEnvironment.Definitions.MyProceduralEnvironmentDefinitionPostprocessor
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Utils;

namespace Sandbox.Game.WorldEnvironment.Definitions
{
  public class MyProceduralEnvironmentDefinitionPostprocessor : MyDefinitionPostprocessor
  {
    public override void AfterLoaded(ref MyDefinitionPostprocessor.Bundle definitions)
    {
    }

    public override void AfterPostprocess(
      MyDefinitionSet set,
      Dictionary<MyStringHash, MyDefinitionBase> definitions)
    {
      foreach (KeyValuePair<MyStringHash, MyDefinitionBase> definition in definitions)
        ((MyProceduralEnvironmentDefinition) definition.Value).Prepare();
    }
  }
}
