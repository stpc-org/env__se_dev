// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.WorldEnvironment.Definitions.MyRuntimeEnvironmentItemInfo
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.WorldEnvironment.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Game.WorldEnvironment.Definitions
{
  public class MyRuntimeEnvironmentItemInfo
  {
    public MyItemTypeDefinition Type;
    public MyStringHash Subtype;
    public float Offset;
    public float Density;
    public short Index;

    public MyRuntimeEnvironmentItemInfo(
      MyProceduralEnvironmentDefinition def,
      MyEnvironmentItemInfo info,
      int id)
    {
      this.Index = (short) id;
      this.Type = def.ItemTypes[info.Type];
      this.Subtype = info.Subtype;
      this.Offset = info.Offset;
      this.Density = info.Density;
    }
  }
}
