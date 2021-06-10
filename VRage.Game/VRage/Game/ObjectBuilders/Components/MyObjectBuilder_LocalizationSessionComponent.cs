// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Components.MyObjectBuilder_LocalizationSessionComponent
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Components
{
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_LocalizationSessionComponent : MyObjectBuilder_SessionComponent
  {
    public List<string> AdditionalPaths = new List<string>();
    public List<string> CampaignPaths = new List<string>();
    public string CampaignModFolderName = string.Empty;
    public string Language = "English";

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_LocalizationSessionComponent\u003C\u003EAdditionalPaths\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_LocalizationSessionComponent, List<string>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_LocalizationSessionComponent owner,
        in List<string> value)
      {
        owner.AdditionalPaths = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_LocalizationSessionComponent owner,
        out List<string> value)
      {
        value = owner.AdditionalPaths;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_LocalizationSessionComponent\u003C\u003ECampaignPaths\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_LocalizationSessionComponent, List<string>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_LocalizationSessionComponent owner,
        in List<string> value)
      {
        owner.CampaignPaths = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_LocalizationSessionComponent owner,
        out List<string> value)
      {
        value = owner.CampaignPaths;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_LocalizationSessionComponent\u003C\u003ECampaignModFolderName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_LocalizationSessionComponent, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_LocalizationSessionComponent owner,
        in string value)
      {
        owner.CampaignModFolderName = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_LocalizationSessionComponent owner,
        out string value)
      {
        value = owner.CampaignModFolderName;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_LocalizationSessionComponent\u003C\u003ELanguage\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_LocalizationSessionComponent, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_LocalizationSessionComponent owner,
        in string value)
      {
        owner.Language = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_LocalizationSessionComponent owner,
        out string value)
      {
        value = owner.Language;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_LocalizationSessionComponent\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_LocalizationSessionComponent, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_LocalizationSessionComponent owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_LocalizationSessionComponent owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_LocalizationSessionComponent\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_LocalizationSessionComponent, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_LocalizationSessionComponent owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_LocalizationSessionComponent owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_LocalizationSessionComponent\u003C\u003EDefinition\u003C\u003EAccessor : MyObjectBuilder_SessionComponent.VRage_Game_MyObjectBuilder_SessionComponent\u003C\u003EDefinition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_LocalizationSessionComponent, SerializableDefinitionId?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_LocalizationSessionComponent owner,
        in SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_SessionComponent&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_LocalizationSessionComponent owner,
        out SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_SessionComponent&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_LocalizationSessionComponent\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_LocalizationSessionComponent, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_LocalizationSessionComponent owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_LocalizationSessionComponent owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_LocalizationSessionComponent\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_LocalizationSessionComponent, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_LocalizationSessionComponent owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_LocalizationSessionComponent owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    private class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_LocalizationSessionComponent\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_LocalizationSessionComponent>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_LocalizationSessionComponent();

      MyObjectBuilder_LocalizationSessionComponent IActivator<MyObjectBuilder_LocalizationSessionComponent>.CreateInstance() => new MyObjectBuilder_LocalizationSessionComponent();
    }
  }
}
