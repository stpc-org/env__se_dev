// Decompiled with JetBrains decompiler
// Type: System.Reflection.FieldAccess
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Collections.Concurrent;
using System.Collections.Generic;

namespace System.Reflection
{
  public static class FieldAccess
  {
    private static readonly ConcurrentDictionary<FieldAccess.FieldKey, FieldAccess.FieldHelper> Helpers = new ConcurrentDictionary<FieldAccess.FieldKey, FieldAccess.FieldHelper>();

    private static FieldAccess.FieldHelper<TInstance, TMember> GetHelper<TInstance, TMember>(
      FieldInfo info)
    {
      FieldAccess.FieldKey key = new FieldAccess.FieldKey(typeof (TInstance), info);
      FieldAccess.FieldHelper fieldHelper;
      if (!FieldAccess.Helpers.TryGetValue(key, out fieldHelper))
      {
        fieldHelper = (FieldAccess.FieldHelper) new FieldAccess.FieldHelper<TInstance, TMember>(info);
        FieldAccess.Helpers.TryAdd(key, fieldHelper);
      }
      return (FieldAccess.FieldHelper<TInstance, TMember>) fieldHelper;
    }

    public static Func<TType, TMember> CreateGetter<TType, TMember>(this FieldInfo field) => FieldAccess.GetHelper<TType, TMember>(field).Getter;

    public static Action<TType, TMember> CreateSetter<TType, TMember>(this FieldInfo field) => FieldAccess.GetHelper<TType, TMember>(field).Setter;

    public static Getter<TType, TMember> CreateGetterRef<TType, TMember>(this FieldInfo field) => FieldAccess.GetHelper<TType, TMember>(field).GetterRef;

    public static Setter<TType, TMember> CreateSetterRef<TType, TMember>(this FieldInfo field) => FieldAccess.GetHelper<TType, TMember>(field).SetterRef;

    private struct FieldKey
    {
      public readonly Type ReflectedType;
      public readonly FieldInfo Field;

      public FieldKey(Type reflectedType, FieldInfo field)
      {
        this.ReflectedType = reflectedType;
        this.Field = field;
      }

      public static IEqualityComparer<FieldAccess.FieldKey> Comparer { get; } = (IEqualityComparer<FieldAccess.FieldKey>) new FieldAccess.FieldKey.ReflectedTypeFieldEqualityComparer();

      private sealed class ReflectedTypeFieldEqualityComparer : IEqualityComparer<FieldAccess.FieldKey>
      {
        public bool Equals(FieldAccess.FieldKey x, FieldAccess.FieldKey y) => object.Equals((object) x.ReflectedType, (object) y.ReflectedType) && object.Equals((object) x.Field, (object) y.Field);

        public int GetHashCode(FieldAccess.FieldKey obj) => (obj.ReflectedType != (Type) null ? obj.ReflectedType.GetHashCode() : 0) * 397 ^ (obj.Field != (FieldInfo) null ? obj.Field.GetHashCode() : 0);
      }
    }

    private class FieldHelper
    {
    }

    private class FieldHelper<TType, TMember> : FieldAccess.FieldHelper
    {
      private readonly FieldInfo m_info;
      public readonly Func<TType, TMember> Getter;
      public readonly Action<TType, TMember> Setter;
      public readonly Getter<TType, TMember> GetterRef;
      public readonly Setter<TType, TMember> SetterRef;

      public FieldHelper(FieldInfo info)
      {
        this.m_info = info;
        this.Getter = new Func<TType, TMember>(this.Get);
        this.Setter = new Action<TType, TMember>(this.Set);
        this.GetterRef = new Getter<TType, TMember>(this.Get);
        this.SetterRef = new Setter<TType, TMember>(this.Set);
      }

      private TMember Get(TType instance) => (TMember) this.m_info.GetValue((object) instance);

      private void Get(ref TType instance, out TMember memberValue) => memberValue = (TMember) this.m_info.GetValue((object) instance);

      private void Set(TType instance, TMember memberValue) => this.m_info.SetValue((object) instance, (object) memberValue);

      private void Set(ref TType instance, in TMember memberValue)
      {
        object obj = (object) instance;
        this.m_info.SetValue(obj, (object) memberValue);
        instance = (TType) obj;
      }
    }
  }
}
