// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_ProjectorBase
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game.ObjectBuilders;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public abstract class MyObjectBuilder_ProjectorBase : MyObjectBuilder_FunctionalBlock
  {
    [ProtoMember(1)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public MyObjectBuilder_CubeGrid ProjectedGrid;
    [ProtoMember(2)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public List<MyObjectBuilder_CubeGrid> ProjectedGrids;
    [ProtoMember(4)]
    public Vector3I ProjectionOffset;
    [ProtoMember(7)]
    public Vector3I ProjectionRotation;
    [ProtoMember(10)]
    public bool KeepProjection;
    [ProtoMember(13)]
    public bool ShowOnlyBuildable;
    [ProtoMember(16)]
    public bool InstantBuildingEnabled;
    [ProtoMember(19)]
    public int MaxNumberOfProjections = 5;
    [ProtoMember(22)]
    public int MaxNumberOfBlocks = 200;
    [ProtoMember(25)]
    public int ProjectionsRemaining;
    [ProtoMember(28)]
    public bool GetOwnershipFromProjector;
    [ProtoMember(31)]
    public float Scale = 1f;
    [ProtoMember(34)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public List<MySerializedTextPanelData> TextPanels;

    public override void Remap(IMyRemapHelper remapHelper)
    {
      base.Remap(remapHelper);
      if (this.ProjectedGrids == null || this.ProjectedGrids.Count <= 0)
        return;
      foreach (MyObjectBuilder_EntityBase projectedGrid in this.ProjectedGrids)
        projectedGrid.Remap(remapHelper);
    }

    protected class VRage_Game_MyObjectBuilder_ProjectorBase\u003C\u003EProjectedGrid\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProjectorBase, MyObjectBuilder_CubeGrid>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProjectorBase owner,
        in MyObjectBuilder_CubeGrid value)
      {
        owner.ProjectedGrid = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProjectorBase owner,
        out MyObjectBuilder_CubeGrid value)
      {
        value = owner.ProjectedGrid;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ProjectorBase\u003C\u003EProjectedGrids\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProjectorBase, List<MyObjectBuilder_CubeGrid>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProjectorBase owner,
        in List<MyObjectBuilder_CubeGrid> value)
      {
        owner.ProjectedGrids = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProjectorBase owner,
        out List<MyObjectBuilder_CubeGrid> value)
      {
        value = owner.ProjectedGrids;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ProjectorBase\u003C\u003EProjectionOffset\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProjectorBase, Vector3I>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProjectorBase owner, in Vector3I value) => owner.ProjectionOffset = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProjectorBase owner, out Vector3I value) => value = owner.ProjectionOffset;
    }

    protected class VRage_Game_MyObjectBuilder_ProjectorBase\u003C\u003EProjectionRotation\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProjectorBase, Vector3I>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProjectorBase owner, in Vector3I value) => owner.ProjectionRotation = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProjectorBase owner, out Vector3I value) => value = owner.ProjectionRotation;
    }

    protected class VRage_Game_MyObjectBuilder_ProjectorBase\u003C\u003EKeepProjection\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProjectorBase, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProjectorBase owner, in bool value) => owner.KeepProjection = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProjectorBase owner, out bool value) => value = owner.KeepProjection;
    }

    protected class VRage_Game_MyObjectBuilder_ProjectorBase\u003C\u003EShowOnlyBuildable\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProjectorBase, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProjectorBase owner, in bool value) => owner.ShowOnlyBuildable = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProjectorBase owner, out bool value) => value = owner.ShowOnlyBuildable;
    }

    protected class VRage_Game_MyObjectBuilder_ProjectorBase\u003C\u003EInstantBuildingEnabled\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProjectorBase, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProjectorBase owner, in bool value) => owner.InstantBuildingEnabled = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProjectorBase owner, out bool value) => value = owner.InstantBuildingEnabled;
    }

    protected class VRage_Game_MyObjectBuilder_ProjectorBase\u003C\u003EMaxNumberOfProjections\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProjectorBase, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProjectorBase owner, in int value) => owner.MaxNumberOfProjections = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProjectorBase owner, out int value) => value = owner.MaxNumberOfProjections;
    }

    protected class VRage_Game_MyObjectBuilder_ProjectorBase\u003C\u003EMaxNumberOfBlocks\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProjectorBase, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProjectorBase owner, in int value) => owner.MaxNumberOfBlocks = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProjectorBase owner, out int value) => value = owner.MaxNumberOfBlocks;
    }

    protected class VRage_Game_MyObjectBuilder_ProjectorBase\u003C\u003EProjectionsRemaining\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProjectorBase, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProjectorBase owner, in int value) => owner.ProjectionsRemaining = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProjectorBase owner, out int value) => value = owner.ProjectionsRemaining;
    }

    protected class VRage_Game_MyObjectBuilder_ProjectorBase\u003C\u003EGetOwnershipFromProjector\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProjectorBase, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProjectorBase owner, in bool value) => owner.GetOwnershipFromProjector = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProjectorBase owner, out bool value) => value = owner.GetOwnershipFromProjector;
    }

    protected class VRage_Game_MyObjectBuilder_ProjectorBase\u003C\u003EScale\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProjectorBase, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProjectorBase owner, in float value) => owner.Scale = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProjectorBase owner, out float value) => value = owner.Scale;
    }

    protected class VRage_Game_MyObjectBuilder_ProjectorBase\u003C\u003ETextPanels\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProjectorBase, List<MySerializedTextPanelData>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProjectorBase owner,
        in List<MySerializedTextPanelData> value)
      {
        owner.TextPanels = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProjectorBase owner,
        out List<MySerializedTextPanelData> value)
      {
        value = owner.TextPanels;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ProjectorBase\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_FunctionalBlock.VRage_Game_MyObjectBuilder_FunctionalBlock\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProjectorBase owner, in bool value) => this.Set((MyObjectBuilder_FunctionalBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProjectorBase owner, out bool value) => this.Get((MyObjectBuilder_FunctionalBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ProjectorBase\u003C\u003ECustomName\u003C\u003EAccessor : MyObjectBuilder_TerminalBlock.VRage_Game_MyObjectBuilder_TerminalBlock\u003C\u003ECustomName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProjectorBase owner, in string value) => this.Set((MyObjectBuilder_TerminalBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProjectorBase owner, out string value) => this.Get((MyObjectBuilder_TerminalBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ProjectorBase\u003C\u003EShowOnHUD\u003C\u003EAccessor : MyObjectBuilder_TerminalBlock.VRage_Game_MyObjectBuilder_TerminalBlock\u003C\u003EShowOnHUD\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProjectorBase owner, in bool value) => this.Set((MyObjectBuilder_TerminalBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProjectorBase owner, out bool value) => this.Get((MyObjectBuilder_TerminalBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ProjectorBase\u003C\u003EShowInTerminal\u003C\u003EAccessor : MyObjectBuilder_TerminalBlock.VRage_Game_MyObjectBuilder_TerminalBlock\u003C\u003EShowInTerminal\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProjectorBase owner, in bool value) => this.Set((MyObjectBuilder_TerminalBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProjectorBase owner, out bool value) => this.Get((MyObjectBuilder_TerminalBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ProjectorBase\u003C\u003EShowInToolbarConfig\u003C\u003EAccessor : MyObjectBuilder_TerminalBlock.VRage_Game_MyObjectBuilder_TerminalBlock\u003C\u003EShowInToolbarConfig\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProjectorBase owner, in bool value) => this.Set((MyObjectBuilder_TerminalBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProjectorBase owner, out bool value) => this.Get((MyObjectBuilder_TerminalBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ProjectorBase\u003C\u003EShowInInventory\u003C\u003EAccessor : MyObjectBuilder_TerminalBlock.VRage_Game_MyObjectBuilder_TerminalBlock\u003C\u003EShowInInventory\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProjectorBase owner, in bool value) => this.Set((MyObjectBuilder_TerminalBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProjectorBase owner, out bool value) => this.Get((MyObjectBuilder_TerminalBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ProjectorBase\u003C\u003EEntityId\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EEntityId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProjectorBase owner, in long value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProjectorBase owner, out long value) => this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ProjectorBase\u003C\u003EName\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProjectorBase owner, in string value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProjectorBase owner, out string value) => this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ProjectorBase\u003C\u003EMin\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EMin\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, SerializableVector3I>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProjectorBase owner,
        in SerializableVector3I value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProjectorBase owner,
        out SerializableVector3I value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_ProjectorBase\u003C\u003Em_orientation\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003Em_orientation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, SerializableQuaternion>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProjectorBase owner,
        in SerializableQuaternion value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProjectorBase owner,
        out SerializableQuaternion value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_ProjectorBase\u003C\u003EIntegrityPercent\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EIntegrityPercent\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProjectorBase owner, in float value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProjectorBase owner, out float value) => this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ProjectorBase\u003C\u003EBuildPercent\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EBuildPercent\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProjectorBase owner, in float value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProjectorBase owner, out float value) => this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ProjectorBase\u003C\u003EBlockOrientation\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EBlockOrientation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, SerializableBlockOrientation>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProjectorBase owner,
        in SerializableBlockOrientation value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProjectorBase owner,
        out SerializableBlockOrientation value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_ProjectorBase\u003C\u003EConstructionInventory\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EConstructionInventory\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, MyObjectBuilder_Inventory>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProjectorBase owner,
        in MyObjectBuilder_Inventory value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProjectorBase owner,
        out MyObjectBuilder_Inventory value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_ProjectorBase\u003C\u003EColorMaskHSV\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EColorMaskHSV\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, SerializableVector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProjectorBase owner, in SerializableVector3 value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProjectorBase owner,
        out SerializableVector3 value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_ProjectorBase\u003C\u003ESkinSubtypeId\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003ESkinSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProjectorBase owner, in string value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProjectorBase owner, out string value) => this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ProjectorBase\u003C\u003EConstructionStockpile\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EConstructionStockpile\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, MyObjectBuilder_ConstructionStockpile>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProjectorBase owner,
        in MyObjectBuilder_ConstructionStockpile value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProjectorBase owner,
        out MyObjectBuilder_ConstructionStockpile value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_ProjectorBase\u003C\u003EOwner\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EOwner\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProjectorBase owner, in long value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProjectorBase owner, out long value) => this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ProjectorBase\u003C\u003EBuiltBy\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EBuiltBy\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProjectorBase owner, in long value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProjectorBase owner, out long value) => this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ProjectorBase\u003C\u003EShareMode\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EShareMode\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, MyOwnershipShareModeEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProjectorBase owner,
        in MyOwnershipShareModeEnum value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProjectorBase owner,
        out MyOwnershipShareModeEnum value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_ProjectorBase\u003C\u003EDeformationRatio\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EDeformationRatio\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProjectorBase owner, in float value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProjectorBase owner, out float value) => this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ProjectorBase\u003C\u003ESubBlocks\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003ESubBlocks\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, MyObjectBuilder_CubeBlock.MySubBlockId[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProjectorBase owner,
        in MyObjectBuilder_CubeBlock.MySubBlockId[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProjectorBase owner,
        out MyObjectBuilder_CubeBlock.MySubBlockId[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_ProjectorBase\u003C\u003EMultiBlockId\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EMultiBlockId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProjectorBase owner, in int value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProjectorBase owner, out int value) => this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ProjectorBase\u003C\u003EMultiBlockDefinition\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EMultiBlockDefinition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, SerializableDefinitionId?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProjectorBase owner,
        in SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProjectorBase owner,
        out SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_ProjectorBase\u003C\u003EMultiBlockIndex\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EMultiBlockIndex\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProjectorBase owner, in int value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProjectorBase owner, out int value) => this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ProjectorBase\u003C\u003EBlockGeneralDamageModifier\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EBlockGeneralDamageModifier\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProjectorBase owner, in float value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProjectorBase owner, out float value) => this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ProjectorBase\u003C\u003EComponentContainer\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EComponentContainer\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, MyObjectBuilder_ComponentContainer>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProjectorBase owner,
        in MyObjectBuilder_ComponentContainer value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProjectorBase owner,
        out MyObjectBuilder_ComponentContainer value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_ProjectorBase\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProjectorBase owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProjectorBase owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ProjectorBase\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProjectorBase owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProjectorBase owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ProjectorBase\u003C\u003EOrientation\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EOrientation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, SerializableQuaternion>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProjectorBase owner,
        in SerializableQuaternion value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProjectorBase owner,
        out SerializableQuaternion value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_ProjectorBase\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProjectorBase owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProjectorBase owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ProjectorBase\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProjectorBase owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProjectorBase owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }
  }
}
