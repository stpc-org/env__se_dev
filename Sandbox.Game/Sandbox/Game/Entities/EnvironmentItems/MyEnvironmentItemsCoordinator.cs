// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.EnvironmentItems.MyEnvironmentItemsCoordinator
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Utils;

namespace Sandbox.Game.Entities.EnvironmentItems
{
  [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation, 500)]
  public class MyEnvironmentItemsCoordinator : MySessionComponentBase
  {
    private static MyEnvironmentItemsCoordinator Static;
    private HashSet<MyEnvironmentItems> m_tmpItems;
    private List<MyEnvironmentItemsCoordinator.TransferData> m_transferList;
    private float? m_transferTime;

    public override bool IsRequiredByGame => false;

    public override MyObjectBuilder_SessionComponent GetObjectBuilder()
    {
      if (this.m_transferTime.HasValue)
        this.FinalizeTransfers();
      return base.GetObjectBuilder();
    }

    public override void LoadData()
    {
      base.LoadData();
      this.m_transferList = new List<MyEnvironmentItemsCoordinator.TransferData>();
      this.m_tmpItems = new HashSet<MyEnvironmentItems>();
      MyEnvironmentItemsCoordinator.Static = this;
    }

    protected override void UnloadData()
    {
      base.UnloadData();
      MyEnvironmentItemsCoordinator.Static = (MyEnvironmentItemsCoordinator) null;
    }

    public override void UpdateBeforeSimulation()
    {
      base.UpdateBeforeSimulation();
      if (!this.m_transferTime.HasValue)
        return;
      this.m_transferTime = new float?(this.m_transferTime.Value - 0.01666667f);
      float? transferTime = this.m_transferTime;
      float num = 0.0f;
      if (!((double) transferTime.GetValueOrDefault() < (double) num & transferTime.HasValue))
        return;
      this.FinalizeTransfers();
    }

    private void FinalizeTransfers()
    {
      foreach (MyEnvironmentItemsCoordinator.TransferData transfer in this.m_transferList)
      {
        if (this.MakeTransfer(transfer))
          this.m_tmpItems.Add(transfer.To);
      }
      this.m_transferList.Clear();
      this.m_transferTime = new float?();
      foreach (MyEnvironmentItems tmpItem in this.m_tmpItems)
        tmpItem.EndBatch(true);
      this.m_tmpItems.Clear();
    }

    private bool MakeTransfer(MyEnvironmentItemsCoordinator.TransferData data)
    {
      MyEnvironmentItems.ItemInfo result;
      if (!data.From.TryGetItemInfoById(data.LocalId, out result))
        return false;
      data.From.RemoveItem(data.LocalId, true);
      if (!data.To.IsBatching)
        data.To.BeginBatch(true);
      data.To.BatchAddItem(result.Transform.Position, data.SubtypeId, true);
      return true;
    }

    private void StartTimer(int updateTimeS)
    {
      if (this.m_transferTime.HasValue)
        return;
      this.m_transferTime = new float?((float) updateTimeS);
    }

    public static void TransferItems(
      MyEnvironmentItems from,
      MyEnvironmentItems to,
      int localId,
      MyStringHash subtypeId,
      int timeS = 10)
    {
      MyEnvironmentItemsCoordinator.Static.AddTransferData(from, to, localId, subtypeId);
      MyEnvironmentItemsCoordinator.Static.StartTimer(timeS);
    }

    private void AddTransferData(
      MyEnvironmentItems from,
      MyEnvironmentItems to,
      int localId,
      MyStringHash subtypeId)
    {
      this.m_transferList.Add(new MyEnvironmentItemsCoordinator.TransferData()
      {
        From = from,
        To = to,
        LocalId = localId,
        SubtypeId = subtypeId
      });
    }

    private struct TransferData
    {
      public MyEnvironmentItems From;
      public MyEnvironmentItems To;
      public int LocalId;
      public MyStringHash SubtypeId;
    }
  }
}
