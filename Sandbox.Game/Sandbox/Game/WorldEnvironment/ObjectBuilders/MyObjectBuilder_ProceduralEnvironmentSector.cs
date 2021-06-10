// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.WorldEnvironment.ObjectBuilders.MyObjectBuilder_ProceduralEnvironmentSector
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ProtoBuf;
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
  public class MyObjectBuilder_ProceduralEnvironmentSector : MyObjectBuilder_EnvironmentSector
  {
    [ProtoMember(7)]
    public MyObjectBuilder_ProceduralEnvironmentSector.Module[] SavedModules;

    [ProtoContract]
    public struct Module
    {
      [ProtoMember(1)]
      public SerializableDefinitionId ModuleId;
      [ProtoMember(4)]
      [Serialize(MyObjectFlags.Dynamic, typeof (MyObjectBuilderDynamicSerializer))]
      [XmlElement(typeof (MyAbstractXmlSerializer<MyObjectBuilder_EnvironmentModuleBase>))]
      public MyObjectBuilder_EnvironmentModuleBase Builder;

      protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_ProceduralEnvironmentSector\u003C\u003EModule\u003C\u003EModuleId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProceduralEnvironmentSector.Module, SerializableDefinitionId>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ProceduralEnvironmentSector.Module owner,
          in SerializableDefinitionId value)
        {
          owner.ModuleId = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ProceduralEnvironmentSector.Module owner,
          out SerializableDefinitionId value)
        {
          value = owner.ModuleId;
        }
      }

      protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_ProceduralEnvironmentSector\u003C\u003EModule\u003C\u003EBuilder\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProceduralEnvironmentSector.Module, MyObjectBuilder_EnvironmentModuleBase>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ProceduralEnvironmentSector.Module owner,
          in MyObjectBuilder_EnvironmentModuleBase value)
        {
          owner.Builder = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ProceduralEnvironmentSector.Module owner,
          out MyObjectBuilder_EnvironmentModuleBase value)
        {
          value = owner.Builder;
        }
      }

      private class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_ProceduralEnvironmentSector\u003C\u003EModule\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ProceduralEnvironmentSector.Module>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_ProceduralEnvironmentSector.Module();

        MyObjectBuilder_ProceduralEnvironmentSector.Module IActivator<MyObjectBuilder_ProceduralEnvironmentSector.Module>.CreateInstance() => new MyObjectBuilder_ProceduralEnvironmentSector.Module();
      }
    }

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_ProceduralEnvironmentSector\u003C\u003ESavedModules\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProceduralEnvironmentSector, MyObjectBuilder_ProceduralEnvironmentSector.Module[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProceduralEnvironmentSector owner,
        in MyObjectBuilder_ProceduralEnvironmentSector.Module[] value)
      {
        owner.SavedModules = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProceduralEnvironmentSector owner,
        out MyObjectBuilder_ProceduralEnvironmentSector.Module[] value)
      {
        value = owner.SavedModules;
      }
    }

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_ProceduralEnvironmentSector\u003C\u003ESectorId\u003C\u003EAccessor : MyObjectBuilder_EnvironmentSector.Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_EnvironmentSector\u003C\u003ESectorId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProceduralEnvironmentSector, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProceduralEnvironmentSector owner,
        in long value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_EnvironmentSector&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProceduralEnvironmentSector owner,
        out long value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_EnvironmentSector&) ref owner, out value);
      }
    }

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_ProceduralEnvironmentSector\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProceduralEnvironmentSector, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProceduralEnvironmentSector owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProceduralEnvironmentSector owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_ProceduralEnvironmentSector\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProceduralEnvironmentSector, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProceduralEnvironmentSector owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProceduralEnvironmentSector owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_ProceduralEnvironmentSector\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProceduralEnvironmentSector, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProceduralEnvironmentSector owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProceduralEnvironmentSector owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_ProceduralEnvironmentSector\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProceduralEnvironmentSector, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProceduralEnvironmentSector owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProceduralEnvironmentSector owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    private class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_ProceduralEnvironmentSector\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ProceduralEnvironmentSector>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_ProceduralEnvironmentSector();

      MyObjectBuilder_ProceduralEnvironmentSector IActivator<MyObjectBuilder_ProceduralEnvironmentSector>.CreateInstance() => new MyObjectBuilder_ProceduralEnvironmentSector();
    }
  }
}
