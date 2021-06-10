// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.MyObjectBuilder_CampaignSessionComponent
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders
{
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_CampaignSessionComponent : MyObjectBuilder_SessionComponent
  {
    public string CampaignName;
    public string ActiveState;
    public bool IsVanilla;
    public MyObjectBuilder_Checkpoint.ModItem Mod;
    public string LocalModFolder;
    public string CurrentOutcome;
    public bool CustomRespawnEnabled;

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_CampaignSessionComponent\u003C\u003ECampaignName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CampaignSessionComponent, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CampaignSessionComponent owner, in string value) => owner.CampaignName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CampaignSessionComponent owner, out string value) => value = owner.CampaignName;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_CampaignSessionComponent\u003C\u003EActiveState\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CampaignSessionComponent, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CampaignSessionComponent owner, in string value) => owner.ActiveState = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CampaignSessionComponent owner, out string value) => value = owner.ActiveState;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_CampaignSessionComponent\u003C\u003EIsVanilla\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CampaignSessionComponent, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CampaignSessionComponent owner, in bool value) => owner.IsVanilla = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CampaignSessionComponent owner, out bool value) => value = owner.IsVanilla;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_CampaignSessionComponent\u003C\u003EMod\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CampaignSessionComponent, MyObjectBuilder_Checkpoint.ModItem>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CampaignSessionComponent owner,
        in MyObjectBuilder_Checkpoint.ModItem value)
      {
        owner.Mod = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CampaignSessionComponent owner,
        out MyObjectBuilder_Checkpoint.ModItem value)
      {
        value = owner.Mod;
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_CampaignSessionComponent\u003C\u003ELocalModFolder\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CampaignSessionComponent, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CampaignSessionComponent owner, in string value) => owner.LocalModFolder = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CampaignSessionComponent owner, out string value) => value = owner.LocalModFolder;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_CampaignSessionComponent\u003C\u003ECurrentOutcome\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CampaignSessionComponent, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CampaignSessionComponent owner, in string value) => owner.CurrentOutcome = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CampaignSessionComponent owner, out string value) => value = owner.CurrentOutcome;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_CampaignSessionComponent\u003C\u003ECustomRespawnEnabled\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CampaignSessionComponent, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CampaignSessionComponent owner, in bool value) => owner.CustomRespawnEnabled = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CampaignSessionComponent owner, out bool value) => value = owner.CustomRespawnEnabled;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_CampaignSessionComponent\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CampaignSessionComponent, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CampaignSessionComponent owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CampaignSessionComponent owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_CampaignSessionComponent\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CampaignSessionComponent, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CampaignSessionComponent owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CampaignSessionComponent owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_CampaignSessionComponent\u003C\u003EDefinition\u003C\u003EAccessor : MyObjectBuilder_SessionComponent.VRage_Game_MyObjectBuilder_SessionComponent\u003C\u003EDefinition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CampaignSessionComponent, SerializableDefinitionId?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CampaignSessionComponent owner,
        in SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_SessionComponent&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CampaignSessionComponent owner,
        out SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_SessionComponent&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_CampaignSessionComponent\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CampaignSessionComponent, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CampaignSessionComponent owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CampaignSessionComponent owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_CampaignSessionComponent\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CampaignSessionComponent, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CampaignSessionComponent owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CampaignSessionComponent owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_MyObjectBuilder_CampaignSessionComponent\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_CampaignSessionComponent>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_CampaignSessionComponent();

      MyObjectBuilder_CampaignSessionComponent IActivator<MyObjectBuilder_CampaignSessionComponent>.CreateInstance() => new MyObjectBuilder_CampaignSessionComponent();
    }
  }
}
