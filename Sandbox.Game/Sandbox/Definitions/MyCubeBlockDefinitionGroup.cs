// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyCubeBlockDefinitionGroup
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using VRage.Game;

namespace Sandbox.Definitions
{
  public class MyCubeBlockDefinitionGroup
  {
    private static int m_sizeCount = Enum.GetValues(typeof (MyCubeSize)).Length;
    private readonly MyCubeBlockDefinition[] m_definitions;

    public MyCubeBlockDefinition this[MyCubeSize size]
    {
      get => this.m_definitions[(int) size];
      set => this.m_definitions[(int) size] = value;
    }

    public int SizeCount => MyCubeBlockDefinitionGroup.m_sizeCount;

    public MyCubeBlockDefinition Large => this[MyCubeSize.Large];

    public MyCubeBlockDefinition Small => this[MyCubeSize.Small];

    public MyCubeBlockDefinition Any
    {
      get
      {
        foreach (MyCubeBlockDefinition definition in this.m_definitions)
        {
          if (definition != null)
            return definition;
        }
        return (MyCubeBlockDefinition) null;
      }
    }

    public bool Contains(MyCubeBlockDefinition defCnt, bool checkStages = true)
    {
      foreach (MyCubeBlockDefinition definition in this.m_definitions)
      {
        if (definition == defCnt)
          return true;
        if (checkStages)
        {
          foreach (MyDefinitionId blockStage in definition.BlockStages)
          {
            if (defCnt.Id == blockStage)
              return true;
          }
        }
      }
      return false;
    }

    public MyCubeBlockDefinition AnyPublic
    {
      get
      {
        foreach (MyCubeBlockDefinition definition in this.m_definitions)
        {
          if (definition != null && definition.Public)
            return definition;
        }
        return (MyCubeBlockDefinition) null;
      }
    }

    internal MyCubeBlockDefinitionGroup() => this.m_definitions = new MyCubeBlockDefinition[MyCubeBlockDefinitionGroup.m_sizeCount];
  }
}
