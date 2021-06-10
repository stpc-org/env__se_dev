// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Replication.History.MySnapshotHistory
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game.Entity;
using VRage.Library.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Replication.History
{
  public class MySnapshotHistory
  {
    public static readonly MyTimeSpan DELAY = MyTimeSpan.FromMilliseconds(100.0);
    private static readonly MyTimeSpan MAX_EXTRAPOLATION_DELAY = MyTimeSpan.FromMilliseconds(5000.0);
    private readonly List<MySnapshotHistory.MyItem> m_history = new List<MySnapshotHistory.MyItem>();

    public int Count => this.m_history.Count;

    public bool Empty() => this.m_history.Count == 0;

    public void GetItem(MyTimeSpan clientTimestamp, out MySnapshotHistory.MyItem item)
    {
      if (this.m_history.Count > 0)
      {
        int index = this.FindIndex(clientTimestamp) - 1;
        if (index >= 0 && index < this.m_history.Count)
        {
          item = this.m_history[index];
          return;
        }
      }
      item = new MySnapshotHistory.MyItem();
    }

    public void Add(ref MySnapshot snapshot, MyTimeSpan timestamp)
    {
      if (this.FindExact(timestamp) != -1)
        return;
      MySnapshotHistory.MyItem myItem = new MySnapshotHistory.MyItem()
      {
        Valid = true,
        Type = MySnapshotHistory.SnapshotType.Exact,
        Timestamp = timestamp,
        Snapshot = snapshot
      };
      this.m_history.Insert(this.FindIndex(timestamp), myItem);
    }

    public void Get(MyTimeSpan clientTimestamp, out MySnapshotHistory.MyItem item)
    {
      if (this.m_history.Count == 0)
      {
        item = new MySnapshotHistory.MyItem();
      }
      else
      {
        MyTimeSpan timestamp = clientTimestamp;
        int index1 = this.FindIndex(timestamp);
        if (index1 < this.m_history.Count && timestamp == this.m_history[index1].Timestamp)
        {
          item = this.m_history[index1];
          item.Type = MySnapshotHistory.SnapshotType.Exact;
        }
        else if (index1 == 0)
        {
          item = this.m_history[0];
          if (timestamp == this.m_history[0].Timestamp)
            item.Type = MySnapshotHistory.SnapshotType.Exact;
          else if (timestamp < this.m_history[0].Timestamp)
            item.Type = MySnapshotHistory.SnapshotType.TooNew;
          else
            item.Type = MySnapshotHistory.SnapshotType.TooOld;
        }
        else if (index1 < this.m_history.Count && this.m_history.Count > 1)
        {
          this.Lerp(timestamp, index1 - 1, out item);
          item.Type = MySnapshotHistory.SnapshotType.Interpolation;
        }
        else if (this.m_history.Count > 1 && timestamp - this.m_history[this.m_history.Count - 1].Timestamp < MySnapshotHistory.MAX_EXTRAPOLATION_DELAY)
        {
          if (!this.m_history[this.m_history.Count - 1].Snapshot.Active)
          {
            item = this.m_history[this.m_history.Count - 1];
            item.Timestamp = timestamp;
          }
          else
          {
            int index2 = this.m_history.Count - 2;
            this.Lerp(timestamp, index2, out item);
            item.Type = MySnapshotHistory.SnapshotType.Extrapolation;
          }
        }
        else
        {
          item = this.m_history[this.m_history.Count - 1];
          item.Type = MySnapshotHistory.SnapshotType.TooOld;
        }
      }
    }

    public void Prune(MyTimeSpan clientTimestamp, MyTimeSpan delay, int leaveCount = 2) => this.m_history.RemoveRange(0, Math.Max(0, this.FindIndex(clientTimestamp - delay) - leaveCount));

    public void PruneTooOld(MyTimeSpan clientTimestamp) => this.Prune(clientTimestamp, MySnapshotHistory.MAX_EXTRAPOLATION_DELAY);

    private int FindIndex(MyTimeSpan timestamp)
    {
      int index = 0;
      while (index < this.m_history.Count && timestamp > this.m_history[index].Timestamp)
        ++index;
      return index;
    }

    private int FindExact(MyTimeSpan timestamp)
    {
      int index = 0;
      while (index < this.m_history.Count && timestamp != this.m_history[index].Timestamp)
        ++index;
      return index < this.m_history.Count ? index : -1;
    }

    private static float Factor(
      MyTimeSpan timestamp,
      ref MySnapshotHistory.MyItem item1,
      ref MySnapshotHistory.MyItem item2)
    {
      return (float) (timestamp - item1.Timestamp).Ticks / (float) (item2.Timestamp - item1.Timestamp).Ticks;
    }

    public static void Lerp(
      MyTimeSpan timestamp,
      ref MySnapshotHistory.MyItem item1,
      ref MySnapshotHistory.MyItem item2,
      out MySnapshotHistory.MyItem item)
    {
      float factor = MySnapshotHistory.Factor(timestamp, ref item1, ref item2);
      item = new MySnapshotHistory.MyItem()
      {
        Valid = true,
        Timestamp = timestamp
      };
      item1.Snapshot.Lerp(ref item2.Snapshot, factor, out item.Snapshot);
    }

    private void Lerp(MyTimeSpan timestamp, int index, out MySnapshotHistory.MyItem item)
    {
      MySnapshotHistory.MyItem myItem1;
      MySnapshotHistory.MyItem myItem2;
      if (this.GetItems(index, out myItem1, out myItem2))
        MySnapshotHistory.Lerp(timestamp, ref myItem1, ref myItem2, out item);
      else
        item = new MySnapshotHistory.MyItem()
        {
          Valid = false
        };
    }

    public bool GetItems(
      int index,
      out MySnapshotHistory.MyItem item1,
      out MySnapshotHistory.MyItem item2)
    {
      item1 = this.m_history[index];
      item2 = this.m_history[index + 1];
      if (item1.Snapshot.ParentId != item2.Snapshot.ParentId || item1.Snapshot.InheritRotation != item2.Snapshot.InheritRotation)
      {
        if (this.m_history.Count < index + 2)
        {
          ++index;
          item1 = item2;
          item2 = this.m_history[index + 1];
        }
        else if (index > 0)
        {
          --index;
          item2 = item1;
          item1 = this.m_history[index];
        }
      }
      return item1.Snapshot.ParentId == item2.Snapshot.ParentId && item1.Snapshot.InheritRotation == item2.Snapshot.InheritRotation;
    }

    public void ApplyDeltaPosition(MyTimeSpan timestamp, Vector3D positionDelta)
    {
      for (int index = 0; index < this.m_history.Count; ++index)
      {
        if (timestamp <= this.m_history[index].Timestamp)
        {
          MySnapshotHistory.MyItem myItem = this.m_history[index];
          myItem.Snapshot.Position += positionDelta;
          this.m_history[index] = myItem;
        }
      }
    }

    public void ApplyDeltaLinearVelocity(MyTimeSpan timestamp, Vector3 linearVelocityDelta)
    {
      for (int index = 0; index < this.m_history.Count; ++index)
      {
        if (timestamp <= this.m_history[index].Timestamp)
        {
          MySnapshotHistory.MyItem myItem = this.m_history[index];
          myItem.Snapshot.LinearVelocity += linearVelocityDelta;
          this.m_history[index] = myItem;
        }
      }
    }

    public void ApplyDeltaAngularVelocity(MyTimeSpan timestamp, Vector3 angularVelocityDelta)
    {
      for (int index = 0; index < this.m_history.Count; ++index)
      {
        if (timestamp <= this.m_history[index].Timestamp)
        {
          MySnapshotHistory.MyItem myItem = this.m_history[index];
          myItem.Snapshot.AngularVelocity += angularVelocityDelta;
          this.m_history[index] = myItem;
        }
      }
    }

    public void ApplyDeltaRotation(MyTimeSpan timestamp, Quaternion rotationDelta)
    {
      for (int index = 0; index < this.m_history.Count; ++index)
      {
        if (timestamp <= this.m_history[index].Timestamp)
        {
          MySnapshotHistory.MyItem myItem = this.m_history[index];
          myItem.Snapshot.Rotation *= Quaternion.Inverse(rotationDelta);
          myItem.Snapshot.Rotation.Normalize();
          this.m_history[index] = myItem;
        }
      }
    }

    public void ApplyDelta(MyTimeSpan timestamp, ref MySnapshot delta)
    {
      for (int index = 0; index < this.m_history.Count; ++index)
      {
        if (timestamp <= this.m_history[index].Timestamp)
        {
          MySnapshotHistory.MyItem myItem = this.m_history[index];
          myItem.Snapshot.Add(ref delta);
          this.m_history[index] = myItem;
        }
      }
    }

    public void Reset() => this.m_history.Clear();

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < this.m_history.Count; ++index)
        stringBuilder.Append(this.m_history[index].Timestamp.ToString() + " (" + this.m_history[index].Snapshot.Position.ToString("N3") + ") ");
      return stringBuilder.ToString();
    }

    public string ToStringRotation()
    {
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < this.m_history.Count; ++index)
        stringBuilder.Append(this.m_history[index].Timestamp.ToString() + " (" + this.m_history[index].Snapshot.Rotation.ToStringAxisAngle("N3") + ") ");
      return stringBuilder.ToString();
    }

    public string ToStringTimestamps()
    {
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < this.m_history.Count; ++index)
        stringBuilder.Append(this.m_history[index].Timestamp.ToString() + " ");
      return stringBuilder.ToString();
    }

    public void GetLast(out MySnapshotHistory.MyItem item, int index = 0) => item = this.m_history.Count < index + 1 ? new MySnapshotHistory.MyItem() : this.m_history[this.m_history.Count - index - 1];

    public void GetFirst(out MySnapshotHistory.MyItem item) => item = this.m_history.Count > 0 ? this.m_history[0] : new MySnapshotHistory.MyItem();

    public bool IsLastActive() => this.m_history.Count >= 1 && this.m_history[this.m_history.Count - 1].Snapshot.Active;

    public MyTimeSpan GetLastTimestamp() => this.m_history[this.m_history.Count - 1].Timestamp;

    public long GetLastParentId() => this.m_history[this.m_history.Count - 1].Snapshot.ParentId;

    public void DebugDrawPos(MyEntity entity, MyTimeSpan timestamp, Color color)
    {
      int index = 0;
      MatrixD? nullable = new MatrixD?();
      for (; index < this.m_history.Count; ++index)
      {
        if (timestamp <= this.m_history[index].Timestamp)
        {
          MatrixD mat;
          this.m_history[index].Snapshot.GetMatrix(entity, out mat);
          MyRenderProxy.DebugDrawAxis(mat, 0.2f, false);
          if (nullable.HasValue)
            MyRenderProxy.DebugDrawArrow3D(nullable.Value.Translation, mat.Translation, color);
          nullable = new MatrixD?(mat);
        }
      }
    }

    public enum SnapshotType
    {
      Exact,
      TooNew,
      Interpolation,
      Extrapolation,
      TooOld,
      Reset,
    }

    public struct MyItem
    {
      public bool Valid;
      public MySnapshotHistory.SnapshotType Type;
      public MyTimeSpan Timestamp;
      public MySnapshot Snapshot;

      public override string ToString() => "Item timestamp: " + (object) this.Timestamp;
    }
  }
}
