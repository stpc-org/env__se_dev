// Decompiled with JetBrains decompiler
// Type: VRage.Utils.MyValueFormatter
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Globalization;
using System.Text;

namespace VRage.Utils
{
  public class MyValueFormatter
  {
    private static NumberFormatInfo m_numberFormatInfoHelper;
    private static readonly string[] m_genericUnitNames = new string[5]
    {
      "",
      "k",
      "M",
      "G",
      "T"
    };
    private static readonly float[] m_genericUnitMultipliers = new float[5]
    {
      1f,
      1000f,
      1000000f,
      1E+09f,
      1E+12f
    };
    private static readonly int[] m_genericUnitDigits = new int[5]
    {
      1,
      1,
      1,
      1,
      1
    };
    private static readonly string[] m_forceUnitNames = new string[9]
    {
      "N",
      "kN",
      "MN",
      "GN",
      "TN",
      "PN",
      "EN",
      "ZN",
      "YN"
    };
    private static readonly float[] m_forceUnitMultipliers = new float[9]
    {
      1f,
      1000f,
      1000000f,
      1E+09f,
      1E+12f,
      1E+15f,
      1E+18f,
      1E+21f,
      1E+24f
    };
    private static readonly int[] m_forceUnitDigits = new int[9]
    {
      1,
      1,
      1,
      1,
      1,
      1,
      1,
      1,
      1
    };
    private static readonly string[] m_torqueUnitNames = new string[3]
    {
      "Nm",
      "kNm",
      "MNm"
    };
    private static readonly float[] m_torqueUnitMultipliers = new float[3]
    {
      1f,
      1000f,
      1000000f
    };
    private static readonly int[] m_torqueUnitDigits = new int[3]
    {
      0,
      1,
      1
    };
    private static readonly string[] m_workUnitNames = new string[4]
    {
      "W",
      "kW",
      "MW",
      "GW"
    };
    private static readonly float[] m_workUnitMultipliers = new float[4]
    {
      1E-06f,
      1f / 1000f,
      1f,
      1000f
    };
    private static readonly int[] m_workUnitDigits = new int[4]
    {
      0,
      2,
      2,
      2
    };
    private static readonly string[] m_workHoursUnitNames = new string[4]
    {
      "Wh",
      "kWh",
      "MWh",
      "GWh"
    };
    private static readonly float[] m_workHoursUnitMultipliers = new float[4]
    {
      1E-06f,
      1f / 1000f,
      1f,
      1000f
    };
    private static readonly int[] m_workHoursUnitDigits = new int[4]
    {
      0,
      2,
      2,
      2
    };
    private static readonly string[] m_timeUnitNames = new string[5]
    {
      "Unit_sec",
      "Unit_min",
      "Unit_hours",
      "Unit_days",
      "Unit_years"
    };
    private static readonly float[] m_timeUnitMultipliers = new float[5]
    {
      1f,
      60f,
      3600f,
      86400f,
      3.1536E+07f
    };
    private static readonly int[] m_timeUnitDigits = new int[5];
    private static readonly string[] m_weightUnitNames = new string[4]
    {
      "g",
      "kg",
      "T",
      "MT"
    };
    private static readonly float[] m_weightUnitMultipliers = new float[4]
    {
      1f / 1000f,
      1f,
      1000f,
      1000000f
    };
    private static readonly int[] m_weightUnitDigits = new int[4]
    {
      0,
      2,
      2,
      2
    };
    private static readonly string[] m_volumeUnitNames = new string[6]
    {
      "mL",
      "cL",
      "dL",
      "L",
      "hL",
      "m\x00B3"
    };
    private static readonly float[] m_volumeUnitMultipliers = new float[6]
    {
      1E-06f,
      1E-05f,
      0.0001f,
      1f / 1000f,
      0.1f,
      1f
    };
    private static readonly int[] m_volumeUnitDigits = new int[6]
    {
      0,
      0,
      0,
      0,
      2,
      1
    };
    private static readonly string[] m_distanceUnitNames = new string[4]
    {
      "mm",
      "cm",
      "m",
      "km"
    };
    private static readonly float[] m_distanceUnitMultipliers = new float[4]
    {
      1f / 1000f,
      0.01f,
      1f,
      1000f
    };
    private static readonly int[] m_distanceUnitDigits = new int[4]
    {
      0,
      1,
      2,
      2
    };

