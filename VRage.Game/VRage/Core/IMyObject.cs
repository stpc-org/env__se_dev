﻿// Decompiled with JetBrains decompiler
// Type: VRage.Core.IMyObject
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRage.Game;
using VRage.ObjectBuilders;

namespace VRage.Core
{
  public interface IMyObject
  {
    MyDefinitionId DefinitionId { get; }

    bool NeedsSerialize { get; }

    void Deserialize(MyObjectBuilder_Base builder);

    MyObjectBuilder_Base Serialize();
  }
}