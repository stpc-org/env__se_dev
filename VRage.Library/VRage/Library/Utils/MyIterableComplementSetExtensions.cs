// Decompiled with JetBrains decompiler
// Type: VRage.Library.Utils.MyIterableComplementSetExtensions
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

namespace VRage.Library.Utils
{
  public static class MyIterableComplementSetExtensions
  {
    public static void AddOrEnsureOnComplement<T>(this MyIterableComplementSet<T> self, T item)
    {
      if (!self.Contains(item))
      {
        self.AddToComplement(item);
      }
      else
      {
        if (self.IsInComplement(item))
          return;
        self.MoveToComplement(item);
      }
    }

    public static void AddOrEnsureOnSet<T>(this MyIterableComplementSet<T> self, T item)
    {
      if (!self.Contains(item))
      {
        self.Add(item);
      }
      else
      {
        if (!self.IsInComplement(item))
          return;
        self.MoveToSet(item);
      }
    }

    public static void EnsureOnComplementIfContained<T>(
      this MyIterableComplementSet<T> self,
      T item)
    {
      if (!self.Contains(item) || self.IsInComplement(item))
        return;
      self.MoveToComplement(item);
    }

    public static void EnsureOnSetIfContained<T>(this MyIterableComplementSet<T> self, T item)
    {
      if (!self.Contains(item) || !self.IsInComplement(item))
        return;
      self.MoveToSet(item);
    }

    public static void RemoveIfContained<T>(this MyIterableComplementSet<T> self, T item)
    {
      if (!self.Contains(item))
        return;
      self.Remove(item);
    }
  }
}
