// Decompiled with JetBrains decompiler
// Type: VRage.Game.Components.Session.MySessionComponentScriptSharedStorage
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using VRage.Game.ObjectBuilders.Components;
using VRage.ObjectBuilder;
using VRage.Serialization;
using VRageMath;

namespace VRage.Game.Components.Session
{
  [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate, 1000, typeof (MyObjectBuilder_SharedStorageComponent), null, false)]
  public class MySessionComponentScriptSharedStorage : MySessionComponentBase
  {
    private MyObjectBuilder_SharedStorageComponent m_objectBuilder = new MyObjectBuilder_SharedStorageComponent();
    private static MySessionComponentScriptSharedStorage m_instance;

    public static MySessionComponentScriptSharedStorage Instance => MySessionComponentScriptSharedStorage.m_instance;

    public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
    {
      base.Init(sessionComponent);
      this.m_objectBuilder = sessionComponent as MyObjectBuilder_SharedStorageComponent;
      MySessionComponentScriptSharedStorage.m_instance = this;
    }

    protected override void UnloadData()
    {
      base.UnloadData();
      MySessionComponentScriptSharedStorage.m_instance = (MySessionComponentScriptSharedStorage) null;
    }

    public override MyObjectBuilder_SessionComponent GetObjectBuilder() => (MyObjectBuilder_SessionComponent) this.m_objectBuilder;

    public bool Write(string variableName, string secondaryKey, int value)
    {
      if (this.m_objectBuilder == null)
        return false;
      if (string.IsNullOrEmpty(secondaryKey))
      {
        this.m_objectBuilder.IntStorage.Dictionary[variableName] = value;
      }
      else
      {
        if (!this.m_objectBuilder.IntStorageSecondary.Dictionary.ContainsKey(variableName))
          this.m_objectBuilder.IntStorageSecondary.Dictionary.Add(variableName, new SerializableDictionary<string, int>());
        this.m_objectBuilder.IntStorageSecondary.Dictionary[variableName][secondaryKey] = value;
      }
      return true;
    }

    public bool Write(string variableName, string secondaryKey, long value)
    {
      if (this.m_objectBuilder == null)
        return false;
      if (string.IsNullOrEmpty(secondaryKey))
      {
        this.m_objectBuilder.LongStorage.Dictionary[variableName] = value;
      }
      else
      {
        if (!this.m_objectBuilder.LongStorageSecondary.Dictionary.ContainsKey(variableName))
          this.m_objectBuilder.LongStorageSecondary.Dictionary.Add(variableName, new SerializableDictionary<string, long>());
        this.m_objectBuilder.LongStorageSecondary.Dictionary[variableName][secondaryKey] = value;
      }
      return true;
    }

    public bool Write(string variableName, string secondaryKey, ulong value)
    {
      if (this.m_objectBuilder == null)
        return false;
      if (string.IsNullOrEmpty(secondaryKey))
      {
        this.m_objectBuilder.ULongStorage.Dictionary[variableName] = value;
      }
      else
      {
        if (!this.m_objectBuilder.ULongStorageSecondary.Dictionary.ContainsKey(variableName))
          this.m_objectBuilder.ULongStorageSecondary.Dictionary.Add(variableName, new SerializableDictionary<string, ulong>());
        this.m_objectBuilder.ULongStorageSecondary.Dictionary[variableName][secondaryKey] = value;
      }
      return true;
    }

    public bool Write(string variableName, string secondaryKey, bool value)
    {
      if (this.m_objectBuilder == null)
        return false;
      if (string.IsNullOrEmpty(secondaryKey))
      {
        this.m_objectBuilder.BoolStorage.Dictionary[variableName] = value;
      }
      else
      {
        if (!this.m_objectBuilder.BoolStorageSecondary.Dictionary.ContainsKey(variableName))
          this.m_objectBuilder.BoolStorageSecondary.Dictionary.Add(variableName, new SerializableDictionary<string, bool>());
        this.m_objectBuilder.BoolStorageSecondary.Dictionary[variableName][secondaryKey] = value;
      }
      return true;
    }

    public bool Write(string variableName, string secondaryKey, float value)
    {
      if (this.m_objectBuilder == null)
        return false;
      if (string.IsNullOrEmpty(secondaryKey))
      {
        this.m_objectBuilder.FloatStorage.Dictionary[variableName] = value;
      }
      else
      {
        if (!this.m_objectBuilder.FloatStorageSecondary.Dictionary.ContainsKey(variableName))
          this.m_objectBuilder.FloatStorageSecondary.Dictionary.Add(variableName, new SerializableDictionary<string, float>());
        this.m_objectBuilder.FloatStorageSecondary.Dictionary[variableName][secondaryKey] = value;
      }
      return true;
    }

    public bool Write(string variableName, string secondaryKey, string value)
    {
      if (this.m_objectBuilder == null)
        return false;
      if (string.IsNullOrEmpty(secondaryKey))
      {
        this.m_objectBuilder.StringStorage.Dictionary[variableName] = value;
      }
      else
      {
        if (!this.m_objectBuilder.StringStorageSecondary.Dictionary.ContainsKey(variableName))
          this.m_objectBuilder.StringStorageSecondary.Dictionary.Add(variableName, new SerializableDictionary<string, string>());
        this.m_objectBuilder.StringStorageSecondary.Dictionary[variableName][secondaryKey] = value;
      }
      return true;
    }

    public bool Write(string variableName, string secondaryKey, Vector3D value)
    {
      if (this.m_objectBuilder == null)
        return false;
      if (string.IsNullOrEmpty(secondaryKey))
      {
        this.m_objectBuilder.Vector3DStorage.Dictionary[variableName] = (SerializableVector3D) value;
      }
      else
      {
        if (!this.m_objectBuilder.Vector3DStorageSecondary.Dictionary.ContainsKey(variableName))
          this.m_objectBuilder.Vector3DStorageSecondary.Dictionary.Add(variableName, new SerializableDictionary<string, SerializableVector3D>());
        this.m_objectBuilder.Vector3DStorageSecondary.Dictionary[variableName][secondaryKey] = (SerializableVector3D) value;
      }
      return true;
    }

    public int ReadInt(string variableName, string secondaryKey)
    {
      if (this.m_objectBuilder == null)
        return -1;
      if (string.IsNullOrEmpty(secondaryKey))
      {
        int num;
        if (this.m_objectBuilder.IntStorage.Dictionary.TryGetValue(variableName, out num))
          return num;
      }
      else
      {
        SerializableDictionary<string, int> serializableDictionary;
        int num;
        if (this.m_objectBuilder.IntStorageSecondary.Dictionary.TryGetValue(variableName, out serializableDictionary) && serializableDictionary.Dictionary.TryGetValue(secondaryKey, out num))
          return num;
      }
      return -1;
    }

    public long ReadLong(string variableName, string secondaryKey)
    {
      if (this.m_objectBuilder == null)
        return -1;
      if (string.IsNullOrEmpty(secondaryKey))
      {
        long num;
        if (this.m_objectBuilder.LongStorage.Dictionary.TryGetValue(variableName, out num))
          return num;
      }
      else
      {
        SerializableDictionary<string, long> serializableDictionary;
        long num;
        if (this.m_objectBuilder.LongStorageSecondary.Dictionary.TryGetValue(variableName, out serializableDictionary) && serializableDictionary.Dictionary.TryGetValue(secondaryKey, out num))
          return num;
      }
      return -1;
    }

    public ulong ReadULong(string variableName, string secondaryKey)
    {
      if (this.m_objectBuilder == null)
        return 0;
      if (string.IsNullOrEmpty(secondaryKey))
      {
        ulong num;
        if (this.m_objectBuilder.ULongStorage.Dictionary.TryGetValue(variableName, out num))
          return num;
      }
      else
      {
        SerializableDictionary<string, ulong> serializableDictionary;
        ulong num;
        if (this.m_objectBuilder.ULongStorageSecondary.Dictionary.TryGetValue(variableName, out serializableDictionary) && serializableDictionary.Dictionary.TryGetValue(secondaryKey, out num))
          return num;
      }
      return 0;
    }

    public float ReadFloat(string variableName, string secondaryKey)
    {
      if (this.m_objectBuilder == null)
        return 0.0f;
      if (string.IsNullOrEmpty(secondaryKey))
      {
        float num;
        if (this.m_objectBuilder.FloatStorage.Dictionary.TryGetValue(variableName, out num))
          return num;
      }
      else
      {
        SerializableDictionary<string, float> serializableDictionary;
        float num;
        if (this.m_objectBuilder.FloatStorageSecondary.Dictionary.TryGetValue(variableName, out serializableDictionary) && serializableDictionary.Dictionary.TryGetValue(secondaryKey, out num))
          return num;
      }
      return 0.0f;
    }

    public string ReadString(string variableName, string secondaryKey)
    {
      if (this.m_objectBuilder == null)
        return "";
      if (string.IsNullOrEmpty(secondaryKey))
      {
        string str;
        if (this.m_objectBuilder.StringStorage.Dictionary.TryGetValue(variableName, out str))
          return str;
      }
      else
      {
        SerializableDictionary<string, string> serializableDictionary;
        string str;
        if (this.m_objectBuilder.StringStorageSecondary.Dictionary.TryGetValue(variableName, out serializableDictionary) && serializableDictionary.Dictionary.TryGetValue(secondaryKey, out str))
          return str;
      }
      return "";
    }

    public Vector3D ReadVector3D(string variableName, string secondaryKey)
    {
      if (this.m_objectBuilder == null)
        return Vector3D.Zero;
      if (string.IsNullOrEmpty(secondaryKey))
      {
        SerializableVector3D serializableVector3D;
        if (this.m_objectBuilder.Vector3DStorage.Dictionary.TryGetValue(variableName, out serializableVector3D))
          return (Vector3D) serializableVector3D;
      }
      else
      {
        SerializableDictionary<string, SerializableVector3D> serializableDictionary;
        SerializableVector3D serializableVector3D;
        if (this.m_objectBuilder.Vector3DStorageSecondary.Dictionary.TryGetValue(variableName, out serializableDictionary) && serializableDictionary.Dictionary.TryGetValue(secondaryKey, out serializableVector3D))
          return (Vector3D) serializableVector3D;
      }
      return Vector3D.Zero;
    }

    public bool ReadBool(string variableName, string secondaryKey)
    {
      if (this.m_objectBuilder == null)
        return false;
      if (string.IsNullOrEmpty(secondaryKey))
      {
        bool flag;
        if (this.m_objectBuilder.BoolStorage.Dictionary.TryGetValue(variableName, out flag))
          return flag;
      }
      else
      {
        SerializableDictionary<string, bool> serializableDictionary;
        bool flag;
        if (this.m_objectBuilder.BoolStorageSecondary.Dictionary.TryGetValue(variableName, out serializableDictionary) && serializableDictionary.Dictionary.TryGetValue(secondaryKey, out flag))
          return flag;
      }
      return false;
    }

    public bool Write(string variableName, string secondaryKey, List<int> value)
    {
      if (this.m_objectBuilder == null)
        return false;
      if (string.IsNullOrEmpty(secondaryKey))
      {
        this.m_objectBuilder.IntListStorage.Dictionary[variableName] = new MySerializableList<int>((IEnumerable<int>) value);
      }
      else
      {
        if (!this.m_objectBuilder.IntListStorageSecondary.Dictionary.ContainsKey(variableName))
          this.m_objectBuilder.IntListStorageSecondary.Dictionary.Add(variableName, new SerializableDictionary<string, MySerializableList<int>>());
        this.m_objectBuilder.IntListStorageSecondary.Dictionary[variableName][secondaryKey] = new MySerializableList<int>((IEnumerable<int>) value);
      }
      return true;
    }

    public bool Write(string variableName, string secondaryKey, List<long> value)
    {
      if (this.m_objectBuilder == null)
        return false;
      if (string.IsNullOrEmpty(secondaryKey))
      {
        this.m_objectBuilder.LongListStorage.Dictionary[variableName] = new MySerializableList<long>((IEnumerable<long>) value);
      }
      else
      {
        if (!this.m_objectBuilder.LongListStorageSecondary.Dictionary.ContainsKey(variableName))
          this.m_objectBuilder.LongListStorageSecondary.Dictionary.Add(variableName, new SerializableDictionary<string, MySerializableList<long>>());
        this.m_objectBuilder.LongListStorageSecondary.Dictionary[variableName][secondaryKey] = new MySerializableList<long>((IEnumerable<long>) value);
      }
      return true;
    }

    public bool Write(string variableName, string secondaryKey, List<ulong> value)
    {
      if (this.m_objectBuilder == null)
        return false;
      if (string.IsNullOrEmpty(secondaryKey))
      {
        this.m_objectBuilder.ULongListStorage.Dictionary[variableName] = new MySerializableList<ulong>((IEnumerable<ulong>) value);
      }
      else
      {
        if (!this.m_objectBuilder.ULongListStorageSecondary.Dictionary.ContainsKey(variableName))
          this.m_objectBuilder.ULongListStorageSecondary.Dictionary.Add(variableName, new SerializableDictionary<string, MySerializableList<ulong>>());
        this.m_objectBuilder.ULongListStorageSecondary.Dictionary[variableName][secondaryKey] = new MySerializableList<ulong>((IEnumerable<ulong>) value);
      }
      return true;
    }

    public bool Write(string variableName, string secondaryKey, List<bool> value)
    {
      if (this.m_objectBuilder == null)
        return false;
      if (string.IsNullOrEmpty(secondaryKey))
      {
        this.m_objectBuilder.BoolListStorage.Dictionary[variableName] = new MySerializableList<bool>((IEnumerable<bool>) value);
      }
      else
      {
        if (!this.m_objectBuilder.BoolListStorageSecondary.Dictionary.ContainsKey(variableName))
          this.m_objectBuilder.BoolListStorageSecondary.Dictionary.Add(variableName, new SerializableDictionary<string, MySerializableList<bool>>());
        this.m_objectBuilder.BoolListStorageSecondary.Dictionary[variableName][secondaryKey] = new MySerializableList<bool>((IEnumerable<bool>) value);
      }
      return true;
    }

    public bool Write(string variableName, string secondaryKey, List<float> value)
    {
      if (this.m_objectBuilder == null)
        return false;
      if (string.IsNullOrEmpty(secondaryKey))
      {
        this.m_objectBuilder.FloatListStorage.Dictionary[variableName] = new MySerializableList<float>((IEnumerable<float>) value);
      }
      else
      {
        if (!this.m_objectBuilder.FloatListStorageSecondary.Dictionary.ContainsKey(variableName))
          this.m_objectBuilder.FloatListStorageSecondary.Dictionary.Add(variableName, new SerializableDictionary<string, MySerializableList<float>>());
        this.m_objectBuilder.FloatListStorageSecondary.Dictionary[variableName][secondaryKey] = new MySerializableList<float>((IEnumerable<float>) value);
      }
      return true;
    }

    public bool Write(string variableName, string secondaryKey, List<string> value)
    {
      if (this.m_objectBuilder == null)
        return false;
      if (string.IsNullOrEmpty(secondaryKey))
      {
        this.m_objectBuilder.StringListStorage.Dictionary[variableName] = new MySerializableList<string>((IEnumerable<string>) value);
      }
      else
      {
        if (!this.m_objectBuilder.StringListStorageSecondary.Dictionary.ContainsKey(variableName))
          this.m_objectBuilder.StringListStorageSecondary.Dictionary.Add(variableName, new SerializableDictionary<string, MySerializableList<string>>());
        this.m_objectBuilder.StringListStorageSecondary.Dictionary[variableName][secondaryKey] = new MySerializableList<string>((IEnumerable<string>) value);
      }
      return true;
    }

    public bool Write(string variableName, string secondaryKey, List<Vector3D> value)
    {
      if (this.m_objectBuilder == null)
        return false;
      if (string.IsNullOrEmpty(secondaryKey))
      {
        this.m_objectBuilder.Vector3DListStorage.Dictionary[variableName] = new MySerializableList<SerializableVector3D>(value.Cast<SerializableVector3D>());
      }
      else
      {
        if (!this.m_objectBuilder.Vector3DListStorageSecondary.Dictionary.ContainsKey(variableName))
          this.m_objectBuilder.Vector3DListStorageSecondary.Dictionary.Add(variableName, new SerializableDictionary<string, MySerializableList<SerializableVector3D>>());
        this.m_objectBuilder.Vector3DListStorageSecondary.Dictionary[variableName][secondaryKey] = new MySerializableList<SerializableVector3D>(value.Cast<SerializableVector3D>());
      }
      return true;
    }

    public List<int> ReadIntList(string variableName, string secondaryKey)
    {
      if (this.m_objectBuilder == null)
        return new List<int>();
      if (string.IsNullOrEmpty(secondaryKey))
      {
        MySerializableList<int> serializableList1;
        if (this.m_objectBuilder.IntListStorage.Dictionary.TryGetValue(variableName, out serializableList1))
          return (List<int>) serializableList1;
        MySerializableList<int> serializableList2 = new MySerializableList<int>();
        this.m_objectBuilder.IntListStorage.Dictionary[variableName] = serializableList2;
        return (List<int>) serializableList2;
      }
      SerializableDictionary<string, MySerializableList<int>> serializableDictionary;
      MySerializableList<int> serializableList;
      return this.m_objectBuilder.IntListStorageSecondary.Dictionary.TryGetValue(variableName, out serializableDictionary) && serializableDictionary.Dictionary.TryGetValue(secondaryKey, out serializableList) ? (List<int>) serializableList : new List<int>();
    }

    public List<long> ReadLongList(string variableName, string secondaryKey)
    {
      if (this.m_objectBuilder == null)
        return new List<long>();
      if (string.IsNullOrEmpty(secondaryKey))
      {
        MySerializableList<long> serializableList1;
        if (this.m_objectBuilder.LongListStorage.Dictionary.TryGetValue(variableName, out serializableList1))
          return (List<long>) serializableList1;
        MySerializableList<long> serializableList2 = new MySerializableList<long>();
        this.m_objectBuilder.LongListStorage.Dictionary[variableName] = serializableList2;
        return (List<long>) serializableList2;
      }
      SerializableDictionary<string, MySerializableList<long>> serializableDictionary;
      MySerializableList<long> serializableList;
      return this.m_objectBuilder.LongListStorageSecondary.Dictionary.TryGetValue(variableName, out serializableDictionary) && serializableDictionary.Dictionary.TryGetValue(secondaryKey, out serializableList) ? (List<long>) serializableList : new List<long>();
    }

    public List<ulong> ReadULongList(string variableName, string secondaryKey)
    {
      if (this.m_objectBuilder == null)
        return new List<ulong>();
      if (string.IsNullOrEmpty(secondaryKey))
      {
        MySerializableList<ulong> serializableList1;
        if (this.m_objectBuilder.ULongListStorage.Dictionary.TryGetValue(variableName, out serializableList1))
          return (List<ulong>) serializableList1;
        MySerializableList<ulong> serializableList2 = new MySerializableList<ulong>();
        this.m_objectBuilder.ULongListStorage.Dictionary[variableName] = serializableList2;
        return (List<ulong>) serializableList2;
      }
      SerializableDictionary<string, MySerializableList<ulong>> serializableDictionary;
      MySerializableList<ulong> serializableList;
      return this.m_objectBuilder.ULongListStorageSecondary.Dictionary.TryGetValue(variableName, out serializableDictionary) && serializableDictionary.Dictionary.TryGetValue(secondaryKey, out serializableList) ? (List<ulong>) serializableList : new List<ulong>();
    }

    public List<float> ReadFloatList(string variableName, string secondaryKey)
    {
      if (this.m_objectBuilder == null)
        return new List<float>();
      if (string.IsNullOrEmpty(secondaryKey))
      {
        MySerializableList<float> serializableList1;
        if (this.m_objectBuilder.FloatListStorage.Dictionary.TryGetValue(variableName, out serializableList1))
          return (List<float>) serializableList1;
        MySerializableList<float> serializableList2 = new MySerializableList<float>();
        this.m_objectBuilder.FloatListStorage.Dictionary[variableName] = serializableList2;
        return (List<float>) serializableList2;
      }
      SerializableDictionary<string, MySerializableList<float>> serializableDictionary;
      MySerializableList<float> serializableList;
      return this.m_objectBuilder.FloatListStorageSecondary.Dictionary.TryGetValue(variableName, out serializableDictionary) && serializableDictionary.Dictionary.TryGetValue(secondaryKey, out serializableList) ? (List<float>) serializableList : new List<float>();
    }

    public List<string> ReadStringList(string variableName, string secondaryKey)
    {
      if (this.m_objectBuilder == null)
        return new List<string>();
      if (string.IsNullOrEmpty(secondaryKey))
      {
        MySerializableList<string> serializableList1;
        if (this.m_objectBuilder.StringListStorage.Dictionary.TryGetValue(variableName, out serializableList1))
          return (List<string>) serializableList1;
        MySerializableList<string> serializableList2 = new MySerializableList<string>();
        this.m_objectBuilder.StringListStorage.Dictionary[variableName] = serializableList2;
        return (List<string>) serializableList2;
      }
      SerializableDictionary<string, MySerializableList<string>> serializableDictionary;
      MySerializableList<string> serializableList;
      return this.m_objectBuilder.StringListStorageSecondary.Dictionary.TryGetValue(variableName, out serializableDictionary) && serializableDictionary.Dictionary.TryGetValue(secondaryKey, out serializableList) ? (List<string>) serializableList : new List<string>();
    }

    public List<Vector3D> ReadVector3DList(string variableName, string secondaryKey)
    {
      if (this.m_objectBuilder == null)
        return new List<Vector3D>();
      if (string.IsNullOrEmpty(secondaryKey))
      {
        MySerializableList<SerializableVector3D> source1;
        if (this.m_objectBuilder.Vector3DListStorage.Dictionary.TryGetValue(variableName, out source1))
          return source1.Cast<Vector3D>().ToList<Vector3D>();
        List<Vector3D> source2 = new List<Vector3D>();
        this.m_objectBuilder.Vector3DListStorage.Dictionary[variableName] = new MySerializableList<SerializableVector3D>(source2.Cast<SerializableVector3D>());
        return source2;
      }
      SerializableDictionary<string, MySerializableList<SerializableVector3D>> serializableDictionary;
      MySerializableList<SerializableVector3D> source;
      return this.m_objectBuilder.Vector3DListStorageSecondary.Dictionary.TryGetValue(variableName, out serializableDictionary) && serializableDictionary.Dictionary.TryGetValue(secondaryKey, out source) ? source.Cast<Vector3D>().ToList<Vector3D>() : new List<Vector3D>();
    }

    public List<bool> ReadBoolList(string variableName, string secondaryKey)
    {
      if (this.m_objectBuilder == null)
        return new List<bool>();
      if (string.IsNullOrEmpty(secondaryKey))
      {
        MySerializableList<bool> serializableList1;
        if (this.m_objectBuilder.BoolListStorage.Dictionary.TryGetValue(variableName, out serializableList1))
          return (List<bool>) serializableList1;
        MySerializableList<bool> serializableList2 = new MySerializableList<bool>();
        this.m_objectBuilder.BoolListStorage.Dictionary[variableName] = serializableList2;
        return (List<bool>) serializableList2;
      }
      SerializableDictionary<string, MySerializableList<bool>> serializableDictionary;
      MySerializableList<bool> serializableList;
      return this.m_objectBuilder.BoolListStorageSecondary.Dictionary.TryGetValue(variableName, out serializableDictionary) && serializableDictionary.Dictionary.TryGetValue(secondaryKey, out serializableList) ? (List<bool>) serializableList : new List<bool>();
    }

    public SerializableDictionary<string, bool> GetBools(string secondaryKey = null)
    {
      if (string.IsNullOrEmpty(secondaryKey))
        return this.m_objectBuilder.BoolStorage;
      SerializableDictionary<string, bool> serializableDictionary;
      return this.m_objectBuilder.BoolStorageSecondary.Dictionary.TryGetValue(secondaryKey, out serializableDictionary) ? serializableDictionary : new SerializableDictionary<string, bool>();
    }

    public SerializableDictionary<string, int> GetInts(string secondaryKey = null)
    {
      if (string.IsNullOrEmpty(secondaryKey))
        return this.m_objectBuilder.IntStorage;
      SerializableDictionary<string, int> serializableDictionary;
      return this.m_objectBuilder.IntStorageSecondary.Dictionary.TryGetValue(secondaryKey, out serializableDictionary) ? serializableDictionary : new SerializableDictionary<string, int>();
    }

    public SerializableDictionary<string, long> GetLongs(string secondaryKey = null)
    {
      if (string.IsNullOrEmpty(secondaryKey))
        return this.m_objectBuilder.LongStorage;
      SerializableDictionary<string, long> serializableDictionary;
      return this.m_objectBuilder.LongStorageSecondary.Dictionary.TryGetValue(secondaryKey, out serializableDictionary) ? serializableDictionary : new SerializableDictionary<string, long>();
    }

    public SerializableDictionary<string, ulong> GetULongs(string secondaryKey = null)
    {
      if (string.IsNullOrEmpty(secondaryKey))
        return this.m_objectBuilder.ULongStorage;
      SerializableDictionary<string, ulong> serializableDictionary;
      return this.m_objectBuilder.ULongStorageSecondary.Dictionary.TryGetValue(secondaryKey, out serializableDictionary) ? serializableDictionary : new SerializableDictionary<string, ulong>();
    }

    public SerializableDictionary<string, string> GetStrings(
      string secondaryKey = null)
    {
      if (string.IsNullOrEmpty(secondaryKey))
        return this.m_objectBuilder.StringStorage;
      SerializableDictionary<string, string> serializableDictionary;
      return this.m_objectBuilder.StringStorageSecondary.Dictionary.TryGetValue(secondaryKey, out serializableDictionary) ? serializableDictionary : new SerializableDictionary<string, string>();
    }

    public SerializableDictionary<string, float> GetFloats(string secondaryKey = null)
    {
      if (string.IsNullOrEmpty(secondaryKey))
        return this.m_objectBuilder.FloatStorage;
      SerializableDictionary<string, float> serializableDictionary;
      return this.m_objectBuilder.FloatStorageSecondary.Dictionary.TryGetValue(secondaryKey, out serializableDictionary) ? serializableDictionary : new SerializableDictionary<string, float>();
    }

    public SerializableDictionary<string, SerializableVector3D> GetVector3D(
      string secondaryKey = null)
    {
      if (string.IsNullOrEmpty(secondaryKey))
        return this.m_objectBuilder.Vector3DStorage;
      SerializableDictionary<string, SerializableVector3D> serializableDictionary;
      return this.m_objectBuilder.Vector3DStorageSecondary.Dictionary.TryGetValue(secondaryKey, out serializableDictionary) ? serializableDictionary : new SerializableDictionary<string, SerializableVector3D>();
    }

    public Dictionary<string, bool> GetBoolsByRegex(Regex nameRegex, string secondaryKey = null)
    {
      Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
      if (string.IsNullOrEmpty(secondaryKey))
      {
        foreach (KeyValuePair<string, bool> keyValuePair in this.m_objectBuilder.BoolStorage.Dictionary)
        {
          if (nameRegex.IsMatch(keyValuePair.Key))
            dictionary.Add(keyValuePair.Key, keyValuePair.Value);
        }
      }
      else
      {
        SerializableDictionary<string, bool> serializableDictionary;
        if (this.m_objectBuilder.BoolStorageSecondary.Dictionary.TryGetValue(secondaryKey, out serializableDictionary))
        {
          foreach (KeyValuePair<string, bool> keyValuePair in serializableDictionary.Dictionary)
          {
            if (nameRegex.IsMatch(keyValuePair.Key))
              dictionary.Add(keyValuePair.Key, keyValuePair.Value);
          }
        }
      }
      return dictionary;
    }
  }
}
