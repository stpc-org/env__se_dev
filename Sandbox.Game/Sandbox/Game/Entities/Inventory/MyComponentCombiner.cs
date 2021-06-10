// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Inventory.MyComponentCombiner
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Generics;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities.Inventory
{
  public class MyComponentCombiner
  {
    private MyDynamicObjectPool<List<int>> m_listAllocator = new MyDynamicObjectPool<List<int>>(2);
    private Dictionary<MyDefinitionId, List<int>> m_groups = new Dictionary<MyDefinitionId, List<int>>();
    private Dictionary<int, int> m_presentItems = new Dictionary<int, int>();
    private int m_totalItemCounter;
    private int m_solvedItemCounter;
    private List<MyComponentChange> m_solution = new List<MyComponentChange>();
    private static Dictionary<MyDefinitionId, MyFixedPoint> m_componentCounts = new Dictionary<MyDefinitionId, MyFixedPoint>();

    public MyFixedPoint GetItemAmountCombined(
      MyInventoryBase inventory,
      MyDefinitionId contentId)
    {
      if (inventory == null)
        return (MyFixedPoint) 0;
      int amount = 0;
      MyComponentGroupDefinition groupForComponent = MyDefinitionManager.Static.GetGroupForComponent(contentId, out amount);
      if (groupForComponent == null)
        return (MyFixedPoint) amount + inventory.GetItemAmount(contentId, substitute: true);
      this.Clear();
      inventory.CountItems(MyComponentCombiner.m_componentCounts);
      this.AddItem(groupForComponent.Id, amount, int.MaxValue);
      this.Solve(MyComponentCombiner.m_componentCounts);
      return (MyFixedPoint) this.GetSolvedItemCount();
    }

    public bool CanCombineItems(
      MyInventoryBase inventory,
      DictionaryReader<MyDefinitionId, int> items)
    {
      bool flag = true;
      this.Clear();
      inventory.CountItems(MyComponentCombiner.m_componentCounts);
      foreach (KeyValuePair<MyDefinitionId, int> keyValuePair in items)
      {
        int amount1 = 0;
        int amount2 = keyValuePair.Value;
        MyComponentGroupDefinition groupForComponent = MyDefinitionManager.Static.GetGroupForComponent(keyValuePair.Key, out amount1);
        if (groupForComponent == null)
        {
          MyFixedPoint myFixedPoint;
          if (!MyComponentCombiner.m_componentCounts.TryGetValue(keyValuePair.Key, out myFixedPoint))
          {
            flag = false;
            break;
          }
          if (myFixedPoint < (MyFixedPoint) amount2)
          {
            flag = false;
            break;
          }
        }
        else
          this.AddItem(groupForComponent.Id, amount1, amount2);
      }
      if (flag)
        flag &= this.Solve(MyComponentCombiner.m_componentCounts);
      if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW)
      {
        if (!flag)
        {
          MyRenderProxy.DebugDrawText2D(new Vector2(0.0f, 0.0f), "Can not build", Color.Red, 1f);
        }
        else
        {
          List<MyComponentChange> changes = (List<MyComponentChange>) null;
          this.GetSolution(out changes);
          float y1 = 0.0f;
          foreach (MyComponentChange myComponentChange in changes)
          {
            string str = "";
            int amount;
            if (myComponentChange.IsAddition())
            {
              string[] strArray = new string[5]
              {
                str,
                "+ ",
                null,
                null,
                null
              };
              amount = myComponentChange.Amount;
              strArray[2] = amount.ToString();
              strArray[3] = "x";
              strArray[4] = myComponentChange.ToAdd.ToString();
              string text = string.Concat(strArray);
              MyRenderProxy.DebugDrawText2D(new Vector2(0.0f, y1), text, Color.Green, 1f);
              y1 += 20f;
            }
            else if (myComponentChange.IsRemoval())
            {
              string[] strArray = new string[5]
              {
                str,
                "- ",
                null,
                null,
                null
              };
              amount = myComponentChange.Amount;
              strArray[2] = amount.ToString();
              strArray[3] = "x";
              strArray[4] = myComponentChange.ToRemove.ToString();
              string text = string.Concat(strArray);
              MyRenderProxy.DebugDrawText2D(new Vector2(0.0f, y1), text, Color.Red, 1f);
              y1 += 20f;
            }
            else
            {
              string[] strArray1 = new string[5]
              {
                str,
                "- ",
                null,
                null,
                null
              };
              amount = myComponentChange.Amount;
              strArray1[2] = amount.ToString();
              strArray1[3] = "x";
              strArray1[4] = myComponentChange.ToRemove.ToString();
              string text1 = string.Concat(strArray1);
              MyRenderProxy.DebugDrawText2D(new Vector2(0.0f, y1), text1, Color.Orange, 1f);
              float y2 = y1 + 20f;
              string[] strArray2 = new string[5]
              {
                "",
                "+ ",
                null,
                null,
                null
              };
              amount = myComponentChange.Amount;
              strArray2[2] = amount.ToString();
              strArray2[3] = "x";
              strArray2[4] = myComponentChange.ToAdd.ToString();
              string text2 = string.Concat(strArray2);
              MyRenderProxy.DebugDrawText2D(new Vector2(0.0f, y2), text2, Color.Orange, 1f);
              y1 = y2 + 20f;
            }
          }
        }
      }
      return flag;
    }

    public void RemoveItemsCombined(
      MyInventoryBase inventory,
      DictionaryReader<MyDefinitionId, int> toRemove)
    {
      this.Clear();
      foreach (KeyValuePair<MyDefinitionId, int> keyValuePair in toRemove)
      {
        int amount = 0;
        MyComponentGroupDefinition groupForComponent = MyDefinitionManager.Static.GetGroupForComponent(keyValuePair.Key, out amount);
        if (groupForComponent == null)
          inventory.RemoveItemsOfType((MyFixedPoint) keyValuePair.Value, keyValuePair.Key);
        else
          this.AddItem(groupForComponent.Id, amount, keyValuePair.Value);
      }
      inventory.CountItems(MyComponentCombiner.m_componentCounts);
      this.Solve(MyComponentCombiner.m_componentCounts);
      inventory.ApplyChanges(this.m_solution);
    }

    public void Clear()
    {
      foreach (KeyValuePair<MyDefinitionId, List<int>> group in this.m_groups)
      {
        group.Value.Clear();
        this.m_listAllocator.Deallocate(group.Value);
      }
      this.m_groups.Clear();
      this.m_totalItemCounter = 0;
      this.m_solvedItemCounter = 0;
      MyComponentCombiner.m_componentCounts.Clear();
    }

    public void AddItem(MyDefinitionId groupId, int itemValue, int amount)
    {
      List<int> intList = (List<int>) null;
      MyComponentGroupDefinition componentGroup = MyDefinitionManager.Static.GetComponentGroup(groupId);
      if (componentGroup == null)
        return;
      if (!this.m_groups.TryGetValue(groupId, out intList))
      {
        intList = this.m_listAllocator.Allocate();
        intList.Clear();
        for (int index = 0; index <= componentGroup.GetComponentNumber(); ++index)
          intList.Add(0);
        this.m_groups.Add(groupId, intList);
      }
      intList[itemValue] += amount;
      this.m_totalItemCounter += amount;
    }

    public bool Solve(
      Dictionary<MyDefinitionId, MyFixedPoint> componentCounts)
    {
      this.m_solution.Clear();
      this.m_solvedItemCounter = 0;
      foreach (KeyValuePair<MyDefinitionId, List<int>> group in this.m_groups)
      {
        MyComponentGroupDefinition componentGroup = MyDefinitionManager.Static.GetComponentGroup(group.Key);
        List<int> intList = group.Value;
        this.UpdatePresentItems(componentGroup, componentCounts);
        for (int index = 1; index <= componentGroup.GetComponentNumber(); ++index)
        {
          int removeCount1 = intList[index];
          int removeCount2 = this.TryRemovePresentItems(index, removeCount1);
          if (removeCount2 > 0)
          {
            this.AddRemovalToSolution(componentGroup.GetComponentDefinition(index).Id, removeCount2);
            intList[index] = Math.Max(0, removeCount1 - removeCount2);
          }
          this.m_solvedItemCounter += removeCount2;
        }
        for (int componentNumber = componentGroup.GetComponentNumber(); componentNumber >= 1; --componentNumber)
        {
          int itemCount = intList[componentNumber];
          int num = this.TryCreatingItemsBySplit(componentGroup, componentNumber, itemCount);
          intList[componentNumber] = itemCount - num;
          this.m_solvedItemCounter += num;
        }
        for (int index = 1; index <= componentGroup.GetComponentNumber(); ++index)
        {
          int itemCount = intList[index];
          if (itemCount > 0)
          {
            int num = this.TryCreatingItemsByMerge(componentGroup, index, itemCount);
            intList[index] = itemCount - num;
            this.m_solvedItemCounter += num;
          }
        }
      }
      return this.m_totalItemCounter == this.m_solvedItemCounter;
    }

    private int GetSolvedItemCount() => this.m_solvedItemCounter;

    public void GetSolution(out List<MyComponentChange> changes) => changes = this.m_solution;

    private int TryCreatingItemsBySplit(
      MyComponentGroupDefinition group,
      int itemValue,
      int itemCount)
    {
      int num1 = 0;
      for (int index = itemValue + 1; index <= group.GetComponentNumber(); ++index)
      {
        int splitCount1 = index / itemValue;
        int val2 = itemCount / splitCount1;
        int splitCount2 = itemCount % splitCount1;
        int num2 = splitCount2 == 0 ? 0 : 1;
        int val1 = this.TryRemovePresentItems(index, val2 + num2);
        if (val1 > 0)
        {
          int numItemsToSplit = Math.Min(val1, val2);
          if (numItemsToSplit != 0)
          {
            int num3 = this.SplitHelper(group, index, itemValue, numItemsToSplit, splitCount1);
            num1 += num3;
            itemCount -= num3;
          }
          if (val1 - val2 > 0)
          {
            int num3 = this.SplitHelper(group, index, itemValue, 1, splitCount2);
            num1 += num3;
            itemCount -= num3;
          }
        }
      }
      return num1;
    }

    private int SplitHelper(
      MyComponentGroupDefinition group,
      int splitItemValue,
      int resultItemValue,
      int numItemsToSplit,
      int splitCount)
    {
      int num = splitItemValue - splitCount * resultItemValue;
      MyDefinitionId id1 = group.GetComponentDefinition(splitItemValue).Id;
      if (num != 0)
      {
        MyDefinitionId id2 = group.GetComponentDefinition(num).Id;
        this.AddPresentItems(num, numItemsToSplit);
        this.AddChangeToSolution(id1, id2, numItemsToSplit);
      }
      else
        this.AddRemovalToSolution(id1, numItemsToSplit);
      return splitCount * numItemsToSplit;
    }

    private int TryCreatingItemsByMerge(
      MyComponentGroupDefinition group,
      int itemValue,
      int itemCount)
    {
      List<int> intList = this.m_listAllocator.Allocate();
      intList.Clear();
      for (int index = 0; index <= group.GetComponentNumber(); ++index)
        intList.Add(0);
      int num1 = 0;
      for (int index1 = 0; index1 < itemCount; ++index1)
      {
        int num2 = itemValue;
        for (int index2 = itemValue - 1; index2 >= 1; --index2)
        {
          int val2 = 0;
          if (this.m_presentItems.TryGetValue(index2, out val2))
          {
            int num3 = Math.Min(num2 / index2, val2);
            if (num3 > 0)
            {
              num2 -= index2 * num3;
              val2 -= num3;
              intList[index2] += num3;
            }
          }
        }
        if (num2 != itemValue)
        {
          if (num2 != 0)
          {
            for (int index2 = num2 + 1; index2 <= group.GetComponentNumber(); ++index2)
            {
              int num3 = 0;
              this.m_presentItems.TryGetValue(index2, out num3);
              if (num3 > intList[index2])
              {
                this.AddChangeToSolution(group.GetComponentDefinition(index2).Id, group.GetComponentDefinition(index2 - num2).Id, 1);
                this.TryRemovePresentItems(index2, 1);
                this.AddPresentItems(index2 - num2, 1);
                num2 = 0;
                break;
              }
            }
          }
          if (num2 == 0)
          {
            ++num1;
            for (int index2 = 1; index2 <= group.GetComponentNumber(); ++index2)
            {
              if (intList[index2] > 0)
              {
                MyDefinitionId id = group.GetComponentDefinition(index2).Id;
                this.TryRemovePresentItems(index2, intList[index2]);
                this.AddRemovalToSolution(id, intList[index2]);
                intList[index2] = 0;
              }
            }
          }
          else if (num2 > 0)
            break;
        }
        else
          break;
      }
      this.m_listAllocator.Deallocate(intList);
      return num1;
    }

    private void AddRemovalToSolution(MyDefinitionId removedComponentId, int removeCount)
    {
      for (int index = 0; index < this.m_solution.Count; ++index)
      {
        MyComponentChange myComponentChange = this.m_solution[index];
        if ((myComponentChange.IsChange() || myComponentChange.IsAddition()) && myComponentChange.ToAdd == removedComponentId)
        {
          int num = myComponentChange.Amount - removeCount;
          int amount = Math.Min(removeCount, myComponentChange.Amount);
          removeCount -= myComponentChange.Amount;
          if (num > 0)
          {
            myComponentChange.Amount = num;
            this.m_solution[index] = myComponentChange;
          }
          else
            this.m_solution.RemoveAtFast<MyComponentChange>(index);
          if (myComponentChange.IsChange())
            this.m_solution.Add(MyComponentChange.CreateRemoval(myComponentChange.ToRemove, amount));
          if (removeCount <= 0)
            break;
        }
      }
      if (removeCount <= 0)
        return;
      this.m_solution.Add(MyComponentChange.CreateRemoval(removedComponentId, removeCount));
    }

    private void AddChangeToSolution(
      MyDefinitionId removedComponentId,
      MyDefinitionId addedComponentId,
      int numChanged)
    {
      for (int index = 0; index < this.m_solution.Count; ++index)
      {
        MyComponentChange myComponentChange = this.m_solution[index];
        if ((myComponentChange.IsChange() || myComponentChange.IsAddition()) && myComponentChange.ToAdd == removedComponentId)
        {
          int num = myComponentChange.Amount - numChanged;
          int amount = Math.Min(numChanged, myComponentChange.Amount);
          numChanged -= myComponentChange.Amount;
          if (num > 0)
          {
            myComponentChange.Amount = num;
            this.m_solution[index] = myComponentChange;
          }
          else
            this.m_solution.RemoveAtFast<MyComponentChange>(index);
          if (myComponentChange.IsChange())
            this.m_solution.Add(MyComponentChange.CreateChange(myComponentChange.ToRemove, addedComponentId, amount));
          else
            this.m_solution.Add(MyComponentChange.CreateAddition(addedComponentId, amount));
          if (numChanged <= 0)
            break;
        }
      }
      if (numChanged <= 0)
        return;
      this.m_solution.Add(MyComponentChange.CreateChange(removedComponentId, addedComponentId, numChanged));
    }

    private void UpdatePresentItems(
      MyComponentGroupDefinition group,
      Dictionary<MyDefinitionId, MyFixedPoint> componentCounts)
    {
      this.m_presentItems.Clear();
      for (int index = 1; index <= group.GetComponentNumber(); ++index)
      {
        MyComponentDefinition componentDefinition = group.GetComponentDefinition(index);
        MyFixedPoint myFixedPoint = (MyFixedPoint) 0;
        componentCounts.TryGetValue(componentDefinition.Id, out myFixedPoint);
        this.m_presentItems[index] = (int) myFixedPoint;
      }
    }

    private int TryRemovePresentItems(int itemValue, int removeCount)
    {
      int num = 0;
      this.m_presentItems.TryGetValue(itemValue, out num);
      if (num > removeCount)
      {
        this.m_presentItems[itemValue] = num - removeCount;
        return removeCount;
      }
      this.m_presentItems.Remove(itemValue);
      return num;
    }

    private void AddPresentItems(int itemValue, int addCount)
    {
      int num1 = 0;
      this.m_presentItems.TryGetValue(itemValue, out num1);
      int num2 = num1 + addCount;
      this.m_presentItems[itemValue] = num2;
    }
  }
}
