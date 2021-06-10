// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyDefinitionId
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
  [Serializable]
  public struct MyDefinitionId : IEquatable<MyDefinitionId>
  {
    public static readonly MyDefinitionId.DefinitionIdComparerType Comparer = new MyDefinitionId.DefinitionIdComparerType();
    public readonly MyObjectBuilderType TypeId;
    public readonly MyStringHash SubtypeId;

    public static MyDefinitionId FromContent(MyObjectBuilder_Base content) => new MyDefinitionId(content.TypeId, content.SubtypeId);

    public static MyDefinitionId Parse(string id)
    {
      MyDefinitionId definitionId;
      if (!MyDefinitionId.TryParse(id, out definitionId))
        throw new ArgumentException("The provided type does not conform to a definition ID.", nameof (id));
      return definitionId;
    }

    public static bool TryParse(string id, out MyDefinitionId definitionId)
    {
      if (string.IsNullOrEmpty(id))
      {
        definitionId = new MyDefinitionId();
        return false;
      }
      int length = id.IndexOf('/');
      if (length == -1)
      {
        definitionId = new MyDefinitionId();
        return false;
      }
      MyObjectBuilderType result;
      if (MyObjectBuilderType.TryParse(id.Substring(0, length).Trim(), out result))
      {
        string subtypeName = id.Substring(length + 1).Trim();
        if (subtypeName == "(null)")
          subtypeName = (string) null;
        definitionId = new MyDefinitionId(result, subtypeName);
        return true;
      }
      definitionId = new MyDefinitionId();
      return false;
    }

    public static bool TryParse(string type, string subtype, out MyDefinitionId definitionId)
    {
      if (string.IsNullOrEmpty(type) || string.IsNullOrEmpty(subtype))
      {
        definitionId = new MyDefinitionId();
        return false;
      }
      MyObjectBuilderType result;
      if (MyObjectBuilderType.TryParse(type, out result))
      {
        definitionId = new MyDefinitionId(result, subtype);
        return true;
      }
      definitionId = new MyDefinitionId();
      return false;
    }

    public string SubtypeName => this.SubtypeId.ToString();

    public MyDefinitionId(MyObjectBuilderType type)
      : this(type, MyStringHash.GetOrCompute((string) null))
    {
    }

    public MyDefinitionId(MyObjectBuilderType type, string subtypeName)
      : this(type, MyStringHash.GetOrCompute(subtypeName))
    {
    }

    public MyDefinitionId(MyObjectBuilderType type, MyStringHash subtypeId)
    {
      this.TypeId = type;
      this.SubtypeId = subtypeId;
    }

    public MyDefinitionId(MyRuntimeObjectBuilderId type, MyStringHash subtypeId)
    {
      this.TypeId = (MyObjectBuilderType) type;
      this.SubtypeId = subtypeId;
    }

    public override int GetHashCode() => this.TypeId.GetHashCode() << 16 ^ this.SubtypeId.GetHashCode();

    public long GetHashCodeLong() => (long) this.TypeId.GetHashCode() << 32 ^ (long) this.SubtypeId.GetHashCode();

    public override bool Equals(object obj) => obj is MyDefinitionId other && this.Equals(other);

    public override string ToString() => string.Format("{0}/{1}", !this.TypeId.IsNull ? (object) this.TypeId.ToString() : (object) "(null)", !string.IsNullOrEmpty(this.SubtypeName) ? (object) this.SubtypeName : (object) "(null)");

    public bool Equals(MyDefinitionId other) => this.TypeId == other.TypeId && this.SubtypeId == other.SubtypeId;

    public static bool operator ==(MyDefinitionId l, MyDefinitionId r) => l.Equals(r);

    public static bool operator !=(MyDefinitionId l, MyDefinitionId r) => !l.Equals(r);

    public static implicit operator MyDefinitionId(SerializableDefinitionId v) => new MyDefinitionId(v.TypeId, v.SubtypeName);

    public static implicit operator SerializableDefinitionId(
      MyDefinitionId v)
    {
      return new SerializableDefinitionId(v.TypeId, v.SubtypeName);
    }

    public class DefinitionIdComparerType : IEqualityComparer<MyDefinitionId>
    {
      public bool Equals(MyDefinitionId x, MyDefinitionId y) => x.TypeId == y.TypeId && x.SubtypeId == y.SubtypeId;

      public int GetHashCode(MyDefinitionId obj) => obj.GetHashCode();
    }

    protected class VRage_Game_MyDefinitionId\u003C\u003ETypeId\u003C\u003EAccessor : IMemberAccessor<MyDefinitionId, MyObjectBuilderType>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyDefinitionId owner, in MyObjectBuilderType value) => owner.TypeId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyDefinitionId owner, out MyObjectBuilderType value) => value = owner.TypeId;
    }

    protected class VRage_Game_MyDefinitionId\u003C\u003ESubtypeId\u003C\u003EAccessor : IMemberAccessor<MyDefinitionId, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyDefinitionId owner, in MyStringHash value) => owner.SubtypeId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyDefinitionId owner, out MyStringHash value) => value = owner.SubtypeId;
    }
  }
}
