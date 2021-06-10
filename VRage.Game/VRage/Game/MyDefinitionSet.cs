// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyDefinitionSet
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game.Definitions;
using VRage.Utils;

namespace VRage.Game
{
  public class MyDefinitionSet
  {
    public MyModContext Context;
    public readonly Dictionary<Type, Dictionary<MyStringHash, MyDefinitionBase>> Definitions = new Dictionary<Type, Dictionary<MyStringHash, MyDefinitionBase>>();

    public void AddDefinition(MyDefinitionBase def)
    {
      Dictionary<MyStringHash, MyDefinitionBase> dictionary;
      if (!this.Definitions.TryGetValue((Type) def.Id.TypeId, out dictionary))
      {
        dictionary = new Dictionary<MyStringHash, MyDefinitionBase>();
        this.Definitions[(Type) def.Id.TypeId] = dictionary;
      }
      dictionary[def.Id.SubtypeId] = def;
    }

    public bool AddOrRelaceDefinition(MyDefinitionBase def)
    {
      Dictionary<MyStringHash, MyDefinitionBase> dictionary;
      if (!this.Definitions.TryGetValue((Type) def.Id.TypeId, out dictionary))
      {
        dictionary = new Dictionary<MyStringHash, MyDefinitionBase>();
        this.Definitions[(Type) def.Id.TypeId] = dictionary;
      }
      int num = dictionary.ContainsKey(def.Id.SubtypeId) ? 1 : 0;
      dictionary[def.Id.SubtypeId] = def;
      return num != 0;
    }

    public void RemoveDefinition(ref MyDefinitionId defId)
    {
      Dictionary<MyStringHash, MyDefinitionBase> dictionary;
      if (!this.Definitions.TryGetValue((Type) defId.TypeId, out dictionary))
        return;
      dictionary.Remove(defId.SubtypeId);
    }

    public IEnumerable<T> GetDefinitionsOfType<T>() where T : MyDefinitionBase
    {
      Dictionary<MyStringHash, MyDefinitionBase> dictionary;
      return this.Definitions.TryGetValue(MyDefinitionManagerBase.GetObjectBuilderType(typeof (T)), out dictionary) ? dictionary.Values.Cast<T>() : (IEnumerable<T>) null;
    }

    public IEnumerable<T> GetDefinitionsOfTypeAndSubtypes<T>() where T : MyDefinitionBase
    {
      HashSet<Type> subtypes = MyDefinitionManagerBase.Static.GetSubtypes<T>();
      Dictionary<MyStringHash, MyDefinitionBase> dictionary = (Dictionary<MyStringHash, MyDefinitionBase>) null;
      if (subtypes != null)
        return subtypes.SelectMany<Type, T>((Func<Type, IEnumerable<T>>) (x => this.Definitions.GetOrEmpty<Type, MyStringHash, MyDefinitionBase>(MyDefinitionManagerBase.GetObjectBuilderType(x)).Cast<T>()));
      return this.Definitions.TryGetValue(MyDefinitionManagerBase.GetObjectBuilderType(typeof (T)), out dictionary) ? dictionary.Values.Cast<T>() : (IEnumerable<T>) null;
    }

    public bool ContainsDefinition(MyDefinitionId id)
    {
      Dictionary<MyStringHash, MyDefinitionBase> dictionary;
      return this.Definitions.TryGetValue((Type) id.TypeId, out dictionary) && dictionary.ContainsKey(id.SubtypeId);
    }

    public T GetDefinition<T>(MyStringHash subtypeId) where T : MyDefinitionBase
    {
      MyDefinitionBase myDefinitionBase = (MyDefinitionBase) null;
      Dictionary<MyStringHash, MyDefinitionBase> dictionary;
      if (this.Definitions.TryGetValue(MyDefinitionManagerBase.GetObjectBuilderType(typeof (T)), out dictionary))
        dictionary.TryGetValue(subtypeId, out myDefinitionBase);
      return (T) myDefinitionBase;
    }

    public T GetDefinition<T>(MyDefinitionId id) where T : MyDefinitionBase
    {
      MyDefinitionBase myDefinitionBase = (MyDefinitionBase) null;
      Dictionary<MyStringHash, MyDefinitionBase> dictionary;
      if (this.Definitions.TryGetValue((Type) id.TypeId, out dictionary))
        dictionary.TryGetValue(id.SubtypeId, out myDefinitionBase);
      return myDefinitionBase as T;
    }

    public virtual void OverrideBy(MyDefinitionSet definitionSet)
    {
      MyDefinitionPostprocessor.Bundle currentDefinitions = new MyDefinitionPostprocessor.Bundle()
      {
        Set = this,
        Context = this.Context
      };
      MyDefinitionPostprocessor.Bundle overrideBySet = new MyDefinitionPostprocessor.Bundle()
      {
        Set = definitionSet,
        Context = definitionSet.Context
      };
      foreach (KeyValuePair<Type, Dictionary<MyStringHash, MyDefinitionBase>> definition in definitionSet.Definitions)
      {
        Dictionary<MyStringHash, MyDefinitionBase> dictionary;
        if (!this.Definitions.TryGetValue(definition.Key, out dictionary))
        {
          dictionary = new Dictionary<MyStringHash, MyDefinitionBase>();
          this.Definitions[definition.Key] = dictionary;
        }
        MyDefinitionPostprocessor definitionPostprocessor = MyDefinitionManagerBase.GetPostProcessor(definition.Key) ?? MyDefinitionManagerBase.GetPostProcessor(MyDefinitionManagerBase.GetObjectBuilderType(definition.Value.First<KeyValuePair<MyStringHash, MyDefinitionBase>>().Value.GetType()));
        currentDefinitions.Definitions = dictionary;
        overrideBySet.Definitions = definition.Value;
        definitionPostprocessor.OverrideBy(ref currentDefinitions, ref overrideBySet);
      }
    }

    public void Clear()
    {
      foreach (KeyValuePair<Type, Dictionary<MyStringHash, MyDefinitionBase>> definition in this.Definitions)
        definition.Value.Clear();
    }
  }
}
