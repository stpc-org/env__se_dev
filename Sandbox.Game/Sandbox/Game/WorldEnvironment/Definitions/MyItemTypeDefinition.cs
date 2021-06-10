// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.WorldEnvironment.Definitions.MyItemTypeDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.WorldEnvironment.ObjectBuilders;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Game.WorldEnvironment.Definitions
{
  public class MyItemTypeDefinition
  {
    public string Name;
    public int LodFrom;
    public int LodTo;
    public MyItemTypeDefinition.Module StorageModule;
    public MyItemTypeDefinition.Module[] ProxyModules;

    public MyItemTypeDefinition(MyEnvironmentItemTypeDefinition def)
    {
      this.Name = def.Name;
      this.LodFrom = def.LodFrom == -1 ? 15 : def.LodFrom;
      this.LodTo = def.LodTo;
      if (def.Provider.HasValue)
      {
        MyProceduralEnvironmentModuleDefinition definition = MyDefinitionManager.Static.GetDefinition<MyProceduralEnvironmentModuleDefinition>((MyDefinitionId) def.Provider.Value);
        if (definition == null)
        {
          MyLog.Default.Error("Could not find module definition for type {0}.", (object) def.Provider.Value);
        }
        else
        {
          this.StorageModule.Type = definition.ModuleType;
          this.StorageModule.Definition = (MyDefinitionId) def.Provider.Value;
        }
      }
      if (def.Proxies == null)
        return;
      List<MyItemTypeDefinition.Module> moduleList = new List<MyItemTypeDefinition.Module>();
      foreach (SerializableDefinitionId proxy in def.Proxies)
      {
        MyEnvironmentModuleProxyDefinition definition = MyDefinitionManager.Static.GetDefinition<MyEnvironmentModuleProxyDefinition>((MyDefinitionId) proxy);
        if (definition == null)
          MyLog.Default.Error("Could not find proxy module definition for type {0}.", (object) proxy);
        else
          moduleList.Add(new MyItemTypeDefinition.Module()
          {
            Type = definition.ModuleType,
            Definition = (MyDefinitionId) proxy
          });
      }
      moduleList.Capacity = moduleList.Count;
      this.ProxyModules = moduleList.ToArray();
    }

    public struct Module
    {
      public Type Type;
      public MyDefinitionId Definition;
    }
  }
}
