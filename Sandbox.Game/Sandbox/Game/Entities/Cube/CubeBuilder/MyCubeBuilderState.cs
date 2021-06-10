// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.CubeBuilder.MyCubeBuilderState
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRageMath;

namespace Sandbox.Game.Entities.Cube.CubeBuilder
{
  public class MyCubeBuilderState
  {
    public Dictionary<MyDefinitionId, Quaternion> RotationsByDefinitionHash = new Dictionary<MyDefinitionId, Quaternion>((IEqualityComparer<MyDefinitionId>) MyDefinitionId.Comparer);
    public Dictionary<MyDefinitionId, int> LastSelectedStageIndexForGroup = new Dictionary<MyDefinitionId, int>((IEqualityComparer<MyDefinitionId>) MyDefinitionId.Comparer);
    public List<MyCubeBlockDefinition> CurrentBlockDefinitionStages = new List<MyCubeBlockDefinition>();
    private MyCubeBlockDefinitionWithVariants m_definitionWithVariants;
    private MyCubeSize m_cubeSizeMode;

    public event Action<MyCubeSize> OnBlockSizeChanged;

    public MyCubeBlockDefinition CurrentBlockDefinition
    {
      get => (MyCubeBlockDefinition) this.m_definitionWithVariants;
      set
      {
        if (value == null)
        {
          this.m_definitionWithVariants = (MyCubeBlockDefinitionWithVariants) null;
          this.CurrentBlockDefinitionStages.Clear();
        }
        else
        {
          this.m_definitionWithVariants = new MyCubeBlockDefinitionWithVariants(value, -1);
          if (!MyFakes.ENABLE_BLOCK_STAGES || this.CurrentBlockDefinitionStages.Contains(value))
            return;
          this.CurrentBlockDefinitionStages.Clear();
          if (value.BlockStages == null)
            return;
          this.CurrentBlockDefinitionStages.Add(value);
          foreach (MyDefinitionId blockStage in value.BlockStages)
          {
            MyCubeBlockDefinition blockDefinition;
            MyDefinitionManager.Static.TryGetCubeBlockDefinition(blockStage, out blockDefinition);
            if (blockDefinition != null)
              this.CurrentBlockDefinitionStages.Add(blockDefinition);
          }
        }
      }
    }

    public MyCubeBlockDefinition StartBlockDefinition { get; private set; }

    public MyCubeSize CubeSizeMode => this.m_cubeSizeMode;

    public void SetCurrentBlockForBlockVariantGroup(MyCubeBlockDefinitionGroup blockGroup)
    {
      MyBlockVariantGroup blockVariantsGroup = blockGroup.AnyPublic?.BlockVariantsGroup;
      if (blockVariantsGroup == null)
        return;
      int num = Array.IndexOf<MyCubeBlockDefinitionGroup>(blockVariantsGroup.BlockGroups, blockGroup);
      this.LastSelectedStageIndexForGroup[blockVariantsGroup.Id] = num;
    }

    public MyCubeBlockDefinitionGroup GetCurrentBlockForBlockVariantGroup(
      MyBlockVariantGroup variants,
      bool respectRestrictions = false)
    {
      return this.GetCurrentBlockForBlockVariantGroup(this.LastSelectedStageIndexForGroup.GetValueOrDefault<MyDefinitionId, int>(variants.Id, 0), variants, respectRestrictions);
    }

    public MyCubeBlockDefinitionGroup GetFirstBlockForBlockVariantGroup(
      MyBlockVariantGroup variants,
      bool respectRestrictions = false)
    {
      return this.GetCurrentBlockForBlockVariantGroup(0, variants, respectRestrictions);
    }

