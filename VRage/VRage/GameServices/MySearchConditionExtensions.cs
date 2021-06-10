// Decompiled with JetBrains decompiler
// Type: VRage.GameServices.MySearchConditionExtensions
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

namespace VRage.GameServices
{
  public static class MySearchConditionExtensions
  {
    public static bool Contains(this MySearchConditionFlags self, MySearchCondition condition) => (uint) ((MySearchConditionFlags) (1U << (int) (condition & (MySearchCondition) 31)) & self) > 0U;
  }
}
