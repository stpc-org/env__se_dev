// Decompiled with JetBrains decompiler
// Type: VRage.GameServices.MyModMetadataLoader
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.IO;
using System.Xml.Serialization;
using VRage.FileSystem;
using VRage.Utils;

namespace VRage.GameServices
{
  public class MyModMetadataLoader
  {
    public static ModMetadataFile Parse(string xml)
    {
      if (string.IsNullOrEmpty(xml))
        return (ModMetadataFile) null;
      ModMetadataFile modMetadataFile = (ModMetadataFile) null;
      try
      {
        using (TextReader textReader = (TextReader) new StringReader(xml))
          modMetadataFile = (ModMetadataFile) new XmlSerializer(typeof (ModMetadataFile)).Deserialize(textReader);
      }
      catch (Exception ex)
      {
        MyLog.Default.Warning("Failed parsing mod metadata: {0}", (object) ex.Message);
      }
      return modMetadataFile;
    }

    public static string Serialize(ModMetadataFile data)
    {
      if (data == null)
        return (string) null;
      try
      {
        using (TextWriter textWriter = (TextWriter) new StringWriter())
        {
          new XmlSerializer(typeof (ModMetadataFile)).Serialize(textWriter, (object) data);
          return textWriter.ToString();
        }
      }
      catch (Exception ex)
      {
        MyLog.Default.Warning("Failed serializing mod metadata: {0}", (object) ex.Message);
        return (string) null;
      }
    }

    public static ModMetadataFile Load(string filename)
    {
      if (string.IsNullOrEmpty(filename))
        return (ModMetadataFile) null;
      ModMetadataFile modMetadataFile = (ModMetadataFile) null;
      try
      {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof (ModMetadataFile));
        Stream stream = MyFileSystem.OpenRead(filename);
        if (stream != null)
        {
          modMetadataFile = (ModMetadataFile) xmlSerializer.Deserialize(stream);
          stream.Close();
        }
      }
      catch (Exception ex)
      {
        MyLog.Default.Warning("Failed loading mod metadata file: {0} with exception: {1}", (object) filename, (object) ex.Message);
      }
      return modMetadataFile;
    }

    public static bool Save(string filename, ModMetadataFile file)
    {
      if (!string.IsNullOrEmpty(filename))
      {
        if (file != null)
        {
          try
          {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof (ModMetadataFile));
            TextWriter textWriter1 = (TextWriter) new StreamWriter(filename);
            TextWriter textWriter2 = textWriter1;
            ModMetadataFile modMetadataFile = file;
            xmlSerializer.Serialize(textWriter2, (object) modMetadataFile);
            textWriter1.Close();
          }
          catch (Exception ex)
          {
            MyLog.Default.Warning("Failed saving mod metadata file: {0} with exception: {1}", (object) filename, (object) ex.Message);
            return false;
          }
          return true;
        }
      }
      return false;
    }
  }
}
