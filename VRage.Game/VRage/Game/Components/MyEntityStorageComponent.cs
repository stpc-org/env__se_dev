// Decompiled with JetBrains decompiler
// Type: VRage.Game.Components.MyEntityStorageComponent
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRage.ObjectBuilder;
using VRage.Serialization;
using VRageMath;

namespace VRage.Game.Components
{
  [MyComponentType(typeof (MyEntityStorageComponent))]
  [MyComponentBuilder(typeof (MyObjectBuilder_EntityStorageComponent), true)]
  public class MyEntityStorageComponent : MyEntityComponentBase
  {
    private MyObjectBuilder_EntityStorageComponent m_objectBuilder = new MyObjectBuilder_EntityStorageComponent();

    public override string ComponentTypeDebugString => "Entity Storage";

    public override MyObjectBuilder_ComponentBase Serialize(bool copy = false) => (MyObjectBuilder_ComponentBase) this.m_objectBuilder;

    public override bool IsSerialized() => true;

    public override void Deserialize(MyObjectBuilder_ComponentBase builder)
    {
      MyObjectBuilder_EntityStorageComponent storageComponent = (MyObjectBuilder_EntityStorageComponent) builder;
      this.m_objectBuilder = new MyObjectBuilder_EntityStorageComponent()
      {
        BoolStorage = storageComponent.BoolStorage,
        FloatStorage = storageComponent.FloatStorage,
        StringStorage = storageComponent.StringStorage,
        IntStorage = storageComponent.IntStorage,
        Vector3DStorage = storageComponent.Vector3DStorage,
        LongStorage = storageComponent.LongStorage,
        BoolListStorage = storageComponent.BoolListStorage,
        FloatListStorage = storageComponent.FloatListStorage,
        StringListStorage = storageComponent.StringListStorage,
        IntListStorage = storageComponent.IntListStorage,
        Vector3DListStorage = storageComponent.Vector3DListStorage,
        LongListStorage = storageComponent.LongListStorage
      };
    }

    public bool Write(string variableName, int value)
    {
      if (this.m_objectBuilder == null)
        return false;
      this.m_objectBuilder.IntStorage.Dictionary[variableName] = value;
      return true;
    }

    public bool Write(string variableName, long value)
    {
      this.m_objectBuilder.LongStorage.Dictionary[variableName] = value;
      return true;
    }

    public bool Write(string variableName, bool value)
    {
      this.m_objectBuilder.BoolStorage.Dictionary[variableName] = value;
      return true;
    }

    public bool Write(string variableName, float value)
    {
      this.m_objectBuilder.FloatStorage.Dictionary[variableName] = value;
      return true;
    }

    public bool Write(string variableName, string value)
    {
      this.m_objectBuilder.StringStorage.Dictionary[variableName] = value;
      return true;
    }

    public bool Write(string variableName, Vector3D value)
    {
      this.m_objectBuilder.Vector3DStorage.Dictionary[variableName] = (SerializableVector3D) value;
      return true;
    }

    public int ReadInt(string variableName)
    {
      int num;
      return this.m_objectBuilder.IntStorage.Dictionary.TryGetValue(variableName, out num) ? num : -1;
    }

    public long ReadLong(string variableName)
    {
      long num;
      return this.m_objectBuilder.LongStorage.Dictionary.TryGetValue(variableName, out num) ? num : -1L;
    }

    public float ReadFloat(string variableName)
    {
      float num;
      return this.m_objectBuilder.FloatStorage.Dictionary.TryGetValue(variableName, out num) ? num : 0.0f;
    }

    public string ReadString(string variableName)
    {
      string str;
      return this.m_objectBuilder.StringStorage.Dictionary.TryGetValue(variableName, out str) ? str : (string) null;
    }

    public Vector3D ReadVector3D(string variableName)
    {
      SerializableVector3D serializableVector3D;
      return this.m_objectBuilder.Vector3DStorage.Dictionary.TryGetValue(variableName, out serializableVector3D) ? (Vector3D) serializableVector3D : Vector3D.Zero;
    }

    public bool ReadBool(string variableName)
    {
      bool flag;
      return this.m_objectBuilder.BoolStorage.Dictionary.TryGetValue(variableName, out flag) && flag;
    }

