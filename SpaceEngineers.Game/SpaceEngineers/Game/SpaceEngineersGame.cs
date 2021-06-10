// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.SpaceEngineersGame
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using EmptyKeys.UserInterface.Mvvm;
using Multiplayer;
using Sandbox;
using Sandbox.Engine.Analytics;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Platform.VideoMode;
using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using Sandbox.Game;
using Sandbox.Game.AI.Pathfinding.Obsolete;
using Sandbox.Game.AI.Pathfinding.RecastDetour;
using Sandbox.Game.Entities;
using Sandbox.Game.Screens;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using SpaceEngineers.Game.AI;
using SpaceEngineers.Game.GUI;
using SpaceEngineers.Game.VoiceChat;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VRage;
using VRage.Analytics;
using VRage.Data.Audio;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Library.Utils;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Messages;
using World;

namespace SpaceEngineers.Game
{
  public class SpaceEngineersGame : MySandboxGame
  {
    public const int SE_VERSION = 1198033;
    private Vector2I m_initializedScreenSize;

    public SpaceEngineersGame(string[] commandlineArgs)
      : base(commandlineArgs, IntPtr.Zero)
    {
      MySandboxGame.GameCustomInitialization = (MySandboxGame.IGameCustomInitialization) new MySpaceGameCustomInitialization();
      SpaceEngineersGame.FillCredits();
    }

    private void OnRenderInitialized(Vector2I size)
    {
      this.OnScreenSize = this.OnScreenSize - new Action<Vector2I>(this.OnRenderInitialized);
      this.m_initializedScreenSize = size;
      MySandboxGame.m_windowCreatedEvent.Set();
    }

    protected override void InitializeRender(IntPtr windowHandle)
    {
      this.OnScreenSize = this.OnScreenSize + new Action<Vector2I>(this.OnRenderInitialized);
      base.InitializeRender(windowHandle);
      this.StartIntroVideo();
    }

    private void StartIntroVideo()
    {
      if (!MyPlatformGameSettings.ENABLE_LOGOS || !MyPlatformGameSettings.ENABLE_LOGOS_ASAP)
        return;
      this.ProcessInvoke();
      MyRenderProxy.Settings.RenderThreadHighPriority = true;
      MyRenderProxy.SwitchRenderSettings(MyRenderProxy.Settings);
      this.IntroVideoId = MyRenderProxy.PlayVideo(Path.Combine(MyFileSystem.ContentPath, "Videos\\KSH.wmv"), 1f);
      MyRenderProxy.UpdateVideo(this.IntroVideoId);
      MyRenderProxy.DrawVideo(this.IntroVideoId, new Rectangle(0, 0, this.m_initializedScreenSize.X, this.m_initializedScreenSize.Y), Color.White, MyVideoRectangleFitMode.AutoFit, true);
      MyScreenManager.AddScreen((MyGuiScreenBase) MyGuiScreenInitialLoading.Instance);
      MyRenderProxy.AfterUpdate(new MyTimeSpan?());
      MyRenderProxy.BeforeUpdate();
      MyVRage.Platform.Windows.Window.ShowAndFocus();
    }

    public static void SetupBasicGameInfo()
    {
      MyPerGameSettings.BasicGameInfo.GameVersion = new int?(1198033);
      MyPerGameSettings.BasicGameInfo.GameName = "Space Engineers";
      MyPerGameSettings.BasicGameInfo.GameNameSafe = "SpaceEngineers";
      MyPerGameSettings.BasicGameInfo.ApplicationName = "SpaceEngineers";
      MyPerGameSettings.BasicGameInfo.GameAcronym = "SE";
      MyPerGameSettings.BasicGameInfo.MinimumRequirementsWeb = "http://www.spaceengineersgame.com";
      MyPerGameSettings.BasicGameInfo.SplashScreenImage = "..\\Content\\Textures\\Logo\\splashscreen.png";
      if (Sandbox.Engine.Platform.Game.IsDedicated)
        MyPerGameSettings.BasicGameInfo.AnalyticId = MyPerGameSettings.BasicGameInfo.GameAcronym + "DS";
      else
        MyPerGameSettings.BasicGameInfo.AnalyticId = MyPerGameSettings.BasicGameInfo.GameAcronym;
    }