    static MyValueFormatter()
    {
      MyValueFormatter.m_numberFormatInfoHelper = new NumberFormatInfo();
      MyValueFormatter.m_numberFormatInfoHelper.NumberDecimalSeparator = ".";
      MyValueFormatter.m_numberFormatInfoHelper.NumberGroupSeparator = " ";
    }

    public static string GetFormatedFloat(float num, int decimalDigits)
    {
      MyValueFormatter.m_numberFormatInfoHelper.NumberDecimalDigits = decimalDigits;
      return num.ToString("N", (IFormatProvider) MyValueFormatter.m_numberFormatInfoHelper);
    }

    public static string GetFormatedFloat(float num, int decimalDigits, string groupSeparator)
    {
      string numberGroupSeparator = MyValueFormatter.m_numberFormatInfoHelper.NumberGroupSeparator;
      MyValueFormatter.m_numberFormatInfoHelper.NumberGroupSeparator = groupSeparator;
      MyValueFormatter.m_numberFormatInfoHelper.NumberDecimalDigits = decimalDigits;
      string str = num.ToString("N", (IFormatProvider) MyValueFormatter.m_numberFormatInfoHelper);
      MyValueFormatter.m_numberFormatInfoHelper.NumberGroupSeparator = numberGroupSeparator;
      return str;
    }

    public static string GetFormatedDouble(double num, int decimalDigits)
    {
      MyValueFormatter.m_numberFormatInfoHelper.NumberDecimalDigits = decimalDigits;
      return num.ToString("N", (IFormatProvider) MyValueFormatter.m_numberFormatInfoHelper);
    }

    public static string GetFormatedQP(Decimal num) => MyValueFormatter.GetFormatedDecimal(num, 1);

    public static string GetFormatedDecimal(Decimal num, int decimalDigits)
    {
      MyValueFormatter.m_numberFormatInfoHelper.NumberDecimalDigits = decimalDigits;
      return num.ToString("N", (IFormatProvider) MyValueFormatter.m_numberFormatInfoHelper);
    }

    public static string GetFormatedGameMoney(Decimal num) => MyValueFormatter.GetFormatedDecimal(num, 2);

    public static Decimal GetDecimalFromString(string number, int decimalDigits)
    {
      try
      {
        MyValueFormatter.m_numberFormatInfoHelper.NumberDecimalDigits = decimalDigits;
        return Math.Round(Convert.ToDecimal(number, (IFormatProvider) MyValueFormatter.m_numberFormatInfoHelper), decimalDigits);
      }
      catch
      {
      }
      return 0M;
    }

    public static float? GetFloatFromString(
      string number,
      int decimalDigits,
      string groupSeparator)
    {
      float? nullable = new float?();
      string numberGroupSeparator = MyValueFormatter.m_numberFormatInfoHelper.NumberGroupSeparator;
      MyValueFormatter.m_numberFormatInfoHelper.NumberGroupSeparator = groupSeparator;
      MyValueFormatter.m_numberFormatInfoHelper.NumberDecimalDigits = decimalDigits;
      try
      {
        nullable = new float?((float) Math.Round(Convert.ToDouble(number, (IFormatProvider) MyValueFormatter.m_numberFormatInfoHelper), decimalDigits));
      }
      catch
      {
      }
      MyValueFormatter.m_numberFormatInfoHelper.NumberGroupSeparator = numberGroupSeparator;
      return nullable;
    }

    public static string GetFormatedLong(long l) => l.ToString("#,0", (IFormatProvider) CultureInfo.InvariantCulture);

    public static string GetFormatedDateTimeOffset(DateTimeOffset dt) => dt.ToString("yyyy-MM-dd HH:mm:ss.fff", (IFormatProvider) DateTimeFormatInfo.InvariantInfo);