    public bool Write(string variableName, List<int> value)
    {
      this.m_objectBuilder.IntListStorage.Dictionary[variableName] = new MySerializableList<int>((IEnumerable<int>) value);
      return true;
    }

    public bool Write(string variableName, List<long> value)
    {
      this.m_objectBuilder.LongListStorage.Dictionary[variableName] = new MySerializableList<long>((IEnumerable<long>) value);
      return true;
    }

    public bool Write(string variableName, List<bool> value)
    {
      this.m_objectBuilder.BoolListStorage.Dictionary[variableName] = new MySerializableList<bool>((IEnumerable<bool>) value);
      return true;
    }

    public bool Write(string variableName, List<float> value)
    {
      this.m_objectBuilder.FloatListStorage.Dictionary[variableName] = new MySerializableList<float>((IEnumerable<float>) value);
      return true;
    }

    public bool Write(string variableName, List<string> value)
    {
      this.m_objectBuilder.StringListStorage.Dictionary[variableName] = new MySerializableList<string>((IEnumerable<string>) value);
      return true;
    }

    public bool Write(string variableName, List<Vector3D> value)
    {
      this.m_objectBuilder.Vector3DListStorage.Dictionary[variableName] = new MySerializableList<SerializableVector3D>(value.Cast<SerializableVector3D>());
      return true;
    }

    public List<int> ReadIntList(string variableName)
    {
      MySerializableList<int> serializableList;
      return this.m_objectBuilder.IntListStorage.Dictionary.TryGetValue(variableName, out serializableList) ? (List<int>) serializableList : new List<int>();
    }

    public List<long> ReadLongList(string variableName)
    {
      MySerializableList<long> serializableList;
      return this.m_objectBuilder.LongListStorage.Dictionary.TryGetValue(variableName, out serializableList) ? (List<long>) serializableList : new List<long>();
    }

    public List<float> ReadFloatList(string variableName)
    {
      MySerializableList<float> serializableList;
      return this.m_objectBuilder.FloatListStorage.Dictionary.TryGetValue(variableName, out serializableList) ? (List<float>) serializableList : new List<float>();
    }

    public List<string> ReadStringList(string variableName)
    {
      MySerializableList<string> serializableList;
      return this.m_objectBuilder.StringListStorage.Dictionary.TryGetValue(variableName, out serializableList) ? (List<string>) serializableList : new List<string>();
    }

    public List<Vector3D> ReadVector3DList(string variableName)
    {
      MySerializableList<SerializableVector3D> source;
      return this.m_objectBuilder.Vector3DListStorage.Dictionary.TryGetValue(variableName, out source) ? source.Cast<Vector3D>().ToList<Vector3D>() : new List<Vector3D>();
    }

    public List<bool> ReadBoolList(string variableName)
    {
      MySerializableList<bool> serializableList;
      return this.m_objectBuilder.BoolListStorage.Dictionary.TryGetValue(variableName, out serializableList) ? (List<bool>) serializableList : new List<bool>();
    }

    public SerializableDictionary<string, bool> GetBools() => this.m_objectBuilder.BoolStorage;

    public SerializableDictionary<string, int> GetInts() => this.m_objectBuilder.IntStorage;

    public SerializableDictionary<string, long> GetLongs() => this.m_objectBuilder.LongStorage;

    public SerializableDictionary<string, string> GetStrings() => this.m_objectBuilder.StringStorage;

    public SerializableDictionary<string, float> GetFloats() => this.m_objectBuilder.FloatStorage;

    public SerializableDictionary<string, SerializableVector3D> GetVector3D() => this.m_objectBuilder.Vector3DStorage;

    public Dictionary<string, bool> GetBoolsByRegex(Regex nameRegex)
    {
      Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
      foreach (KeyValuePair<string, bool> keyValuePair in this.m_objectBuilder.BoolStorage.Dictionary)
      {
        if (nameRegex.IsMatch(keyValuePair.Key))
          dictionary.Add(keyValuePair.Key, keyValuePair.Value);
      }
      return dictionary;
    }

    private class VRage_Game_Components_MyEntityStorageComponent\u003C\u003EActor : IActivator, IActivator<MyEntityStorageComponent>
    {
      object IActivator.CreateInstance() => (object) new MyEntityStorageComponent();

      MyEntityStorageComponent IActivator<MyEntityStorageComponent>.CreateInstance() => new MyEntityStorageComponent();
    }
  }
}
