// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Campaign.MyObjectBuilder_CampaignSM
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
  public class MyObjectBuilder_CampaignSM : MyObjectBuilder_Base
  {
    public string Name;
    public MyObjectBuilder_CampaignSMNode[] Nodes;
    public MyObjectBuilder_CampaignSMTransition[] Transitions;
    public int MaxLobbyPlayers;
    public int MaxLobbyPlayersExperimental;

    protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_CampaignSM\u003C\u003EName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CampaignSM, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CampaignSM owner, in string value) => owner.Name = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CampaignSM owner, out string value) => value = owner.Name;
    }

    protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_CampaignSM\u003C\u003ENodes\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CampaignSM, MyObjectBuilder_CampaignSMNode[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CampaignSM owner,
        in MyObjectBuilder_CampaignSMNode[] value)
      {
        owner.Nodes = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CampaignSM owner,
        out MyObjectBuilder_CampaignSMNode[] value)
      {
        value = owner.Nodes;
      }
    }

    protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_CampaignSM\u003C\u003ETransitions\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CampaignSM, MyObjectBuilder_CampaignSMTransition[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CampaignSM owner,
        in MyObjectBuilder_CampaignSMTransition[] value)
      {
        owner.Transitions = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CampaignSM owner,
        out MyObjectBuilder_CampaignSMTransition[] value)
      {
        value = owner.Transitions;
      }
    }

    protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_CampaignSM\u003C\u003EMaxLobbyPlayers\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CampaignSM, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CampaignSM owner, in int value) => owner.MaxLobbyPlayers = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CampaignSM owner, out int value) => value = owner.MaxLobbyPlayers;
    }

    protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_CampaignSM\u003C\u003EMaxLobbyPlayersExperimental\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CampaignSM, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CampaignSM owner, in int value) => owner.MaxLobbyPlayersExperimental = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CampaignSM owner, out int value) => value = owner.MaxLobbyPlayersExperimental;
    }

    protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_CampaignSM\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CampaignSM, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CampaignSM owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CampaignSM owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_CampaignSM\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CampaignSM, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CampaignSM owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CampaignSM owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_CampaignSM\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CampaignSM, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CampaignSM owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CampaignSM owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_CampaignSM\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CampaignSM, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CampaignSM owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CampaignSM owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_CampaignSM\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_CampaignSM>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_CampaignSM();

      MyObjectBuilder_CampaignSM IActivator<MyObjectBuilder_CampaignSM>.CreateInstance() => new MyObjectBuilder_CampaignSM();
    }
  }
}
