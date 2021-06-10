// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_HudEntityParams
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game.Gui;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game
{
  [ProtoContract]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_HudEntityParams : MyObjectBuilder_Base
  {
    [ProtoMember(1)]
    public Vector3D Position;
    [ProtoMember(2)]
    public long EntityId;
    [ProtoMember(3)]
    public string Text;
    [ProtoMember(4)]
    public MyHudIndicatorFlagsEnum FlagsEnum;
    [ProtoMember(5)]
    public long Owner;
    [ProtoMember(6)]
    public MyOwnershipShareModeEnum Share;
    [ProtoMember(7)]
    public float BlinkingTime;

    protected class VRage_Game_MyObjectBuilder_HudEntityParams\u003C\u003EPosition\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HudEntityParams, Vector3D>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HudEntityParams owner, in Vector3D value) => owner.Position = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HudEntityParams owner, out Vector3D value) => value = owner.Position;
    }

    protected class VRage_Game_MyObjectBuilder_HudEntityParams\u003C\u003EEntityId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HudEntityParams, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HudEntityParams owner, in long value) => owner.EntityId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HudEntityParams owner, out long value) => value = owner.EntityId;
    }

    protected class VRage_Game_MyObjectBuilder_HudEntityParams\u003C\u003EText\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HudEntityParams, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HudEntityParams owner, in string value) => owner.Text = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HudEntityParams owner, out string value) => value = owner.Text;
    }

    protected class VRage_Game_MyObjectBuilder_HudEntityParams\u003C\u003EFlagsEnum\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HudEntityParams, MyHudIndicatorFlagsEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_HudEntityParams owner,
        in MyHudIndicatorFlagsEnum value)
      {
        owner.FlagsEnum = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_HudEntityParams owner,
        out MyHudIndicatorFlagsEnum value)
      {
        value = owner.FlagsEnum;
      }
    }

    protected class VRage_Game_MyObjectBuilder_HudEntityParams\u003C\u003EOwner\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HudEntityParams, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HudEntityParams owner, in long value) => owner.Owner = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HudEntityParams owner, out long value) => value = owner.Owner;
    }

    protected class VRage_Game_MyObjectBuilder_HudEntityParams\u003C\u003EShare\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HudEntityParams, MyOwnershipShareModeEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_HudEntityParams owner,
        in MyOwnershipShareModeEnum value)
      {
        owner.Share = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_HudEntityParams owner,
        out MyOwnershipShareModeEnum value)
      {
        value = owner.Share;
      }
    }

    protected class VRage_Game_MyObjectBuilder_HudEntityParams\u003C\u003EBlinkingTime\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HudEntityParams, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HudEntityParams owner, in float value) => owner.BlinkingTime = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HudEntityParams owner, out float value) => value = owner.BlinkingTime;
    }

    protected class VRage_Game_MyObjectBuilder_HudEntityParams\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_HudEntityParams, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HudEntityParams owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HudEntityParams owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_HudEntityParams\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_HudEntityParams, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HudEntityParams owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HudEntityParams owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_HudEntityParams\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_HudEntityParams, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HudEntityParams owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HudEntityParams owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_HudEntityParams\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_HudEntityParams, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HudEntityParams owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HudEntityParams owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_HudEntityParams\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_HudEntityParams>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_HudEntityParams();

      MyObjectBuilder_HudEntityParams IActivator<MyObjectBuilder_HudEntityParams>.CreateInstance() => new MyObjectBuilder_HudEntityParams();
    }
  }
}
