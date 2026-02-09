using MelonLoader;
using UnityEngine;
using System.Collections;
using static MelonLoader.MelonLaunchOptions;

[assembly: MelonInfo(typeof(LMDModMenu.ModCore), "LMD UI Mod Menu", "1.0.0", "DevdudeX", null)]
[assembly: MelonGame("Megagon Industries", "Lonely Mountains: Downhill")]

namespace LMDModMenu
{
	public class ModCore : MelonMod
	{
		public const string MOD_VERSION = "1.0.0";
		private static Logger _logger;
		private static ObjectReferenceManager _objectRefManager;

		bool _mainMenuWasLoaded = false;
		bool _menuHasBeenSetUp = false;
		float _loadTimer = 0f;

		public override void OnInitializeMelon()
		{
			_logger = new Logger(LoggerInstance);
			_logger.LogInfo("Initializing.");

			_objectRefManager = new ObjectReferenceManager(_logger);
		}

		public override void OnSceneWasLoaded(int buildIndex, string sceneName)
		{
			string[] whitelistedLoadScenes = ["gameplay"];
			if (Array.IndexOf(whitelistedLoadScenes, sceneName) != -1)
			{
				LoggerInstance.Msg($"Scene {sceneName} with build index {buildIndex} has been loaded!");
				_mainMenuWasLoaded = true;
			}

			//MelonCoroutines.Start(DelayedSceneLoad());
		}

		public override void OnUpdate()
		{
			if (!_mainMenuWasLoaded) return;

			if (!_menuHasBeenSetUp)
			{
				TryToSetUpMenu();
			}
		}



		void TryToSetUpMenu()
		{
			_loadTimer += Time.deltaTime;
			if (_loadTimer > 10f)
			{
				_objectRefManager.FindMainMenuReferences();

				if (!_objectRefManager.MenuIsActive)
				{
					_logger.LogInfo("Failed to set up menu. Delaying for 10s...");
					_loadTimer = 0f;
					return;
				}

				_logger.LogInfo("Updating main menu version label.");
				_objectRefManager.SetModdedVersionLabel();

				_logger.LogInfo("Generating 'Mods' menu button.");
				_objectRefManager.GenerateModsButton();

				_logger.LogInfo("Generating mod menu screen.");
				_objectRefManager.CreateModMenuScreen();


				_menuHasBeenSetUp = true;
			}
		}
	}
}