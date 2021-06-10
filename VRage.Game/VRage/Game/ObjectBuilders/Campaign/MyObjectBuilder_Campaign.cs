// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Campaign.MyObjectBuilder_Campaign
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Campaign
{
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_Campaign : MyObjectBuilder_Base
  {
    public MyObjectBuilder_CampaignSM StateMachine;
    [XmlArrayItem("Platform")]
    public MyObjectBuilder_Campaign.MySupportedPlatform[] SupportedPlatforms;
    [XmlArrayItem("StateMachine")]
    public MyObjectBuilder_CampaignSM[] StateMachines;
    [XmlArrayItem("Path")]
    public List<string> LocalizationPaths = new List<string>();
    [XmlArrayItem("Language")]
    public List<string> LocalizationLanguages = new List<string>();
    public string DefaultLocalizationLanguage;
    public string DescriptionLocalizationFile;
    public string CampaignPath;
    public string Name;
    public bool ForceDisableTrashRemoval;
    public string Description;
    public string ImagePath;
    public bool IsMultiplayer;
    public string Difficulty;
    public string Author;
    public int Order;
    public int MaxPlayers = 16;
    public string DLC;
    public bool IsOfflineEnabled = true;
    [XmlIgnore]
    public bool IsVanilla = true;
    [XmlIgnore]
    public bool IsLocalMod = true;
    [XmlIgnore]
    public string ModFolderPath;
    [XmlIgnore]
    public ulong PublishedFileId;
    [XmlIgnore]
    public string PublishedServiceName;
    [XmlIgnore]
    public bool IsDebug;

    public MyObjectBuilder_CampaignSM GetStateMachine(
      string platform = null,
      bool platformMandatory = false)
    {
      MyObjectBuilder_CampaignSM builderCampaignSm1 = this.StateMachine;
      if (this.SupportedPlatforms != null && this.StateMachines != null)
      {
        MyObjectBuilder_Campaign.MySupportedPlatform supportedPlatform = ((IEnumerable<MyObjectBuilder_Campaign.MySupportedPlatform>) this.SupportedPlatforms).FirstOrDefault<MyObjectBuilder_Campaign.MySupportedPlatform>((Func<MyObjectBuilder_Campaign.MySupportedPlatform, bool>) (x => x.Name == platform));
        MyObjectBuilder_CampaignSM builderCampaignSm2 = ((IEnumerable<MyObjectBuilder_CampaignSM>) this.StateMachines).FirstOrDefault<MyObjectBuilder_CampaignSM>((Func<MyObjectBuilder_CampaignSM, bool>) (x => x.Name == supportedPlatform.StateMachine));
        if (builderCampaignSm2 != null)
        {
          builderCampaignSm1 = builderCampaignSm2;
        }
        else
        {
          if (platformMandatory)
            return (MyObjectBuilder_CampaignSM) null;
          if (this.StateMachines.Length != 0)
            builderCampaignSm1 = this.StateMachines[0];
        }
      }
      return builderCampaignSm1;
    }

    public struct MySupportedPlatform
    {
      public string Name;
      public string StateMachine;
    }

    protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign\u003C\u003EStateMachine\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Campaign, MyObjectBuilder_CampaignSM>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Campaign owner,
        in MyObjectBuilder_CampaignSM value)
      {
        owner.StateMachine = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Campaign owner,
        out MyObjectBuilder_CampaignSM value)
      {
        value = owner.StateMachine;
      }
    }

    protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign\u003C\u003ESupportedPlatforms\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Campaign, MyObjectBuilder_Campaign.MySupportedPlatform[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Campaign owner,
        in MyObjectBuilder_Campaign.MySupportedPlatform[] value)
      {
        owner.SupportedPlatforms = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Campaign owner,
        out MyObjectBuilder_Campaign.MySupportedPlatform[] value)
      {
        value = owner.SupportedPlatforms;
      }
    }

    protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign\u003C\u003EStateMachines\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Campaign, MyObjectBuilder_CampaignSM[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Campaign owner,
        in MyObjectBuilder_CampaignSM[] value)
      {
        owner.StateMachines = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Campaign owner,
        out MyObjectBuilder_CampaignSM[] value)
      {
        value = owner.StateMachines;
      }
    }

    protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign\u003C\u003ELocalizationPaths\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Campaign, List<string>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Campaign owner, in List<string> value) => owner.LocalizationPaths = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Campaign owner, out List<string> value) => value = owner.LocalizationPaths;
    }

    protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign\u003C\u003ELocalizationLanguages\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Campaign, List<string>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Campaign owner, in List<string> value) => owner.LocalizationLanguages = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Campaign owner, out List<string> value) => value = owner.LocalizationLanguages;
    }

    protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign\u003C\u003EDefaultLocalizationLanguage\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Campaign, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Campaign owner, in string value) => owner.DefaultLocalizationLanguage = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Campaign owner, out string value) => value = owner.DefaultLocalizationLanguage;
    }

    protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign\u003C\u003EDescriptionLocalizationFile\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Campaign, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Campaign owner, in string value) => owner.DescriptionLocalizationFile = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Campaign owner, out string value) => value = owner.DescriptionLocalizationFile;
    }

    protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign\u003C\u003ECampaignPath\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Campaign, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Campaign owner, in string value) => owner.CampaignPath = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Campaign owner, out string value) => value = owner.CampaignPath;
    }

    protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign\u003C\u003EName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Campaign, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Campaign owner, in string value) => owner.Name = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Campaign owner, out string value) => value = owner.Name;
    }

    protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign\u003C\u003EForceDisableTrashRemoval\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Campaign, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Campaign owner, in bool value) => owner.ForceDisableTrashRemoval = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Campaign owner, out bool value) => value = owner.ForceDisableTrashRemoval;
    }

    protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign\u003C\u003EDescription\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Campaign, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Campaign owner, in string value) => owner.Description = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Campaign owner, out string value) => value = owner.Description;
    }

    protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign\u003C\u003EImagePath\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Campaign, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Campaign owner, in string value) => owner.ImagePath = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Campaign owner, out string value) => value = owner.ImagePath;
    }

    protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign\u003C\u003EIsMultiplayer\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Campaign, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Campaign owner, in bool value) => owner.IsMultiplayer = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Campaign owner, out bool value) => value = owner.IsMultiplayer;
    }

    protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign\u003C\u003EDifficulty\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Campaign, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Campaign owner, in string value) => owner.Difficulty = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Campaign owner, out string value) => value = owner.Difficulty;
    }

    protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign\u003C\u003EAuthor\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Campaign, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Campaign owner, in string value) => owner.Author = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Campaign owner, out string value) => value = owner.Author;
    }

    protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign\u003C\u003EOrder\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Campaign, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Campaign owner, in int value) => owner.Order = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Campaign owner, out int value) => value = owner.Order;
    }

    protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign\u003C\u003EMaxPlayers\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Campaign, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Campaign owner, in int value) => owner.MaxPlayers = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Campaign owner, out int value) => value = owner.MaxPlayers;
    }

    protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign\u003C\u003EDLC\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Campaign, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Campaign owner, in string value) => owner.DLC = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Campaign owner, out string value) => value = owner.DLC;
    }

    protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign\u003C\u003EIsOfflineEnabled\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Campaign, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Campaign owner, in bool value) => owner.IsOfflineEnabled = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Campaign owner, out bool value) => value = owner.IsOfflineEnabled;
    }

    protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign\u003C\u003EIsVanilla\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Campaign, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Campaign owner, in bool value) => owner.IsVanilla = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Campaign owner, out bool value) => value = owner.IsVanilla;
    }

    protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign\u003C\u003EIsLocalMod\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Campaign, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Campaign owner, in bool value) => owner.IsLocalMod = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Campaign owner, out bool value) => value = owner.IsLocalMod;
    }

    protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign\u003C\u003EModFolderPath\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Campaign, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Campaign owner, in string value) => owner.ModFolderPath = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Campaign owner, out string value) => value = owner.ModFolderPath;
    }

    protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign\u003C\u003EPublishedFileId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Campaign, ulong>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Campaign owner, in ulong value) => owner.PublishedFileId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Campaign owner, out ulong value) => value = owner.PublishedFileId;
    }

    protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign\u003C\u003EPublishedServiceName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Campaign, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Campaign owner, in string value) => owner.PublishedServiceName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Campaign owner, out string value) => value = owner.PublishedServiceName;
    }

    protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign\u003C\u003EIsDebug\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Campaign, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Campaign owner, in bool value) => owner.IsDebug = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Campaign owner, out bool value) => value = owner.IsDebug;
    }

    protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Campaign, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Campaign owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Campaign owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Campaign, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Campaign owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Campaign owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Campaign, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Campaign owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Campaign owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Campaign, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Campaign owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Campaign owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_Campaign\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_Campaign>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_Campaign();

      MyObjectBuilder_Campaign IActivator<MyObjectBuilder_Campaign>.CreateInstance() => new MyObjectBuilder_Campaign();
    }
  }
}
