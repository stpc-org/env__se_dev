// Decompiled with JetBrains decompiler
// Type: VRage.MyAudioWave
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using ProtoBuf;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Data;
using VRage.Data.Audio;
using VRage.Network;

namespace VRage
{
  [ProtoContract]
  [XmlType("Wave")]
  public sealed class MyAudioWave
  {
    [ProtoMember(10)]
    [XmlAttribute]
    public MySoundDimensions Type;
    [ProtoMember(13)]
    [DefaultValue("")]
    [ModdableContentFile(new string[] {"xwm", "wav"})]
    public string Start;
    [ProtoMember(16)]
    [DefaultValue("")]
    [ModdableContentFile(new string[] {"xwm", "wav"})]
    public string Loop;
    [ProtoMember(19)]
    [DefaultValue("")]
    [ModdableContentFile(new string[] {"xwm", "wav"})]
    public string End;

    protected class VRage_MyAudioWave\u003C\u003EType\u003C\u003EAccessor : IMemberAccessor<MyAudioWave, MySoundDimensions>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyAudioWave owner, in MySoundDimensions value) => owner.Type = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyAudioWave owner, out MySoundDimensions value) => value = owner.Type;
    }

    protected class VRage_MyAudioWave\u003C\u003EStart\u003C\u003EAccessor : IMemberAccessor<MyAudioWave, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyAudioWave owner, in string value) => owner.Start = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyAudioWave owner, out string value) => value = owner.Start;
    }

    protected class VRage_MyAudioWave\u003C\u003ELoop\u003C\u003EAccessor : IMemberAccessor<MyAudioWave, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyAudioWave owner, in string value) => owner.Loop = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyAudioWave owner, out string value) => value = owner.Loop;
    }

    protected class VRage_MyAudioWave\u003C\u003EEnd\u003C\u003EAccessor : IMemberAccessor<MyAudioWave, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyAudioWave owner, in string value) => owner.End = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyAudioWave owner, out string value) => value = owner.End;
    }
  }
}
