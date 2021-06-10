// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MemberInfoExtensions
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Reflection;

namespace Sandbox.Game.Gui
{
  public static class MemberInfoExtensions
  {
    public static object GetValue(this MemberInfo info, object instance)
    {
      object obj = (object) null;
      FieldInfo fieldInfo = info as FieldInfo;
      if (fieldInfo != (FieldInfo) null)
        obj = fieldInfo.GetValue(instance);
      PropertyInfo propertyInfo = info as PropertyInfo;
      if (propertyInfo != (PropertyInfo) null && propertyInfo.GetIndexParameters().Length == 0)
        obj = propertyInfo.GetValue(instance, (object[]) null);
      return obj;
    }
  }
}
