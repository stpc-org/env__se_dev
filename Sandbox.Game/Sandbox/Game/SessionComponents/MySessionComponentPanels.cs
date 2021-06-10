// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MySessionComponentPanels
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Platform.VideoMode;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ObjectBuilders.Components;
using VRage.Game.Utils;
using VRage.Library.Collections;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.SessionComponents
{
  [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation, 2000, typeof (MyObjectBuilder_MySessionComponentPanels), null, false)]
  public class MySessionComponentPanels : MySessionComponentBase
  {
    private float m_maxDistanceMultiplierSquared;
    private ulong m_memoryBudget;
    private readonly MyFreeList<MySessionComponentPanels.MyPanelData> m_insideList = new MyFreeList<MySessionComponentPanels.MyPanelData>();
    private readonly List<int> m_orderedList = new List<int>();
    private bool m_insideListChange;
    private int m_insideListBudgetIndex;
    private int m_sortingPressureCount;

    public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
    {
      base.Init(sessionComponent);
      this.m_memoryBudget = MyVRage.Platform.Render.GetMemoryBudgetForGeneratedTextures();
      MyVideoSettingsManager.OnSettingsChanged += new Action(this.OnSettingsChanged);
      this.OnSettingsChanged();
    }

    private void OnSettingsChanged()
    {
      float multiplierForQuality = MySessionComponentPanels.GetDrawDistanceMultiplierForQuality(MyVideoSettingsManager.CurrentGraphicsSettings.PerformanceSettings.RenderSettings.TextureQuality);
      this.m_maxDistanceMultiplierSquared = multiplierForQuality * multiplierForQuality;
    }

    protected override void UnloadData()
    {
      MyVideoSettingsManager.OnSettingsChanged -= new Action(this.OnSettingsChanged);
      base.UnloadData();
    }

    private static float GetDrawDistanceForQuality(MyTextureQuality quality)
    {
      switch (quality)
      {
        case MyTextureQuality.LOW:
          return 60f;
        case MyTextureQuality.MEDIUM:
          return 120f;
        case MyTextureQuality.HIGH:
          return 180f;
        default:
          return 120f;
      }
    }

    private static float GetDrawDistanceMultiplierForQuality(MyTextureQuality quality)
    {
      switch (quality)
      {
        case MyTextureQuality.LOW:
          return 0.3333333f;
        case MyTextureQuality.MEDIUM:
          return 0.6666667f;
        case MyTextureQuality.HIGH:
          return 1f;
        default:
          return 0.6666667f;
      }
    }

    public override void UpdateAfterSimulation()
    {
      if (MySession.Static.GameplayFrameCounter % 10 != 4)
        return;
      lock (this.m_orderedList)
        this.UpdateBudget();
    }

    private void UpdateBudget()
    {
      int num1 = Math.Min(10, this.m_orderedList.Count / 10);
      int num2 = 0;
      int swaps = 0;
      bool swapsInBudget = false;
      if (this.m_orderedList.Count > 1)
      {
        for (; num2 < this.m_orderedList.Count - 1; ++num2)
        {
          int num3 = num2 + 1;
          if ((double) this.m_insideList[this.m_orderedList[num2]].DistanceSquared > (double) this.m_insideList[this.m_orderedList[num3]].DistanceSquared)
          {
            ApplySwap(num2, num3);
            if (swaps > num1)
              break;
          }
          int num4 = this.m_orderedList.Count - num2 - 1;
          int num5 = num4 - 1;
          if ((double) this.m_insideList[this.m_orderedList[num4]].DistanceSquared < (double) this.m_insideList[this.m_orderedList[num5]].DistanceSquared)
          {
            ApplySwap(num4, num5);
            if (swaps > num1)
              break;
          }
        }
        if (swaps > num1)
        {
          ++this.m_sortingPressureCount;
          if (this.m_sortingPressureCount > 4)
          {
            this.m_orderedList.Sort((Comparison<int>) ((x, y) => this.m_insideList[x].DistanceSquared.CompareTo(this.m_insideList[y].DistanceSquared)));
            for (int index = 0; index < this.m_orderedList.Count; ++index)
              this.m_insideList[this.m_orderedList[index]].OrderedIndex = index;
            this.m_sortingPressureCount = 0;
            swapsInBudget = true;
          }
        }
        else
          this.m_sortingPressureCount = 0;
      }
      if (!swapsInBudget && !this.m_insideListChange)
        return;
      ulong num6 = 0;
      int num7 = 0;
      while (num7 < this.m_orderedList.Count && num6 < this.m_memoryBudget)
        num6 += (ulong) this.m_insideList[this.m_orderedList[num7++]].TextureMemorySize;
      if (num6 > this.m_memoryBudget)
        --num7;
      this.m_insideListBudgetIndex = num7;
      this.m_insideListChange = false;

      void ApplySwap(int current, int next)
      {
        int ordered = this.m_orderedList[next];
        this.m_orderedList[next] = this.m_orderedList[current];
        this.m_orderedList[current] = ordered;
        this.m_insideList[this.m_orderedList[current]].OrderedIndex = current;
        this.m_insideList[this.m_orderedList[next]].OrderedIndex = next;
        ++swaps;
        if (current > this.m_insideListBudgetIndex)
          return;
        swapsInBudget = true;
      }
    }

    public bool IsInRange(IMyTextPanelProvider panelProvider, float maxDistanceSquared)
    {
      MyCamera mainCamera = MySector.MainCamera;
      if (mainCamera == null || Sync.IsDedicated)
        return false;
      float distanceSquared = ((Vector3) (panelProvider.WorldPosition - mainCamera.Position)).LengthSquared();
      maxDistanceSquared *= this.m_maxDistanceMultiplierSquared;
      bool flag1 = (double) distanceSquared < (double) maxDistanceSquared;
      int rangeIndex = panelProvider.RangeIndex;
      bool flag2 = rangeIndex != -1;
      if (flag2 != flag1)
      {
        lock (this.m_orderedList)
        {
          if (flag2)
            this.RemovePanel(panelProvider);
          else
            this.AddPanel(panelProvider, distanceSquared);
        }
      }
      else if (flag2)
      {
        this.m_insideList[rangeIndex].DistanceSquared = distanceSquared;
        return this.m_insideList[rangeIndex].UpdateIsRendered(this.m_insideListBudgetIndex);
      }
      return false;
    }

    private void AddPanel(IMyTextPanelProvider panelProvider, float distanceSquared)
    {
      int rangeIndex = panelProvider.RangeIndex;
      int index;
      this.m_orderedList.Add(index = this.m_insideList.Allocate());
      this.m_insideList[index] = new MySessionComponentPanels.MyPanelData()
      {
        DistanceSquared = distanceSquared,
        TextureMemorySize = panelProvider.PanelTexturesByteCount,
        OrderedIndex = this.m_orderedList.Count - 1
      };
      panelProvider.RangeIndex = index;
      this.m_insideListChange = true;
    }

    private void RemovePanel(IMyTextPanelProvider panelProvider)
    {
      int rangeIndex = panelProvider.RangeIndex;
      int orderedIndex = this.m_insideList[rangeIndex].OrderedIndex;
      this.m_orderedList.RemoveAtFast<int>(orderedIndex);
      if (this.m_orderedList.Count > orderedIndex)
        this.m_insideList[this.m_orderedList[orderedIndex]].OrderedIndex = orderedIndex;
      this.m_insideList.Free(rangeIndex);
      panelProvider.RangeIndex = -1;
      this.m_insideListChange = true;
    }

    public void Remove(IMyTextPanelProvider panelProvider)
    {
      if (panelProvider.RangeIndex == -1)
        return;
      lock (this.m_orderedList)
        this.RemovePanel(panelProvider);
    }

    private struct MyPanelData
    {
      public float DistanceSquared;
      public int TextureMemorySize;
      public int VisibleCounter;
      public int OrderedIndex;

      public bool UpdateIsRendered(int budgetIndex)
      {
        if (this.OrderedIndex < budgetIndex)
          ++this.VisibleCounter;
        else
          this.VisibleCounter = 0;
        return this.VisibleCounter > 5;
      }
    }
  }
}
