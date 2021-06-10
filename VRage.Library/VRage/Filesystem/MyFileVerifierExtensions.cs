// Decompiled with JetBrains decompiler
// Type: VRage.FileSystem.MyFileVerifierExtensions
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.IO;

namespace VRage.FileSystem
{
  public static class MyFileVerifierExtensions
  {
    public static Stream Verify(this IFileVerifier verifier, string path, Stream stream) => verifier.Verify(path, stream);
  }
}
