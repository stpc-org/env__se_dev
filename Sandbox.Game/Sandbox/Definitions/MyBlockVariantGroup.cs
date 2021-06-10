// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyBlockVariantGroup
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Library.Utils;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_BlockVariantGroup), null)]
  public class MyBlockVariantGroup : MyDefinitionBase
  {
    public MyCubeBlockDefinition[] Blocks;
    private SerializableDefinitionId[] m_blockIdsToResolve;

    public MyCubeBlockDefinitionGroup[] BlockGroups { get; private set; }

    public MyCubeBlockDefinition PrimaryGUIBlock { get; private set; }

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      this.m_blockIdsToResolve = ((MyObjectBuilder_BlockVariantGroup) builder).Blocks;
    }

    public void ResolveBlocks()
    {
      this.Blocks = new MyCubeBlockDefinition[this.m_blockIdsToResolve.Length];
      bool flag = false;
      for (int index = 0; index < this.m_blockIdsToResolve.Length; ++index)
      {
        MyCubeBlockDefinition definition;
        if (MyDefinitionManager.Static.TryGetDefinition<MyCubeBlockDefinition>((MyDefinitionId) this.m_blockIdsToResolve[index], out definition))
          this.Blocks[index] = definition;
        else
          flag = true;
      }
      if (flag)
        this.Blocks = ((IEnumerable<MyCubeBlockDefinition>) this.Blocks).Where<MyCubeBlockDefinition>((Func<MyCubeBlockDefinition, bool>) (x => x != null)).ToArray<MyCubeBlockDefinition>();
      this.m_blockIdsToResolve = (SerializableDefinitionId[]) null;
    }

    public new void Postprocess()
    {
      HashSet<MyCubeBlockDefinitionGroup> source = new HashSet<MyCubeBlockDefinitionGroup>();
      foreach (MyCubeBlockDefinition block in this.Blocks)
        source.Add(MyDefinitionManager.Static.GetDefinitionGroup(block.BlockPairName));
      this.BlockGroups = source.ToArray<MyCubeBlockDefinitionGroup>();
      this.CreateBlockStages();
      this.PrimaryGUIBlock = this.Blocks[0];
      foreach (MyCubeBlockDefinition block in this.Blocks)
        block.GuiVisible = this.PrimaryGUIBlock == block;
      if (this.Icons.IsNullOrEmpty<string>())
        this.Icons = this.PrimaryGUIBlock.Icons;
      if (this.DisplayNameEnum.HasValue)
        return;
      if (!string.IsNullOrEmpty(this.DisplayNameString))
        this.DisplayNameEnum = new MyStringId?(MyStringId.GetOrCompute(this.DisplayNameString));
      else if (!string.IsNullOrEmpty(this.DisplayNameText))
        this.DisplayNameEnum = new MyStringId?(MyStringId.GetOrCompute(this.DisplayNameText));
      else if (this.PrimaryGUIBlock.DisplayNameEnum.HasValue)
        this.DisplayNameEnum = new MyStringId?(this.PrimaryGUIBlock.DisplayNameEnum.Value);
      else
        this.DisplayNameEnum = new MyStringId?(MyStringId.GetOrCompute(this.PrimaryGUIBlock.DisplayNameText));
    }

    internal void CleanUp()
    {
      this.Blocks = (MyCubeBlockDefinition[]) null;
      this.BlockGroups = (MyCubeBlockDefinitionGroup[]) null;
      this.PrimaryGUIBlock = (MyCubeBlockDefinition) null;
    }

    private void CreateBlockStages()
    {
      MyCubeSize[] blockSizes = MyEnum<MyCubeSize>.Values;
      MyCubeBlockDefinitionGroup element = (MyCubeBlockDefinitionGroup) null;
      foreach (MyCubeBlockDefinition block in this.Blocks)
      {
        MyCubeBlockDefinitionGroup definitionGroup = MyDefinitionManager.Static.GetDefinitionGroup(block.BlockPairName);
        if (HasBlocksForAllSizes(definitionGroup))
        {
          element = definitionGroup;
          break;
        }
      }
      if (element != null)
      {
        int offset;
        for (offset = 0; offset < this.Blocks.Length; ++offset)
        {
          MyCubeBlockDefinition block = this.Blocks[offset];
          bool flag = false;
          foreach (MyCubeSize size in blockSizes)
          {
            if (element[size] == block)
            {
              flag = true;
              break;
            }
          }
          if (!flag)
            break;
        }
        foreach (MyCubeSize size in blockSizes)
        {
          MyCubeBlockDefinition cubeBlockDefinition = element[size];
          ConstructFullVariantsFor(cubeBlockDefinition);
          if (MoveFront<MyCubeBlockDefinition>(this.Blocks, cubeBlockDefinition, offset))
            ++offset;
        }
        MoveFront<MyCubeBlockDefinitionGroup>(this.BlockGroups, element, 0);
      }
      else
      {
        foreach (MyCubeSize myCubeSize in blockSizes)
        {
          MyCubeSize size = myCubeSize;
          MyCubeBlockDefinition block = ((IEnumerable<MyCubeBlockDefinition>) this.Blocks).FirstOrDefault<MyCubeBlockDefinition>((Func<MyCubeBlockDefinition, bool>) (x => x.CubeSize == size && x.Public));
          if (block != null)
            ConstructFullVariantsFor(block);
        }
      }

      void ConstructFullVariantsFor(MyCubeBlockDefinition block) => block.BlockStages = ((IEnumerable<MyCubeBlockDefinition>) this.Blocks).Where<MyCubeBlockDefinition>((Func<MyCubeBlockDefinition, bool>) (x => x != block && x.CubeSize == block.CubeSize)).Select<MyCubeBlockDefinition, MyDefinitionId>((Func<MyCubeBlockDefinition, MyDefinitionId>) (x => x.Id)).ToArray<MyDefinitionId>();

      bool HasBlocksForAllSizes(MyCubeBlockDefinitionGroup blockPair)
      {
        foreach (MyCubeSize blockSiz in blockSizes)
        {
          if (blockPair[blockSiz] == null)
            return false;
        }
        return true;
      }

      bool MoveFront<T>(T[] array, T element, int offset)
      {
        int num = Array.IndexOf<T>(array, element);
        if (num <= offset)
          return false;
        Array.Copy((Array) array, offset, (Array) array, offset + 1, num - offset);
        array[offset] = element;
        return true;
      }
    }

    private class Sandbox_Definitions_MyBlockVariantGroup\u003C\u003EActor : IActivator, IActivator<MyBlockVariantGroup>
    {
      object IActivator.CreateInstance() => (object) new MyBlockVariantGroup();

      MyBlockVariantGroup IActivator<MyBlockVariantGroup>.CreateInstance() => new MyBlockVariantGroup();
    }
  }
}
