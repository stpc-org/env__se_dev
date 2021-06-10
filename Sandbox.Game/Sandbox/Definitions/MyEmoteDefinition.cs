// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyEmoteDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.Definitions.Animation;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_EmoteDefinition), null)]
  public class MyEmoteDefinition : MyDefinitionBase
  {
    private readonly Dictionary<MyDefinitionId, MyDefinitionId> m_overrides = new Dictionary<MyDefinitionId, MyDefinitionId>();

    public DictionaryReader<MyDefinitionId, MyDefinitionId> Overrides => (DictionaryReader<MyDefinitionId, MyDefinitionId>) this.m_overrides;

    public MyDefinitionId AnimationId { get; private set; }

    public string ChatCommand { get; private set; }

    public string ChatCommandName { get; private set; }

    public string ChatCommandDescription { get; private set; }

    public int Priority { get; private set; }

    public bool Public { get; private set; }

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_EmoteDefinition builderEmoteDefinition = builder as MyObjectBuilder_EmoteDefinition;
      this.AnimationId = (MyDefinitionId) builderEmoteDefinition.AnimationId;
      this.ChatCommand = builderEmoteDefinition.ChatCommand;
      this.ChatCommandName = builderEmoteDefinition.ChatCommandName;
      this.ChatCommandDescription = builderEmoteDefinition.ChatCommandDescription;
      this.Priority = builderEmoteDefinition.Priority;
      this.Public = builderEmoteDefinition.Public;
      if (builderEmoteDefinition.Overrides == null || builderEmoteDefinition.Overrides.Length == 0)
        return;
      foreach (MyObjectBuilder_EmoteDefinition.AnimOverrideDef animOverrideDef in builderEmoteDefinition.Overrides)
        this.m_overrides[(MyDefinitionId) animOverrideDef.CharacterId] = (MyDefinitionId) animOverrideDef.AnimationId;
    }

    public MyAnimationDefinition GetAnimationForCharacter(
      MyDefinitionId characterId)
    {
      MyDefinitionId myDefinitionId;
      if (this.m_overrides.TryGetValue(characterId, out myDefinitionId))
      {
        MyAnimationDefinition animationDefinition = MyDefinitionManager.Static.TryGetAnimationDefinition(myDefinitionId.SubtypeName);
        if (animationDefinition != null)
          return animationDefinition;
      }
      return MyDefinitionManager.Static.TryGetAnimationDefinition(this.AnimationId.SubtypeName);
    }

    private class Sandbox_Definitions_MyEmoteDefinition\u003C\u003EActor : IActivator, IActivator<MyEmoteDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyEmoteDefinition();

      MyEmoteDefinition IActivator<MyEmoteDefinition>.CreateInstance() => new MyEmoteDefinition();
    }
  }
}
