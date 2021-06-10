// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Character.Components.MyCharacterComponentTypes
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using VRage.Utils;

namespace Sandbox.Game.Entities.Character.Components
{
  [Obsolete("Use MyComponentDefinitionBase and MyContainerDefinition to define enabled types of components on entities")]
  public static class MyCharacterComponentTypes
  {
    [Obsolete("Use MyComponentDefinitionBase and MyContainerDefinition to define enabled types of components on entities")]
    private static Dictionary<MyStringId, Tuple<Type, Type>> m_types;

    [Obsolete("Use MyComponentDefinitionBase and MyContainerDefinition to define enabled types of components on entities")]
    public static Dictionary<MyStringId, Tuple<Type, Type>> CharacterComponents
    {
      get
      {
        if (MyCharacterComponentTypes.m_types == null)
          MyCharacterComponentTypes.m_types = new Dictionary<MyStringId, Tuple<Type, Type>>()
          {
            {
              MyStringId.GetOrCompute("RagdollComponent"),
              new Tuple<Type, Type>(typeof (MyCharacterRagdollComponent), typeof (MyCharacterRagdollComponent))
            },
            {
              MyStringId.GetOrCompute("InventorySpawnComponent"),
              new Tuple<Type, Type>(typeof (MyInventorySpawnComponent), typeof (MyInventorySpawnComponent))
            }
          };
        return MyCharacterComponentTypes.m_types;
      }
    }
  }
}
