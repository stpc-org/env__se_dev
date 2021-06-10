// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Components.BankingAndCurrency.MyObjectBuilder_Account
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

namespace VRage.Game.ObjectBuilders.Components.BankingAndCurrency
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_Account : MyObjectBuilder_Base
  {
    [ProtoMember(1)]
    public long OwnerIdentifier;
    [ProtoMember(3)]
    public long Balance;
    [ProtoMember(5)]
    public List<MyObjectBuilder_Account.MyObjectBuilder_AccountLogEntry> Log;

    [ProtoContract]
    public struct MyObjectBuilder_AccountLogEntry
    {
      [ProtoMember(1)]
      public long ChangeIdentifier;
      [ProtoMember(3)]
      public long Amount;
      [ProtoMember(5)]
      public long DateTime;

      protected class VRage_Game_ObjectBuilders_Components_BankingAndCurrency_MyObjectBuilder_Account\u003C\u003EMyObjectBuilder_AccountLogEntry\u003C\u003EChangeIdentifier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Account.MyObjectBuilder_AccountLogEntry, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_Account.MyObjectBuilder_AccountLogEntry owner,
          in long value)
        {
          owner.ChangeIdentifier = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_Account.MyObjectBuilder_AccountLogEntry owner,
          out long value)
        {
          value = owner.ChangeIdentifier;
        }
      }

      protected class VRage_Game_ObjectBuilders_Components_BankingAndCurrency_MyObjectBuilder_Account\u003C\u003EMyObjectBuilder_AccountLogEntry\u003C\u003EAmount\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Account.MyObjectBuilder_AccountLogEntry, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_Account.MyObjectBuilder_AccountLogEntry owner,
          in long value)
        {
          owner.Amount = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_Account.MyObjectBuilder_AccountLogEntry owner,
          out long value)
        {
          value = owner.Amount;
        }
      }

      protected class VRage_Game_ObjectBuilders_Components_BankingAndCurrency_MyObjectBuilder_Account\u003C\u003EMyObjectBuilder_AccountLogEntry\u003C\u003EDateTime\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Account.MyObjectBuilder_AccountLogEntry, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_Account.MyObjectBuilder_AccountLogEntry owner,
          in long value)
        {
          owner.DateTime = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_Account.MyObjectBuilder_AccountLogEntry owner,
          out long value)
        {
          value = owner.DateTime;
        }
      }

      private class VRage_Game_ObjectBuilders_Components_BankingAndCurrency_MyObjectBuilder_Account\u003C\u003EMyObjectBuilder_AccountLogEntry\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_Account.MyObjectBuilder_AccountLogEntry>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_Account.MyObjectBuilder_AccountLogEntry();

        MyObjectBuilder_Account.MyObjectBuilder_AccountLogEntry IActivator<MyObjectBuilder_Account.MyObjectBuilder_AccountLogEntry>.CreateInstance() => new MyObjectBuilder_Account.MyObjectBuilder_AccountLogEntry();
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_BankingAndCurrency_MyObjectBuilder_Account\u003C\u003EOwnerIdentifier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Account, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Account owner, in long value) => owner.OwnerIdentifier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Account owner, out long value) => value = owner.OwnerIdentifier;
    }

    protected class VRage_Game_ObjectBuilders_Components_BankingAndCurrency_MyObjectBuilder_Account\u003C\u003EBalance\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Account, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Account owner, in long value) => owner.Balance = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Account owner, out long value) => value = owner.Balance;
    }

    protected class VRage_Game_ObjectBuilders_Components_BankingAndCurrency_MyObjectBuilder_Account\u003C\u003ELog\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Account, List<MyObjectBuilder_Account.MyObjectBuilder_AccountLogEntry>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Account owner,
        in List<MyObjectBuilder_Account.MyObjectBuilder_AccountLogEntry> value)
      {
        owner.Log = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Account owner,
        out List<MyObjectBuilder_Account.MyObjectBuilder_AccountLogEntry> value)
      {
        value = owner.Log;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_BankingAndCurrency_MyObjectBuilder_Account\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Account, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Account owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Account owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_BankingAndCurrency_MyObjectBuilder_Account\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Account, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Account owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Account owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_BankingAndCurrency_MyObjectBuilder_Account\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Account, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Account owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Account owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_BankingAndCurrency_MyObjectBuilder_Account\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Account, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Account owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Account owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_Components_BankingAndCurrency_MyObjectBuilder_Account\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_Account>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_Account();

      MyObjectBuilder_Account IActivator<MyObjectBuilder_Account>.CreateInstance() => new MyObjectBuilder_Account();
    }
  }
}
