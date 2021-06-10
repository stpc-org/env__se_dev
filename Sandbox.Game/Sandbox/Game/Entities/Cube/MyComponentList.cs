// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyComponentList
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using VRage;
using VRage.Collections;
using VRage.Game;

namespace Sandbox.Game.Entities.Cube
{
  public class MyComponentList
  {
    private List<MyTuple<MyDefinitionId, int>> m_displayList = new List<MyTuple<MyDefinitionId, int>>();
    private Dictionary<MyDefinitionId, int> m_totalMaterials = new Dictionary<MyDefinitionId, int>();
    private Dictionary<MyDefinitionId, int> m_requiredMaterials = new Dictionary<MyDefinitionId, int>();

    public DictionaryReader<MyDefinitionId, int> TotalMaterials => new DictionaryReader<MyDefinitionId, int>(this.m_totalMaterials);

    public DictionaryReader<MyDefinitionId, int> RequiredMaterials => new DictionaryReader<MyDefinitionId, int>(this.m_requiredMaterials);

    public void AddMaterial(
      MyDefinitionId myDefinitionId,
      int amount,
      int requiredAmount = 0,
      bool addToDisplayList = true)
    {
      if (requiredAmount > amount)
        requiredAmount = amount;
      if (addToDisplayList)
        this.m_displayList.Add(new MyTuple<MyDefinitionId, int>(myDefinitionId, amount));
      this.AddToDictionary(this.m_totalMaterials, myDefinitionId, amount);
      if (requiredAmount <= 0)
        return;
      this.AddToDictionary(this.m_requiredMaterials, myDefinitionId, requiredAmount);
    }

    public void Clear()
    {
      this.m_displayList.Clear();
      this.m_totalMaterials.Clear();
      this.m_requiredMaterials.Clear();
    }

    private void AddToDictionary(
      Dictionary<MyDefinitionId, int> dict,
      MyDefinitionId myDefinitionId,
      int amount)
    {
      int num1 = 0;
      dict.TryGetValue(myDefinitionId, out num1);
      int num2 = num1 + amount;
      dict[myDefinitionId] = num2;
    }
  }
}
