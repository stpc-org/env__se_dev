// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyExportModel
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using VRage.Game.Models;
using VRageMath;
using VRageMath.PackedVector;
using VRageRender;
using VRageRender.Import;

namespace Sandbox.Game.Entities
{
  public class MyExportModel
  {
    private readonly MyModel m_model;
    private List<MyExportModel.Material> m_materials;

    public float PatternScale => this.m_model.PatternScale;

    public MyExportModel(MyModel model)
    {
      this.m_model = new MyModel(model.AssetName);
      this.m_model.LoadUV = true;
      this.m_model.LoadData(true);
      this.ExtractMaterialsFromModel();
    }

    public HalfVector2[] GetTexCoords() => this.m_model.TexCoords;

    public List<MyExportModel.Material> GetMaterials() => this.m_materials;

    public int GetVerticesCount() => this.m_model.GetVerticesCount();

    public int GetTrianglesCount() => this.m_model.GetTrianglesCount();

    public MyTriangleVertexIndices GetTriangle(int index) => this.m_model.GetTriangle(index);

    public Vector3 GetVertex(int index) => this.m_model.GetVertex(index);

    private void ExtractMaterialsFromModel()
    {
      this.m_materials = new List<MyExportModel.Material>();
      List<VRageRender.Models.MyMesh> meshList = this.m_model.GetMeshList();
      if (meshList == null)
        return;
      foreach (VRageRender.Models.MyMesh myMesh in meshList)
      {
        if (myMesh.Material != null)
        {
          Dictionary<string, string> textures = myMesh.Material.Textures;
          if (textures != null)
            this.m_materials.Add(new MyExportModel.Material()
            {
              AddMapsTexture = textures.Get<string, string>("AddMapsTexture"),
              AlphamaskTexture = textures.Get<string, string>("AlphamaskTexture"),
              ColorMetalTexture = textures.Get<string, string>("ColorMetalTexture"),
              NormalGlossTexture = textures.Get<string, string>("NormalGlossTexture"),
              FirstTri = myMesh.IndexStart / 3,
              LastTri = myMesh.IndexStart / 3 + myMesh.TriCount - 1,
              IsGlass = myMesh.Material.DrawTechnique == MyMeshDrawTechnique.GLASS || myMesh.Material.DrawTechnique == MyMeshDrawTechnique.HOLO || myMesh.Material.DrawTechnique == MyMeshDrawTechnique.SHIELD || myMesh.Material.DrawTechnique == MyMeshDrawTechnique.SHIELD_LIT
            });
        }
      }
    }

    public struct Material
    {
      public int LastTri;
      public int FirstTri;
      public bool IsGlass;
      public Vector3 ColorMaskHSV;
      public string ExportedMaterialName;
      public string AddMapsTexture;
      public string AlphamaskTexture;
      public string ColorMetalTexture;
      public string NormalGlossTexture;

      public bool EqualsMaterialWise(MyExportModel.Material x) => string.Equals(this.AddMapsTexture, x.AddMapsTexture, StringComparison.OrdinalIgnoreCase) && string.Equals(this.AlphamaskTexture, x.AlphamaskTexture, StringComparison.OrdinalIgnoreCase) && string.Equals(this.ColorMetalTexture, x.ColorMetalTexture, StringComparison.OrdinalIgnoreCase) && string.Equals(this.NormalGlossTexture, x.NormalGlossTexture, StringComparison.OrdinalIgnoreCase);
    }
  }
}
