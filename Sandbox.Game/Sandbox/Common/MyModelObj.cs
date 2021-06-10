// Decompiled with JetBrains decompiler
// Type: Sandbox.Common.MyModelObj
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using VRage.Game.Models;
using VRageMath;

namespace Sandbox.Common
{
  internal class MyModelObj
  {
    public List<Vector3> Vertexes;
    public List<Vector3> Normals;
    public List<MyTriangleVertexIndices> Triangles;

    public MyModelObj(string filename)
    {
      this.Vertexes = new List<Vector3>();
      this.Normals = new List<Vector3>();
      this.Triangles = new List<MyTriangleVertexIndices>();
      foreach (string[] lineToken in this.GetLineTokens(filename))
        this.ParseObjLine(lineToken);
    }

    private IEnumerable<string[]> GetLineTokens(string filename)
    {
      using (StreamReader reader = new StreamReader(filename))
      {
        int lineNumber = 1;
        while (!reader.EndOfStream)
        {
          string[] strArray = Regex.Split(reader.ReadLine().Trim(), "\\s+");
          if (strArray.Length != 0 && strArray[0] != string.Empty && !strArray[0].StartsWith("#"))
            yield return strArray;
          ++lineNumber;
        }
      }
    }

    private void ParseObjLine(string[] lineTokens)
    {
      string lower = lineTokens[0].ToLower();
      if (!(lower == "v"))
      {
        if (!(lower == "vn"))
        {
          if (!(lower == "f"))
            return;
          int[] numArray = new int[3];
          for (int index = 1; index <= 3; ++index)
          {
            string[] strArray = lineTokens[index].Split('/');
            if (strArray.Length != 0)
              numArray[index - 1] = int.Parse(strArray[0], (IFormatProvider) CultureInfo.InvariantCulture);
          }
          this.Triangles.Add(new MyTriangleVertexIndices(numArray[0] - 1, numArray[1] - 1, numArray[2] - 1));
        }
        else
          this.Normals.Add(MyModelObj.ParseVector3(lineTokens));
      }
      else
        this.Vertexes.Add(MyModelObj.ParseVector3(lineTokens));
    }

    private static Vector3 ParseVector3(string[] lineTokens) => new Vector3(float.Parse(lineTokens[1], (IFormatProvider) CultureInfo.InvariantCulture), float.Parse(lineTokens[2], (IFormatProvider) CultureInfo.InvariantCulture), float.Parse(lineTokens[3], (IFormatProvider) CultureInfo.InvariantCulture));
  }
}
