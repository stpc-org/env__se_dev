// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.TextSurfaceScripts.MyTSSFactionIcon
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using System;
using VRage;
using VRage.Game;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI;
using VRageMath;

namespace Sandbox.Game.GameSystems.TextSurfaceScripts
{
  [MyTextSurfaceScript("TSS_FactionIcon", "DisplayName_TSS_FactionIcon")]
  public class MyTSSFactionIcon : MyTSSCommon
  {
    private const string BG_TEXTURE_NAME = "SquareSimple";
    private MyFaction m_faction;
    private float m_iconSize;
    private string m_errorMsg;
    private bool m_updateNeeded = true;

    public MyTSSFactionIcon(Sandbox.ModAPI.IMyTextSurface surface, VRage.Game.ModAPI.IMyCubeBlock block, Vector2 size)
      : base((Sandbox.ModAPI.Ingame.IMyTextSurface) surface, (VRage.Game.ModAPI.Ingame.IMyCubeBlock) block, size)
    {
      MySession.Static.Factions.FactionEdited += new Action<long>(this.OnFactionEdited);
      MySession.Static.Factions.FactionCreated += new Action<long>(this.OnFactionEdited);
      this.UpdateFactionData();
      this.m_iconSize = Math.Min(this.m_surface.SurfaceSize.X, this.m_surface.SurfaceSize.Y);
      this.m_errorMsg = MyTexts.GetString(MySpaceTexts.TSS_FactionIcon_Error);
      if (!(block is MyTerminalBlock myTerminalBlock))
        return;
      myTerminalBlock.OwnershipChanged += new Action<MyTerminalBlock>(this.OnOwnershipChanged);
    }

    private bool UpdateFactionData(long editedFactionId = 0)
    {
      MyFaction factionByTag = MySession.Static.Factions.TryGetFactionByTag(this.m_block.GetOwnerFactionTag(), (IMyFaction) null);
      if (factionByTag == this.m_faction && (factionByTag == null || factionByTag.FactionId != editedFactionId && editedFactionId != 0L))
        return false;
      this.m_faction = factionByTag;
      return true;
    }

    public override void Run()
    {
      base.Run();
      if (!this.m_updateNeeded)
        return;
      using (MySpriteDrawFrame frame = this.m_surface.DrawFrame())
      {
        Vector2 position = this.m_halfSize - this.m_surface.SurfaceSize * 0.5f + new Vector2(this.m_surface.SurfaceSize.X * 0.5f, this.m_surface.SurfaceSize.Y * 0.5f);
        if (this.m_faction == null || !this.m_faction.FactionIcon.HasValue)
        {
          this.AddBackground(frame, new Color?(new Color(this.m_backgroundColor, 0.66f)));
          MySprite text = MySprite.CreateText(this.m_errorMsg, this.m_fontId, Color.White);
          text.Position = new Vector2?(position);
          frame.Add(text);
          this.m_updateNeeded = false;
          return;
        }
        MySprite sprite1 = MySprite.CreateSprite("SquareSimple", position, this.m_surface.SurfaceSize);
        sprite1.Color = new Color?(MyColorPickerConstants.HSVOffsetToHSV(this.m_faction.CustomColor).HSVtoColor());
        frame.Add(sprite1);
        MySprite sprite2 = MySprite.CreateSprite(this.m_faction.FactionIcon.Value.ToString(), position, new Vector2(this.m_iconSize));
        sprite2.Color = new Color?(MyColorPickerConstants.HSVOffsetToHSV(this.m_faction.IconColor).HSVtoColor());
        frame.Add(sprite2);
      }
      this.m_updateNeeded = false;
    }

    private void OnFactionEdited(long factionId)
    {
      if (this.m_faction == null)
      {
        if (!this.UpdateFactionData(factionId))
          return;
        this.m_updateNeeded = true;
      }
      else
      {
        if (this.m_faction == null || factionId != this.m_faction.FactionId)
          return;
        this.m_updateNeeded = true;
      }
    }

    private void OnOwnershipChanged(MyTerminalBlock obj)
    {
      if (!this.UpdateFactionData())
        return;
      this.m_updateNeeded = true;
    }

    public override void Dispose()
    {
      if (this.m_block is MyTerminalBlock block)
        block.OwnershipChanged -= new Action<MyTerminalBlock>(this.OnOwnershipChanged);
      MySession.Static.Factions.FactionEdited -= new Action<long>(this.OnFactionEdited);
      MySession.Static.Factions.FactionCreated -= new Action<long>(this.OnFactionEdited);
      this.m_faction = (MyFaction) null;
    }

    public override ScriptUpdate NeedsUpdate => ScriptUpdate.Update10;
  }
}