    public static string GetFormatedDateTime(DateTime dt) => dt.ToString("yyyy-MM-dd HH:mm:ss.fff", (IFormatProvider) DateTimeFormatInfo.InvariantInfo);

    public static string GetFormatedDateTimeForFilename(DateTime dt) => dt.ToString("yyyy-MM-dd-HH-mm-ss-fff", (IFormatProvider) DateTimeFormatInfo.InvariantInfo);

    public static string GetFormatedPriceEUR(Decimal num) => MyValueFormatter.GetFormatedDecimal(num, 2) + " €";

    public static string GetFormatedPriceUSD(Decimal num) => "$" + MyValueFormatter.GetFormatedDecimal(num, 2);

    public static string GetFormatedPriceUSD(Decimal priceInEur, Decimal exchangeRateEurToUsd) => "~" + MyValueFormatter.GetFormatedDecimal(Decimal.Round(exchangeRateEurToUsd * priceInEur, 2), 2) + " $";

    public static string GetFormatedInt(int i) => i.ToString("#,0", (IFormatProvider) CultureInfo.InvariantCulture);

    public static string GetFormatedArray<T>(T[] array)
    {
      string empty = string.Empty;
      for (int index = 0; index < array.Length; ++index)
      {
        empty += array[index].ToString();
        if (index < array.Length - 1)
          empty += ", ";
      }
      return empty;
    }

    public static void AppendFormattedValueInBestUnit(
      float value,
      string[] unitNames,
      float[] unitMultipliers,
      int unitDecimalDigits,
      StringBuilder output)
    {
      float num = Math.Abs(value);
      int index1 = 1;
      while (index1 < unitMultipliers.Length && (double) num >= (double) unitMultipliers[index1])
        ++index1;
      int index2 = index1 - 1;
      value /= unitMultipliers[index2];
      output.AppendDecimal(Math.Round((double) value, unitDecimalDigits), unitDecimalDigits);
      output.Append(' ').Append(unitNames[index2]);
    }

    public static void AppendFormattedValueInBestUnit(
      float value,
      string[] unitNames,
      float[] unitMultipliers,
      int[] unitDecimalDigits,
      StringBuilder output)
    {
      if (float.IsInfinity(value))
      {
        output.Append('-');
      }
      else
      {
        float num = Math.Abs(value);
        int index1 = 1;
        while (index1 < unitMultipliers.Length && (double) num >= (double) unitMultipliers[index1])
          ++index1;
        int index2 = index1 - 1;
        value /= unitMultipliers[index2];
        output.AppendDecimal(Math.Round((double) value, unitDecimalDigits[index2]), unitDecimalDigits[index2]);
        output.Append(' ').Append((object) MyTexts.Get(MyStringId.GetOrCompute(unitNames[index2])));
      }
    }

    public static void AppendGenericInBestUnit(float genericInBase, StringBuilder output) => MyValueFormatter.AppendFormattedValueInBestUnit(genericInBase, MyValueFormatter.m_genericUnitNames, MyValueFormatter.m_genericUnitMultipliers, MyValueFormatter.m_genericUnitDigits, output);

    public static void AppendGenericInBestUnit(
      float genericInBase,
      int numDecimals,
      StringBuilder output)
    {
      MyValueFormatter.AppendFormattedValueInBestUnit(genericInBase, MyValueFormatter.m_genericUnitNames, MyValueFormatter.m_genericUnitMultipliers, numDecimals, output);
    }

    public static void AppendForceInBestUnit(float forceInNewtons, StringBuilder output) => MyValueFormatter.AppendFormattedValueInBestUnit(forceInNewtons, MyValueFormatter.m_forceUnitNames, MyValueFormatter.m_forceUnitMultipliers, MyValueFormatter.m_forceUnitDigits, output);

    public static void AppendTorqueInBestUnit(float torqueInNewtonMeters, StringBuilder output) => MyValueFormatter.AppendFormattedValueInBestUnit(torqueInNewtonMeters, MyValueFormatter.m_torqueUnitNames, MyValueFormatter.m_torqueUnitMultipliers, MyValueFormatter.m_torqueUnitDigits, output);