    public MyCubeBlockDefinitionGroup GetCurrentBlockForBlockVariantGroup(
      int idx,
      MyBlockVariantGroup variants,
      bool respectRestrictions = false)
    {
      if (idx >= variants.BlockGroups.Length)
        idx = 0;
      int num = 0;
      int length;
      for (length = variants.BlockGroups.Length; num < length; ++num)
      {
        MyCubeBlockDefinition anyPublic = variants.BlockGroups[(idx + num) % length].AnyPublic;
        if (!respectRestrictions || MySession.Static.GetComponent<MySessionComponentDLC>().HasDefinitionDLC((MyDefinitionBase) anyPublic, Sync.MyId) && (MySession.Static.CreativeToolsEnabled(Sync.MyId) || MySessionComponentResearch.Static.CanUse(MySession.Static.LocalCharacter, anyPublic.Id)))
          break;
      }
      return variants.BlockGroups[(idx + num) % length];
    }

    public void UpdateCubeBlockDefinition(MyDefinitionId? id, MatrixD localMatrixAdd)
    {
      if (!id.HasValue)
        return;
      if (this.CurrentBlockDefinition != null)
      {
        MyCubeBlockDefinitionGroup definitionGroup = MyDefinitionManager.Static.GetDefinitionGroup(this.CurrentBlockDefinition.BlockPairName);
        if (this.CurrentBlockDefinitionStages.Count > 1)
          definitionGroup = MyDefinitionManager.Static.GetDefinitionGroup(this.CurrentBlockDefinitionStages[0].BlockPairName);
        Quaternion fromRotationMatrix = Quaternion.CreateFromRotationMatrix(in localMatrixAdd);
        if (definitionGroup.Small != null)
          this.RotationsByDefinitionHash[definitionGroup.Small.Id] = fromRotationMatrix;
        if (definitionGroup.Large != null)
          this.RotationsByDefinitionHash[definitionGroup.Large.Id] = fromRotationMatrix;
      }
      MyCubeBlockDefinition cubeBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition(id.Value);
      this.CurrentBlockDefinition = cubeBlockDefinition.Public || MyFakes.ENABLE_NON_PUBLIC_BLOCKS ? cubeBlockDefinition : (cubeBlockDefinition.CubeSize == MyCubeSize.Large ? MyDefinitionManager.Static.GetDefinitionGroup(cubeBlockDefinition.BlockPairName).Small : MyDefinitionManager.Static.GetDefinitionGroup(cubeBlockDefinition.BlockPairName).Large);
      this.StartBlockDefinition = this.CurrentBlockDefinition;
    }

    public void UpdateCurrentBlockToLastSelectedVariant()
    {
      MyBlockVariantGroup blockVariantsGroup = this.CurrentBlockDefinition?.BlockVariantsGroup;
      if (blockVariantsGroup == null || this.CurrentBlockDefinitionStages.Count == 0)
        return;
      MyCubeBlockDefinition cubeBlockDefinition = this.GetCurrentBlockForBlockVariantGroup(blockVariantsGroup, true)[this.CurrentBlockDefinition.CubeSize];
      if ((cubeBlockDefinition != null ? (!cubeBlockDefinition.Public ? 1 : 0) : 1) != 0)
        return;
      this.CurrentBlockDefinition = cubeBlockDefinition;
    }

    public void ChooseComplementBlock()
    {
      MyCubeBlockDefinitionWithVariants definitionWithVariants = this.m_definitionWithVariants;
      if (definitionWithVariants == null)
        return;
      MyCubeBlockDefinitionGroup definitionGroup = MyDefinitionManager.Static.GetDefinitionGroup(definitionWithVariants.Base.BlockPairName);
      if (definitionWithVariants.Base.CubeSize == MyCubeSize.Small)
      {
        if (definitionGroup.Large == null || !definitionGroup.Large.Public && !MyFakes.ENABLE_NON_PUBLIC_BLOCKS)
          return;
        this.CurrentBlockDefinition = definitionGroup.Large;
      }
      else
      {
        if (definitionWithVariants.Base.CubeSize != MyCubeSize.Large || definitionGroup.Small == null || !definitionGroup.Small.Public && !MyFakes.ENABLE_NON_PUBLIC_BLOCKS)
          return;
        this.CurrentBlockDefinition = definitionGroup.Small;
      }
    }

