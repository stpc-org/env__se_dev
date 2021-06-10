// Decompiled with JetBrains decompiler
// Type: VRage.Serialization.SerializableDictionaryHack`2
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using ProtoBuf;
using System;
using System.Collections;
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
  public class SerializableDictionaryHack<T, U>
  {
    private System.Collections.Generic.Dictionary<T, U> m_dictionary = new System.Collections.Generic.Dictionary<T, U>();

    public SerializableDictionaryHack()
    {
    }

    public SerializableDictionaryHack(System.Collections.Generic.Dictionary<T, U> dict) => this.Dictionary = dict;

    [ProtoMember(1)]
    [XmlIgnore]
    public System.Collections.Generic.Dictionary<T, U> Dictionary
    {
      set => this.m_dictionary = value;
      get => this.m_dictionary;
    }

    [XmlArray("dictionary")]
    [XmlArrayItem("item", Type = typeof (DictionaryEntry))]
    public DictionaryEntry[] DictionaryEntryProp
    {
      get
      {
        DictionaryEntry[] dictionaryEntryArray = new DictionaryEntry[this.Dictionary.Count];
        int index = 0;
        foreach (KeyValuePair<T, U> keyValuePair in this.Dictionary)
        {
          dictionaryEntryArray[index] = new DictionaryEntry()
          {
            Key = (object) keyValuePair.Key,
            Value = (object) keyValuePair.Value
          };
          ++index;
        }
        return dictionaryEntryArray;
      }
      set
      {
        this.Dictionary.Clear();
        for (int index = 0; index < value.Length; ++index)
        {
          try
          {
            this.Dictionary.Add((T) value[index].Key, (U) value[index].Value);
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

    protected class VRage_Serialization_SerializableDictionaryHack`2\u003C\u003Em_dictionary\u003C\u003EAccessor : IMemberAccessor<SerializableDictionaryHack<T, U>, System.Collections.Generic.Dictionary<T, U>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableDictionaryHack<T, U> owner, in System.Collections.Generic.Dictionary<T, U> value) => owner.m_dictionary = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref SerializableDictionaryHack<T, U> owner,
        out System.Collections.Generic.Dictionary<T, U> value)
      {
        value = owner.m_dictionary;
      }
    }

    protected class VRage_Serialization_SerializableDictionaryHack`2\u003C\u003EDictionary\u003C\u003EAccessor : IMemberAccessor<SerializableDictionaryHack<T, U>, System.Collections.Generic.Dictionary<T, U>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableDictionaryHack<T, U> owner, in System.Collections.Generic.Dictionary<T, U> value) => owner.Dictionary = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref SerializableDictionaryHack<T, U> owner,
        out System.Collections.Generic.Dictionary<T, U> value)
      {
        value = owner.Dictionary;
      }
    }

    protected class VRage_Serialization_SerializableDictionaryHack`2\u003C\u003EDictionaryEntryProp\u003C\u003EAccessor : IMemberAccessor<SerializableDictionaryHack<T, U>, DictionaryEntry[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref SerializableDictionaryHack<T, U> owner,
        in DictionaryEntry[] value)
      {
        ((SerializableDictionaryHack<,>) owner).DictionaryEntryProp = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref SerializableDictionaryHack<T, U> owner,
        out DictionaryEntry[] value)
      {
        value = ((SerializableDictionaryHack<,>) owner).DictionaryEntryProp;
      }
    }
  }
}
