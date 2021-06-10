// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.WorldEnvironment.ObjectBuilders.MyProceduralEnvironmentMapping
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Xml.Serialization;
using VRageMath;

namespace Sandbox.Game.WorldEnvironment.ObjectBuilders
{
  public class MyProceduralEnvironmentMapping
  {
    [XmlElement("Material")]
    public string[] Materials;
    [XmlElement("Biome")]
    public int[] Biomes;
    [XmlElement("Item")]
    public MyEnvironmentItemInfo[] Items;
    public SerializableRange Height = new SerializableRange(0.0f, 1f);
    public SymmetricSerializableRange Latitude = new SymmetricSerializableRange(-90f, 90f);
    public SerializableRange Longitude = new SerializableRange(-180f, 180f);
    public SerializableRange Slope = new SerializableRange(0.0f, 90f);
  }
}
