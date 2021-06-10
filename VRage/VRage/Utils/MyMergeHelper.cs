// Decompiled with JetBrains decompiler
// Type: VRage.Utils.MyMergeHelper
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Reflection;

namespace VRage.Utils
{
  public static class MyMergeHelper
  {
    public static void Merge<T>(T self, T source, T other) where T : class
    {
      if ((object) self == null)
        MyLog.Default.WriteLine("self cannot be null!!! type: " + (object) typeof (T));
      if ((object) source == null)
        MyLog.Default.WriteLine("Source cannot be null!!! type: " + (object) typeof (T));
      if ((object) other == null)
        MyLog.Default.WriteLine("Other cannot be null!!! type: " + (object) typeof (T));
      object self1 = (object) self;
      object source1 = (object) source;
      object other1 = (object) other;
      MyMergeHelper.MergeInternal(typeof (T), ref self1, ref source1, ref other1);
    }

    public static void Merge<T>(ref T self, ref T source, ref T other) where T : struct
    {
      object self1 = (object) self;
      object source1 = (object) source;
      object other1 = (object) other;
      MyMergeHelper.MergeInternal(typeof (T), ref self1, ref source1, ref other1);
      self = (T) self1;
    }

    private static void MergeInternal(
      Type type,
      ref object self,
      ref object source,
      ref object other)
    {
      if (type == (Type) null)
        MyLog.Default.WriteLine("type cannot be null!!! self: " + self + " source: " + source + " other: " + other);
      if (self == null)
        self = Activator.CreateInstance(type);
      if (source == null)
        source = Activator.CreateInstance(type);
      foreach (FieldInfo field in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
      {
        object source1 = field.GetValue(source);
        object other1 = field.GetValue(other);
        if (source1 != other1)
        {
          if (source1 == null)
          {
            MyLog.Default.WriteLine("ERROR: Error detected related to the following resource: " + other1 + " Please check your definition files and reload");
            MyLog.Default.WriteLine("More info MergeInternal: field: " + (object) field + " source: " + source + " , other: " + other + " , valueOther: " + other1);
          }
          else
          {
            bool flag = false;
            if (MyMergeHelper.IsPrimitive(field.FieldType) && !(flag = source1.Equals(other1)) || source1 != null && other1 == null)
              field.SetValue(self, source1);
            else if (!flag)
            {
              object self1 = field.GetValue(self);
              MyMergeHelper.MergeInternal(field.FieldType, ref self1, ref source1, ref other1);
              field.SetValue(self, self1);
            }
          }
        }
      }
    }

    private static bool IsPrimitive(Type type) => type.IsPrimitive || type == typeof (string) || type == typeof (Type);
  }
}
