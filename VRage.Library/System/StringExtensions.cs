// Decompiled with JetBrains decompiler
// Type: System.StringExtensions
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Runtime.CompilerServices;

namespace System
{
  public static class StringExtensions
  {
    public static unsafe bool Equals(this string text, char* compareTo, int length)
    {
      int index1 = Math.Min(length, text.Length);
      for (int index2 = 0; index2 < index1; ++index2)
      {
        if ((int) text[index2] != (int) compareTo[index2])
          return false;
      }
      return length <= index1 || compareTo[index1] == char.MinValue;
    }

    public static bool Contains(this string text, string testSequence, StringComparison comparison) => text.IndexOf(testSequence, comparison) != -1;

    public static unsafe long GetHashCode64(this string self)
    {
      string str = self;
      char* chPtr = (char*) str;
      if ((IntPtr) chPtr != IntPtr.Zero)
        chPtr += RuntimeHelpers.OffsetToStringData;
      int length = self.Length;
      long* numPtr1 = (long*) chPtr;
      long num1 = 1692801359929;
      ushort* numPtr2 = (ushort*) chPtr;
      for (; length >= 4; length -= 4)
      {
        num1 = (num1 << 5) + num1 + (num1 >> 59) ^ *numPtr1;
        ++numPtr1;
        numPtr2 += 4;
      }
      if (length > 0)
      {
        long num2 = 0;
        for (; length > 0; --length)
          num2 = num2 << 16 | (long) *numPtr2++;
        num1 = (num1 << 5) + num1 + (num1 >> 59) ^ num2;
      }
      return num1;
    }
  }
}
