// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.WorldEnvironment.Definitions.MyEnvironmentModuleProxyDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.WorldEnvironment.ObjectBuilders;
using System;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilder;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Game.WorldEnvironment.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_EnvironmentModuleProxyDefinition), null)]
  public class MyEnvironmentModuleProxyDefinition : MyDefinitionBase
  {
    public Type ModuleType;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_EnvironmentModuleProxyDefinition moduleProxyDefinition = (MyObjectBuilder_EnvironmentModuleProxyDefinition) builder;
      this.ModuleType = MyGlobalTypeMetadata.Static.GetType(moduleProxyDefinition.QualifiedTypeName, false);
      if (this.ModuleType == (Type) null)
      {
        MyLog.Default.Error("Could not find module type {0}!", (object) moduleProxyDefinition.QualifiedTypeName);
        throw new ArgumentException("Could not find module type;");
      }
    }

    private class Sandbox_Game_WorldEnvironment_Definitions_MyEnvironmentModuleProxyDefinition\u003C\u003EActor : IActivator, IActivator<MyEnvironmentModuleProxyDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyEnvironmentModuleProxyDefinition();

      MyEnvironmentModuleProxyDefinition IActivator<MyEnvironmentModuleProxyDefinition>.CreateInstance() => new MyEnvironmentModuleProxyDefinition();
    }
  }
}
