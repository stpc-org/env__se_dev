// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.MyCreditsDepartment
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using System.Text;
using VRageMath;

namespace Sandbox.Game
{
  public class MyCreditsDepartment
  {
    public StringBuilder Name;
    public List<MyCreditsPerson> Persons;
    public string LogoTexture;
    public Vector2? LogoTextureSize;
    public float? LogoScale;
    public float LogoOffsetPre = 0.07f;
    public float LogoOffsetPost = 0.07f;

    public MyCreditsDepartment(string name)
    {
      this.Name = new StringBuilder(name);
      this.Persons = new List<MyCreditsPerson>();
    }
  }
}
