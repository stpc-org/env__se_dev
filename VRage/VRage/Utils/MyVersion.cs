// Decompiled with JetBrains decompiler
// Type: VRage.Utils.MyVersion
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System.Text;

namespace VRage.Utils
{
  public class MyVersion
  {
    public readonly int Version;
    public readonly StringBuilder FormattedText;
    public readonly StringBuilder FormattedTextFriendly;
    private readonly System.Version _version;

    public MyVersion(int version)
    {
      this.Version = version;
      this.FormattedText = new StringBuilder(MyBuildNumbers.ConvertBuildNumberFromIntToString(version));
      string stringFriendly = MyBuildNumbers.ConvertBuildNumberFromIntToStringFriendly(version, ".");
      this.FormattedTextFriendly = new StringBuilder(stringFriendly);
      this._version = new System.Version(stringFriendly);
    }

    public static implicit operator MyVersion(int version) => new MyVersion(version);

    public static implicit operator int(MyVersion version) => version.Version;

    public static implicit operator System.Version(MyVersion version) => version._version;

    public override string ToString() => this.Version.ToString();
  }
}
