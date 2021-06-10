// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyControllerSchemaDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_ControllerSchemaDefinition), null)]
  public class MyControllerSchemaDefinition : MyDefinitionBase
  {
    public List<int> CompatibleDevices;
    public Dictionary<string, List<MyControllerSchemaDefinition.ControlGroup>> Schemas;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_ControllerSchemaDefinition schemaDefinition = builder as MyObjectBuilder_ControllerSchemaDefinition;
      if (schemaDefinition.CompatibleDeviceIds != null)
      {
        this.CompatibleDevices = new List<int>(schemaDefinition.CompatibleDeviceIds.Count);
        byte[] arr = new byte[4];
        foreach (string compatibleDeviceId in schemaDefinition.CompatibleDeviceIds)
        {
          if (compatibleDeviceId.Length >= 8 && this.TryGetByteArray(compatibleDeviceId, 8, out arr))
            this.CompatibleDevices.Add(BitConverter.ToInt32(arr, 0));
        }
      }
      if (schemaDefinition.Schemas == null)
        return;
      this.Schemas = new Dictionary<string, List<MyControllerSchemaDefinition.ControlGroup>>(schemaDefinition.Schemas.Count);
      foreach (MyObjectBuilder_ControllerSchemaDefinition.Schema schema in schemaDefinition.Schemas)
      {
        if (schema.ControlGroups != null)
        {
          List<MyControllerSchemaDefinition.ControlGroup> controlGroupList = new List<MyControllerSchemaDefinition.ControlGroup>(schema.ControlGroups.Count);
          this.Schemas[schema.SchemaName] = controlGroupList;
          foreach (MyObjectBuilder_ControllerSchemaDefinition.ControlGroup controlGroup1 in schema.ControlGroups)
          {
            MyControllerSchemaDefinition.ControlGroup controlGroup2 = new MyControllerSchemaDefinition.ControlGroup();
            controlGroup2.Type = controlGroup1.Type;
            controlGroup2.Name = controlGroup1.Name;
            if (controlGroup1.ControlDefs != null)
            {
              controlGroup2.ControlBinding = new Dictionary<string, MyControllerSchemaEnum>(controlGroup1.ControlDefs.Count);
              foreach (MyObjectBuilder_ControllerSchemaDefinition.ControlDef controlDef in controlGroup1.ControlDefs)
                controlGroup2.ControlBinding[controlDef.Type] = controlDef.Control;
            }
          }
        }
      }
    }

    private bool TryGetByteArray(string str, int count, out byte[] arr)
    {
      arr = (byte[]) null;
      if (count % 2 == 1 || str.Length < count)
        return false;
      arr = new byte[count / 2];
      StringBuilder stringBuilder = new StringBuilder();
      int index1 = 0;
      int index2 = 0;
      while (index1 < count)
      {
        stringBuilder.Clear().Append(str[index1]).Append(str[index1 + 1]);
        if (!byte.TryParse(stringBuilder.ToString(), NumberStyles.HexNumber, (IFormatProvider) null, out arr[index2]))
          return false;
        index1 += 2;
        ++index2;
      }
      return true;
    }

    public class ControlGroup
    {
      public string Type;
      public string Name;
      public Dictionary<string, MyControllerSchemaEnum> ControlBinding;
    }

    private class Sandbox_Definitions_MyControllerSchemaDefinition\u003C\u003EActor : IActivator, IActivator<MyControllerSchemaDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyControllerSchemaDefinition();

      MyControllerSchemaDefinition IActivator<MyControllerSchemaDefinition>.CreateInstance() => new MyControllerSchemaDefinition();
    }
  }
}
