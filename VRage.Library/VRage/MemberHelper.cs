// Decompiled with JetBrains decompiler
// Type: VRage.MemberHelper
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Linq.Expressions;
using System.Reflection;

namespace VRage
{
  public static class MemberHelper
  {
    public static MemberInfo GetMember<TValue>(Expression<Func<TValue>> selector)
    {
      Exceptions.ThrowIf<ArgumentNullException>(selector == null, nameof (selector));
      MemberExpression body = selector.Body as MemberExpression;
      Exceptions.ThrowIf<ArgumentNullException>(body == null, "Selector must be a member access expression", nameof (selector));
      return body.Member;
    }
  }
}
