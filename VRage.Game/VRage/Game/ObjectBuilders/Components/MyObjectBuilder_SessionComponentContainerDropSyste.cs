// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Components.MyObjectBuilder_SessionComponentContainerDropSystem
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Components
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_SessionComponentContainerDropSystem : MyObjectBuilder_SessionComponent
  {
    [ProtoMember(28)]
    public List<PlayerContainerData> PlayerData;
    [ProtoMember(31)]
    public List<MyContainerGPS> GPSForRemoval;
    [ProtoMember(34)]
    public List<MyEntityForRemoval> EntitiesForRemoval;
    [ProtoMember(37)]
    public uint ContainerIdSmall = 1;
    [ProtoMember(40)]
    public uint ContainerIdLarge = 1;

    public MyObjectBuilder_SessionComponentContainerDropSystem()
    {
      this.PlayerData = new List<PlayerContainerData>();
      this.GPSForRemoval = new List<MyContainerGPS>();
      this.EntitiesForRemoval = new List<MyEntityForRemoval>();
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentContainerDropSystem\u003C\u003EPlayerData\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionComponentContainerDropSystem, List<PlayerContainerData>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SessionComponentContainerDropSystem owner,
        in List<PlayerContainerData> value)
      {
        owner.PlayerData = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SessionComponentContainerDropSystem owner,
        out List<PlayerContainerData> value)
      {
        value = owner.PlayerData;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentContainerDropSystem\u003C\u003EGPSForRemoval\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionComponentContainerDropSystem, List<MyContainerGPS>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SessionComponentContainerDropSystem owner,
        in List<MyContainerGPS> value)
      {
        owner.GPSForRemoval = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SessionComponentContainerDropSystem owner,
        out List<MyContainerGPS> value)
      {
        value = owner.GPSForRemoval;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentContainerDropSystem\u003C\u003EEntitiesForRemoval\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionComponentContainerDropSystem, List<MyEntityForRemoval>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SessionComponentContainerDropSystem owner,
        in List<MyEntityForRemoval> value)
      {
        owner.EntitiesForRemoval = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SessionComponentContainerDropSystem owner,
        out List<MyEntityForRemoval> value)
      {
        value = owner.EntitiesForRemoval;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentContainerDropSystem\u003C\u003EContainerIdSmall\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionComponentContainerDropSystem, uint>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SessionComponentContainerDropSystem owner,
        in uint value)
      {
        owner.ContainerIdSmall = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SessionComponentContainerDropSystem owner,
        out uint value)
      {
        value = owner.ContainerIdSmall;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentContainerDropSystem\u003C\u003EContainerIdLarge\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionComponentContainerDropSystem, uint>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SessionComponentContainerDropSystem owner,
        in uint value)
      {
        owner.ContainerIdLarge = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SessionComponentContainerDropSystem owner,
        out uint value)
      {
        value = owner.ContainerIdLarge;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentContainerDropSystem\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SessionComponentContainerDropSystem, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SessionComponentContainerDropSystem owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SessionComponentContainerDropSystem owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentContainerDropSystem\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SessionComponentContainerDropSystem, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SessionComponentContainerDropSystem owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SessionComponentContainerDropSystem owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentContainerDropSystem\u003C\u003EDefinition\u003C\u003EAccessor : MyObjectBuilder_SessionComponent.VRage_Game_MyObjectBuilder_SessionComponent\u003C\u003EDefinition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SessionComponentContainerDropSystem, SerializableDefinitionId?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SessionComponentContainerDropSystem owner,
        in SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_SessionComponent&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SessionComponentContainerDropSystem owner,
        out SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_SessionComponent&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentContainerDropSystem\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SessionComponentContainerDropSystem, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SessionComponentContainerDropSystem owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SessionComponentContainerDropSystem owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentContainerDropSystem\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SessionComponentContainerDropSystem, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SessionComponentContainerDropSystem owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SessionComponentContainerDropSystem owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    private class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentContainerDropSystem\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_SessionComponentContainerDropSystem>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_SessionComponentContainerDropSystem();

      MyObjectBuilder_SessionComponentContainerDropSystem IActivator<MyObjectBuilder_SessionComponentContainerDropSystem>.CreateInstance() => new MyObjectBuilder_SessionComponentContainerDropSystem();
    }
  }
}
