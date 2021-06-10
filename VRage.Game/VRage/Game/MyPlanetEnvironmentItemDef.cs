﻿// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyPlanetEnvironmentItemDef
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRageMath;

namespace VRage.Game
{
  [ProtoContract]
  public class MyPlanetEnvironmentItemDef
  {
    [ProtoMember(53)]
    [XmlAttribute(AttributeName = "TypeId")]
    public string TypeId;
    [ProtoMember(54)]
    [XmlAttribute(AttributeName = "SubtypeId")]
    public string SubtypeId;
    [ProtoMember(55)]
    [XmlAttribute(AttributeName = "GroupId")]
    public string GroupId;
    [ProtoMember(56)]
    [XmlAttribute(AttributeName = "ModifierId")]
    public string ModifierId;
    [ProtoMember(57)]
    public int GroupIndex = -1;
    [ProtoMember(58)]
    public int ModifierIndex = -1;
    [ProtoMember(59)]
    [XmlAttribute(AttributeName = "Density")]
    public float Density;
    [ProtoMember(60)]
    [XmlAttribute(AttributeName = "IsDetail")]
    public bool IsDetail;
    [ProtoMember(61)]
    public Vector3 BaseColor = Vector3.Zero;
    [ProtoMember(62)]
    public Vector2 ColorSpread = Vector2.Zero;
    [ProtoMember(63)]
    [XmlAttribute(AttributeName = "Offset")]
    public float Offset;
    [ProtoMember(64)]
    [XmlAttribute(AttributeName = "MaxRoll")]
    public float MaxRoll;

    protected class VRage_Game_MyPlanetEnvironmentItemDef\u003C\u003ETypeId\u003C\u003EAccessor : IMemberAccessor<MyPlanetEnvironmentItemDef, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetEnvironmentItemDef owner, in string value) => owner.TypeId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetEnvironmentItemDef owner, out string value) => value = owner.TypeId;
    }

    protected class VRage_Game_MyPlanetEnvironmentItemDef\u003C\u003ESubtypeId\u003C\u003EAccessor : IMemberAccessor<MyPlanetEnvironmentItemDef, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetEnvironmentItemDef owner, in string value) => owner.SubtypeId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetEnvironmentItemDef owner, out string value) => value = owner.SubtypeId;
    }

    protected class VRage_Game_MyPlanetEnvironmentItemDef\u003C\u003EGroupId\u003C\u003EAccessor : IMemberAccessor<MyPlanetEnvironmentItemDef, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetEnvironmentItemDef owner, in string value) => owner.GroupId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetEnvironmentItemDef owner, out string value) => value = owner.GroupId;
    }

    protected class VRage_Game_MyPlanetEnvironmentItemDef\u003C\u003EModifierId\u003C\u003EAccessor : IMemberAccessor<MyPlanetEnvironmentItemDef, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetEnvironmentItemDef owner, in string value) => owner.ModifierId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetEnvironmentItemDef owner, out string value) => value = owner.ModifierId;
    }

    protected class VRage_Game_MyPlanetEnvironmentItemDef\u003C\u003EGroupIndex\u003C\u003EAccessor : IMemberAccessor<MyPlanetEnvironmentItemDef, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetEnvironmentItemDef owner, in int value) => owner.GroupIndex = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetEnvironmentItemDef owner, out int value) => value = owner.GroupIndex;
    }

    protected class VRage_Game_MyPlanetEnvironmentItemDef\u003C\u003EModifierIndex\u003C\u003EAccessor : IMemberAccessor<MyPlanetEnvironmentItemDef, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetEnvironmentItemDef owner, in int value) => owner.ModifierIndex = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetEnvironmentItemDef owner, out int value) => value = owner.ModifierIndex;
    }

    protected class VRage_Game_MyPlanetEnvironmentItemDef\u003C\u003EDensity\u003C\u003EAccessor : IMemberAccessor<MyPlanetEnvironmentItemDef, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetEnvironmentItemDef owner, in float value) => owner.Density = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetEnvironmentItemDef owner, out float value) => value = owner.Density;
    }

    protected class VRage_Game_MyPlanetEnvironmentItemDef\u003C\u003EIsDetail\u003C\u003EAccessor : IMemberAccessor<MyPlanetEnvironmentItemDef, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetEnvironmentItemDef owner, in bool value) => owner.IsDetail = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetEnvironmentItemDef owner, out bool value) => value = owner.IsDetail;
    }

    protected class VRage_Game_MyPlanetEnvironmentItemDef\u003C\u003EBaseColor\u003C\u003EAccessor : IMemberAccessor<MyPlanetEnvironmentItemDef, Vector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetEnvironmentItemDef owner, in Vector3 value) => owner.BaseColor = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetEnvironmentItemDef owner, out Vector3 value) => value = owner.BaseColor;
    }

    protected class VRage_Game_MyPlanetEnvironmentItemDef\u003C\u003EColorSpread\u003C\u003EAccessor : IMemberAccessor<MyPlanetEnvironmentItemDef, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetEnvironmentItemDef owner, in Vector2 value) => owner.ColorSpread = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetEnvironmentItemDef owner, out Vector2 value) => value = owner.ColorSpread;
    }

    protected class VRage_Game_MyPlanetEnvironmentItemDef\u003C\u003EOffset\u003C\u003EAccessor : IMemberAccessor<MyPlanetEnvironmentItemDef, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetEnvironmentItemDef owner, in float value) => owner.Offset = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetEnvironmentItemDef owner, out float value) => value = owner.Offset;
    }

    protected class VRage_Game_MyPlanetEnvironmentItemDef\u003C\u003EMaxRoll\u003C\u003EAccessor : IMemberAccessor<MyPlanetEnvironmentItemDef, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetEnvironmentItemDef owner, in float value) => owner.MaxRoll = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetEnvironmentItemDef owner, out float value) => value = owner.MaxRoll;
    }

    private class VRage_Game_MyPlanetEnvironmentItemDef\u003C\u003EActor : IActivator, IActivator<MyPlanetEnvironmentItemDef>
    {
      object IActivator.CreateInstance() => (object) new MyPlanetEnvironmentItemDef();

      MyPlanetEnvironmentItemDef IActivator<MyPlanetEnvironmentItemDef>.CreateInstance() => new MyPlanetEnvironmentItemDef();
    }
  }
}
