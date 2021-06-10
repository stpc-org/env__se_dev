// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
  [MyObjectBuilderDefinition(null, null)]
  [XmlType("AddShipPrefab")]
  public class MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab : MyObjectBuilder_WorldGeneratorOperation
  {
    [ProtoMember(136)]
    [XmlAttribute]
    public string PrefabFile;
    [ProtoMember(139)]
    public MyPositionAndOrientation Transform;
    [ProtoMember(142)]
    public bool UseFirstGridOrigin;
    [ProtoMember(145)]
    [XmlAttribute]
    public float RandomRadius;

    public bool ShouldSerializeRandomRadius() => (double) this.RandomRadius != 0.0;

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab\u003C\u003EPrefabFile\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab owner,
        in string value)
      {
        owner.PrefabFile = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab owner,
        out string value)
      {
        value = owner.PrefabFile;
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab\u003C\u003ETransform\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab, MyPositionAndOrientation>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab owner,
        in MyPositionAndOrientation value)
      {
        owner.Transform = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab owner,
        out MyPositionAndOrientation value)
      {
        value = owner.Transform;
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab\u003C\u003EUseFirstGridOrigin\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab owner,
        in bool value)
      {
        owner.UseFirstGridOrigin = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab owner,
        out bool value)
      {
        value = owner.UseFirstGridOrigin;
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab\u003C\u003ERandomRadius\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab owner,
        in float value)
      {
        owner.RandomRadius = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab owner,
        out float value)
      {
        value = owner.RandomRadius;
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab\u003C\u003EFactionTag\u003C\u003EAccessor : MyObjectBuilder_WorldGeneratorOperation.VRage_Game_MyObjectBuilder_WorldGeneratorOperation\u003C\u003EFactionTag\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_WorldGeneratorOperation&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_WorldGeneratorOperation&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    private class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab();

      MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab IActivator<MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab>.CreateInstance() => new MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab();
    }
  }
}
