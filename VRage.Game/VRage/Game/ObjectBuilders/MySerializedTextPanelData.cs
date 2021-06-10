// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.MySerializedTextPanelData
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game.GUI.TextPanel;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRageMath;

namespace VRage.Game.ObjectBuilders
{
  [ProtoContract]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MySerializedTextPanelData
  {
    [ProtoMember(1)]
    public float ChangeInterval;
    [ProtoMember(4)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public List<string> SelectedImages;
    [ProtoMember(7)]
    public SerializableDefinitionId Font = (SerializableDefinitionId) new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_FontDefinition), "Debug");
    [ProtoMember(10)]
    public float FontSize = 1f;
    [ProtoMember(13)]
    [DefaultValue("")]
    public string Text = "";
    [ProtoMember(16)]
    public ShowTextOnScreenFlag ShowText;
    [ProtoMember(19)]
    public Color FontColor = Color.White;
    [ProtoMember(22)]
    public Color BackgroundColor = Color.Black;
    [ProtoMember(25)]
    public int CurrentShownTexture;
    [ProtoMember(28, IsRequired = false)]
    [DefaultValue(0)]
    public int Alignment;
    [ProtoMember(31)]
    [DefaultValue(ContentType.NONE)]
    public ContentType ContentType;
    [ProtoMember(34)]
    [DefaultValue(null)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public string SelectedScript;
    [ProtoMember(37)]
    public float TextPadding = 2f;
    [ProtoMember(40)]
    [DefaultValue(false)]
    public bool PreserveAspectRatio;
    [ProtoMember(43)]
    [DefaultValue(false)]
    public bool CustomizeScripts;
    [ProtoMember(46)]
    public Color ScriptBackgroundColor = new Color(0, 88, 151);
    [ProtoMember(49)]
    public Color ScriptForegroundColor = new Color(179, 237, (int) byte.MaxValue);
    [ProtoMember(54)]
    public MySerializableSpriteCollection Sprites;

    protected class VRage_Game_ObjectBuilders_MySerializedTextPanelData\u003C\u003EChangeInterval\u003C\u003EAccessor : IMemberAccessor<MySerializedTextPanelData, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySerializedTextPanelData owner, in float value) => owner.ChangeInterval = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySerializedTextPanelData owner, out float value) => value = owner.ChangeInterval;
    }

    protected class VRage_Game_ObjectBuilders_MySerializedTextPanelData\u003C\u003ESelectedImages\u003C\u003EAccessor : IMemberAccessor<MySerializedTextPanelData, List<string>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySerializedTextPanelData owner, in List<string> value) => owner.SelectedImages = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySerializedTextPanelData owner, out List<string> value) => value = owner.SelectedImages;
    }

    protected class VRage_Game_ObjectBuilders_MySerializedTextPanelData\u003C\u003EFont\u003C\u003EAccessor : IMemberAccessor<MySerializedTextPanelData, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MySerializedTextPanelData owner,
        in SerializableDefinitionId value)
      {
        owner.Font = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MySerializedTextPanelData owner,
        out SerializableDefinitionId value)
      {
        value = owner.Font;
      }
    }

    protected class VRage_Game_ObjectBuilders_MySerializedTextPanelData\u003C\u003EFontSize\u003C\u003EAccessor : IMemberAccessor<MySerializedTextPanelData, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySerializedTextPanelData owner, in float value) => owner.FontSize = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySerializedTextPanelData owner, out float value) => value = owner.FontSize;
    }

    protected class VRage_Game_ObjectBuilders_MySerializedTextPanelData\u003C\u003EText\u003C\u003EAccessor : IMemberAccessor<MySerializedTextPanelData, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySerializedTextPanelData owner, in string value) => owner.Text = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySerializedTextPanelData owner, out string value) => value = owner.Text;
    }

    protected class VRage_Game_ObjectBuilders_MySerializedTextPanelData\u003C\u003EShowText\u003C\u003EAccessor : IMemberAccessor<MySerializedTextPanelData, ShowTextOnScreenFlag>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySerializedTextPanelData owner, in ShowTextOnScreenFlag value) => owner.ShowText = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySerializedTextPanelData owner, out ShowTextOnScreenFlag value) => value = owner.ShowText;
    }

    protected class VRage_Game_ObjectBuilders_MySerializedTextPanelData\u003C\u003EFontColor\u003C\u003EAccessor : IMemberAccessor<MySerializedTextPanelData, Color>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySerializedTextPanelData owner, in Color value) => owner.FontColor = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySerializedTextPanelData owner, out Color value) => value = owner.FontColor;
    }

    protected class VRage_Game_ObjectBuilders_MySerializedTextPanelData\u003C\u003EBackgroundColor\u003C\u003EAccessor : IMemberAccessor<MySerializedTextPanelData, Color>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySerializedTextPanelData owner, in Color value) => owner.BackgroundColor = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySerializedTextPanelData owner, out Color value) => value = owner.BackgroundColor;
    }

    protected class VRage_Game_ObjectBuilders_MySerializedTextPanelData\u003C\u003ECurrentShownTexture\u003C\u003EAccessor : IMemberAccessor<MySerializedTextPanelData, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySerializedTextPanelData owner, in int value) => owner.CurrentShownTexture = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySerializedTextPanelData owner, out int value) => value = owner.CurrentShownTexture;
    }

    protected class VRage_Game_ObjectBuilders_MySerializedTextPanelData\u003C\u003EAlignment\u003C\u003EAccessor : IMemberAccessor<MySerializedTextPanelData, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySerializedTextPanelData owner, in int value) => owner.Alignment = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySerializedTextPanelData owner, out int value) => value = owner.Alignment;
    }

    protected class VRage_Game_ObjectBuilders_MySerializedTextPanelData\u003C\u003EContentType\u003C\u003EAccessor : IMemberAccessor<MySerializedTextPanelData, ContentType>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySerializedTextPanelData owner, in ContentType value) => owner.ContentType = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySerializedTextPanelData owner, out ContentType value) => value = owner.ContentType;
    }

    protected class VRage_Game_ObjectBuilders_MySerializedTextPanelData\u003C\u003ESelectedScript\u003C\u003EAccessor : IMemberAccessor<MySerializedTextPanelData, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySerializedTextPanelData owner, in string value) => owner.SelectedScript = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySerializedTextPanelData owner, out string value) => value = owner.SelectedScript;
    }

    protected class VRage_Game_ObjectBuilders_MySerializedTextPanelData\u003C\u003ETextPadding\u003C\u003EAccessor : IMemberAccessor<MySerializedTextPanelData, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySerializedTextPanelData owner, in float value) => owner.TextPadding = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySerializedTextPanelData owner, out float value) => value = owner.TextPadding;
    }

    protected class VRage_Game_ObjectBuilders_MySerializedTextPanelData\u003C\u003EPreserveAspectRatio\u003C\u003EAccessor : IMemberAccessor<MySerializedTextPanelData, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySerializedTextPanelData owner, in bool value) => owner.PreserveAspectRatio = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySerializedTextPanelData owner, out bool value) => value = owner.PreserveAspectRatio;
    }

    protected class VRage_Game_ObjectBuilders_MySerializedTextPanelData\u003C\u003ECustomizeScripts\u003C\u003EAccessor : IMemberAccessor<MySerializedTextPanelData, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySerializedTextPanelData owner, in bool value) => owner.CustomizeScripts = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySerializedTextPanelData owner, out bool value) => value = owner.CustomizeScripts;
    }

    protected class VRage_Game_ObjectBuilders_MySerializedTextPanelData\u003C\u003EScriptBackgroundColor\u003C\u003EAccessor : IMemberAccessor<MySerializedTextPanelData, Color>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySerializedTextPanelData owner, in Color value) => owner.ScriptBackgroundColor = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySerializedTextPanelData owner, out Color value) => value = owner.ScriptBackgroundColor;
    }

    protected class VRage_Game_ObjectBuilders_MySerializedTextPanelData\u003C\u003EScriptForegroundColor\u003C\u003EAccessor : IMemberAccessor<MySerializedTextPanelData, Color>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySerializedTextPanelData owner, in Color value) => owner.ScriptForegroundColor = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySerializedTextPanelData owner, out Color value) => value = owner.ScriptForegroundColor;
    }

    protected class VRage_Game_ObjectBuilders_MySerializedTextPanelData\u003C\u003ESprites\u003C\u003EAccessor : IMemberAccessor<MySerializedTextPanelData, MySerializableSpriteCollection>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MySerializedTextPanelData owner,
        in MySerializableSpriteCollection value)
      {
        owner.Sprites = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MySerializedTextPanelData owner,
        out MySerializableSpriteCollection value)
      {
        value = owner.Sprites;
      }
    }

    private class VRage_Game_ObjectBuilders_MySerializedTextPanelData\u003C\u003EActor : IActivator, IActivator<MySerializedTextPanelData>
    {
      object IActivator.CreateInstance() => (object) new MySerializedTextPanelData();

      MySerializedTextPanelData IActivator<MySerializedTextPanelData>.CreateInstance() => new MySerializedTextPanelData();
    }
  }
}
