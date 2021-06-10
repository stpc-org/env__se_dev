// Decompiled with JetBrains decompiler
// Type: VRage.Utils.MyBBSetSampler
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System.Collections.Generic;
using VRageMath;

namespace VRage.Utils
{
  public class MyBBSetSampler
  {
    private MyBBSetSampler.IntervalSampler m_sampler;
    private BoundingBoxD m_bBox;

    public bool Valid => this.m_sampler == null ? this.m_bBox.Volume > 0.0 : this.m_sampler.TotalWeight > 0.0;

    public MyBBSetSampler(Vector3D min, Vector3D max)
    {
      Vector3D max1 = Vector3D.Max(min, max);
      Vector3D min1 = Vector3D.Min(min, max);
      this.m_bBox = new BoundingBoxD(min1, max1);
      this.m_sampler = new MyBBSetSampler.IntervalSampler(min1.X, max1.X, (max1.Y - min1.Y) * (max1.Z - min1.Z), Base6Directions.Axis.LeftRight);
    }

    public void SubtractBB(ref BoundingBoxD bb)
    {
      if (!this.m_bBox.Intersects(ref bb))
        return;
      BoundingBoxD bb1 = this.m_bBox.Intersect(bb);
      this.m_sampler.Subtract(ref this.m_bBox, ref bb1);
    }

    public Vector3D Sample()
    {
      MyBBSetSampler.IntervalSampler childSampler = this.m_sampler;
      Vector3D vector3D;
      vector3D.X = childSampler.Sample(out childSampler);
      vector3D.Y = childSampler == null ? MyUtils.GetRandomDouble(this.m_bBox.Min.Y, this.m_bBox.Max.Y) : childSampler.Sample(out childSampler);
      vector3D.Z = childSampler == null ? MyUtils.GetRandomDouble(this.m_bBox.Min.Z, this.m_bBox.Max.Z) : childSampler.Sample(out childSampler);
      return vector3D;
    }

    private class IntervalSampler
    {
      private Base6Directions.Axis m_axis;
      private double m_min;
      private double m_max;
      private double m_weightMult;
      private List<MyBBSetSampler.IntervalSampler.SamplingEntry> m_entries;
      private double m_totalWeight;

      public double TotalWeight => this.m_totalWeight;

      public IntervalSampler(
        double min,
        double max,
        double weightMultiplier,
        Base6Directions.Axis axis)
      {
        this.m_min = min;
        this.m_max = max;
        this.m_axis = axis;
        this.m_weightMult = weightMultiplier;
        this.m_totalWeight = weightMultiplier * (this.m_max - this.m_min);
        this.m_entries = new List<MyBBSetSampler.IntervalSampler.SamplingEntry>();
        this.m_entries.Add(new MyBBSetSampler.IntervalSampler.SamplingEntry(this.m_max, (MyBBSetSampler.IntervalSampler) null, this.m_totalWeight));
      }

      private IntervalSampler(MyBBSetSampler.IntervalSampler other, double t, bool clone)
      {
        this.m_min = other.m_min;
        this.m_max = other.m_max;
        this.m_axis = other.m_axis;
        this.m_weightMult = other.m_weightMult;
        this.m_totalWeight = other.m_totalWeight;
        this.m_entries = new List<MyBBSetSampler.IntervalSampler.SamplingEntry>((IEnumerable<MyBBSetSampler.IntervalSampler.SamplingEntry>) other.m_entries);
        for (int index = 0; index < other.m_entries.Count; ++index)
          this.m_entries[index] = new MyBBSetSampler.IntervalSampler.SamplingEntry(other.m_entries[index]);
        this.Multiply(t);
        if (clone)
          return;
        other.Multiply(1.0 - t);
      }

      private void Multiply(double t)
      {
        this.m_weightMult *= t;
        this.m_totalWeight *= t;
        for (int index = 0; index < this.m_entries.Count; ++index)
        {
          MyBBSetSampler.IntervalSampler.SamplingEntry entry = this.m_entries[index];
          entry.CumulativeWeight *= t;
          this.m_entries[index] = entry;
          if (entry.Sampler != null)
            entry.Sampler.Multiply(t);
        }
      }

