// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyLootBagDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;

namespace Sandbox.Definitions
{
  public class MyLootBagDefinition
  {
    public MyDefinitionId ContainerDefinition;
    public float SearchRadius;

    public void Init(
      MyObjectBuilder_Configuration.LootBagDefinition objectBuilder)
    {
      this.ContainerDefinition = (MyDefinitionId) objectBuilder.ContainerDefinition;
      this.SearchRadius = objectBuilder.SearchRadius;
    }
  }
}
