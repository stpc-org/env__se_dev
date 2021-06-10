// Decompiled with JetBrains decompiler
// Type: System.Linq.Expressions.ExpressionExtension
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using VRage.Library.Extensions;

namespace System.Linq.Expressions
{
  public static class ExpressionExtension
  {
    public static IActivatorFactory Factory = (IActivatorFactory) new ExpressionBaseActivatorFactory();

    public static Func<T> CreateActivator<T>() where T : new() => ExpressionExtension.Factory.CreateActivator<T>();

    public static Func<T> CreateActivator<T>(Type t) => ExpressionExtension.Factory.CreateActivator<T>(t);
  }
}
