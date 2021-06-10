// Decompiled with JetBrains decompiler
// Type: Sandbox.Common.ObjectBuilders.Definitions.ScreenArea
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.Serialization;

namespace Sandbox.Common.ObjectBuilders.Definitions
{
  [ProtoContract]
  public class ScreenArea
  {
    [ProtoMember(1)]
    [XmlAttribute]
    public string Name;
    [ProtoMember(4)]
    [XmlAttribute]
    public string DisplayName;
    [ProtoMember(7)]
    [XmlAttribute]
    [DefaultValue(512)]
    public int TextureResolution = 512;
    [ProtoMember(10)]
    [XmlAttribute]
    [DefaultValue(1)]
    public int ScreenWidth = 1;
    [ProtoMember(13)]
    [XmlAttribute]
    [DefaultValue(1)]
    public int ScreenHeight = 1;
    [ProtoMember(16)]
    [XmlAttribute]
    [DefaultValue(null)]
    [Nullable]
    public string Script;

    protected class Sandbox_Common_ObjectBuilders_Definitions_ScreenArea\u003C\u003EName\u003C\u003EAccessor : IMemberAccessor<ScreenArea, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ScreenArea owner, in string value) => owner.Name = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ScreenArea owner, out string value) => value = owner.Name;
    }

    protected class Sandbox_Common_ObjectBuilders_Definitions_ScreenArea\u003C\u003EDisplayName\u003C\u003EAccessor : IMemberAccessor<ScreenArea, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ScreenArea owner, in string value) => owner.DisplayName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ScreenArea owner, out string value) => value = owner.DisplayName;
    }

    protected class Sandbox_Common_ObjectBuilders_Definitions_ScreenArea\u003C\u003ETextureResolution\u003C\u003EAccessor : IMemberAccessor<ScreenArea, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ScreenArea owner, in int value) => owner.TextureResolution = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ScreenArea owner, out int value) => value = owner.TextureResolution;
    }

    protected class Sandbox_Common_ObjectBuilders_Definitions_ScreenArea\u003C\u003EScreenWidth\u003C\u003EAccessor : IMemberAccessor<ScreenArea, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ScreenArea owner, in int value) => owner.ScreenWidth = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ScreenArea owner, out int value) => value = owner.ScreenWidth;
    }

    protected class Sandbox_Common_ObjectBuilders_Definitions_ScreenArea\u003C\u003EScreenHeight\u003C\u003EAccessor : IMemberAccessor<ScreenArea, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ScreenArea owner, in int value) => owner.ScreenHeight = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ScreenArea owner, out int value) => value = owner.ScreenHeight;
    }

    protected class Sandbox_Common_ObjectBuilders_Definitions_ScreenArea\u003C\u003EScript\u003C\u003EAccessor : IMemberAccessor<ScreenArea, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ScreenArea owner, in string value) => owner.Script = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ScreenArea owner, out string value) => value = owner.Script;
    }

    private class Sandbox_Common_ObjectBuilders_Definitions_ScreenArea\u003C\u003EActor : IActivator, IActivator<ScreenArea>
    {
      object IActivator.CreateInstance() => (object) new ScreenArea();

      ScreenArea IActivator<ScreenArea>.CreateInstance() => new ScreenArea();
    }
  }
}
