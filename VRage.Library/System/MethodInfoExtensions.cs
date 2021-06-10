// Decompiled with JetBrains decompiler
// Type: System.MethodInfoExtensions
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace System
{
  public static class MethodInfoExtensions
  {
    public static TDelegate CreateDelegate<TDelegate>(this MethodInfo method, object instance) where TDelegate : Delegate => (TDelegate) Delegate.CreateDelegate(typeof (TDelegate), instance, method);

    public static TDelegate CreateDelegate<TDelegate>(this MethodInfo method) where TDelegate : Delegate => (TDelegate) Delegate.CreateDelegate(typeof (TDelegate), method);

    public static ParameterExpression[] ExtractParameterExpressionsFrom<TDelegate>() => ((IEnumerable<ParameterInfo>) typeof (TDelegate).GetMethod("Invoke").GetParameters()).Select<ParameterInfo, ParameterExpression>((Func<ParameterInfo, ParameterExpression>) (s => Expression.Parameter(s.ParameterType))).ToArray<ParameterExpression>();
  }
}