      public void Subtract(ref BoundingBoxD originalBox, ref BoundingBoxD bb)
      {
        double min1;
        double max1;
        this.SelectMinMax(ref bb, this.m_axis, out min1, out max1);
        bool flag = false;
        double prevUpperLimit = this.m_min;
        double prevCumulativeWeight = 0.0;
        for (int index = 0; index < this.m_entries.Count; ++index)
        {
          MyBBSetSampler.IntervalSampler.SamplingEntry oldEntry = this.m_entries[index];
          if (!flag)
          {
            if (oldEntry.UpperLimit >= min1)
            {
              if (oldEntry.UpperLimit == min1)
              {
                flag = true;
              }
              else
              {
                if (prevUpperLimit == min1)
                {
                  flag = true;
                  --index;
                  continue;
                }
                flag = true;
                MyBBSetSampler.IntervalSampler.SamplingEntry samplingEntry = MyBBSetSampler.IntervalSampler.SamplingEntry.Divide(ref oldEntry, prevUpperLimit, prevCumulativeWeight, this.m_weightMult, min1);
                this.m_entries[index] = oldEntry;
                this.m_entries.Insert(index, samplingEntry);
                oldEntry = samplingEntry;
              }
            }
          }
          else if (prevUpperLimit < max1)
          {
            if (oldEntry.UpperLimit > max1)
            {
              MyBBSetSampler.IntervalSampler.SamplingEntry samplingEntry = MyBBSetSampler.IntervalSampler.SamplingEntry.Divide(ref oldEntry, prevUpperLimit, prevCumulativeWeight, this.m_weightMult, max1);
              this.m_entries[index] = oldEntry;
              this.m_entries.Insert(index, samplingEntry);
              oldEntry = samplingEntry;
            }
            if (oldEntry.UpperLimit <= max1)
            {
              if (oldEntry.Sampler == null)
              {
                if (this.m_axis == Base6Directions.Axis.ForwardBackward)
                {
                  oldEntry.Full = true;
                  oldEntry.CumulativeWeight = prevCumulativeWeight;
                }
                else if (!oldEntry.Full)
                {
                  Base6Directions.Axis axis = this.m_axis == Base6Directions.Axis.LeftRight ? Base6Directions.Axis.UpDown : Base6Directions.Axis.ForwardBackward;
                  double min2;
                  double max2;
                  this.SelectMinMax(ref originalBox, axis, out min2, out max2);
                  double num1 = this.m_max - this.m_min;
                  double num2 = this.m_weightMult * num1;
                  double num3 = (oldEntry.UpperLimit - prevUpperLimit) / num1;
                  double num4 = max2 - min2;
                  oldEntry.Sampler = new MyBBSetSampler.IntervalSampler(min2, max2, num2 * num3 / num4, axis);
                }
              }
              if (oldEntry.Sampler != null)
              {
                oldEntry.Sampler.Subtract(ref originalBox, ref bb);
                oldEntry.CumulativeWeight = prevCumulativeWeight + oldEntry.Sampler.TotalWeight;
              }
              this.m_entries[index] = oldEntry;
            }
          }
          else
          {
            oldEntry.CumulativeWeight = oldEntry.Sampler != null ? prevCumulativeWeight + oldEntry.Sampler.TotalWeight : (!oldEntry.Full ? prevCumulativeWeight + (oldEntry.UpperLimit - prevUpperLimit) * this.m_weightMult : prevCumulativeWeight);
            this.m_entries[index] = oldEntry;
          }
          prevUpperLimit = oldEntry.UpperLimit;
          prevCumulativeWeight = oldEntry.CumulativeWeight;
        }
        this.m_totalWeight = prevCumulativeWeight;
      }

