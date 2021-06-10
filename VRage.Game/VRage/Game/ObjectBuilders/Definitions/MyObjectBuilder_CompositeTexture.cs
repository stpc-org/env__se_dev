// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Definitions.MyObjectBuilder_CompositeTexture
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Definitions
{
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_CompositeTexture : MyObjectBuilder_Base
  {
    public MyStringHash LeftTop = MyStringHash.NullOrEmpty;
    public MyStringHash LeftCenter = MyStringHash.NullOrEmpty;
    public MyStringHash LeftBottom = MyStringHash.NullOrEmpty;
    public MyStringHash CenterTop = MyStringHash.NullOrEmpty;
    public MyStringHash Center = MyStringHash.NullOrEmpty;
    public MyStringHash CenterBottom = MyStringHash.NullOrEmpty;
    public MyStringHash RightTop = MyStringHash.NullOrEmpty;
    public MyStringHash RightCenter = MyStringHash.NullOrEmpty;
    public MyStringHash RightBottom = MyStringHash.NullOrEmpty;

    public virtual bool IsValid() => this.LeftTop != MyStringHash.NullOrEmpty || this.LeftTop != MyStringHash.NullOrEmpty || (this.LeftCenter != MyStringHash.NullOrEmpty || this.LeftBottom != MyStringHash.NullOrEmpty) || (this.CenterTop != MyStringHash.NullOrEmpty || this.Center != MyStringHash.NullOrEmpty || (this.CenterBottom != MyStringHash.NullOrEmpty || this.RightTop != MyStringHash.NullOrEmpty)) || this.RightCenter != MyStringHash.NullOrEmpty || this.RightBottom != MyStringHash.NullOrEmpty;

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CompositeTexture\u003C\u003ELeftTop\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CompositeTexture, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CompositeTexture owner, in MyStringHash value) => owner.LeftTop = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CompositeTexture owner, out MyStringHash value) => value = owner.LeftTop;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CompositeTexture\u003C\u003ELeftCenter\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CompositeTexture, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CompositeTexture owner, in MyStringHash value) => owner.LeftCenter = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CompositeTexture owner, out MyStringHash value) => value = owner.LeftCenter;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CompositeTexture\u003C\u003ELeftBottom\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CompositeTexture, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CompositeTexture owner, in MyStringHash value) => owner.LeftBottom = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CompositeTexture owner, out MyStringHash value) => value = owner.LeftBottom;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CompositeTexture\u003C\u003ECenterTop\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CompositeTexture, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CompositeTexture owner, in MyStringHash value) => owner.CenterTop = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CompositeTexture owner, out MyStringHash value) => value = owner.CenterTop;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CompositeTexture\u003C\u003ECenter\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CompositeTexture, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CompositeTexture owner, in MyStringHash value) => owner.Center = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CompositeTexture owner, out MyStringHash value) => value = owner.Center;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CompositeTexture\u003C\u003ECenterBottom\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CompositeTexture, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CompositeTexture owner, in MyStringHash value) => owner.CenterBottom = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CompositeTexture owner, out MyStringHash value) => value = owner.CenterBottom;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CompositeTexture\u003C\u003ERightTop\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CompositeTexture, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CompositeTexture owner, in MyStringHash value) => owner.RightTop = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CompositeTexture owner, out MyStringHash value) => value = owner.RightTop;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CompositeTexture\u003C\u003ERightCenter\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CompositeTexture, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CompositeTexture owner, in MyStringHash value) => owner.RightCenter = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CompositeTexture owner, out MyStringHash value) => value = owner.RightCenter;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CompositeTexture\u003C\u003ERightBottom\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CompositeTexture, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CompositeTexture owner, in MyStringHash value) => owner.RightBottom = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CompositeTexture owner, out MyStringHash value) => value = owner.RightBottom;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CompositeTexture\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CompositeTexture, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CompositeTexture owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CompositeTexture owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CompositeTexture\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CompositeTexture, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CompositeTexture owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CompositeTexture owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CompositeTexture\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CompositeTexture, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CompositeTexture owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CompositeTexture owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CompositeTexture\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CompositeTexture, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CompositeTexture owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CompositeTexture owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CompositeTexture\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_CompositeTexture>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_CompositeTexture();

      MyObjectBuilder_CompositeTexture IActivator<MyObjectBuilder_CompositeTexture>.CreateInstance() => new MyObjectBuilder_CompositeTexture();
    }
  }
}
