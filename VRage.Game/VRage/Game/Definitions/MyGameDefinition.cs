// Decompiled with JetBrains decompiler
// Type: VRage.Game.Definitions.MyGameDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Collections.Generic;
using System.Linq;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_GameDefinition), typeof (MyGameDefinition.Postprocess))]
  public class MyGameDefinition : MyDefinitionBase
  {
    public static readonly MyDefinitionId Default = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_GameDefinition), nameof (Default));
    public Dictionary<string, MyDefinitionId?> SessionComponents;
    public static readonly MyGameDefinition DefaultDefinition;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_GameDefinition builderGameDefinition = (MyObjectBuilder_GameDefinition) builder;
      if (builderGameDefinition.InheritFrom != null)
      {
        MyGameDefinition definition = MyDefinitionManagerBase.Static.GetLoadingSet().GetDefinition<MyGameDefinition>(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_GameDefinition), builderGameDefinition.InheritFrom));
        if (definition == null)
          MyLog.Default.Error("Could not find parent definition {0} for game definition {1}.", (object) builderGameDefinition.InheritFrom, (object) builderGameDefinition.SubtypeId);
        else
          this.SessionComponents = new Dictionary<string, MyDefinitionId?>((IDictionary<string, MyDefinitionId?>) definition.SessionComponents);
      }
      if (this.SessionComponents == null)
        this.SessionComponents = new Dictionary<string, MyDefinitionId?>();
      foreach (MyObjectBuilder_GameDefinition.Comp sessionComponent in builderGameDefinition.SessionComponents)
        this.SessionComponents[sessionComponent.ComponentName] = sessionComponent.Type == null ? new MyDefinitionId?() : new MyDefinitionId?(new MyDefinitionId(MyObjectBuilderType.Parse(sessionComponent.Type), sessionComponent.Subtype));
      if (!builderGameDefinition.Default)
        return;
      this.SetDefault();
    }

    private void SetDefault()
    {
      MyGameDefinition myGameDefinition1 = new MyGameDefinition();
      myGameDefinition1.SessionComponents = this.SessionComponents;
      myGameDefinition1.Id = MyGameDefinition.Default;
      MyGameDefinition myGameDefinition2 = myGameDefinition1;
      MyDefinitionManagerBase.Static.GetLoadingSet().AddOrRelaceDefinition((MyDefinitionBase) myGameDefinition2);
    }

    static MyGameDefinition()
    {
      MyGameDefinition myGameDefinition = new MyGameDefinition();
      myGameDefinition.Id = MyGameDefinition.Default;
      myGameDefinition.SessionComponents = new Dictionary<string, MyDefinitionId?>();
      MyGameDefinition.DefaultDefinition = myGameDefinition;
    }

    private class Postprocess : MyDefinitionPostprocessor
    {
      public override void AfterLoaded(ref MyDefinitionPostprocessor.Bundle definitions)
      {
      }

      public override void AfterPostprocess(
        MyDefinitionSet set,
        Dictionary<MyStringHash, MyDefinitionBase> definitions)
      {
        if (set.ContainsDefinition(MyGameDefinition.Default))
          return;
        set.GetDefinitionsOfType<MyGameDefinition>().First<MyGameDefinition>().SetDefault();
      }
    }

    private class VRage_Game_Definitions_MyGameDefinition\u003C\u003EActor : IActivator, IActivator<MyGameDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyGameDefinition();

      MyGameDefinition IActivator<MyGameDefinition>.CreateInstance() => new MyGameDefinition();
    }
  }
}
