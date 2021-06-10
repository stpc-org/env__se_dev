// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.WorldEnvironment.ItemInfo
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRageMath;

namespace Sandbox.Game.WorldEnvironment
{
  public struct ItemInfo
  {
    public bool IsEnabled;
    public Vector3 Position;
    public short DefinitionIndex;
    public short ModelIndex;
    public Quaternion Rotation;

    public override string ToString() => string.Format("Model: {0}; Def: {1}", (object) this.ModelIndex, (object) this.DefinitionIndex);
  }
}
