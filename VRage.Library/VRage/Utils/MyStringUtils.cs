// Decompiled with JetBrains decompiler
// Type: VRage.Utils.MyStringUtils
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Text;

namespace VRage.Utils
{
  public static class MyStringUtils
  {
    public const string OPEN_SQUARE_BRACKET = "U+005B";
    public const string CLOSED_SQUARE_BRACKET = "U+005D";

    public static string UpdateControlsToNotificationFriendly(this string text) => text.Replace("[", "U+005B").Replace("]", "U+005D");

    public static string UpdateControlsFromNotificationFriendly(this string text) => text.Replace("U+005B", "[").Replace("U+005D", "]");

    public static StringBuilder UpdateControlsFromNotificationFriendly(
      this StringBuilder text)
    {
      return text.Replace("U+005B", "[").Replace("U+005D", "]");
    }

    public static unsafe int GetUniversalHashCode(this string str)
    {
      fixed (char* chPtr1 = str)
      {
        int num1 = 5381;
        int num2 = num1;
        int num3;
        for (char* chPtr2 = chPtr1; (num3 = (int) *chPtr2) != 0; chPtr2 += 2)
        {
          num1 = (num1 << 5) + num1 ^ num3;
          int num4 = (int) chPtr2[1];
          if (num4 != 0)
            num2 = (num2 << 5) + num2 ^ num4;
          else
            break;
        }
        return num1 + num2 * 1566083941;
      }
    }
  }
}
