// Decompiled with JetBrains decompiler
// Type: VRage.Replication.MyPacketTracker
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;

namespace VRage.Replication
{
  public class MyPacketTracker
  {
    private const int BUFFER_LENGTH = 5;
    private readonly List<byte> m_ids = new List<byte>();

    public MyPacketStatistics Statistics { get; set; }

    public MyPacketTracker.OrderType Add(byte id)
    {
      if (this.m_ids.Count == 1 && (int) id == (int) (byte) ((uint) this.m_ids[0] + 1U))
      {
        this.m_ids[0] = id;
        return MyPacketTracker.OrderType.InOrder;
      }
      if (this.m_ids.FindIndex((Predicate<byte>) (x => (int) x == (int) id)) != -1)
        return MyPacketTracker.OrderType.Duplicate;
      this.m_ids.Add(id);
      for (int index = 2; index < this.m_ids.Count; ++index)
      {
        if ((int) (byte) ((uint) this.m_ids[0] + 1U) == (int) this.m_ids[index])
        {
          this.m_ids.RemoveAt(index);
          this.m_ids.RemoveAt(0);
          this.CleanUp();
          return MyPacketTracker.OrderType.OutOfOrder;
        }
      }
      if (this.m_ids.Count < 5)
        return MyPacketTracker.OrderType.InOrder;
      int id1 = (int) this.m_ids[0];
      this.m_ids.RemoveAt(0);
      int id2 = (int) this.m_ids[0];
      this.CleanUp();
      int num = id1;
      return (MyPacketTracker.OrderType) (3 + ((int) (byte) (id2 - num) - 2));
    }

    private void CleanUp()
    {
      byte num = 0;
      bool flag1 = true;
      bool flag2 = true;
      foreach (byte id in this.m_ids)
      {
        flag2 = ((flag2 ? 1 : 0) & (flag1 ? 1 : ((int) (byte) ((uint) num + 1U) == (int) id ? 1 : 0))) != 0;
        num = id;
        flag1 = false;
      }
      if (!flag2)
        return;
      this.m_ids.RemoveRange(0, this.m_ids.Count - 1);
    }

    public enum OrderType
    {
      InOrder,
      OutOfOrder,
      Duplicate,
      Drop1,
      Drop2,
      Drop3,
      Drop4,
      DropX,
    }
  }
}
