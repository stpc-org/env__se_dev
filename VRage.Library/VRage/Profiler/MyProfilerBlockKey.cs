// Decompiled with JetBrains decompiler
// Type: VRage.Profiler.MyProfilerBlockKey
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;

namespace VRage.Profiler
{
  public struct MyProfilerBlockKey : IEquatable<MyProfilerBlockKey>
  {
    public string File;
    public string Member;
    public string Name;
    public int Line;
    public int ParentId;
    public int HashCode;

    public MyProfilerBlockKey(string file, string member, string name, int line, int parentId)
    {
      this.File = file;
      this.Member = member;
      this.Name = name;
      this.Line = line;
      this.ParentId = parentId;
      this.HashCode = file.GetHashCode();
      this.HashCode = 397 * this.HashCode ^ member.GetHashCode();
      this.HashCode = 397 * this.HashCode ^ (name ?? string.Empty).GetHashCode();
      this.HashCode = 397 * this.HashCode ^ parentId.GetHashCode();
    }

    public bool IsSameLocation(MyProfilerBlockKey obj) => this.Name == obj.Name && this.Member == obj.Member;

    public bool IsSimilarLocation(MyProfilerBlockKey obj)
    {
      int startIndex1 = obj.File.IndexOf("Sources");
      int startIndex2 = this.File.IndexOf("Sources");
      if (startIndex1 == -1)
        startIndex1 = 0;
      if (startIndex2 == -1)
        startIndex2 = 0;
      return this.IsSameLocation(obj) && this.File.Substring(startIndex2) == obj.File.Substring(startIndex1) && Math.Abs(this.Line - obj.Line) < 40;
    }

    public bool Equals(MyProfilerBlockKey obj) => this.ParentId == obj.ParentId && this.File == obj.File && this.Line == obj.Line && this.IsSameLocation(obj);

    public override int GetHashCode() => this.HashCode;
  }
}
