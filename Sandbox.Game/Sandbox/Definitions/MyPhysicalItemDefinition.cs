// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyPhysicalItemDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Library;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_PhysicalItemDefinition), null)]
  public class MyPhysicalItemDefinition : MyDefinitionBase
  {
    public Vector3 Size;
    public float Mass;
    public string Model;
    public string[] Models;
    public MyStringId? IconSymbol;
    public float Volume;
    public float ModelVolume;
    public MyStringHash PhysicalMaterial;
    public MyStringHash VoxelMaterial;
    public bool CanSpawnFromScreen;
    public bool RotateOnSpawnX;
    public bool RotateOnSpawnY;
    public bool RotateOnSpawnZ;
    public int Health;
    public MyDefinitionId? DestroyedPieceId;
    public int DestroyedPieces;
    public StringBuilder ExtraInventoryTooltipLine;
    public MyFixedPoint MaxStackAmount;
    public int MinimalPricePerUnit;
    public int MinimumOfferAmount;
    public int MaximumOfferAmount;
    public int MinimumOrderAmount;
    public int MaximumOrderAmount;
    public int MinimumAcquisitionAmount;
    public int MaximumAcquisitionAmount;
    public bool CanPlayerOrder;

    public bool HasIntegralAmounts => this.Id.TypeId != typeof (MyObjectBuilder_Ingot) && this.Id.TypeId != typeof (MyObjectBuilder_Ore);

    public bool IsOre => this.Id.TypeId == typeof (MyObjectBuilder_Ore);

    public bool IsIngot => this.Id.TypeId == typeof (MyObjectBuilder_Ingot);

    public bool HasModelVariants => this.Models != null && (uint) this.Models.Length > 0U;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_PhysicalItemDefinition physicalItemDefinition = builder as MyObjectBuilder_PhysicalItemDefinition;
      this.Size = physicalItemDefinition.Size;
      this.Mass = physicalItemDefinition.Mass;
      this.Model = physicalItemDefinition.Model;
      this.Models = physicalItemDefinition.Models;
      this.Volume = physicalItemDefinition.Volume.HasValue ? physicalItemDefinition.Volume.Value / 1000f : physicalItemDefinition.Size.Volume;
      this.ModelVolume = physicalItemDefinition.ModelVolume.HasValue ? physicalItemDefinition.ModelVolume.Value / 1000f : this.Volume;
      this.IconSymbol = !string.IsNullOrEmpty(physicalItemDefinition.IconSymbol) ? new MyStringId?(MyStringId.GetOrCompute(physicalItemDefinition.IconSymbol)) : new MyStringId?();
      this.PhysicalMaterial = MyStringHash.GetOrCompute(physicalItemDefinition.PhysicalMaterial);
      this.VoxelMaterial = MyStringHash.GetOrCompute(physicalItemDefinition.VoxelMaterial);
      this.CanSpawnFromScreen = physicalItemDefinition.CanSpawnFromScreen;
      this.RotateOnSpawnX = physicalItemDefinition.RotateOnSpawnX;
      this.RotateOnSpawnY = physicalItemDefinition.RotateOnSpawnY;
      this.RotateOnSpawnZ = physicalItemDefinition.RotateOnSpawnZ;
      this.Health = physicalItemDefinition.Health;
      if (physicalItemDefinition.DestroyedPieceId.HasValue)
        this.DestroyedPieceId = new MyDefinitionId?((MyDefinitionId) physicalItemDefinition.DestroyedPieceId.Value);
      this.DestroyedPieces = physicalItemDefinition.DestroyedPieces;
      this.ExtraInventoryTooltipLine = physicalItemDefinition.ExtraInventoryTooltipLine == null ? new StringBuilder() : new StringBuilder().Append(MyEnvironment.NewLine).Append(physicalItemDefinition.ExtraInventoryTooltipLine);
      this.MaxStackAmount = physicalItemDefinition.MaxStackAmount;
      this.MinimalPricePerUnit = physicalItemDefinition.MinimalPricePerUnit;
      this.MinimumOfferAmount = physicalItemDefinition.MinimumOfferAmount;
      this.MaximumOfferAmount = physicalItemDefinition.MaximumOfferAmount;
      this.MinimumOrderAmount = physicalItemDefinition.MinimumOrderAmount;
      this.MaximumOrderAmount = physicalItemDefinition.MaximumOrderAmount;
      this.MinimumAcquisitionAmount = physicalItemDefinition.MinimumAcquisitionAmount;
      this.MaximumAcquisitionAmount = physicalItemDefinition.MaximumAcquisitionAmount;
      this.CanPlayerOrder = physicalItemDefinition.CanPlayerOrder;
    }

    internal virtual string GetTooltipDisplayName(MyObjectBuilder_PhysicalObject content) => this.DisplayNameText;

    private class Sandbox_Definitions_MyPhysicalItemDefinition\u003C\u003EActor : IActivator, IActivator<MyPhysicalItemDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyPhysicalItemDefinition();

      MyPhysicalItemDefinition IActivator<MyPhysicalItemDefinition>.CreateInstance() => new MyPhysicalItemDefinition();
    }
  }
}
