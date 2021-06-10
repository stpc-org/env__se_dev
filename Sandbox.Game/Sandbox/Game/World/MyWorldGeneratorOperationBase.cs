// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.MyWorldGeneratorOperationBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;

namespace Sandbox.Game.World
{
  public abstract class MyWorldGeneratorOperationBase
  {
    public string FactionTag;

    public abstract void Apply();

    public virtual void Init(MyObjectBuilder_WorldGeneratorOperation builder) => this.FactionTag = builder.FactionTag;

    public virtual MyObjectBuilder_WorldGeneratorOperation GetObjectBuilder()
    {
      MyObjectBuilder_WorldGeneratorOperation objectBuilder = MyWorldGenerator.OperationFactory.CreateObjectBuilder(this);
      objectBuilder.FactionTag = this.FactionTag;
      return objectBuilder;
    }
  }
}
