// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Campaign.MyObjectBuilder_CampaignSMTransition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Campaign
{
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_CampaignSMTransition : MyObjectBuilder_Base
  {
    public string Name;
    public string From;
    public string To;

    protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_CampaignSMTransition\u003C\u003EName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CampaignSMTransition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CampaignSMTransition owner, in string value) => owner.Name = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CampaignSMTransition owner, out string value) => value = owner.Name;
    }

    protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_CampaignSMTransition\u003C\u003EFrom\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CampaignSMTransition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CampaignSMTransition owner, in string value) => owner.From = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CampaignSMTransition owner, out string value) => value = owner.From;
    }

    protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_CampaignSMTransition\u003C\u003ETo\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CampaignSMTransition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CampaignSMTransition owner, in string value) => owner.To = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CampaignSMTransition owner, out string value) => value = owner.To;
    }

    protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_CampaignSMTransition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CampaignSMTransition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CampaignSMTransition owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CampaignSMTransition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_CampaignSMTransition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CampaignSMTransition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CampaignSMTransition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CampaignSMTransition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_CampaignSMTransition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CampaignSMTransition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CampaignSMTransition owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CampaignSMTransition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_CampaignSMTransition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CampaignSMTransition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CampaignSMTransition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CampaignSMTransition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_CampaignSMTransition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_CampaignSMTransition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_CampaignSMTransition();

      MyObjectBuilder_CampaignSMTransition IActivator<MyObjectBuilder_CampaignSMTransition>.CreateInstance() => new MyObjectBuilder_CampaignSMTransition();
    }
  }
}
