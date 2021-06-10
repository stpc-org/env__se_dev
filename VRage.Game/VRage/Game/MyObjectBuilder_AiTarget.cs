// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_AiTarget
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
using VRageMath;

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_AiTarget : MyObjectBuilder_Base
  {
    [ProtoMember(7)]
    public MyAiTargetEnum CurrentTarget;
    [ProtoMember(10)]
    public long? EntityId;
    [ProtoMember(13)]
    public ushort? CompoundId;
    [ProtoMember(16)]
    public Vector3I TargetCube = Vector3I.Zero;
    [ProtoMember(19)]
    public Vector3D TargetPosition = Vector3D.Zero;
    [ProtoMember(22)]
    public List<MyObjectBuilder_AiTarget.UnreachableEntitiesData> UnreachableEntities;

    [ProtoContract]
    public class UnreachableEntitiesData
    {
      [ProtoMember(1)]
      public long UnreachableEntityId;
      [ProtoMember(4)]
      public int Timeout;

      protected class VRage_Game_MyObjectBuilder_AiTarget\u003C\u003EUnreachableEntitiesData\u003C\u003EUnreachableEntityId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AiTarget.UnreachableEntitiesData, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_AiTarget.UnreachableEntitiesData owner,
          in long value)
        {
          owner.UnreachableEntityId = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_AiTarget.UnreachableEntitiesData owner,
          out long value)
        {
          value = owner.UnreachableEntityId;
        }
      }

      protected class VRage_Game_MyObjectBuilder_AiTarget\u003C\u003EUnreachableEntitiesData\u003C\u003ETimeout\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AiTarget.UnreachableEntitiesData, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_AiTarget.UnreachableEntitiesData owner,
          in int value)
        {
          owner.Timeout = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_AiTarget.UnreachableEntitiesData owner,
          out int value)
        {
          value = owner.Timeout;
        }
      }

      private class VRage_Game_MyObjectBuilder_AiTarget\u003C\u003EUnreachableEntitiesData\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_AiTarget.UnreachableEntitiesData>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_AiTarget.UnreachableEntitiesData();

        MyObjectBuilder_AiTarget.UnreachableEntitiesData IActivator<MyObjectBuilder_AiTarget.UnreachableEntitiesData>.CreateInstance() => new MyObjectBuilder_AiTarget.UnreachableEntitiesData();
      }
    }

    protected class VRage_Game_MyObjectBuilder_AiTarget\u003C\u003ECurrentTarget\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AiTarget, MyAiTargetEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AiTarget owner, in MyAiTargetEnum value) => owner.CurrentTarget = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AiTarget owner, out MyAiTargetEnum value) => value = owner.CurrentTarget;
    }

    protected class VRage_Game_MyObjectBuilder_AiTarget\u003C\u003EEntityId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AiTarget, long?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AiTarget owner, in long? value) => owner.EntityId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AiTarget owner, out long? value) => value = owner.EntityId;
    }

    protected class VRage_Game_MyObjectBuilder_AiTarget\u003C\u003ECompoundId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AiTarget, ushort?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AiTarget owner, in ushort? value) => owner.CompoundId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AiTarget owner, out ushort? value) => value = owner.CompoundId;
    }

    protected class VRage_Game_MyObjectBuilder_AiTarget\u003C\u003ETargetCube\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AiTarget, Vector3I>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AiTarget owner, in Vector3I value) => owner.TargetCube = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AiTarget owner, out Vector3I value) => value = owner.TargetCube;
    }

    protected class VRage_Game_MyObjectBuilder_AiTarget\u003C\u003ETargetPosition\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AiTarget, Vector3D>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AiTarget owner, in Vector3D value) => owner.TargetPosition = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AiTarget owner, out Vector3D value) => value = owner.TargetPosition;
    }

    protected class VRage_Game_MyObjectBuilder_AiTarget\u003C\u003EUnreachableEntities\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AiTarget, List<MyObjectBuilder_AiTarget.UnreachableEntitiesData>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AiTarget owner,
        in List<MyObjectBuilder_AiTarget.UnreachableEntitiesData> value)
      {
        owner.UnreachableEntities = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AiTarget owner,
        out List<MyObjectBuilder_AiTarget.UnreachableEntitiesData> value)
      {
        value = owner.UnreachableEntities;
      }
    }

    protected class VRage_Game_MyObjectBuilder_AiTarget\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AiTarget, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AiTarget owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AiTarget owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AiTarget\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AiTarget, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AiTarget owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AiTarget owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AiTarget\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AiTarget, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AiTarget owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AiTarget owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AiTarget\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AiTarget, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AiTarget owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AiTarget owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_AiTarget\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_AiTarget>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_AiTarget();

      MyObjectBuilder_AiTarget IActivator<MyObjectBuilder_AiTarget>.CreateInstance() => new MyObjectBuilder_AiTarget();
    }
  }
}