    public static void AppendWorkInBestUnit(float workInMegaWatts, StringBuilder output) => MyValueFormatter.AppendFormattedValueInBestUnit(workInMegaWatts, MyValueFormatter.m_workUnitNames, MyValueFormatter.m_workUnitMultipliers, MyValueFormatter.m_workUnitDigits, output);

    public static void AppendWorkHoursInBestUnit(float workInMegaWatts, StringBuilder output) => MyValueFormatter.AppendFormattedValueInBestUnit(workInMegaWatts, MyValueFormatter.m_workHoursUnitNames, MyValueFormatter.m_workHoursUnitMultipliers, MyValueFormatter.m_workHoursUnitDigits, output);

    public static void AppendTimeInBestUnit(float timeInSeconds, StringBuilder output) => MyValueFormatter.AppendFormattedValueInBestUnit(timeInSeconds, MyValueFormatter.m_timeUnitNames, MyValueFormatter.m_timeUnitMultipliers, MyValueFormatter.m_timeUnitDigits, output);

    public static void AppendWeightInBestUnit(float weightInKG, StringBuilder output) => MyValueFormatter.AppendFormattedValueInBestUnit(weightInKG, MyValueFormatter.m_weightUnitNames, MyValueFormatter.m_weightUnitMultipliers, MyValueFormatter.m_weightUnitDigits, output);

    public static void AppendVolumeInBestUnit(float volumeInCubicMeters, StringBuilder output) => MyValueFormatter.AppendFormattedValueInBestUnit(volumeInCubicMeters, MyValueFormatter.m_volumeUnitNames, MyValueFormatter.m_volumeUnitMultipliers, MyValueFormatter.m_volumeUnitDigits, output);

    public static string GetFormattedGasAmount(int amount) => MyValueFormatter.GetFormatedInt(amount) + " kL";

    public static string GetFormattedPiecesAmount(int amount) => MyValueFormatter.GetFormatedInt(amount) + " pcs";

    public static string GetFormattedOreAmount(int amount) => MyValueFormatter.GetFormatedInt(amount) + " " + MyValueFormatter.m_weightUnitNames[1];

    public static void AppendTimeExact(int timeInSeconds, StringBuilder output)
    {
      if (timeInSeconds >= 86400)
      {
        output.Append(timeInSeconds / 86400);
        output.Append("d ");
      }
      output.ConcatFormat<int>("{0:00}", timeInSeconds / 3600 % 24);
      output.Append(":");
      output.ConcatFormat<int>("{0:00}", timeInSeconds / 60 % 60);
      output.Append(":");
      output.ConcatFormat<int>("{0:00}", timeInSeconds % 60);
    }

    public static void AppendTimeExactHoursMinSec(int timeInSeconds, StringBuilder output)
    {
      int num = timeInSeconds / 3600;
      if (num > 0)
      {
        output.ConcatFormat<int>("{0:00}", num);
        output.Append(":");
      }
      output.ConcatFormat<int>("{0:00}", timeInSeconds / 60 % 60);
      output.Append(":");
      output.ConcatFormat<int>("{0:00}", timeInSeconds % 60);
    }

    public static void AppendTimeExactMinSec(int timeInSeconds, StringBuilder output)
    {
      output.ConcatFormat<int>("{0:00}", timeInSeconds / 60 % 1440);
      output.Append(":");
      output.ConcatFormat<int>("{0:00}", timeInSeconds % 60);
    }

    public static void AppendDistanceInBestUnit(float distanceInMeters, StringBuilder output) => MyValueFormatter.AppendFormattedValueInBestUnit(distanceInMeters, MyValueFormatter.m_distanceUnitNames, MyValueFormatter.m_distanceUnitMultipliers, MyValueFormatter.m_distanceUnitDigits, output);

    public static string GetFormattedFileSizeInMB(ulong bytes) => string.Format("{0:F1} MB", (object) (float) ((double) bytes / 1024.0 / 1024.0));
  }
}
