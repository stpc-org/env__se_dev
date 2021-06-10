// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyGridMassComputer
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using System;
using System.Buffers;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Generics;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
  internal class MyGridMassComputer : MySparseGrid<HkMassElement, MassCellData>
  {
    private const float DefaultUpdateThreshold = 0.05f;
    private float m_updateThreshold;
    private HkMassProperties m_massProperties;

    public float UpdateThreshold => this.m_updateThreshold;

    public MyGridMassComputer(int cellSize, float updateThreshold = 0.05f)
      : base(cellSize)
      => this.m_updateThreshold = updateThreshold;

    public HkMassProperties UpdateMass()
    {
      HkMassElement hkMassElement = new HkMassElement()
      {
        Tranform = Matrix.Identity
      };
      bool flag = false;
      Span<HkMassElement> elements;
      foreach (Vector3I dirtyCell in this.DirtyCells)
      {
        MySparseGrid<HkMassElement, MassCellData>.Cell result;
        if (this.TryGetCell(dirtyCell, out result))
        {
          ArrayPool<HkMassElement> shared = ArrayPool<HkMassElement>.Shared;
          DictionaryReader<Vector3I, HkMassElement> items = result.Items;
          int count1 = items.Count;
          HkMassElement[] array1 = shared.Rent(count1);
          HkMassElement[] array2 = array1;
          items = result.Items;
          int count2 = items.Count;
          elements = new Span<HkMassElement>(array2, 0, count2);
          float num1 = 0.0f;
          int num2 = 0;
          items = result.Items;
          foreach (KeyValuePair<Vector3I, HkMassElement> keyValuePair in items)
          {
            num1 += keyValuePair.Value.Properties.Mass;
            elements[num2++] = keyValuePair.Value;
          }
          if ((double) Math.Abs((float) (1.0 - (double) result.CellData.LastMass / (double) num1)) > (double) this.m_updateThreshold)
          {
            HkInertiaTensorComputer.CombineMassProperties(elements, out hkMassElement.Properties);
            result.CellData.MassElement = hkMassElement;
            result.CellData.LastMass = num1;
            flag = true;
          }
          ArrayPool<HkMassElement>.Shared.Return(array1);
        }
        else
          flag = true;
      }
      this.UnmarkDirtyAll();
      if (!flag)
        return this.m_massProperties;
      HkMassElement[] array = ArrayPool<HkMassElement>.Shared.Rent(this.CellCount);
      elements = new Span<HkMassElement>(array, 0, this.CellCount);
      int num = 0;
      foreach (KeyValuePair<Vector3I, MySparseGrid<HkMassElement, MassCellData>.Cell> keyValuePair in (MySparseGrid<HkMassElement, MassCellData>) this)
        elements[num++] = keyValuePair.Value.CellData.MassElement;
      if (this.ItemCount > 0)
        HkInertiaTensorComputer.CombineMassProperties(elements, out this.m_massProperties);
      else
        this.m_massProperties = new HkMassProperties();
      ArrayPool<HkMassElement>.Shared.Return(array);
      return this.m_massProperties;
    }
  }
}
