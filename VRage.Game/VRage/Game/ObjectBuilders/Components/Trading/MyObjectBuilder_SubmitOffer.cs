// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Components.Trading.MyObjectBuilder_SubmitOffer
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Components.Trading
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_SubmitOffer : MyObjectBuilder_Base
  {
    public List<MyObjectBuilder_InventoryItem> InventoryItems;
    public long CurrencyAmount;
    public int PCUAmount;

    protected class VRage_Game_ObjectBuilders_Components_Trading_MyObjectBuilder_SubmitOffer\u003C\u003EInventoryItems\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SubmitOffer, List<MyObjectBuilder_InventoryItem>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SubmitOffer owner,
        in List<MyObjectBuilder_InventoryItem> value)
      {
        owner.InventoryItems = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SubmitOffer owner,
        out List<MyObjectBuilder_InventoryItem> value)
      {
        value = owner.InventoryItems;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_Trading_MyObjectBuilder_SubmitOffer\u003C\u003ECurrencyAmount\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SubmitOffer, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SubmitOffer owner, in long value) => owner.CurrencyAmount = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SubmitOffer owner, out long value) => value = owner.CurrencyAmount;
    }

    protected class VRage_Game_ObjectBuilders_Components_Trading_MyObjectBuilder_SubmitOffer\u003C\u003EPCUAmount\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SubmitOffer, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SubmitOffer owner, in int value) => owner.PCUAmount = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SubmitOffer owner, out int value) => value = owner.PCUAmount;
    }

    protected class VRage_Game_ObjectBuilders_Components_Trading_MyObjectBuilder_SubmitOffer\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SubmitOffer, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SubmitOffer owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SubmitOffer owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Trading_MyObjectBuilder_SubmitOffer\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SubmitOffer, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SubmitOffer owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SubmitOffer owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Trading_MyObjectBuilder_SubmitOffer\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SubmitOffer, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SubmitOffer owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SubmitOffer owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Trading_MyObjectBuilder_SubmitOffer\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SubmitOffer, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SubmitOffer owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SubmitOffer owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_Components_Trading_MyObjectBuilder_SubmitOffer\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_SubmitOffer>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_SubmitOffer();

      MyObjectBuilder_SubmitOffer IActivator<MyObjectBuilder_SubmitOffer>.CreateInstance() => new MyObjectBuilder_SubmitOffer();
    }
  }
}