    public static void SetupPerGameSettings()
    {
      MyPerGameSettings.Game = GameEnum.SE_GAME;
      MyPerGameSettings.GameIcon = "SpaceEngineers.ico";
      MyPerGameSettings.EnableGlobalGravity = false;
      MyPerGameSettings.GameModAssembly = "SpaceEngineers.Game.dll";
      MyPerGameSettings.GameModObjBuildersAssembly = "SpaceEngineers.ObjectBuilders.dll";
      MyPerGameSettings.OffsetVoxelMapByHalfVoxel = true;
      MyPerGameSettings.EnablePregeneratedAsteroidHack = true;
      if (Sandbox.Engine.Platform.Game.IsDedicated)
        MySandboxGame.ConfigDedicated = (IMyConfigDedicated) new MyConfigDedicated<MyObjectBuilder_SessionSettings>("SpaceEngineers-Dedicated.cfg");
      MySandboxGame.GameCustomInitialization = (MySandboxGame.IGameCustomInitialization) new MySpaceGameCustomInitialization();
      MyPerGameSettings.ShowObfuscationStatus = false;
      MyPerGameSettings.UseNewDamageEffects = true;
      MyPerGameSettings.EnableResearch = true;
      MyPerGameSettings.UseVolumeLimiter = MyFakes.ENABLE_NEW_SOUNDS && MyFakes.ENABLE_REALISTIC_LIMITER;
      MyPerGameSettings.UseSameSoundLimiter = true;
      MyPerGameSettings.UseMusicController = true;
      MyPerGameSettings.UseReverbEffect = true;
      MyPerGameSettings.Destruction = false;
      MyPerGameSettings.MainMenuTrack = new MyMusicTrack?(new MyMusicTrack()
      {
        TransitionCategory = MyStringId.GetOrCompute("NoRandom"),
        MusicCategory = MyStringId.GetOrCompute("MusicMenu")
      });
      MyPerGameSettings.BallFriendlyPhysics = false;
      MyPerGameSettings.PathfindingType = !MyFakes.ENABLE_CESTMIR_PATHFINDING ? typeof (MyRDPathfinding) : typeof (MyPathfinding);
      MyPerGameSettings.BotFactoryType = typeof (MySpaceBotFactory);
      MyPerGameSettings.ControlMenuInitializerType = typeof (MySpaceControlMenuInitializer);
      MyPerGameSettings.EnableScenarios = true;
      MyPerGameSettings.EnableJumpDrive = true;
      MyPerGameSettings.EnableShipSoundSystem = true;
      MyFakes.ENABLE_PLANETS_JETPACK_LIMIT_IN_CREATIVE = true;
      MyFakes.ENABLE_DRIVING_PARTICLES = true;
      MyPerGameSettings.EnablePathfinding = false;
      MyPerGameSettings.CharacterGravityMultiplier = 2f;
      MyDebugDrawSettings.DEBUG_DRAW_MOUNT_POINTS_AXIS_HELPERS = true;
      MyPerGameSettings.EnableRagdollInJetpack = true;
      MyPerGameSettings.GUI.OptionsScreen = typeof (MyGuiScreenOptionsSpace);
      MyPerGameSettings.GUI.PerformanceWarningScreen = typeof (MyGuiScreenPerformanceWarnings);
      MyPerGameSettings.GUI.CreateFactionScreen = typeof (MyGuiScreenCreateOrEditFactionSpace);
      MyPerGameSettings.GUI.MainMenu = typeof (MyGuiScreenMainMenu);
      MyPerGameSettings.DefaultGraphicsRenderer = MySandboxGame.DirectX11RendererKey;
      MyPerGameSettings.EnableWelderAutoswitch = true;
      MyPerGameSettings.CompatHelperType = typeof (MySpaceSessionCompatHelper);
      MyPerGameSettings.GUI.MainMenuBackgroundVideos = new string[10]
      {
        "Videos\\Background01_720p.wmv",
        "Videos\\Background02_720p.wmv",
        "Videos\\Background03_720p.wmv",
        "Videos\\Background04_720p.wmv",
        "Videos\\Background05_720p.wmv",
        "Videos\\Background09_720p.wmv",
        "Videos\\Background10_720p.wmv",
        "Videos\\Background11_720p.wmv",
        "Videos\\Background12_720p.wmv",
        "Videos\\Background13_720p.wmv"
      };
      MyPerGameSettings.VoiceChatEnabled = true;
      MyPerGameSettings.VoiceChatLogic = typeof (MyVoiceChatLogic);
      MyPerGameSettings.ClientStateType = typeof (MySpaceClientState);
      MyVoxelPhysicsBody.UseLod1VoxelPhysics = false;
      MyPerGameSettings.EnableAi = true;
      MyPerGameSettings.EnablePathfinding = true;
      MyPerGameSettings.UpdateOrchestratorType = typeof (MyParallelEntityUpdateOrchestrator);
      MyFakesLocal.SetupLocalPerGameSettings();
    }

