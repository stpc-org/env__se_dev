// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyDefinitionManagerSimple
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using VRage.FileSystem;
using VRage.Game.Definitions;
using VRage.ObjectBuilders;

namespace VRage.Game
{
  public class MyDefinitionManagerSimple : MyDefinitionManagerBase
  {
    private Dictionary<string, string> m_overrideMap = new Dictionary<string, string>();

    public void AddDefinitionOverride(Type overridingType, string typeOverride)
    {
      if (!(overridingType.GetCustomAttribute(typeof (MyDefinitionTypeAttribute), false) is MyDefinitionTypeAttribute customAttribute))
        throw new Exception("Missing type attribute in definition");
      string str = customAttribute.ObjectBuilderType.GetCustomAttribute(typeof (XmlTypeAttribute), false) is XmlTypeAttribute customAttribute ? customAttribute.TypeName : customAttribute.ObjectBuilderType.Name;
      this.m_overrideMap[typeOverride] = str;
    }

    public void LoadDefinitions(string path)
    {
      bool flag = false;
      MyObjectBuilder_Definitions builderDefinitions = (MyObjectBuilder_Definitions) null;
      using (Stream stream = MyFileSystem.OpenRead(path))
      {
        if (stream != null)
        {
          using (Stream reader = stream.UnwrapGZip())
          {
            if (reader != null)
            {
              MyObjectBuilder_Base objectBuilder;
              flag = MyObjectBuilderSerializer.DeserializeXML(reader, out objectBuilder, typeof (MyObjectBuilder_Definitions), this.m_overrideMap);
              builderDefinitions = objectBuilder as MyObjectBuilder_Definitions;
            }
          }
        }
      }
      if (!flag)
        throw new Exception("Error while reading \"" + path + "\"");
      if (builderDefinitions.Definitions == null)
        return;
      foreach (MyObjectBuilder_DefinitionBase definition in builderDefinitions.Definitions)
      {
        MyObjectBuilderType.RemapType(ref definition.Id, this.m_overrideMap);
        MyDefinitionBase instance = MyDefinitionManagerBase.GetObjectFactory().CreateInstance(definition.TypeId);
        instance.Init(builderDefinitions.Definitions[0], new MyModContext());
        this.m_definitions.AddDefinition(instance);
      }
    }

    public override MyDefinitionSet GetLoadingSet() => throw new NotImplementedException();
  }
}
