// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Components.MyObjectBuilder_SessionComponentEconomy
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilder;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Components
{
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_SessionComponentEconomy : MyObjectBuilder_SessionComponent
  {
    public bool GenerateFactionsOnStart = true;
    public long AnalysisTotalCurrency;
    public long AnalysisCurrencyFaucet;
    public long AnalysisCurrencySink;
    public long CurrencyGeneratedThisTick;
    public long CurrencyDestroyedThisTick;
    public MySerializableList<MyObjectBuilder_SessionComponentEconomy.MyIdBalancePair> AnalysisPerPlayerCurrency;
    public MySerializableList<MyObjectBuilder_SessionComponentEconomy.MyIdBalancePair> AnalysisPerFactionCurrency;

    public struct MyIdBalancePair
    {
      public long Id;
      public long Balance;
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentEconomy\u003C\u003EGenerateFactionsOnStart\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionComponentEconomy, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionComponentEconomy owner, in bool value) => owner.GenerateFactionsOnStart = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionComponentEconomy owner, out bool value) => value = owner.GenerateFactionsOnStart;
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentEconomy\u003C\u003EAnalysisTotalCurrency\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionComponentEconomy, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionComponentEconomy owner, in long value) => owner.AnalysisTotalCurrency = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionComponentEconomy owner, out long value) => value = owner.AnalysisTotalCurrency;
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentEconomy\u003C\u003EAnalysisCurrencyFaucet\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionComponentEconomy, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionComponentEconomy owner, in long value) => owner.AnalysisCurrencyFaucet = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionComponentEconomy owner, out long value) => value = owner.AnalysisCurrencyFaucet;
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentEconomy\u003C\u003EAnalysisCurrencySink\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionComponentEconomy, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionComponentEconomy owner, in long value) => owner.AnalysisCurrencySink = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionComponentEconomy owner, out long value) => value = owner.AnalysisCurrencySink;
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentEconomy\u003C\u003ECurrencyGeneratedThisTick\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionComponentEconomy, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionComponentEconomy owner, in long value) => owner.CurrencyGeneratedThisTick = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionComponentEconomy owner, out long value) => value = owner.CurrencyGeneratedThisTick;
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentEconomy\u003C\u003ECurrencyDestroyedThisTick\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionComponentEconomy, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionComponentEconomy owner, in long value) => owner.CurrencyDestroyedThisTick = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionComponentEconomy owner, out long value) => value = owner.CurrencyDestroyedThisTick;
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentEconomy\u003C\u003EAnalysisPerPlayerCurrency\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionComponentEconomy, MySerializableList<MyObjectBuilder_SessionComponentEconomy.MyIdBalancePair>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SessionComponentEconomy owner,
        in MySerializableList<MyObjectBuilder_SessionComponentEconomy.MyIdBalancePair> value)
      {
        owner.AnalysisPerPlayerCurrency = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SessionComponentEconomy owner,
        out MySerializableList<MyObjectBuilder_SessionComponentEconomy.MyIdBalancePair> value)
      {
        value = owner.AnalysisPerPlayerCurrency;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentEconomy\u003C\u003EAnalysisPerFactionCurrency\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionComponentEconomy, MySerializableList<MyObjectBuilder_SessionComponentEconomy.MyIdBalancePair>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SessionComponentEconomy owner,
        in MySerializableList<MyObjectBuilder_SessionComponentEconomy.MyIdBalancePair> value)
      {
        owner.AnalysisPerFactionCurrency = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SessionComponentEconomy owner,
        out MySerializableList<MyObjectBuilder_SessionComponentEconomy.MyIdBalancePair> value)
      {
        value = owner.AnalysisPerFactionCurrency;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentEconomy\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SessionComponentEconomy, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SessionComponentEconomy owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SessionComponentEconomy owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentEconomy\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SessionComponentEconomy, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionComponentEconomy owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionComponentEconomy owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentEconomy\u003C\u003EDefinition\u003C\u003EAccessor : MyObjectBuilder_SessionComponent.VRage_Game_MyObjectBuilder_SessionComponent\u003C\u003EDefinition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SessionComponentEconomy, SerializableDefinitionId?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SessionComponentEconomy owner,
        in SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_SessionComponent&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SessionComponentEconomy owner,
        out SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_SessionComponent&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentEconomy\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SessionComponentEconomy, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SessionComponentEconomy owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SessionComponentEconomy owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentEconomy\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SessionComponentEconomy, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionComponentEconomy owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionComponentEconomy owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentEconomy\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_SessionComponentEconomy>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_SessionComponentEconomy();

      MyObjectBuilder_SessionComponentEconomy IActivator<MyObjectBuilder_SessionComponentEconomy>.CreateInstance() => new MyObjectBuilder_SessionComponentEconomy();
    }
  }
}
