// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Campaign.MyObjectBuilder_CampaignSMNode
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
  public class MyObjectBuilder_CampaignSMNode : MyObjectBuilder_Base
  {
    public string Name;
    public string SaveFilePath;
    public SerializableVector2 Location;

    protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_CampaignSMNode\u003C\u003EName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CampaignSMNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CampaignSMNode owner, in string value) => owner.Name = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CampaignSMNode owner, out string value) => value = owner.Name;
    }

    protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_CampaignSMNode\u003C\u003ESaveFilePath\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CampaignSMNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CampaignSMNode owner, in string value) => owner.SaveFilePath = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CampaignSMNode owner, out string value) => value = owner.SaveFilePath;
    }

    protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_CampaignSMNode\u003C\u003ELocation\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CampaignSMNode, SerializableVector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CampaignSMNode owner,
        in SerializableVector2 value)
      {
        owner.Location = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CampaignSMNode owner,
        out SerializableVector2 value)
      {
        value = owner.Location;
      }
    }

    protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_CampaignSMNode\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CampaignSMNode, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CampaignSMNode owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CampaignSMNode owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_CampaignSMNode\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CampaignSMNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CampaignSMNode owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CampaignSMNode owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_CampaignSMNode\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CampaignSMNode, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CampaignSMNode owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CampaignSMNode owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_CampaignSMNode\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CampaignSMNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CampaignSMNode owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CampaignSMNode owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_CampaignSMNode\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_CampaignSMNode>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_CampaignSMNode();

      MyObjectBuilder_CampaignSMNode IActivator<MyObjectBuilder_CampaignSMNode>.CreateInstance() => new MyObjectBuilder_CampaignSMNode();
    }
  }
}