    private static void FillCredits()
    {
      MyCreditsDepartment creditsDepartment1 = new MyCreditsDepartment("{LOCG:Department_ExecutiveProducer}");
      MyPerGameSettings.Credits.Departments.Add(creditsDepartment1);
      creditsDepartment1.Persons = new List<MyCreditsPerson>();
      creditsDepartment1.Persons.Add(new MyCreditsPerson("MAREK ROSA"));
      MyCreditsDepartment creditsDepartment2 = new MyCreditsDepartment("{LOCG:Department_LeadProducer}");
      MyPerGameSettings.Credits.Departments.Add(creditsDepartment2);
      creditsDepartment2.Persons = new List<MyCreditsPerson>();
      creditsDepartment2.Persons.Add(new MyCreditsPerson("PETR MINARIK"));
      MyCreditsDepartment creditsDepartment3 = new MyCreditsDepartment("{LOCG:Department_TeamOperations}");
      MyPerGameSettings.Credits.Departments.Add(creditsDepartment3);
      creditsDepartment3.Persons = new List<MyCreditsPerson>();
      creditsDepartment3.Persons.Add(new MyCreditsPerson("VLADISLAV POLGAR"));
      MyCreditsDepartment creditsDepartment4 = new MyCreditsDepartment("{LOCG:Department_TechnicalDirector}");
      MyPerGameSettings.Credits.Departments.Add(creditsDepartment4);
      creditsDepartment4.Persons = new List<MyCreditsPerson>();
      creditsDepartment4.Persons.Add(new MyCreditsPerson("JAN \"CENDA\" HLOUSEK"));
      MyCreditsDepartment creditsDepartment5 = new MyCreditsDepartment("{LOCG:Department_LeadProgrammers}");
      MyPerGameSettings.Credits.Departments.Add(creditsDepartment5);
      creditsDepartment5.Persons = new List<MyCreditsPerson>();
      creditsDepartment5.Persons.Add(new MyCreditsPerson("FILIP DUSEK"));
      creditsDepartment5.Persons.Add(new MyCreditsPerson("JAN \"CENDA\" HLOUSEK"));
      creditsDepartment5.Persons.Add(new MyCreditsPerson("PETR MINARIK"));
      MyCreditsDepartment creditsDepartment6 = new MyCreditsDepartment("{LOCG:Department_Programmers}");
      MyPerGameSettings.Credits.Departments.Add(creditsDepartment6);
      creditsDepartment6.Persons = new List<MyCreditsPerson>();
      creditsDepartment6.Persons.Add(new MyCreditsPerson("PETR BERANEK"));
      creditsDepartment6.Persons.Add(new MyCreditsPerson("MARTIN PAVLICEK"));
      creditsDepartment6.Persons.Add(new MyCreditsPerson("DANIEL ILHA"));
      creditsDepartment6.Persons.Add(new MyCreditsPerson("MIRO FARKAS"));
      creditsDepartment6.Persons.Add(new MyCreditsPerson("GRZEGORZ ZADROGA"));
      creditsDepartment6.Persons.Add(new MyCreditsPerson("SANDRA LENARDOVA"));
      MyCreditsDepartment creditsDepartment7 = new MyCreditsDepartment("{LOCG:Department_LeadDesigner}");
      MyPerGameSettings.Credits.Departments.Add(creditsDepartment7);
      creditsDepartment7.Persons = new List<MyCreditsPerson>();
      creditsDepartment7.Persons.Add(new MyCreditsPerson("JOACHIM KOOLHOF"));
      MyCreditsDepartment creditsDepartment8 = new MyCreditsDepartment("{LOCG:Department_Designers}");
      MyPerGameSettings.Credits.Departments.Add(creditsDepartment8);
      creditsDepartment8.Persons = new List<MyCreditsPerson>();
      creditsDepartment8.Persons.Add(new MyCreditsPerson("ALES KOZAK"));
      MyCreditsDepartment creditsDepartment9 = new MyCreditsDepartment("{LOCG:Department_LeadArtist}");
      MyPerGameSettings.Credits.Departments.Add(creditsDepartment9);
      creditsDepartment9.Persons = new List<MyCreditsPerson>();
      creditsDepartment9.Persons.Add(new MyCreditsPerson("NATIQ AGHAYEV"));
      MyCreditsDepartment creditsDepartment10 = new MyCreditsDepartment("{LOCG:Department_Artists}");
      MyPerGameSettings.Credits.Departments.Add(creditsDepartment10);
      creditsDepartment10.Persons = new List<MyCreditsPerson>();
      creditsDepartment10.Persons.Add(new MyCreditsPerson("KRISTIAAN RENAERTS"));
      creditsDepartment10.Persons.Add(new MyCreditsPerson("JAN TRAUSKE"));
      MyCreditsDepartment creditsDepartment11 = new MyCreditsDepartment("{LOCG:Department_SoundDesign}");
      MyPerGameSettings.Credits.Departments.Add(creditsDepartment11);
      creditsDepartment11.Persons = new List<MyCreditsPerson>();
      creditsDepartment11.Persons.Add(new MyCreditsPerson("LUKAS TVRDON"));
      MyCreditsDepartment creditsDepartment12 = new MyCreditsDepartment("{LOCG:Department_Music}");
      MyPerGameSettings.Credits.Departments.Add(creditsDepartment12);
      creditsDepartment12.Persons = new List<MyCreditsPerson>();
      creditsDepartment12.Persons.Add(new MyCreditsPerson("KAREL ANTONIN"));
      creditsDepartment12.Persons.Add(new MyCreditsPerson("ANNA KALHAUSOVA (cello)"));
      creditsDepartment12.Persons.Add(new MyCreditsPerson("MARIE SVOBODOVA (vocals)"));
      MyCreditsDepartment creditsDepartment13 = new MyCreditsDepartment("{LOCG:Department_Video}");
      MyPerGameSettings.Credits.Departments.Add(creditsDepartment13);
      creditsDepartment13.Persons = new List<MyCreditsPerson>();
      creditsDepartment13.Persons.Add(new MyCreditsPerson("JOEL \"XOCLIW\" WILCOX"));
      MyCreditsDepartment creditsDepartment14 = new MyCreditsDepartment("{LOCG:Department_LeadTester}");
      MyPerGameSettings.Credits.Departments.Add(creditsDepartment14);
      creditsDepartment14.Persons = new List<MyCreditsPerson>();
      creditsDepartment14.Persons.Add(new MyCreditsPerson("ONDREJ NAHALKA"));
      MyCreditsDepartment creditsDepartment15 = new MyCreditsDepartment("{LOCG:Department_Testers}");
      MyPerGameSettings.Credits.Departments.Add(creditsDepartment15);
      creditsDepartment15.Persons = new List<MyCreditsPerson>();
      creditsDepartment15.Persons.Add(new MyCreditsPerson("KATERINA CERVENA"));
      creditsDepartment15.Persons.Add(new MyCreditsPerson("JAN HRIVNAC"));
      creditsDepartment15.Persons.Add(new MyCreditsPerson("ALES KOZAK"));
      creditsDepartment15.Persons.Add(new MyCreditsPerson("VOJTECH NEORAL"));
      creditsDepartment15.Persons.Add(new MyCreditsPerson("JAN PETRZILKA"));
      MyCreditsDepartment creditsDepartment16 = new MyCreditsDepartment("{LOCG:Department_CommunityPr}");
      MyPerGameSettings.Credits.Departments.Add(creditsDepartment16);
      creditsDepartment16.Persons = new List<MyCreditsPerson>();
      creditsDepartment16.Persons.Add(new MyCreditsPerson("JESSE BAULE"));
      creditsDepartment16.Persons.Add(new MyCreditsPerson("JOEL \"XOCLIW\" WILCOX"));
      MyCreditsDepartment creditsDepartment17 = new MyCreditsDepartment("Frostbite scenario");
      MyPerGameSettings.Credits.Departments.Add(creditsDepartment17);
      creditsDepartment17.Persons = new List<MyCreditsPerson>();
      creditsDepartment17.Persons.Add(new MyCreditsPerson("Petr Minarik"));
      creditsDepartment17.Persons.Add(new MyCreditsPerson("Jan Vanecek"));
      creditsDepartment17.Persons.Add(new MyCreditsPerson("Joachim Koolhof"));
      creditsDepartment17.Persons.Add(new MyCreditsPerson("Mikko Saari"));
      creditsDepartment17.Persons.Add(new MyCreditsPerson("Timothy Gatton"));
      creditsDepartment17.Persons.Add(new MyCreditsPerson("Pepijn van Duijn"));
      creditsDepartment17.Persons.Add(new MyCreditsPerson("Jesse Baule"));
      creditsDepartment17.Persons.Add(new MyCreditsPerson("Dusan Repik"));
      creditsDepartment17.Persons.Add(new MyCreditsPerson("Joel Wilcox"));
      creditsDepartment17.Persons.Add(new MyCreditsPerson("Natiq Aghayev"));
      creditsDepartment17.Persons.Add(new MyCreditsPerson("Jan Trauske"));
      creditsDepartment17.Persons.Add(new MyCreditsPerson("Jan Golmic"));
      creditsDepartment17.Persons.Add(new MyCreditsPerson("Kristiaan Renaerts"));
      creditsDepartment17.Persons.Add(new MyCreditsPerson("Pavel Konfrst"));
      creditsDepartment17.Persons.Add(new MyCreditsPerson("Lukas Tvrdon"));
      creditsDepartment17.Persons.Add(new MyCreditsPerson("Satoko Yamaoko"));
      creditsDepartment17.Persons.Add(new MyCreditsPerson("Nicole Draper"));
      creditsDepartment17.Persons.Add(new MyCreditsPerson("Victor Hugo Monaco"));
      creditsDepartment17.Persons.Add(new MyCreditsPerson("Chris Bayne a.k.a DirectedEnergy"));
      creditsDepartment17.Persons.Add(new MyCreditsPerson("Lela Kovalenko a.k.a.Naburine"));
      creditsDepartment17.Persons.Add(new MyCreditsPerson("Nathan \"Silverbane\" Steen"));
      creditsDepartment17.Persons.Add(new MyCreditsPerson("Skyler \"Gorhamian\" Gorham"));
      creditsDepartment17.Persons.Add(new MyCreditsPerson("Jacob \"wearsglasses\" Ruttenberg"));
      creditsDepartment17.Persons.Add(new MyCreditsPerson("Yang Yafang"));
      MyCreditsDepartment creditsDepartment18 = new MyCreditsDepartment("{LOCG:Department_Office}");
      MyPerGameSettings.Credits.Departments.Add(creditsDepartment18);
      creditsDepartment18.Persons = new List<MyCreditsPerson>();
      creditsDepartment18.Persons.Add(new MyCreditsPerson("MARIANNA HIRCAKOVA"));
      creditsDepartment18.Persons.Add(new MyCreditsPerson("PETR KREJCI"));
      creditsDepartment18.Persons.Add(new MyCreditsPerson("LUCIE KRESTOVA"));
      creditsDepartment18.Persons.Add(new MyCreditsPerson("VACLAV NOVOTNY"));
      creditsDepartment18.Persons.Add(new MyCreditsPerson("TOMAS STROUHAL"));
      MyCreditsDepartment creditsDepartment19 = new MyCreditsDepartment("{LOCG:Department_CommunityManagers}");
      MyPerGameSettings.Credits.Departments.Add(creditsDepartment19);
      creditsDepartment19.Persons = new List<MyCreditsPerson>();
      creditsDepartment19.Persons.Add(new MyCreditsPerson("Dr Vagax"));
      creditsDepartment19.Persons.Add(new MyCreditsPerson("Conrad Larson"));
      creditsDepartment19.Persons.Add(new MyCreditsPerson("Dan2D3D"));
      creditsDepartment19.Persons.Add(new MyCreditsPerson("RayvenQ"));
      creditsDepartment19.Persons.Add(new MyCreditsPerson("Redphoenix"));
      creditsDepartment19.Persons.Add(new MyCreditsPerson("TodesRitter"));
      MyCreditsDepartment creditsDepartment20 = new MyCreditsDepartment("{LOCG:Department_ModContributors}");
      MyPerGameSettings.Credits.Departments.Add(creditsDepartment20);
      creditsDepartment20.Persons = new List<MyCreditsPerson>();
      creditsDepartment20.Persons.Add(new MyCreditsPerson("Tyrsis"));
      creditsDepartment20.Persons.Add(new MyCreditsPerson("Daniel \"Phoenix84\" Osborne"));
      creditsDepartment20.Persons.Add(new MyCreditsPerson("Morten \"Malware\" Aune Lyrstad"));
      creditsDepartment20.Persons.Add(new MyCreditsPerson("Arindel"));
      creditsDepartment20.Persons.Add(new MyCreditsPerson("Darth Biomech"));
      creditsDepartment20.Persons.Add(new MyCreditsPerson("Night Lone"));
      creditsDepartment20.Persons.Add(new MyCreditsPerson("Mexmer"));
      creditsDepartment20.Persons.Add(new MyCreditsPerson("JD.Horx"));
      creditsDepartment20.Persons.Add(new MyCreditsPerson("John \"Jimmacle\" Gross"));
      creditsDepartment20.Persons.Add(new MyCreditsPerson("Logan Tyran"));
      MyCreditsDepartment creditsDepartment21 = new MyCreditsDepartment("{LOCG:Department_Translators}");
      MyPerGameSettings.Credits.Departments.Add(creditsDepartment21);
      creditsDepartment21.Persons = new List<MyCreditsPerson>();
      creditsDepartment21.Persons.Add(new MyCreditsPerson("Damian \"Truzaku\" Komarek"));
      creditsDepartment21.Persons.Add(new MyCreditsPerson("Julian Tomaszewski"));
      creditsDepartment21.Persons.Add(new MyCreditsPerson("George Grivas"));
      creditsDepartment21.Persons.Add(new MyCreditsPerson("Олег \"AaLeSsHhKka\" Цюпка"));
      creditsDepartment21.Persons.Add(new MyCreditsPerson("Maxim \"Ma)(imuM\" Lyashuk"));
      creditsDepartment21.Persons.Add(new MyCreditsPerson("Axazel"));
      creditsDepartment21.Persons.Add(new MyCreditsPerson("Baly94"));
      creditsDepartment21.Persons.Add(new MyCreditsPerson("Dyret"));
      creditsDepartment21.Persons.Add(new MyCreditsPerson("gon.gged"));
      creditsDepartment21.Persons.Add(new MyCreditsPerson("Huberto"));
      creditsDepartment21.Persons.Add(new MyCreditsPerson("HunterNephilim"));
      creditsDepartment21.Persons.Add(new MyCreditsPerson("nintendo22"));
      creditsDepartment21.Persons.Add(new MyCreditsPerson("Quellix"));
      creditsDepartment21.Persons.Add(new MyCreditsPerson("raviool"));
      creditsDepartment21.Persons.Add(new MyCreditsPerson("Dr. Bell"));
      creditsDepartment21.Persons.Add(new MyCreditsPerson("Dominik Frydl"));
      creditsDepartment21.Persons.Add(new MyCreditsPerson("Daniel Hloušek"));
      creditsDepartment21.Persons.Add(new MyCreditsPerson("Andre Camara Marchi"));
      creditsDepartment21.Persons.Add(new MyCreditsPerson("Ociotek Traducciones"));
      creditsDepartment21.LogoTexture = "Textures\\Logo\\TranslatorsCN.dds";
      creditsDepartment21.LogoScale = new float?(0.85f);
      creditsDepartment21.LogoTextureSize = new Vector2?(MyRenderProxy.GetTextureSize(creditsDepartment21.LogoTexture));
      creditsDepartment21.LogoOffsetPost = 0.11f;
      MyCreditsDepartment creditsDepartment22 = new MyCreditsDepartment("{LOCG:Department_SpecialThanks}");
      MyPerGameSettings.Credits.Departments.Add(creditsDepartment22);
      creditsDepartment22.Persons = new List<MyCreditsPerson>();
      creditsDepartment22.Persons.Add(new MyCreditsPerson("ABDULAZIZ ALDIGS"));
      creditsDepartment22.Persons.Add(new MyCreditsPerson("DUSAN ANDRAS"));
      creditsDepartment22.Persons.Add(new MyCreditsPerson("ONDREJ ANGELOVIC"));
      creditsDepartment22.Persons.Add(new MyCreditsPerson("IVAN BARAN"));
      creditsDepartment22.Persons.Add(new MyCreditsPerson("ANTON \"TOTAL\" BAUER"));
      creditsDepartment22.Persons.Add(new MyCreditsPerson("ALES BRICH"));
      creditsDepartment22.Persons.Add(new MyCreditsPerson("JOAO CARIAS"));
      creditsDepartment22.Persons.Add(new MyCreditsPerson("THEO ESCAMEZ"));
      creditsDepartment22.Persons.Add(new MyCreditsPerson("ALEX FLOREA"));
      creditsDepartment22.Persons.Add(new MyCreditsPerson("JAN GOLMIC"));
      creditsDepartment22.Persons.Add(new MyCreditsPerson("CESTMIR HOUSKA"));
      creditsDepartment22.Persons.Add(new MyCreditsPerson("JAKUB HRNCIR"));
      creditsDepartment22.Persons.Add(new MyCreditsPerson("LUKAS CHRAPEK"));
      creditsDepartment22.Persons.Add(new MyCreditsPerson("LUKAS JANDIK"));
      creditsDepartment22.Persons.Add(new MyCreditsPerson("MARKETA JAROSOVA"));
      creditsDepartment22.Persons.Add(new MyCreditsPerson("MARTIN KOCISEK"));
      creditsDepartment22.Persons.Add(new MyCreditsPerson("JOELLEN KOESTER"));
      creditsDepartment22.Persons.Add(new MyCreditsPerson("GREGORY KONTADAKIS"));
      creditsDepartment22.Persons.Add(new MyCreditsPerson("MARKO KORHONEN"));
      creditsDepartment22.Persons.Add(new MyCreditsPerson("TOMAS KOSEK"));
      creditsDepartment22.Persons.Add(new MyCreditsPerson("RADOVAN KOTRLA"));
      creditsDepartment22.Persons.Add(new MyCreditsPerson("MARTIN KROSLAK"));
      creditsDepartment22.Persons.Add(new MyCreditsPerson("MICHAL KUCIS"));
      creditsDepartment22.Persons.Add(new MyCreditsPerson("DANIEL LEIMBACH"));
      creditsDepartment22.Persons.Add(new MyCreditsPerson("RADKA LISA"));
      creditsDepartment22.Persons.Add(new MyCreditsPerson("PERCY LIU"));
      creditsDepartment22.Persons.Add(new MyCreditsPerson("GEORGE MAMAKOS"));
      creditsDepartment22.Persons.Add(new MyCreditsPerson("BRANT MARTIN"));
      creditsDepartment22.Persons.Add(new MyCreditsPerson("JAN NEKVAPIL"));
      creditsDepartment22.Persons.Add(new MyCreditsPerson("MAREK OBRSAL"));
      creditsDepartment22.Persons.Add(new MyCreditsPerson("PAVEL OCOVAJ"));
      creditsDepartment22.Persons.Add(new MyCreditsPerson("PREMYSL PASKA"));
      creditsDepartment22.Persons.Add(new MyCreditsPerson("ONDREJ PETRZILKA"));
      creditsDepartment22.Persons.Add(new MyCreditsPerson("FRANCESKO PRETTO"));
      creditsDepartment22.Persons.Add(new MyCreditsPerson("TOMAS PSENICKA"));
      creditsDepartment22.Persons.Add(new MyCreditsPerson("DOMINIK RAGANCIK"));
      creditsDepartment22.Persons.Add(new MyCreditsPerson("TOMAS RAMPAS"));
      creditsDepartment22.Persons.Add(new MyCreditsPerson("DUSAN REPIK"));
      creditsDepartment22.Persons.Add(new MyCreditsPerson("VILEM SOULAK"));
      creditsDepartment22.Persons.Add(new MyCreditsPerson("RASTKO STANOJEVIC"));
      creditsDepartment22.Persons.Add(new MyCreditsPerson("SLOBODAN STEVIC"));
      creditsDepartment22.Persons.Add(new MyCreditsPerson("TIM TOXOPEUS"));
      creditsDepartment22.Persons.Add(new MyCreditsPerson("JAN VEBERSIK"));
      creditsDepartment22.Persons.Add(new MyCreditsPerson("LUKAS VILIM"));
      creditsDepartment22.Persons.Add(new MyCreditsPerson("MATEJ VLK"));
      creditsDepartment22.Persons.Add(new MyCreditsPerson("ADAM WILLIAMS"));
      creditsDepartment22.Persons.Add(new MyCreditsPerson("CHARLES WINTERS"));
      creditsDepartment22.Persons.Add(new MyCreditsPerson("MICHAL WROBEL"));
      creditsDepartment22.Persons.Add(new MyCreditsPerson("MICHAL ZAK"));
      creditsDepartment22.Persons.Add(new MyCreditsPerson("MICHAL ZAVADAK"));
      MyCreditsDepartment creditsDepartment23 = new MyCreditsDepartment("{LOCG:Department_MoreInfo}");
      MyPerGameSettings.Credits.Departments.Add(creditsDepartment23);
      creditsDepartment23.Persons = new List<MyCreditsPerson>();
      creditsDepartment23.Persons.Add(new MyCreditsPerson("{LOCG:Person_Web}"));
      creditsDepartment23.Persons.Add(new MyCreditsPerson("{LOCG:Person_FB}"));
      creditsDepartment23.Persons.Add(new MyCreditsPerson("{LOCG:Person_Twitter}"));
      MyCreditsDepartment creditsDepartment24 = new MyCreditsDepartment("{LOCG:Department_Licenses}");
      MyPerGameSettings.Credits.Departments.Add(creditsDepartment24);
      creditsDepartment24.Persons = new List<MyCreditsPerson>();
      creditsDepartment24.Persons.Add(new MyCreditsPerson("Empty Keys UI - Copyright (c) 2018 Empty Keys"));
      creditsDepartment24.Persons.Add(new MyCreditsPerson("mod.io - Copyright (c) 2020 mod.io"));
      creditsDepartment24.Persons.Add(new MyCreditsPerson("FXAA - Copyright (c) 2010 NVIDIA Corporation. All rights reserved."));
      creditsDepartment24.Persons.Add(new MyCreditsPerson("HBAO+ - Copyright (c) 2010 NVIDIA Corporation. All rights reserved."));
      creditsDepartment24.Persons.Add(new MyCreditsPerson("StringBuilderExt - Copyright (c) Gavin Pugh 2010 - Released under the zlib license"));
      creditsDepartment24.Persons.Add(new MyCreditsPerson("SharpDX - Copyright (c) 2010-2012 SharpDX - Alexandre Mutel"));
      creditsDepartment24.Persons.Add(new MyCreditsPerson("Parallel Tasks - Microsoft Public License (Ms-PL), http://paralleltasks.codeplex.com/"));
      creditsDepartment24.Persons.Add(new MyCreditsPerson("Perlin's Improved Noise - http://mrl.nyu.edu/~perlin/noise/"));
      creditsDepartment24.Persons.Add(new MyCreditsPerson("Trace Tool - Common Public License Version 1.0 (CPL)"));
      creditsDepartment24.Persons.Add(new MyCreditsPerson("GImpact - Copyright  (c) 2006 , Francisco León."));
      creditsDepartment24.Persons.Add(new MyCreditsPerson("RestSharp - Copyright © 2009-2020 John Sheehan, Andrew Young, Alexey Zimarev and RestSharp community"));
      creditsDepartment24.Persons.Add(new MyCreditsPerson("LitJson - Unlicensed"));
      creditsDepartment24.Persons.Add(new MyCreditsPerson("Fody - Copyright (c) Simon Cropp"));
      creditsDepartment24.Persons.Add(new MyCreditsPerson("ProtoBuf - Copyright 2008 Google Inc.  All rights reserved."));
      creditsDepartment24.Persons.Add(new MyCreditsPerson("ProtoBuf.Net - Copyright 2008 Marc Gravell"));
      creditsDepartment24.Persons.Add(new MyCreditsPerson("ImageSharp - Copyright (c) Six Labors"));
      creditsDepartment24.Persons.Add(new MyCreditsPerson("Opus codec - Copyright (c) 2010-2011 Xiph.Org Foundation, Skype Limited; Written by Jean-Marc Valin and Koen Vos"));
      creditsDepartment24.Persons.Add(new MyCreditsPerson("RecastDetour - Copyright (c) 2009-2010 Mikko Mononen memon@inside.org"));
      creditsDepartment24.Persons.Add(new MyCreditsPerson("Open Asset Import Library - Copyright (c) 2006-2010, Assimp Development Team"));
      creditsDepartment24.Persons.Add(new MyCreditsPerson("Steamworks - Copyright (c) 1996-2014, Valve Corporation, All rights reserved."));
      creditsDepartment24.Persons.Add(new MyCreditsPerson("Steamworks.NET - Copyright (c) 2013-2019 Riley Labrecque"));
      creditsDepartment24.Persons.Add(new MyCreditsPerson("Epic Online Services - Copyright Epic Games, Inc. All Rights Reserved."));
      creditsDepartment24.Persons.Add(new MyCreditsPerson("Xamarin - Copyright 2011-2016 Xamarin, Copyright 2016-2019 Microsoft Inc"));
      creditsDepartment24.Persons.Add(new MyCreditsPerson("Havok - (C) Copyright 1999-2014 Telekinesys Research Limited t/a Havok. All Rights Reserved."));
      creditsDepartment24.Persons.Add(new MyCreditsPerson("Telerik - Copyright © 2020, Progress Software Corporation and/or its subsidiaries or affiliates. All Rights Reserved."));
      creditsDepartment24.Persons.Add(new MyCreditsPerson("DirectShow Net Library - Copyright (C) 2007 directshownet"));
      creditsDepartment24.Persons.Add(new MyCreditsPerson("GameAnalytics - Copyright 2020 GameAnalytics"));
      creditsDepartment24.Persons.Add(new MyCreditsPerson("NLog - Copyright (c) 2004-2020 Jaroslaw Kowalski <jaak@jkowalski.net>, Kim Christensen, Julian Verdurmen"));
      creditsDepartment24.Persons.Add(new MyCreditsPerson("XamlBehaviors - Copyright (c) 2015 Microsoft"));
      creditsDepartment24.Persons.Add(new MyCreditsPerson("Newtonsoft.Json - Copyright (c) 2007 James Newton-King"));
      MyCreditsNotice myCreditsNotice1 = new MyCreditsNotice()
      {
        LogoScale = new float?(0.8f),
        LogoTexture = "Textures\\Logo\\vrage_logo_2_0_small.dds"
      };
      myCreditsNotice1.LogoTextureSize = new Vector2?(MyRenderProxy.GetTextureSize(myCreditsNotice1.LogoTexture));
      myCreditsNotice1.CreditNoticeLines.Add(new StringBuilder("{LOCG:NoticeLine_01}"));
      myCreditsNotice1.CreditNoticeLines.Add(new StringBuilder("{LOCG:NoticeLine_02}"));
      myCreditsNotice1.CreditNoticeLines.Add(new StringBuilder("{LOCG:NoticeLine_03}"));
      myCreditsNotice1.CreditNoticeLines.Add(new StringBuilder("{LOCG:NoticeLine_04}"));
      MyPerGameSettings.Credits.CreditNotices.Add(myCreditsNotice1);
      MyCreditsNotice myCreditsNotice2 = new MyCreditsNotice()
      {
        LogoTexture = "Textures\\Logo\\havok.dds",
        LogoScale = new float?(0.65f)
      };
      myCreditsNotice2.LogoTextureSize = new Vector2?(MyRenderProxy.GetTextureSize(myCreditsNotice2.LogoTexture));
      myCreditsNotice2.CreditNoticeLines.Add(new StringBuilder("{LOCG:NoticeLine_05}"));
      myCreditsNotice2.CreditNoticeLines.Add(new StringBuilder("{LOCG:NoticeLine_06}"));
      myCreditsNotice2.CreditNoticeLines.Add(new StringBuilder("{LOCG:NoticeLine_07}"));
      MyPerGameSettings.Credits.CreditNotices.Add(myCreditsNotice2);
      SpaceEngineersGame.SetupSecrets();
    }

