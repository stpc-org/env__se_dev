// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_PreloadFileInfo
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_PreloadFileInfo : MyObjectBuilder_Base
  {
    [ProtoMember(4)]
    public string Name;
    [ProtoMember(7)]
    [DefaultValue(false)]
    public bool LoadOnDedicated;
    [ProtoMember(10)]
    public MyObjectBuilder_PreloadFileInfo.PreloadType Type;

    [Flags]
    public enum PreloadType
    {
      MainMenu = 1,
      SessionPreload = 2,
    }

    protected class VRage_Game_MyObjectBuilder_PreloadFileInfo\u003C\u003EName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PreloadFileInfo, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PreloadFileInfo owner, in string value) => owner.Name = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PreloadFileInfo owner, out string value) => value = owner.Name;
    }

    protected class VRage_Game_MyObjectBuilder_PreloadFileInfo\u003C\u003ELoadOnDedicated\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PreloadFileInfo, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PreloadFileInfo owner, in bool value) => owner.LoadOnDedicated = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PreloadFileInfo owner, out bool value) => value = owner.LoadOnDedicated;
    }

    protected class VRage_Game_MyObjectBuilder_PreloadFileInfo\u003C\u003EType\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PreloadFileInfo, MyObjectBuilder_PreloadFileInfo.PreloadType>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PreloadFileInfo owner,
        in MyObjectBuilder_PreloadFileInfo.PreloadType value)
      {
        owner.Type = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PreloadFileInfo owner,
        out MyObjectBuilder_PreloadFileInfo.PreloadType value)
      {
        value = owner.Type;
      }
    }

    protected class VRage_Game_MyObjectBuilder_PreloadFileInfo\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PreloadFileInfo, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PreloadFileInfo owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PreloadFileInfo owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PreloadFileInfo\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PreloadFileInfo, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PreloadFileInfo owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PreloadFileInfo owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PreloadFileInfo\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PreloadFileInfo, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PreloadFileInfo owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PreloadFileInfo owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PreloadFileInfo\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PreloadFileInfo, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PreloadFileInfo owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PreloadFileInfo owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_PreloadFileInfo\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_PreloadFileInfo>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_PreloadFileInfo();

      MyObjectBuilder_PreloadFileInfo IActivator<MyObjectBuilder_PreloadFileInfo>.CreateInstance() => new MyObjectBuilder_PreloadFileInfo();
    }
  }
}
