// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_TransparentMaterial
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.ComponentModel;
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
  public class MyObjectBuilder_TransparentMaterial : MyObjectBuilder_Base
  {
    [ProtoMember(1)]
    public string Name;
    [ProtoMember(4)]
    public bool AlphaMistingEnable;
    [ProtoMember(7)]
    [DefaultValue(1)]
    public float AlphaMistingStart = 1f;
    [ProtoMember(10)]
    [DefaultValue(4)]
    public float AlphaMistingEnd = 4f;
    [ProtoMember(13)]
    [DefaultValue(1)]
    public float AlphaSaturation = 1f;
    [ProtoMember(16)]
    public bool CanBeAffectedByOtherLights;
    [ProtoMember(19)]
    public float Emissivity;
    [ProtoMember(22)]
    public bool IgnoreDepth;
    [ProtoMember(25)]
    [DefaultValue(true)]
    public bool NeedSort = true;
    [ProtoMember(28)]
    public float SoftParticleDistanceScale;
    [ProtoMember(31)]
    public string Texture;
    [ProtoMember(34)]
    public bool UseAtlas;
    [ProtoMember(37)]
    public Vector2 UVOffset = new Vector2(0.0f, 0.0f);
    [ProtoMember(40)]
    public Vector2 UVSize = new Vector2(1f, 1f);
    [ProtoMember(43)]
    public bool Reflection;

    protected class VRage_Game_MyObjectBuilder_TransparentMaterial\u003C\u003EName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterial, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TransparentMaterial owner, in string value) => owner.Name = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TransparentMaterial owner, out string value) => value = owner.Name;
    }

    protected class VRage_Game_MyObjectBuilder_TransparentMaterial\u003C\u003EAlphaMistingEnable\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterial, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TransparentMaterial owner, in bool value) => owner.AlphaMistingEnable = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TransparentMaterial owner, out bool value) => value = owner.AlphaMistingEnable;
    }

    protected class VRage_Game_MyObjectBuilder_TransparentMaterial\u003C\u003EAlphaMistingStart\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterial, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TransparentMaterial owner, in float value) => owner.AlphaMistingStart = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TransparentMaterial owner, out float value) => value = owner.AlphaMistingStart;
    }

    protected class VRage_Game_MyObjectBuilder_TransparentMaterial\u003C\u003EAlphaMistingEnd\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterial, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TransparentMaterial owner, in float value) => owner.AlphaMistingEnd = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TransparentMaterial owner, out float value) => value = owner.AlphaMistingEnd;
    }

    protected class VRage_Game_MyObjectBuilder_TransparentMaterial\u003C\u003EAlphaSaturation\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterial, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TransparentMaterial owner, in float value) => owner.AlphaSaturation = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TransparentMaterial owner, out float value) => value = owner.AlphaSaturation;
    }

    protected class VRage_Game_MyObjectBuilder_TransparentMaterial\u003C\u003ECanBeAffectedByOtherLights\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterial, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TransparentMaterial owner, in bool value) => owner.CanBeAffectedByOtherLights = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TransparentMaterial owner, out bool value) => value = owner.CanBeAffectedByOtherLights;
    }

    protected class VRage_Game_MyObjectBuilder_TransparentMaterial\u003C\u003EEmissivity\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterial, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TransparentMaterial owner, in float value) => owner.Emissivity = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TransparentMaterial owner, out float value) => value = owner.Emissivity;
    }

    protected class VRage_Game_MyObjectBuilder_TransparentMaterial\u003C\u003EIgnoreDepth\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterial, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TransparentMaterial owner, in bool value) => owner.IgnoreDepth = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TransparentMaterial owner, out bool value) => value = owner.IgnoreDepth;
    }

    protected class VRage_Game_MyObjectBuilder_TransparentMaterial\u003C\u003ENeedSort\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterial, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TransparentMaterial owner, in bool value) => owner.NeedSort = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TransparentMaterial owner, out bool value) => value = owner.NeedSort;
    }

    protected class VRage_Game_MyObjectBuilder_TransparentMaterial\u003C\u003ESoftParticleDistanceScale\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterial, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TransparentMaterial owner, in float value) => owner.SoftParticleDistanceScale = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TransparentMaterial owner, out float value) => value = owner.SoftParticleDistanceScale;
    }

    protected class VRage_Game_MyObjectBuilder_TransparentMaterial\u003C\u003ETexture\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterial, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TransparentMaterial owner, in string value) => owner.Texture = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TransparentMaterial owner, out string value) => value = owner.Texture;
    }

    protected class VRage_Game_MyObjectBuilder_TransparentMaterial\u003C\u003EUseAtlas\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterial, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TransparentMaterial owner, in bool value) => owner.UseAtlas = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TransparentMaterial owner, out bool value) => value = owner.UseAtlas;
    }

    protected class VRage_Game_MyObjectBuilder_TransparentMaterial\u003C\u003EUVOffset\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterial, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TransparentMaterial owner, in Vector2 value) => owner.UVOffset = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TransparentMaterial owner, out Vector2 value) => value = owner.UVOffset;
    }

    protected class VRage_Game_MyObjectBuilder_TransparentMaterial\u003C\u003EUVSize\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterial, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TransparentMaterial owner, in Vector2 value) => owner.UVSize = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TransparentMaterial owner, out Vector2 value) => value = owner.UVSize;
    }

    protected class VRage_Game_MyObjectBuilder_TransparentMaterial\u003C\u003EReflection\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterial, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TransparentMaterial owner, in bool value) => owner.Reflection = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TransparentMaterial owner, out bool value) => value = owner.Reflection;
    }

    protected class VRage_Game_MyObjectBuilder_TransparentMaterial\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TransparentMaterial, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TransparentMaterial owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TransparentMaterial owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TransparentMaterial\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TransparentMaterial, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TransparentMaterial owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TransparentMaterial owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TransparentMaterial\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TransparentMaterial, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TransparentMaterial owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TransparentMaterial owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TransparentMaterial\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TransparentMaterial, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TransparentMaterial owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TransparentMaterial owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_TransparentMaterial\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_TransparentMaterial>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_TransparentMaterial();

      MyObjectBuilder_TransparentMaterial IActivator<MyObjectBuilder_TransparentMaterial>.CreateInstance() => new MyObjectBuilder_TransparentMaterial();
    }
  }
}