    private static void SetupSecrets()
    {
      MyPerGameSettings.GA_Public_GameKey = "27bae5ba5219bcd64ddbf83113eabb30";
      MyPerGameSettings.GA_Public_SecretKey = "d04e0431f97f90fae73b9d6ea99fc9746695bd11";
      MyPerGameSettings.GA_Dev_GameKey = "3a6b6ebdc48552beba3efe173488d8ba";
      MyPerGameSettings.GA_Dev_SecretKey = "caecaaa4a91f6b2598cf8ffb931b3573f20b4343";
      MyPerGameSettings.GA_Pirate_GameKey = "41827f7c8bfed902495e0e27cb57c495";
      MyPerGameSettings.GA_Pirate_SecretKey = "493b7cb3f0a472f940c0ba0c38efbb49e902cbec";
      MyPerGameSettings.GA_Other_GameKey = "4f02769277e62b4344da70967e99a2a0";
      MyPerGameSettings.GA_Other_SecretKey = "7fa773c228ce9534181adcfebf30d18bc6807d2b";
    }

    protected override void CheckGraphicsCard(
      MyRenderMessageVideoAdaptersResponse msgVideoAdapters)
    {
      base.CheckGraphicsCard(msgVideoAdapters);
      MyAdapterInfo adapter = msgVideoAdapters.Adapters[MyVideoSettingsManager.CurrentDeviceSettings.AdapterOrdinal];
      MyPerformanceSettings preset = MyGuiScreenOptionsGraphics.GetPreset(adapter.Quality);
      if (adapter.VRAM < 512000000UL)
        preset.RenderSettings.TextureQuality = preset.RenderSettings.VoxelTextureQuality = MyTextureQuality.LOW;
      else if (adapter.VRAM < 2000000000UL && adapter.Quality == MyRenderPresetEnum.HIGH)
        preset.RenderSettings.TextureQuality = preset.RenderSettings.VoxelTextureQuality = MyTextureQuality.MEDIUM;
      bool force = adapter.Quality > MyRenderPresetEnum.CUSTOM;
      MyVideoSettingsManager.UpdateRenderSettingsFromConfig(ref preset, force);
    }

