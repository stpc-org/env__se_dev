// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyFinalBuildConstants
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Text;
using VRage.Utils;

namespace VRage.Game
{
  public class MyFinalBuildConstants
  {
    public static MyVersion APP_VERSION;
    public const bool IS_CLOUD_GAMING = false;
    public const bool IS_OFFICIAL = true;
    public const bool IS_DEBUG = false;
    public const int IP_ADDRESS_ANY = 0;
    public const short DEDICATED_SERVER_PORT = 27015;
    public const short DEDICATED_STEAM_AUTH_PORT = 8766;

    public static StringBuilder APP_VERSION_STRING => MyFinalBuildConstants.APP_VERSION.FormattedText;

    public static StringBuilder APP_VERSION_STRING_DOTS => MyFinalBuildConstants.APP_VERSION.FormattedTextFriendly;
  }
}
