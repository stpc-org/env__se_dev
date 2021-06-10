// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.EntityComponents.MyModStorageComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using System;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRage.Serialization;
using VRage.Utils;

namespace Sandbox.Game.EntityComponents
{
  [MyComponentType(typeof (MyModStorageComponent))]
  [MyComponentBuilder(typeof (MyObjectBuilder_ModStorageComponent), true)]
  public class MyModStorageComponent : MyModStorageComponentBase
  {
    private HashSet<Guid> m_cachedGuids = new HashSet<Guid>();

    public IReadOnlyDictionary<Guid, string> Storage => (IReadOnlyDictionary<Guid, string>) this.m_storageData;

    public override bool IsSerialized() => this.m_storageData.Count > 0;

    public override string GetValue(Guid guid) => this.m_storageData[guid];

    public override bool TryGetValue(Guid guid, out string value)
    {
      if (this.m_storageData.ContainsKey(guid))
      {
        value = this.m_storageData[guid];
        return true;
      }
      value = (string) null;
      return false;
    }

    public override void SetValue(Guid guid, string value) => this.m_storageData[guid] = value;

    public override bool RemoveValue(Guid guid) => this.m_storageData.Remove(guid);

    public override MyObjectBuilder_ComponentBase Serialize(bool copy = false)
    {
      MyObjectBuilder_ModStorageComponent storageComponent = (MyObjectBuilder_ModStorageComponent) base.Serialize(copy);
      storageComponent.Storage = new SerializableDictionary<Guid, string>();
      foreach (MyModStorageComponentDefinition componentDefinition in MyDefinitionManager.Static.GetEntityComponentDefinitions<MyModStorageComponentDefinition>())
      {
        foreach (Guid registeredStorageGuid in componentDefinition.RegisteredStorageGuids)
        {
          if (!this.m_cachedGuids.Add(registeredStorageGuid))
            MyLog.Default.Log(MyLogSeverity.Warning, "Duplicate ModStorageComponent GUID: {0}, in {1}:{2} - {3}", (object) registeredStorageGuid.ToString(), (object) componentDefinition.Context.ModServiceName, (object) componentDefinition.Context.ModId, (object) componentDefinition.Id.ToString());
        }
      }
      foreach (Guid key in this.Storage.Keys)
      {
        if (this.m_cachedGuids.Contains(key))
          storageComponent.Storage[key] = this.Storage[key];
        else
          MyLog.Default.Log(MyLogSeverity.Warning, "Not saving ModStorageComponent GUID: {0}, not claimed", (object) key.ToString());
      }
      this.m_cachedGuids.Clear();
      return storageComponent.Storage.Dictionary.Count == 0 ? (MyObjectBuilder_ComponentBase) null : (MyObjectBuilder_ComponentBase) storageComponent;
    }

    public override void Deserialize(MyObjectBuilder_ComponentBase builder)
    {
      SerializableDictionary<Guid, string> storage = ((MyObjectBuilder_ModStorageComponent) builder).Storage;
      if (storage == null || storage.Dictionary == null)
        return;
      this.m_storageData = (IDictionary<Guid, string>) new Dictionary<Guid, string>((IDictionary<Guid, string>) storage.Dictionary);
    }

    private class Sandbox_Game_EntityComponents_MyModStorageComponent\u003C\u003EActor : IActivator, IActivator<MyModStorageComponent>
    {
      object IActivator.CreateInstance() => (object) new MyModStorageComponent();

      MyModStorageComponent IActivator<MyModStorageComponent>.CreateInstance() => new MyModStorageComponent();
    }
  }
}