    public bool HasComplementBlock()
    {
      if (this.m_definitionWithVariants != null)
      {
        MyCubeBlockDefinitionGroup definitionGroup = MyDefinitionManager.Static.GetDefinitionGroup(this.m_definitionWithVariants.Base.BlockPairName);
        if (this.m_definitionWithVariants.Base.CubeSize == MyCubeSize.Small)
        {
          if (definitionGroup.Large != null && (definitionGroup.Large.Public || MyFakes.ENABLE_NON_PUBLIC_BLOCKS))
            return true;
        }
        else if (this.m_definitionWithVariants.Base.CubeSize == MyCubeSize.Large && definitionGroup.Small != null && (definitionGroup.Small.Public || MyFakes.ENABLE_NON_PUBLIC_BLOCKS))
          return true;
      }
      return false;
    }

    public void SetCubeSize(MyCubeSize newCubeSize)
    {
      this.m_cubeSizeMode = newCubeSize;
      bool flag = true;
      if (this.CurrentBlockDefinitionStages.Count != 0)
      {
        MyCubeBlockDefinition currentBlockDefinition = this.CurrentBlockDefinition;
        MyCubeBlockDefinition cubeBlockDefinition1;
        if (currentBlockDefinition == null)
        {
          cubeBlockDefinition1 = (MyCubeBlockDefinition) null;
        }
        else
        {
          MyBlockVariantGroup blockVariantsGroup = currentBlockDefinition.BlockVariantsGroup;
          if (blockVariantsGroup == null)
          {
            cubeBlockDefinition1 = (MyCubeBlockDefinition) null;
          }
          else
          {
            MyCubeBlockDefinition[] blocks = blockVariantsGroup.Blocks;
            cubeBlockDefinition1 = blocks != null ? ((IEnumerable<MyCubeBlockDefinition>) blocks).FirstOrDefault<MyCubeBlockDefinition>((Func<MyCubeBlockDefinition, bool>) (x => x.CubeSize == this.m_cubeSizeMode && x.BlockStages != null)) : (MyCubeBlockDefinition) null;
          }
        }
        MyCubeBlockDefinition cubeBlockDefinition2 = cubeBlockDefinition1;
        if (cubeBlockDefinition2 != null)
        {
          flag = false;
          this.CurrentBlockDefinition = cubeBlockDefinition2;
          this.UpdateCurrentBlockToLastSelectedVariant();
        }
      }
      if (flag)
        this.UpdateComplementBlock();
      Action<MyCubeSize> blockSizeChanged = this.OnBlockSizeChanged;
      if (blockSizeChanged == null)
        return;
      blockSizeChanged(newCubeSize);
    }

    internal void UpdateComplementBlock()
    {
      MyCubeBlockDefinition currentBlockDefinition = this.CurrentBlockDefinition;
      MyCubeBlockDefinition startBlockDefinition = this.StartBlockDefinition;
      if (this.CurrentBlockDefinition == null || this.StartBlockDefinition == null)
        return;
      MyCubeBlockDefinition cubeBlockDefinition = MyDefinitionManager.Static.GetDefinitionGroup(this.CurrentBlockDefinition.BlockPairName)[this.m_cubeSizeMode] ?? MyDefinitionManager.Static.GetDefinitionGroup(this.StartBlockDefinition.BlockPairName)[this.m_cubeSizeMode];
      if (cubeBlockDefinition == null && this.CurrentBlockDefinitionStages.Count != 0)
      {
        MyBlockVariantGroup blockVariantsGroup = this.StartBlockDefinition.BlockVariantsGroup;
        if (blockVariantsGroup != null)
          cubeBlockDefinition = ((IEnumerable<MyCubeBlockDefinition>) blockVariantsGroup.Blocks).FirstOrDefault<MyCubeBlockDefinition>((Func<MyCubeBlockDefinition, bool>) (x => x.CubeSize == this.m_cubeSizeMode));
      }
      this.CurrentBlockDefinition = cubeBlockDefinition;
    }
  }
}
