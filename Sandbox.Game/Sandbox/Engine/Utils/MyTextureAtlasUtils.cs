// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Utils.MyTextureAtlasUtils
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Graphics;
using System;
using System.Globalization;
using System.IO;
using VRage.FileSystem;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Engine.Utils
{
  internal class MyTextureAtlasUtils
  {
    private static MyTextureAtlas LoadTextureAtlas(
      string textureDir,
      string atlasFile)
    {
      MyTextureAtlas myTextureAtlas = new MyTextureAtlas(64);
      using (Stream stream = MyFileSystem.OpenRead(Path.Combine(MyFileSystem.ContentPath, atlasFile)))
      {
        using (StreamReader streamReader = new StreamReader(stream))
        {
          while (!streamReader.EndOfStream)
          {
            string str1 = streamReader.ReadLine();
            if (!str1.StartsWith("#"))
            {
              if (str1.Trim(' ').Length != 0)
              {
                string[] strArray = str1.Split(new char[3]
                {
                  ' ',
                  '\t',
                  ','
                }, StringSplitOptions.RemoveEmptyEntries);
                string key = strArray[0];
                string str2 = strArray[1];
                Vector4 uvOffsets = new Vector4(Convert.ToSingle(strArray[4], (IFormatProvider) CultureInfo.InvariantCulture), Convert.ToSingle(strArray[5], (IFormatProvider) CultureInfo.InvariantCulture), Convert.ToSingle(strArray[7], (IFormatProvider) CultureInfo.InvariantCulture), Convert.ToSingle(strArray[8], (IFormatProvider) CultureInfo.InvariantCulture));
                MyTextureAtlasItem textureAtlasItem = new MyTextureAtlasItem(textureDir + str2, uvOffsets);
                myTextureAtlas.Add(key, textureAtlasItem);
              }
            }
          }
        }
      }
      return myTextureAtlas;
    }

    public static void LoadTextureAtlas(
      string[] enumsToStrings,
      string textureDir,
      string atlasFile,
      out string texture,
      out MyAtlasTextureCoordinate[] textureCoords)
    {
      MyTextureAtlas myTextureAtlas = MyTextureAtlasUtils.LoadTextureAtlas(textureDir, atlasFile);
      textureCoords = new MyAtlasTextureCoordinate[enumsToStrings.Length];
      texture = (string) null;
      for (int index = 0; index < enumsToStrings.Length; ++index)
      {
        MyTextureAtlasItem textureAtlasItem = myTextureAtlas[enumsToStrings[index]];
        textureCoords[index] = new MyAtlasTextureCoordinate(new Vector2(textureAtlasItem.UVOffsets.X, textureAtlasItem.UVOffsets.Y), new Vector2(textureAtlasItem.UVOffsets.Z, textureAtlasItem.UVOffsets.W));
        if (texture == null)
          texture = textureAtlasItem.AtlasTexture;
      }
    }
  }
}
