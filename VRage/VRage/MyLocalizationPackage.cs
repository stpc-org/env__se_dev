// Decompiled with JetBrains decompiler
// Type: VRage.MyLocalizationPackage
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using VRage.Utils;

namespace VRage
{
  public sealed class MyLocalizationPackage
  {
    private readonly Dictionary<MyStringId, MyLocalizationPackage.Entry> m_entries = new Dictionary<MyStringId, MyLocalizationPackage.Entry>((IEqualityComparer<MyStringId>) MyStringId.Comparer);
    private readonly ConcurrentDictionary<string, StringBuilder> m_stringBuilderCache = new ConcurrentDictionary<string, StringBuilder>();
    public static Func<string, bool> ValidateVariantName;

    public ICollection<MyStringId> Keys => (ICollection<MyStringId>) this.m_entries.Keys;

    public bool AddMessage(string key, string message, bool overwrite = false)
    {
      int length1 = key.IndexOf(":", StringComparison.Ordinal);
      MyStringId orCompute;
      MyStringId variantId;
      if (length1 > 0)
      {
        orCompute = MyStringId.GetOrCompute(key.Substring(0, length1));
        string str = key.Substring(length1 + 1);
        Func<string, bool> validateVariantName = MyLocalizationPackage.ValidateVariantName;
        variantId = (validateVariantName != null ? (!validateVariantName(str) ? 1 : 0) : 0) == 0 ? MyStringId.GetOrCompute(str) : throw new ArgumentException("Variant name '" + str + "' is not valid", nameof (key));
      }
      else
      {
        orCompute = MyStringId.GetOrCompute(key);
        variantId = MyStringId.NullOrEmpty;
      }
      MyLocalizationPackage.Entry entry;
      bool flag = this.m_entries.TryGetValue(orCompute, out entry);
      if (!flag)
        entry.Variants = Array.Empty<MyLocalizationPackage.Variant>();
      if (variantId == MyStringId.NullOrEmpty)
      {
        if (!flag | overwrite || entry.Message == null)
          entry.Message = message;
      }
      else
      {
        flag = false;
        for (int index = 0; index < entry.Variants.Length; ++index)
        {
          if (variantId == entry.Variants[index].VariantId)
          {
            if (overwrite)
              entry.Variants[index].Message = message;
            flag = true;
          }
        }
        if (!flag)
        {
          int length2 = entry.Variants.Length;
          Array.Resize<MyLocalizationPackage.Variant>(ref entry.Variants, length2 + 1);
          entry.Variants[length2] = new MyLocalizationPackage.Variant(variantId, message);
        }
      }
      this.m_entries[orCompute] = entry;
      return !flag;
    }

    public void Clear()
    {
      this.m_entries.Clear();
      this.m_stringBuilderCache.Clear();
    }

    public bool ContainsKey(MyStringId key) => this.m_entries.ContainsKey(key);

    public bool TryGet(MyStringId key, MyStringId variant, out string message)
    {
      MyLocalizationPackage.Entry entry;
      if (this.m_entries.TryGetValue(key, out entry))
      {
        if (variant != MyStringId.NullOrEmpty)
        {
          for (int index = 0; index < entry.Variants.Length; ++index)
          {
            if (variant == entry.Variants[index].VariantId)
            {
              message = entry.Variants[index].Message;
              return true;
            }
          }
        }
        message = entry.Message;
        return true;
      }
      message = (string) null;
      return false;
    }

    public bool TryGetStringBuilder(
      MyStringId key,
      MyStringId variant,
      out StringBuilder messageSb)
    {
      string message;
      if (this.TryGet(key, variant, out message))
      {
        if (!this.m_stringBuilderCache.TryGetValue(message, out messageSb))
          this.m_stringBuilderCache.TryAdd(message, messageSb = new StringBuilder(message));
        return true;
      }
      messageSb = (StringBuilder) null;
      return false;
    }

    private struct Entry
    {
      public string Message;
      public MyLocalizationPackage.Variant[] Variants;
    }

    private struct Variant
    {
      public MyStringId VariantId;
      public string Message;

      public Variant(MyStringId variantId, string message)
      {
        this.VariantId = variantId;
        this.Message = message;
      }
    }
  }
}
