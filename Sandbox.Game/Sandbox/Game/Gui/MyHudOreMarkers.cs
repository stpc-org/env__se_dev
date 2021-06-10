// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyHudOreMarkers
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Entities.Cube;
using System.Collections;
using System.Collections.Generic;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Utils;

namespace Sandbox.Game.Gui
{
  public class MyHudOreMarkers : IEnumerable<MyEntityOreDeposit>, IEnumerable
  {
    private readonly HashSet<MyEntityOreDeposit> m_markers = new HashSet<MyEntityOreDeposit>((IEqualityComparer<MyEntityOreDeposit>) MyEntityOreDeposit.Comparer);
    private string[] m_oreNames;

    public bool Visible { get; set; }

    public MyHudOreMarkers()
    {
      this.Visible = true;
      this.Reload();
    }

    internal void RegisterMarker(MyEntityOreDeposit deposit) => this.m_markers.Add(deposit);

    internal void UnregisterMarker(MyEntityOreDeposit deposit) => this.m_markers.Remove(deposit);

    internal void Clear() => this.m_markers.Clear();

    public HashSet<MyEntityOreDeposit>.Enumerator GetEnumerator() => this.m_markers.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    IEnumerator<MyEntityOreDeposit> IEnumerable<MyEntityOreDeposit>.GetEnumerator() => (IEnumerator<MyEntityOreDeposit>) this.GetEnumerator();

    public void Reload()
    {
      DictionaryValuesReader<string, MyVoxelMaterialDefinition> materialDefinitions = MyDefinitionManager.Static.GetVoxelMaterialDefinitions();
      this.m_oreNames = new string[materialDefinitions.Count];
      foreach (MyVoxelMaterialDefinition materialDefinition in materialDefinitions)
        this.m_oreNames[(int) materialDefinition.Index] = MyTexts.GetString(MyStringId.GetOrCompute(materialDefinition.MinedOre));
    }

    public string GetOreName(MyVoxelMaterialDefinition def) => this.m_oreNames[(int) def.Index];
  }
}
