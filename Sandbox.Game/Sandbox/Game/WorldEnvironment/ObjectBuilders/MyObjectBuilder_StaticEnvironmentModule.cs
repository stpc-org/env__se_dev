// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.WorldEnvironment.ObjectBuilders.MyObjectBuilder_StaticEnvironmentModule
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;

namespace Sandbox.Game.WorldEnvironment.ObjectBuilders
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("Sandbox.Game.XmlSerializers")]
  public class MyObjectBuilder_StaticEnvironmentModule : MyObjectBuilder_EnvironmentModuleBase
  {
    [ProtoMember(1)]
    public HashSet<int> DisabledItems;
    [Nullable]
    [ProtoMember(4)]
    public List<SerializableOrientedBoundingBoxD> Boxes;
    [ProtoMember(7)]
    public int MinScanned = 15;

    public MyObjectBuilder_StaticEnvironmentModule()
    {
      this.DisabledItems = new HashSet<int>();
      this.Boxes = new List<SerializableOrientedBoundingBoxD>();
    }

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_StaticEnvironmentModule\u003C\u003EDisabledItems\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_StaticEnvironmentModule, HashSet<int>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_StaticEnvironmentModule owner,
        in HashSet<int> value)
      {
        owner.DisabledItems = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_StaticEnvironmentModule owner,
        out HashSet<int> value)
      {
        value = owner.DisabledItems;
      }
    }

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_StaticEnvironmentModule\u003C\u003EBoxes\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_StaticEnvironmentModule, List<SerializableOrientedBoundingBoxD>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_StaticEnvironmentModule owner,
        in List<SerializableOrientedBoundingBoxD> value)
      {
        owner.Boxes = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_StaticEnvironmentModule owner,
        out List<SerializableOrientedBoundingBoxD> value)
      {
        value = owner.Boxes;
      }
    }

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_StaticEnvironmentModule\u003C\u003EMinScanned\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_StaticEnvironmentModule, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_StaticEnvironmentModule owner, in int value) => owner.MinScanned = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_StaticEnvironmentModule owner, out int value) => value = owner.MinScanned;
    }

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_StaticEnvironmentModule\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_StaticEnvironmentModule, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_StaticEnvironmentModule owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_StaticEnvironmentModule owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_StaticEnvironmentModule\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_StaticEnvironmentModule, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_StaticEnvironmentModule owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_StaticEnvironmentModule owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_StaticEnvironmentModule\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_StaticEnvironmentModule, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_StaticEnvironmentModule owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_StaticEnvironmentModule owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_StaticEnvironmentModule\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_StaticEnvironmentModule, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_StaticEnvironmentModule owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_StaticEnvironmentModule owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_StaticEnvironmentModule\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_StaticEnvironmentModule>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_StaticEnvironmentModule();

      MyObjectBuilder_StaticEnvironmentModule IActivator<MyObjectBuilder_StaticEnvironmentModule>.CreateInstance() => new MyObjectBuilder_StaticEnvironmentModule();
    }
  }
}
