// Decompiled with JetBrains decompiler
// Type: VRage.Analytics.MyReportTypeData
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

namespace VRage.Analytics
{
  public class MyReportTypeData
  {
    public MyReportType ReportType;
    public string Arg1;
    public string Arg2;

    public MyReportTypeData()
    {
    }

    public MyReportTypeData(MyReportType reportType, string arg1, string arg2)
    {
      this.ReportType = reportType;
      this.Arg1 = arg1;
      this.Arg2 = arg2;
    }
  }
}
