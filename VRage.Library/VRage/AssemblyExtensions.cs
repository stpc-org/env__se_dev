// Decompiled with JetBrains decompiler
// Type: VRage.AssemblyExtensions
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Reflection;

namespace VRage
{
  public static class AssemblyExtensions
  {
    public static ProcessorArchitecture ToProcessorArchitecture(
      this PortableExecutableKinds peKind)
    {
      switch (peKind & ~PortableExecutableKinds.ILOnly)
      {
        case PortableExecutableKinds.Required32Bit:
          return ProcessorArchitecture.X86;
        case PortableExecutableKinds.PE32Plus:
          return ProcessorArchitecture.Amd64;
        case PortableExecutableKinds.Unmanaged32Bit:
          return ProcessorArchitecture.X86;
        default:
          return (peKind & PortableExecutableKinds.ILOnly) == PortableExecutableKinds.NotAPortableExecutableImage ? ProcessorArchitecture.None : ProcessorArchitecture.MSIL;
      }
    }

    public static PortableExecutableKinds GetPeKind(this Assembly assembly)
    {
      PortableExecutableKinds peKind;
      assembly.ManifestModule.GetPEKind(out peKind, out ImageFileMachine _);
      return peKind;
    }

    public static ProcessorArchitecture GetArchitecture(this Assembly assembly) => assembly.GetPeKind().ToProcessorArchitecture();

    public static ProcessorArchitecture TryGetArchitecture(string assemblyName)
    {
      try
      {
        return AssemblyName.GetAssemblyName(assemblyName).ProcessorArchitecture;
      }
      catch
      {
        return ProcessorArchitecture.None;
      }
    }

    public static ProcessorArchitecture TryGetArchitecture(
      this Assembly assembly)
    {
      try
      {
        return assembly.GetArchitecture();
      }
      catch
      {
        return ProcessorArchitecture.None;
      }
    }
  }
}