      private void SelectMinMax(
        ref BoundingBoxD bb,
        Base6Directions.Axis axis,
        out double min,
        out double max)
      {
        if (axis == Base6Directions.Axis.UpDown)
        {
          min = bb.Min.Y;
          max = bb.Max.Y;
        }
        else if (axis == Base6Directions.Axis.ForwardBackward)
        {
          min = bb.Min.Z;
          max = bb.Max.Z;
        }
        else
        {
          min = bb.Min.X;
          max = bb.Max.X;
        }
      }

      public double Sample(out MyBBSetSampler.IntervalSampler childSampler)
      {
        double randomDouble = MyUtils.GetRandomDouble(0.0, this.TotalWeight);
        double num1 = this.m_min;
        double num2 = 0.0;
        for (int index = 0; index < this.m_entries.Count; ++index)
        {
          if (this.m_entries[index].CumulativeWeight >= randomDouble)
          {
            childSampler = this.m_entries[index].Sampler;
            double num3 = this.m_entries[index].CumulativeWeight - num2;
            double num4 = (randomDouble - num2) / num3;
            return num4 * this.m_entries[index].UpperLimit + (1.0 - num4) * num1;
          }
          num1 = this.m_entries[index].UpperLimit;
          num2 = this.m_entries[index].CumulativeWeight;
        }
        childSampler = (MyBBSetSampler.IntervalSampler) null;
        return this.m_max;
      }

      private struct SamplingEntry
      {
        public double UpperLimit;
        public double CumulativeWeight;
        public bool Full;
        public MyBBSetSampler.IntervalSampler Sampler;

        public SamplingEntry(double limit, MyBBSetSampler.IntervalSampler sampler, double weight)
        {
          this.UpperLimit = limit;
          this.Sampler = sampler;
          this.CumulativeWeight = weight;
          this.Full = false;
        }

        public SamplingEntry(MyBBSetSampler.IntervalSampler.SamplingEntry other)
        {
          this.UpperLimit = other.UpperLimit;
          this.CumulativeWeight = other.CumulativeWeight;
          this.Full = other.Full;
          if (other.Sampler == null)
            this.Sampler = (MyBBSetSampler.IntervalSampler) null;
          else
            this.Sampler = new MyBBSetSampler.IntervalSampler(other.Sampler, 1.0, true);
        }

        public static MyBBSetSampler.IntervalSampler.SamplingEntry Divide(
          ref MyBBSetSampler.IntervalSampler.SamplingEntry oldEntry,
          double prevUpperLimit,
          double prevCumulativeWeight,
          double weightMult,
          double newUpperLimit)
        {
          MyBBSetSampler.IntervalSampler.SamplingEntry samplingEntry = new MyBBSetSampler.IntervalSampler.SamplingEntry();
          samplingEntry.UpperLimit = newUpperLimit;
          double num1 = newUpperLimit - prevUpperLimit;
          double num2 = oldEntry.UpperLimit - newUpperLimit;
          double t = num1 / (num1 + num2);
          samplingEntry.Full = oldEntry.Full;
          if (oldEntry.Sampler != null)
          {
            samplingEntry.Sampler = new MyBBSetSampler.IntervalSampler(oldEntry.Sampler, t, false);
            samplingEntry.CumulativeWeight = prevCumulativeWeight + samplingEntry.Sampler.TotalWeight;
            oldEntry.CumulativeWeight = samplingEntry.CumulativeWeight + oldEntry.Sampler.TotalWeight;
          }
          else
          {
            samplingEntry.Sampler = (MyBBSetSampler.IntervalSampler) null;
            if (oldEntry.Full)
            {
              samplingEntry.CumulativeWeight = oldEntry.CumulativeWeight = prevCumulativeWeight;
            }
            else
            {
              samplingEntry.CumulativeWeight = prevCumulativeWeight + weightMult * num1;
              oldEntry.CumulativeWeight = samplingEntry.CumulativeWeight + weightMult * num2;
            }
          }
          return samplingEntry;
        }
      }
    }
  }
}