    public static void SetupAnalytics()
    {
      MyLog.Default.WriteLine("SpaceEngineersGame.SetupAnalytics - START");
      IMyAnalytics tracker = MyVRage.Platform.InitAnalytics("27bae5ba5219bcd64ddbf83113eabb30:d04e0431f97f90fae73b9d6ea99fc9746695bd11", 1198033.ToString());
      if (tracker != null)
        MySpaceAnalytics.Instance.RegisterAnalyticsTracker(tracker);
      string str;
      string apiKeyId;
      string apiKey;
      if (MyGameService.BranchName == null || MyGameService.BranchName == "RETAIL" || MyGameService.BranchName == "default")
      {
        str = "se-events-live";
        apiKeyId = "_NLm-3MBTFi_5jOj-49_";
        apiKey = "vs5rglJCQBSKUoUoGFEpeA";
      }
      else
      {
        str = "se-events-dev";
        apiKeyId = "QGXc-3MBVmkVU1r8Ck-4";
        apiKey = "3H_nScelS8WqrGpvlP6s3Q";
      }
      string eventStoragePath = Path.Combine(MyFileSystem.TempPath, "ElasticAnalyticsEvents");
      int maxStoredEvents = 1000;
      MySpaceAnalytics.Instance.RegisterAnalyticsTracker((IMyAnalytics) new MyElasticAnalytics("https://9de1fbe0eed74ab49772fa5324e02c8c.eu-central-1.aws.cloud.es.io:9243/" + str + "/_doc", apiKeyId, apiKey, eventStoragePath, maxStoredEvents));
      MySpaceAnalytics.Instance.RegisterHeartbeatTracker((IMyAnalytics) new MyElasticAnalytics("https://9de1fbe0eed74ab49772fa5324e02c8c.eu-central-1.aws.cloud.es.io:9243/" + "se-heartbeat" + "/_doc", "m4DAFngBTFi_5jOjnt_q", "54U_nUvSSYmuXTkEYtdR3g", (string) null, 0), TimeSpan.FromMinutes(5.0));
      MyVRage.Platform.Render.OnSuspending += new Action(MySpaceAnalytics.Instance.OnSuspending);
      MyVRage.Platform.Render.OnResuming += new Action(MySpaceAnalytics.Instance.OnResuming);
      MyLog.Default.WriteLine("SpaceEngineersGame.SetupAnalytics - END");
    }

    protected override void InitServices()
    {
      base.InitServices();
      ServiceManager.Instance.AddService<IMyGuiScreenFactoryService>((IMyGuiScreenFactoryService) new MyGuiScreenFactoryService());
    }
  }
}
