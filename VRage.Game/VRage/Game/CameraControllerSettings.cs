// Decompiled with JetBrains decompiler
// Type: VRage.Game.CameraControllerSettings
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.Game
{
  [ProtoContract]
  public class CameraControllerSettings
  {
    [ProtoMember(1)]
    public bool IsFirstPerson;
    [ProtoMember(4)]
    public double Distance;
    [ProtoMember(7)]
    public SerializableVector2? HeadAngle;
    [XmlAttribute]
    public long EntityId;

    protected class VRage_Game_CameraControllerSettings\u003C\u003EIsFirstPerson\u003C\u003EAccessor : IMemberAccessor<CameraControllerSettings, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref CameraControllerSettings owner, in bool value) => owner.IsFirstPerson = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref CameraControllerSettings owner, out bool value) => value = owner.IsFirstPerson;
    }

    protected class VRage_Game_CameraControllerSettings\u003C\u003EDistance\u003C\u003EAccessor : IMemberAccessor<CameraControllerSettings, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref CameraControllerSettings owner, in double value) => owner.Distance = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref CameraControllerSettings owner, out double value) => value = owner.Distance;
    }

    protected class VRage_Game_CameraControllerSettings\u003C\u003EHeadAngle\u003C\u003EAccessor : IMemberAccessor<CameraControllerSettings, SerializableVector2?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref CameraControllerSettings owner, in SerializableVector2? value) => owner.HeadAngle = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref CameraControllerSettings owner, out SerializableVector2? value) => value = owner.HeadAngle;
    }

    protected class VRage_Game_CameraControllerSettings\u003C\u003EEntityId\u003C\u003EAccessor : IMemberAccessor<CameraControllerSettings, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref CameraControllerSettings owner, in long value) => owner.EntityId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref CameraControllerSettings owner, out long value) => value = owner.EntityId;
    }

    private class VRage_Game_CameraControllerSettings\u003C\u003EActor : IActivator, IActivator<CameraControllerSettings>
    {
      object IActivator.CreateInstance() => (object) new CameraControllerSettings();

      CameraControllerSettings IActivator<CameraControllerSettings>.CreateInstance() => new CameraControllerSettings();
    }
  }
}
