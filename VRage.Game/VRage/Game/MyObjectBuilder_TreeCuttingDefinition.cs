// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_TreeCuttingDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_TreeCuttingDefinition : MyObjectBuilder_CuttingDefinition
  {
    protected class VRage_Game_MyObjectBuilder_TreeCuttingDefinition\u003C\u003EEntityId\u003C\u003EAccessor : MyObjectBuilder_CuttingDefinition.VRage_Game_MyObjectBuilder_CuttingDefinition\u003C\u003EEntityId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TreeCuttingDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TreeCuttingDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CuttingDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TreeCuttingDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CuttingDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_TreeCuttingDefinition\u003C\u003EScrapWoodBranchesId\u003C\u003EAccessor : MyObjectBuilder_CuttingDefinition.VRage_Game_MyObjectBuilder_CuttingDefinition\u003C\u003EScrapWoodBranchesId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TreeCuttingDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TreeCuttingDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CuttingDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TreeCuttingDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CuttingDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_TreeCuttingDefinition\u003C\u003EScrapWoodId\u003C\u003EAccessor : MyObjectBuilder_CuttingDefinition.VRage_Game_MyObjectBuilder_CuttingDefinition\u003C\u003EScrapWoodId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TreeCuttingDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TreeCuttingDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CuttingDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TreeCuttingDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CuttingDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_TreeCuttingDefinition\u003C\u003EScrapWoodAmountMin\u003C\u003EAccessor : MyObjectBuilder_CuttingDefinition.VRage_Game_MyObjectBuilder_CuttingDefinition\u003C\u003EScrapWoodAmountMin\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TreeCuttingDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TreeCuttingDefinition owner, in int value) => this.Set((MyObjectBuilder_CuttingDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TreeCuttingDefinition owner, out int value) => this.Get((MyObjectBuilder_CuttingDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TreeCuttingDefinition\u003C\u003EScrapWoodAmountMax\u003C\u003EAccessor : MyObjectBuilder_CuttingDefinition.VRage_Game_MyObjectBuilder_CuttingDefinition\u003C\u003EScrapWoodAmountMax\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TreeCuttingDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TreeCuttingDefinition owner, in int value) => this.Set((MyObjectBuilder_CuttingDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TreeCuttingDefinition owner, out int value) => this.Get((MyObjectBuilder_CuttingDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TreeCuttingDefinition\u003C\u003ECraftingScrapWoodAmountMin\u003C\u003EAccessor : MyObjectBuilder_CuttingDefinition.VRage_Game_MyObjectBuilder_CuttingDefinition\u003C\u003ECraftingScrapWoodAmountMin\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TreeCuttingDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TreeCuttingDefinition owner, in int value) => this.Set((MyObjectBuilder_CuttingDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TreeCuttingDefinition owner, out int value) => this.Get((MyObjectBuilder_CuttingDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TreeCuttingDefinition\u003C\u003ECraftingScrapWoodAmountMax\u003C\u003EAccessor : MyObjectBuilder_CuttingDefinition.VRage_Game_MyObjectBuilder_CuttingDefinition\u003C\u003ECraftingScrapWoodAmountMax\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TreeCuttingDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TreeCuttingDefinition owner, in int value) => this.Set((MyObjectBuilder_CuttingDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TreeCuttingDefinition owner, out int value) => this.Get((MyObjectBuilder_CuttingDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TreeCuttingDefinition\u003C\u003ECuttingPrefabs\u003C\u003EAccessor : MyObjectBuilder_CuttingDefinition.VRage_Game_MyObjectBuilder_CuttingDefinition\u003C\u003ECuttingPrefabs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TreeCuttingDefinition, MyObjectBuilder_CuttingDefinition.MyCuttingPrefab[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TreeCuttingDefinition owner,
        in MyObjectBuilder_CuttingDefinition.MyCuttingPrefab[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CuttingDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TreeCuttingDefinition owner,
        out MyObjectBuilder_CuttingDefinition.MyCuttingPrefab[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CuttingDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_TreeCuttingDefinition\u003C\u003EDestroySourceAfterCrafting\u003C\u003EAccessor : MyObjectBuilder_CuttingDefinition.VRage_Game_MyObjectBuilder_CuttingDefinition\u003C\u003EDestroySourceAfterCrafting\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TreeCuttingDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TreeCuttingDefinition owner, in bool value) => this.Set((MyObjectBuilder_CuttingDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TreeCuttingDefinition owner, out bool value) => this.Get((MyObjectBuilder_CuttingDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TreeCuttingDefinition\u003C\u003ECraftingTimeInSeconds\u003C\u003EAccessor : MyObjectBuilder_CuttingDefinition.VRage_Game_MyObjectBuilder_CuttingDefinition\u003C\u003ECraftingTimeInSeconds\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TreeCuttingDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TreeCuttingDefinition owner, in float value) => this.Set((MyObjectBuilder_CuttingDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TreeCuttingDefinition owner, out float value) => this.Get((MyObjectBuilder_CuttingDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TreeCuttingDefinition\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TreeCuttingDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TreeCuttingDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TreeCuttingDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_TreeCuttingDefinition\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TreeCuttingDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TreeCuttingDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TreeCuttingDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TreeCuttingDefinition\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TreeCuttingDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TreeCuttingDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TreeCuttingDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TreeCuttingDefinition\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TreeCuttingDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TreeCuttingDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TreeCuttingDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TreeCuttingDefinition\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TreeCuttingDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TreeCuttingDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TreeCuttingDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TreeCuttingDefinition\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TreeCuttingDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TreeCuttingDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TreeCuttingDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TreeCuttingDefinition\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TreeCuttingDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TreeCuttingDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TreeCuttingDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TreeCuttingDefinition\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TreeCuttingDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TreeCuttingDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TreeCuttingDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TreeCuttingDefinition\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TreeCuttingDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TreeCuttingDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TreeCuttingDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TreeCuttingDefinition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TreeCuttingDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TreeCuttingDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TreeCuttingDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_TreeCuttingDefinition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TreeCuttingDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TreeCuttingDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TreeCuttingDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TreeCuttingDefinition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TreeCuttingDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TreeCuttingDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TreeCuttingDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_TreeCuttingDefinition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TreeCuttingDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TreeCuttingDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TreeCuttingDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_TreeCuttingDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_TreeCuttingDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_TreeCuttingDefinition();

      MyObjectBuilder_TreeCuttingDefinition IActivator<MyObjectBuilder_TreeCuttingDefinition>.CreateInstance() => new MyObjectBuilder_TreeCuttingDefinition();
    }
  }
}
