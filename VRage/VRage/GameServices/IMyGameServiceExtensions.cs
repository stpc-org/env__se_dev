// Decompiled with JetBrains decompiler
// Type: VRage.GameServices.IMyGameServiceExtensions
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;

namespace VRage.GameServices
{
  public static class IMyGameServiceExtensions
  {
    public static void RequestPermissions(
      this IMyGameService thiz,
      Permissions permission,
      bool attemptResolution,
      Action<bool> onDone)
    {
      thiz.RequestPermissions(permission, attemptResolution, (Action<PermissionResult>) (result => onDone(result == PermissionResult.Granted)));
    }

    public static void RequestPermissionsWithTargetUser(
      this IMyGameService thiz,
      Permissions permission,
      ulong targetUserId,
      Action<bool> onDone)
    {
      thiz.RequestPermissionsWithTargetUser(permission, targetUserId, (Action<PermissionResult>) (result => onDone(result == PermissionResult.Granted)));
    }
  }
}
