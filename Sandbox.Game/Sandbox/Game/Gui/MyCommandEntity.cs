// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.MyCommandEntity
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game.Entity;

namespace Sandbox.Game.GUI
{
  [PreloadRequired]
  public class MyCommandEntity : MyCommand
  {
    static MyCommandEntity() => MyConsole.AddCommand((MyCommand) new MyCommandEntity());

    public override string Prefix() => "Entity";

    private MyCommandEntity()
    {
      this.m_methods.Add("SetDisplayName", new MyCommand.MyCommandAction()
      {
        AutocompleteHint = new StringBuilder("long_EntityId string_NewDisplayName"),
        Parser = (ParserDelegate) (x => this.ParseDisplayName(x)),
        CallAction = (ActionDelegate) (x => this.ChangeDisplayName(x))
      });
      this.m_methods.Add("MethodA", new MyCommand.MyCommandAction()
      {
        Parser = (ParserDelegate) (x => this.ParseDisplayName(x)),
        CallAction = (ActionDelegate) (x => this.ChangeDisplayName(x))
      });
      this.m_methods.Add("MethodB", new MyCommand.MyCommandAction()
      {
        Parser = (ParserDelegate) (x => this.ParseDisplayName(x)),
        CallAction = (ActionDelegate) (x => this.ChangeDisplayName(x))
      });
      this.m_methods.Add("MethodC", new MyCommand.MyCommandAction()
      {
        Parser = (ParserDelegate) (x => this.ParseDisplayName(x)),
        CallAction = (ActionDelegate) (x => this.ChangeDisplayName(x))
      });
      this.m_methods.Add("MethodD", new MyCommand.MyCommandAction()
      {
        Parser = (ParserDelegate) (x => this.ParseDisplayName(x)),
        CallAction = (ActionDelegate) (x => this.ChangeDisplayName(x))
      });
    }

    private StringBuilder ChangeDisplayName(MyCommandArgs args)
    {
      MyCommandEntity.MyCommandArgsDisplayName commandArgsDisplayName = args as MyCommandEntity.MyCommandArgsDisplayName;
      MyEntity entity;
      if (!MyEntities.TryGetEntityById(commandArgsDisplayName.EntityId, out entity))
        return new StringBuilder().Append("Entity not found");
      if (commandArgsDisplayName.newDisplayName == null)
        return new StringBuilder().Append("Invalid Display name");
      string displayName = entity.DisplayName;
      entity.DisplayName = commandArgsDisplayName.newDisplayName;
      return new StringBuilder().Append("Changed name from entity ").Append(commandArgsDisplayName.EntityId).Append(" from ").Append(displayName).Append(" to ").Append(entity.DisplayName);
    }

    private MyCommandArgs ParseDisplayName(List<string> args) => (MyCommandArgs) new MyCommandEntity.MyCommandArgsDisplayName()
    {
      EntityId = long.Parse(args[0]),
      newDisplayName = args[1]
    };

    private class MyCommandArgsDisplayName : MyCommandArgs
    {
      public long EntityId;
      public string newDisplayName;
    }
  }
}
