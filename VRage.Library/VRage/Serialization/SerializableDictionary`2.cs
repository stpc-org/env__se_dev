// Decompiled with JetBrains decompiler
// Type: VRage.Serialization.SerializableDictionary`2
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.Serialization
{
  [ProtoContract]
  [XmlRoot("Dictionary")]
  [Obfuscation(ApplyToMembers = true, Exclude = true, Feature = "cw symbol renaming")]
  public class SerializableDictionary<T, U>
  {
    private System.Collections.Generic.Dictionary<T, U> m_dictionary = new System.Collections.Generic.Dictionary<T, U>();

    public SerializableDictionary()
    {
    }

    public SerializableDictionary(System.Collections.Generic.Dictionary<T, U> dict) => this.Dictionary = dict;

    [ProtoMember(1)]
    [XmlIgnore]
    public System.Collections.Generic.Dictionary<T, U> Dictionary
    {
      set => this.m_dictionary = value;
      get => this.m_dictionary;
    }

    [XmlArray("dictionary")]
    [XmlArrayItem("item")]
    [NoSerialize]
    public SerializableDictionary<T, U>.Entry[] DictionaryEntryProp
    {
      get
      {
        SerializableDictionary<T, U>.Entry[] entryArray = new SerializableDictionary<T, U>.Entry[this.Dictionary.Count];
        int index = 0;
        foreach (KeyValuePair<T, U> keyValuePair in this.Dictionary)
        {
          entryArray[index] = new SerializableDictionary<T, U>.Entry()
          {
            Key = keyValuePair.Key,
            Value = keyValuePair.Value
          };
          ++index;
        }
        return entryArray;
      }
      set
      {
        this.Dictionary.Clear();
        for (int index = 0; index < value.Length; ++index)
        {
          try
          {
            this.Dictionary.Add(value[index].Key, value[index].Value);
          }
          catch (Exception ex)
          {
          }
        }
      }
    }

    public U this[T key]
    {
      get => this.Dictionary[key];
      set => this.Dictionary[key] = value;
    }

    public struct Entry
    {
      public T Key;
      public U Value;
    }

    protected class VRage_Serialization_SerializableDictionary`2\u003C\u003Em_dictionary\u003C\u003EAccessor : IMemberAccessor<SerializableDictionary<T, U>, System.Collections.Generic.Dictionary<T, U>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableDictionary<T, U> owner, in System.Collections.Generic.Dictionary<T, U> value) => owner.m_dictionary = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableDictionary<T, U> owner, out System.Collections.Generic.Dictionary<T, U> value) => value = owner.m_dictionary;
    }

    protected class VRage_Serialization_SerializableDictionary`2\u003C\u003EDictionary\u003C\u003EAccessor : IMemberAccessor<SerializableDictionary<T, U>, System.Collections.Generic.Dictionary<T, U>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableDictionary<T, U> owner, in System.Collections.Generic.Dictionary<T, U> value) => owner.Dictionary = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableDictionary<T, U> owner, out System.Collections.Generic.Dictionary<T, U> value) => value = owner.Dictionary;
    }

    protected class VRage_Serialization_SerializableDictionary`2\u003C\u003EDictionaryEntryProp\u003C\u003EAccessor : IMemberAccessor<SerializableDictionary<T, U>, SerializableDictionary<T, U>.Entry[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref SerializableDictionary<T, U> owner,
        in SerializableDictionary<T, U>.Entry[] value)
      {
        owner.DictionaryEntryProp = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref SerializableDictionary<T, U> owner,
        out SerializableDictionary<T, U>.Entry[] value)
      {
        value = owner.DictionaryEntryProp;
      }
    }
  }
}
