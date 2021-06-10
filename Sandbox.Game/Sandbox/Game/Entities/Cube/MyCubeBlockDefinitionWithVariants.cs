// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyCubeBlockDefinitionWithVariants
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;

namespace Sandbox.Game.Entities.Cube
{
  internal class MyCubeBlockDefinitionWithVariants
  {
    private MyCubeBlockDefinition m_baseDefinition;
    private int m_variantIndex = -1;

    public void Next()
    {
      if (this.m_baseDefinition.Variants == null || this.m_baseDefinition.Variants.Count <= 0)
        return;
      ++this.m_variantIndex;
      ++this.m_variantIndex;
      this.m_variantIndex %= this.m_baseDefinition.Variants.Count + 1;
      --this.m_variantIndex;
    }

    public void Prev()
    {
      if (this.m_baseDefinition.Variants == null || this.m_baseDefinition.Variants.Count <= 0)
        return;
      this.m_variantIndex = (this.m_variantIndex + this.m_baseDefinition.Variants.Count + 1) % (this.m_baseDefinition.Variants.Count + 1);
      --this.m_variantIndex;
    }

    public void Reset() => this.m_variantIndex = -1;

    public int VariantIndex => this.m_variantIndex;

    public MyCubeBlockDefinition Base => this.m_baseDefinition;

    public MyCubeBlockDefinitionWithVariants(MyCubeBlockDefinition definition, int variantIndex)
    {
      this.m_baseDefinition = definition;
      this.m_variantIndex = variantIndex;
      if (this.m_baseDefinition.Variants == null || this.m_baseDefinition.Variants.Count == 0)
      {
        this.m_variantIndex = -1;
      }
      else
      {
        if (this.m_variantIndex == -1)
          return;
        this.m_variantIndex %= this.m_baseDefinition.Variants.Count;
      }
    }

    public static implicit operator MyCubeBlockDefinitionWithVariants(
      MyCubeBlockDefinition definition)
    {
      return new MyCubeBlockDefinitionWithVariants(definition, -1);
    }

    public static implicit operator MyCubeBlockDefinition(
      MyCubeBlockDefinitionWithVariants definition)
    {
      if (definition == null)
        return (MyCubeBlockDefinition) null;
      return definition.m_variantIndex == -1 ? definition.m_baseDefinition : definition.m_baseDefinition.Variants[definition.m_variantIndex];
    }
  }
}
