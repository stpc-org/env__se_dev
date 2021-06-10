// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.Ingame.Utilities.MyIniValue
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace VRage.Game.ModAPI.Ingame.Utilities
{
  public struct MyIniValue
  {
    public static readonly MyIniValue EMPTY = new MyIniValue();
    private static readonly char[] NEWLINE_CHARS = new char[2]
    {
      '\r',
      '\n'
    };
    private readonly string m_value;
    public readonly MyIniKey Key;

    public MyIniValue(MyIniKey key, string value)
    {
      if (key.IsEmpty)
        throw new ArgumentException("Configuration value cannot use an empty key", nameof (key));
      this.m_value = value ?? "";
      this.Key = key;
    }

    public bool IsEmpty => this.m_value == null;

    public bool ToBoolean(bool defaultValue = false)
    {
      bool flag;
      return !this.TryGetBoolean(out flag) ? defaultValue : flag;
    }

    public bool TryGetBoolean(out bool value)
    {
      string b = this.m_value;
      if (b == null)
      {
        value = false;
        return false;
      }
      if (string.Equals("true", b, StringComparison.OrdinalIgnoreCase) || string.Equals("yes", b, StringComparison.OrdinalIgnoreCase) || (string.Equals("1", b, StringComparison.OrdinalIgnoreCase) || string.Equals("on", b, StringComparison.OrdinalIgnoreCase)))
      {
        value = true;
        return true;
      }
      if (string.Equals("false", b, StringComparison.OrdinalIgnoreCase) || string.Equals("no", b, StringComparison.OrdinalIgnoreCase) || (string.Equals("0", b, StringComparison.OrdinalIgnoreCase) || string.Equals("off", b, StringComparison.OrdinalIgnoreCase)))
      {
        value = false;
        return true;
      }
      value = false;
      return false;
    }

    public char ToChar(char defaultValue = '\0')
    {
      char ch;
      return this.TryGetChar(out ch) ? ch : defaultValue;
    }

    public bool TryGetChar(out char value)
    {
      if (this.m_value == null)
      {
        value = char.MinValue;
        return false;
      }
      if (this.m_value.Length == 1)
      {
        value = this.m_value[0];
        return true;
      }
      value = char.MinValue;
      return false;
    }

    public sbyte ToSByte(sbyte defaultValue = 0)
    {
      sbyte num;
      return !this.TryGetSByte(out num) ? defaultValue : num;
    }

    public bool TryGetSByte(out sbyte value)
    {
      if (this.m_value != null)
        return sbyte.TryParse(this.m_value, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out value);
      value = (sbyte) 0;
      return false;
    }

    public byte ToByte(byte defaultValue = 0)
    {
      byte num;
      return !this.TryGetByte(out num) ? defaultValue : num;
    }

    public bool TryGetByte(out byte value)
    {
      if (this.m_value != null)
        return byte.TryParse(this.m_value, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out value);
      value = (byte) 0;
      return false;
    }

    public ushort ToUInt16(ushort defaultValue = 0)
    {
      ushort num;
      return !this.TryGetUInt16(out num) ? defaultValue : num;
    }

    public bool TryGetUInt16(out ushort value)
    {
      if (this.m_value != null)
        return ushort.TryParse(this.m_value, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out value);
      value = (ushort) 0;
      return false;
    }

    public short ToInt16(short defaultValue = 0)
    {
      short num;
      return !this.TryGetInt16(out num) ? defaultValue : num;
    }

    public bool TryGetInt16(out short value)
    {
      if (this.m_value != null)
        return short.TryParse(this.m_value, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out value);
      value = (short) 0;
      return false;
    }

    public uint ToUInt32(uint defaultValue = 0)
    {
      uint num;
      return !this.TryGetUInt32(out num) ? defaultValue : num;
    }

    public bool TryGetUInt32(out uint value)
    {
      if (this.m_value != null)
        return uint.TryParse(this.m_value, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out value);
      value = 0U;
      return false;
    }

    public int ToInt32(int defaultValue = 0)
    {
      int num;
      return !this.TryGetInt32(out num) ? defaultValue : num;
    }

    public bool TryGetInt32(out int value)
    {
      if (this.m_value != null)
        return int.TryParse(this.m_value, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out value);
      value = 0;
      return false;
    }

    public ulong ToUInt64(ulong defaultValue = 0)
    {
      ulong num;
      return !this.TryGetUInt64(out num) ? defaultValue : num;
    }

    public bool TryGetUInt64(out ulong value)
    {
      if (this.m_value != null)
        return ulong.TryParse(this.m_value, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out value);
      value = 0UL;
      return false;
    }

    public long ToInt64(long defaultValue = 0)
    {
      long num;
      return !this.TryGetInt64(out num) ? defaultValue : num;
    }

    public bool TryGetInt64(out long value)
    {
      if (this.m_value != null)
        return long.TryParse(this.m_value, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out value);
      value = 0L;
      return false;
    }

    public float ToSingle(float defaultValue = 0.0f)
    {
      float num;
      return this.TryGetSingle(out num) ? num : defaultValue;
    }

    public bool TryGetSingle(out float value)
    {
      if (this.m_value != null)
        return float.TryParse(this.m_value, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out value);
      value = 0.0f;
      return false;
    }

    public double ToDouble(double defaultValue = 0.0)
    {
      double num;
      return !this.TryGetDouble(out num) ? defaultValue : num;
    }

    public bool TryGetDouble(out double value)
    {
      if (this.m_value != null)
        return double.TryParse(this.m_value, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out value);
      value = 0.0;
      return false;
    }

    public Decimal ToDecimal(Decimal defaultValue = 0M)
    {
      Decimal num;
      return !this.TryGetDecimal(out num) ? defaultValue : num;
    }

    public bool TryGetDecimal(out Decimal value)
    {
      if (this.m_value != null)
        return Decimal.TryParse(this.m_value, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out value);
      value = 0M;
      return false;
    }

    public void GetLines(List<string> lines)
    {
      if (lines == null)
        return;
      string str = this.m_value;
      if (string.IsNullOrEmpty(str))
        return;
      lines.Clear();
      int num1 = 0;
      int num2 = 0;
label_10:
      while (num2 < str.Length)
      {
        num2 = str.IndexOfAny(MyIniValue.NEWLINE_CHARS, num1);
        if (num2 < 0)
        {
          lines.Add(str.Substring(num1, str.Length - num1));
          break;
        }
        lines.Add(str.Substring(num1, num2 - num1));
        num1 = num2 + 1;
        while (true)
        {
          if (num1 < str.Length && Array.IndexOf<char>(MyIniValue.NEWLINE_CHARS, str[num1]) >= 0)
            ++num1;
          else
            goto label_10;
        }
      }
    }

    public override string ToString() => this.m_value ?? "";

    public string ToString(string defaultValue) => this.m_value ?? defaultValue ?? "";

    public bool TryGetString(out string value)
    {
      value = this.m_value;
      return value != null;
    }

    public void Write(StringBuilder stringBuilder)
    {
      if (stringBuilder == null)
        throw new ArgumentNullException(nameof (stringBuilder));
      stringBuilder.Append(this.m_value ?? "");
    }
  }
}
