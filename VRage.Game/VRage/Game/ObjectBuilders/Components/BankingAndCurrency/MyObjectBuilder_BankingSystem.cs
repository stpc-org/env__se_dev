// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Components.BankingAndCurrency.MyObjectBuilder_BankingSystem
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
  public class MyObjectBuilder_BankingSystem : MyObjectBuilder_SessionComponent
  {
    [ProtoMember(1)]
    public List<MyObjectBuilder_BankingSystem.MyObjectBuilder_AccountEntry> Accounts;
    [ProtoMember(3)]
    public long OverallBalance;

    [ProtoContract]
    public struct MyObjectBuilder_AccountEntry
    {
      [ProtoMember(1)]
      public long OwnerIdentifier;
      [ProtoMember(3)]
      public MyObjectBuilder_Account Account;

      protected class VRage_Game_ObjectBuilders_Components_BankingAndCurrency_MyObjectBuilder_BankingSystem\u003C\u003EMyObjectBuilder_AccountEntry\u003C\u003EOwnerIdentifier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BankingSystem.MyObjectBuilder_AccountEntry, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_BankingSystem.MyObjectBuilder_AccountEntry owner,
          in long value)
        {
          owner.OwnerIdentifier = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_BankingSystem.MyObjectBuilder_AccountEntry owner,
          out long value)
        {
          value = owner.OwnerIdentifier;
        }
      }

      protected class VRage_Game_ObjectBuilders_Components_BankingAndCurrency_MyObjectBuilder_BankingSystem\u003C\u003EMyObjectBuilder_AccountEntry\u003C\u003EAccount\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BankingSystem.MyObjectBuilder_AccountEntry, MyObjectBuilder_Account>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_BankingSystem.MyObjectBuilder_AccountEntry owner,
          in MyObjectBuilder_Account value)
        {
          owner.Account = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_BankingSystem.MyObjectBuilder_AccountEntry owner,
          out MyObjectBuilder_Account value)
        {
          value = owner.Account;
        }
      }

      private class VRage_Game_ObjectBuilders_Components_BankingAndCurrency_MyObjectBuilder_BankingSystem\u003C\u003EMyObjectBuilder_AccountEntry\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_BankingSystem.MyObjectBuilder_AccountEntry>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_BankingSystem.MyObjectBuilder_AccountEntry();

        MyObjectBuilder_BankingSystem.MyObjectBuilder_AccountEntry IActivator<MyObjectBuilder_BankingSystem.MyObjectBuilder_AccountEntry>.CreateInstance() => new MyObjectBuilder_BankingSystem.MyObjectBuilder_AccountEntry();
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_BankingAndCurrency_MyObjectBuilder_BankingSystem\u003C\u003EAccounts\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BankingSystem, List<MyObjectBuilder_BankingSystem.MyObjectBuilder_AccountEntry>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_BankingSystem owner,
        in List<MyObjectBuilder_BankingSystem.MyObjectBuilder_AccountEntry> value)
      {
        owner.Accounts = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_BankingSystem owner,
        out List<MyObjectBuilder_BankingSystem.MyObjectBuilder_AccountEntry> value)
      {
        value = owner.Accounts;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_BankingAndCurrency_MyObjectBuilder_BankingSystem\u003C\u003EOverallBalance\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BankingSystem, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_BankingSystem owner, in long value) => owner.OverallBalance = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_BankingSystem owner, out long value) => value = owner.OverallBalance;
    }

    protected class VRage_Game_ObjectBuilders_Components_BankingAndCurrency_MyObjectBuilder_BankingSystem\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BankingSystem, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_BankingSystem owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_BankingSystem owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_BankingAndCurrency_MyObjectBuilder_BankingSystem\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BankingSystem, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_BankingSystem owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_BankingSystem owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_BankingAndCurrency_MyObjectBuilder_BankingSystem\u003C\u003EDefinition\u003C\u003EAccessor : MyObjectBuilder_SessionComponent.VRage_Game_MyObjectBuilder_SessionComponent\u003C\u003EDefinition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BankingSystem, SerializableDefinitionId?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_BankingSystem owner,
        in SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_SessionComponent&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_BankingSystem owner,
        out SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_SessionComponent&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_BankingAndCurrency_MyObjectBuilder_BankingSystem\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BankingSystem, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_BankingSystem owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_BankingSystem owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_BankingAndCurrency_MyObjectBuilder_BankingSystem\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BankingSystem, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_BankingSystem owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_BankingSystem owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_Components_BankingAndCurrency_MyObjectBuilder_BankingSystem\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_BankingSystem>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_BankingSystem();

      MyObjectBuilder_BankingSystem IActivator<MyObjectBuilder_BankingSystem>.CreateInstance() => new MyObjectBuilder_BankingSystem();
    }
  }
}
