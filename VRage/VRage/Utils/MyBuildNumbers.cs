// Decompiled with JetBrains decompiler
// Type: VRage.Utils.MyBuildNumbers
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System.IO;

namespace VRage.Utils
{
  public static class MyBuildNumbers
  {
    private const int LENGTH_MAJOR = 2;
    private const int LENGTH_MINOR1 = 3;
    private const int LENGTH_MINOR2 = 3;
    public const string SEPARATOR = "_";

    public static int GetBuildNumberWithoutMajor(int buildNumberInt)
    {
      int num = 1;
      for (int index = 0; index < 6; ++index)
        num *= 10;
      return buildNumberInt - buildNumberInt / num * num;
    }

    public static string ConvertBuildNumberFromIntToString(int buildNumberInt) => MyBuildNumbers.ConvertBuildNumberFromIntToString(buildNumberInt, "_");

    public static string ConvertBuildNumberFromIntToString(int buildNumberInt, string separator)
    {
      string right = MyUtils.AlignIntToRight(buildNumberInt, 8, '0');
      return right.Substring(0, 2) + separator + right.Substring(2, 3) + separator + right.Substring(5, 3);
    }

    public static string ConvertBuildNumberFromIntToStringFriendly(
      int buildNumberInt,
      string separator)
    {
      int num = 1;
      string right = MyUtils.AlignIntToRight(buildNumberInt, num + 3 + 3, '0');
      return right.Substring(0, num) + separator + right.Substring(num, 3) + separator + right.Substring(num + 3, 3);
    }

    public static bool IsValidBuildNumber(string buildNumberString) => MyBuildNumbers.ConvertBuildNumberFromStringToInt(buildNumberString).HasValue;

    public static int? ConvertBuildNumberFromStringToInt(string buildNumberString)
    {
      if (buildNumberString.Length < 2 * "_".Length + 2 + 3 + 3)
        return new int?();
      if (buildNumberString.Substring(2, "_".Length) != "_" || buildNumberString.Substring(2 + "_".Length + 3, "_".Length) != "_")
        return new int?();
      string s1 = buildNumberString.Substring(0, 2);
      string s2 = buildNumberString.Substring(2 + "_".Length, 3);
      string s3 = buildNumberString.Substring(2 + "_".Length + 3 + "_".Length, 3);
      if (!int.TryParse(s1, out int _))
        return new int?();
      if (!int.TryParse(s2, out int _))
        return new int?();
      return !int.TryParse(s3, out int _) ? new int?() : new int?(int.Parse(s1 + s2 + s3));
    }

    public static int? GetBuildNumberFromFileName(
      string filename,
      string executableFileName,
      string extensionName)
    {
      if (filename.Length < executableFileName.Length + 3 * "_".Length + 2 + 3 + 3)
        return new int?();
      if (filename.Substring(executableFileName.Length, "_".Length) != "_")
        return new int?();
      return new FileInfo(filename).Extension != extensionName ? new int?() : MyBuildNumbers.ConvertBuildNumberFromStringToInt(filename.Substring(executableFileName.Length + "_".Length, 2 + "_".Length + 3 + "_".Length + 3));
    }

    public static string GetFilenameFromBuildNumber(int buildNumber, string executableFileName) => executableFileName + "_" + MyBuildNumbers.ConvertBuildNumberFromIntToString(buildNumber) + ".exe";
  }
}
