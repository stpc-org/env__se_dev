// Decompiled with JetBrains decompiler
// Type: VRage.Serialization.MySerializerQuaternion
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using VRage.Library.Collections;
using VRageMath;

namespace VRage.Serialization
{
  public class MySerializerQuaternion : MySerializer<Quaternion>
  {
    public override void Clone(ref Quaternion value)
    {
    }

    public override bool Equals(ref Quaternion a, ref Quaternion b) => a == b;

    public override void Read(BitStream stream, out Quaternion value, MySerializeInfo info)
    {
      if (info.IsNormalized)
      {
        int num1 = stream.ReadBool() ? 1 : 0;
        int num2 = stream.ReadBool() ? 1 : 0;
        bool flag1 = stream.ReadBool();
        bool flag2 = stream.ReadBool();
        ushort num3 = stream.ReadUInt16();
        ushort num4 = stream.ReadUInt16();
        ushort num5 = stream.ReadUInt16();
        value.X = (float) num3 / (float) ushort.MaxValue;
        value.Y = (float) num4 / (float) ushort.MaxValue;
        value.Z = (float) num5 / (float) ushort.MaxValue;
        if (num2 != 0)
          value.X = -value.X;
        if (flag1)
          value.Y = -value.Y;
        if (flag2)
          value.Z = -value.Z;
        float num6 = (float) (1.0 - (double) value.X * (double) value.X - (double) value.Y * (double) value.Y - (double) value.Z * (double) value.Z);
        if ((double) num6 < 0.0)
          num6 = 0.0f;
        value.W = (float) Math.Sqrt((double) num6);
        if (num1 == 0)
          return;
        value.W = -value.W;
      }
      else
      {
        value.X = stream.ReadFloat();
        value.Y = stream.ReadFloat();
        value.Z = stream.ReadFloat();
        value.W = stream.ReadFloat();
      }
    }

    public override void Write(BitStream stream, ref Quaternion value, MySerializeInfo info)
    {
      if (info.IsNormalized)
      {
        stream.WriteBool((double) value.W < 0.0);
        stream.WriteBool((double) value.X < 0.0);
        stream.WriteBool((double) value.Y < 0.0);
        stream.WriteBool((double) value.Z < 0.0);
        stream.WriteUInt16((ushort) ((double) Math.Abs(value.X) * (double) ushort.MaxValue));
        stream.WriteUInt16((ushort) ((double) Math.Abs(value.Y) * (double) ushort.MaxValue));
        stream.WriteUInt16((ushort) ((double) Math.Abs(value.Z) * (double) ushort.MaxValue));
      }
      else
      {
        stream.WriteFloat(value.X);
        stream.WriteFloat(value.Y);
        stream.WriteFloat(value.Z);
        stream.WriteFloat(value.W);
      }
    }
  }
}
