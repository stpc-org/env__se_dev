// Decompiled with JetBrains decompiler
// Type: VRage.Scripting.IMyWhitelistBatch
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Reflection;

namespace VRage.Scripting
{
  public interface IMyWhitelistBatch : IDisposable
  {
    void AllowNamespaceOfTypes(MyWhitelistTarget target, params Type[] types);

    void AllowTypes(MyWhitelistTarget target, params Type[] types);

    void AllowMembers(MyWhitelistTarget target, params MemberInfo[] members);
  }
}
