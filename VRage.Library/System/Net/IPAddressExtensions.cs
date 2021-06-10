// Decompiled with JetBrains decompiler
// Type: System.Net.IPAddressExtensions
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Globalization;

namespace System.Net
{
  public static class IPAddressExtensions
  {
    public static uint ToIPv4NetworkOrder(this IPAddress ip) => (uint) IPAddress.HostToNetworkOrder((int) (uint) ip.Address);

    public static IPAddress FromIPv4NetworkOrder(uint ip) => new IPAddress((long) (uint) IPAddress.NetworkToHostOrder((int) ip));

    public static IPAddress ParseOrAny(string ip)
    {
      IPAddress address;
      return !IPAddress.TryParse(ip, out address) ? IPAddress.Any : address;
    }

    public static bool TryParseEndpoint(
      string s,
      out string prefix,
      out IPEndPoint result,
      uint defaultPort = 27016)
    {
      int num1 = s.IndexOf("://", StringComparison.Ordinal);
      if (num1 != -1)
      {
        int num2 = num1 + 3;
        prefix = s.Substring(0, num2);
        s = s.Substring(num2);
      }
      else
        prefix = "";
      int length1 = s.Length;
      int length2 = s.LastIndexOf(':');
      if (length2 > 0)
      {
        if (s[length2 - 1] == ']')
          length1 = length2;
        else if (s.Substring(0, length2).LastIndexOf(':') == -1)
          length1 = length2;
      }
      IPAddress address;
      if (IPAddress.TryParse(s.Substring(0, length1), out address))
      {
        uint result1 = defaultPort;
        if (length1 == s.Length || uint.TryParse(s.Substring(length1 + 1), NumberStyles.None, (IFormatProvider) CultureInfo.InvariantCulture, out result1) && result1 <= (uint) ushort.MaxValue)
        {
          result = new IPEndPoint(address, (int) result1);
          return true;
        }
      }
      result = (IPEndPoint) null;
      return false;
    }
  }
}
