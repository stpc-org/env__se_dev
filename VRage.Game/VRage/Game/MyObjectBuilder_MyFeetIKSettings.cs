// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_MyFeetIKSettings
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game
{
  [ProtoContract]
  public class MyObjectBuilder_MyFeetIKSettings
  {
    [ProtoMember(14)]
    public string MovementState;
    [ProtoMember(15)]
    public bool Enabled;
    [ProtoMember(16)]
    public float BelowReachableDistance;
    [ProtoMember(17)]
    public float AboveReachableDistance;
    [ProtoMember(18)]
    public float VerticalShiftUpGain;
    [ProtoMember(19)]
    public float VerticalShiftDownGain;
    [ProtoMember(20)]
    public float FootLenght;
    [ProtoMember(21)]
    public float FootWidth;
    [ProtoMember(22)]
    public float AnkleHeight;

    protected class VRage_Game_MyObjectBuilder_MyFeetIKSettings\u003C\u003EMovementState\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_MyFeetIKSettings, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_MyFeetIKSettings owner, in string value) => owner.MovementState = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_MyFeetIKSettings owner, out string value) => value = owner.MovementState;
    }

    protected class VRage_Game_MyObjectBuilder_MyFeetIKSettings\u003C\u003EEnabled\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_MyFeetIKSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_MyFeetIKSettings owner, in bool value) => owner.Enabled = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_MyFeetIKSettings owner, out bool value) => value = owner.Enabled;
    }

    protected class VRage_Game_MyObjectBuilder_MyFeetIKSettings\u003C\u003EBelowReachableDistance\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_MyFeetIKSettings, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_MyFeetIKSettings owner, in float value) => owner.BelowReachableDistance = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_MyFeetIKSettings owner, out float value) => value = owner.BelowReachableDistance;
    }

    protected class VRage_Game_MyObjectBuilder_MyFeetIKSettings\u003C\u003EAboveReachableDistance\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_MyFeetIKSettings, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_MyFeetIKSettings owner, in float value) => owner.AboveReachableDistance = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_MyFeetIKSettings owner, out float value) => value = owner.AboveReachableDistance;
    }

    protected class VRage_Game_MyObjectBuilder_MyFeetIKSettings\u003C\u003EVerticalShiftUpGain\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_MyFeetIKSettings, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_MyFeetIKSettings owner, in float value) => owner.VerticalShiftUpGain = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_MyFeetIKSettings owner, out float value) => value = owner.VerticalShiftUpGain;
    }

    protected class VRage_Game_MyObjectBuilder_MyFeetIKSettings\u003C\u003EVerticalShiftDownGain\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_MyFeetIKSettings, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_MyFeetIKSettings owner, in float value) => owner.VerticalShiftDownGain = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_MyFeetIKSettings owner, out float value) => value = owner.VerticalShiftDownGain;
    }

    protected class VRage_Game_MyObjectBuilder_MyFeetIKSettings\u003C\u003EFootLenght\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_MyFeetIKSettings, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_MyFeetIKSettings owner, in float value) => owner.FootLenght = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_MyFeetIKSettings owner, out float value) => value = owner.FootLenght;
    }

    protected class VRage_Game_MyObjectBuilder_MyFeetIKSettings\u003C\u003EFootWidth\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_MyFeetIKSettings, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_MyFeetIKSettings owner, in float value) => owner.FootWidth = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_MyFeetIKSettings owner, out float value) => value = owner.FootWidth;
    }

    protected class VRage_Game_MyObjectBuilder_MyFeetIKSettings\u003C\u003EAnkleHeight\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_MyFeetIKSettings, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_MyFeetIKSettings owner, in float value) => owner.AnkleHeight = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_MyFeetIKSettings owner, out float value) => value = owner.AnkleHeight;
    }

    private class VRage_Game_MyObjectBuilder_MyFeetIKSettings\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_MyFeetIKSettings>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_MyFeetIKSettings();

      MyObjectBuilder_MyFeetIKSettings IActivator<MyObjectBuilder_MyFeetIKSettings>.CreateInstance() => new MyObjectBuilder_MyFeetIKSettings();
    }
  }
}
