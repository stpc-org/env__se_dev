// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.MyCommandScript
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using VRage;

namespace Sandbox.Game.GUI
{
  public class MyCommandScript : MyCommand
  {
    private Type m_type;
    private static StringBuilder m_cache = new StringBuilder();

    public override string Prefix() => this.m_type.Name;

    public MyCommandScript(Type type)
    {
      this.m_type = type;
      int num = 0;
      foreach (MethodInfo method1 in type.GetMethods())
      {
        MethodInfo method = method1;
        if (method.IsPublic && method.IsStatic)
        {
          MyCommand.MyCommandAction myCommandAction = new MyCommand.MyCommandAction()
          {
            AutocompleteHint = this.GetArgsString(method),
            Parser = (ParserDelegate) (x => this.ParseArgs(x, method)),
            CallAction = (ActionDelegate) (x => this.Invoke(x, method))
          };
          this.m_methods.Add(string.Format("{0}{1}", (object) num++, (object) method.Name), myCommandAction);
        }
      }
    }

    private StringBuilder GetArgsString(MethodInfo method)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (ParameterInfo parameter in method.GetParameters())
        stringBuilder.Append(string.Format("{0} {1}, ", (object) parameter.ParameterType.Name, (object) parameter.Name));
      return stringBuilder;
    }

    private StringBuilder Invoke(MyCommandArgs x, MethodInfo method)
    {
      MyCommandScript.m_cache.Clear();
      MyCommandScript.MyCommandMethodArgs commandMethodArgs = x as MyCommandScript.MyCommandMethodArgs;
      if (commandMethodArgs.Args != null)
      {
        MyCommandScript.m_cache.Append("Success. ");
        object obj = method.Invoke((object) null, commandMethodArgs.Args);
        if (obj != null)
          MyCommandScript.m_cache.Append(obj.ToString());
      }
      else
        MyCommandScript.m_cache.Append(string.Format("Invoking {0} failed", (object) method.Name));
      return MyCommandScript.m_cache;
    }

    private MyCommandArgs ParseArgs(List<string> x, MethodInfo method)
    {
      MyCommandScript.MyCommandMethodArgs commandMethodArgs = new MyCommandScript.MyCommandMethodArgs();
      ParameterInfo[] parameters1 = method.GetParameters();
      List<object> objectList = new List<object>();
      for (int index = 0; index < parameters1.Length && index < x.Count; ++index)
      {
        Type parameterType = parameters1[index].ParameterType;
        MethodInfo method1 = parameterType.GetMethod("TryParse", new Type[2]
        {
          typeof (string),
          parameterType.MakeByRefType()
        });
        if (method1 != (MethodInfo) null)
        {
          object instance = Activator.CreateInstance(parameterType);
          object[] parameters2 = new object[2]
          {
            (object) x[index],
            instance
          };
          method1.Invoke((object) null, parameters2);
          objectList.Add(parameters2[1]);
        }
        else
          objectList.Add((object) x[index]);
      }
      if (parameters1.Length == objectList.Count)
        commandMethodArgs.Args = objectList.ToArray();
      return (MyCommandArgs) commandMethodArgs;
    }

    private class MyCommandMethodArgs : MyCommandArgs
    {
      public object[] Args;
    }
  }
}
