using Il2CppMegagon.Downhill.UI;
using Il2CppMegagon.Downhill.UI.Animations;
using Il2CppMegagon.Downhill.UI.Screens;
using Il2CppMegagon.Downhill.UI.Screens.Helper;
using Il2CppTMPro;
using LMDModMenu.Core;
using MelonLoader;
using UnityEngine;

namespace LMDModMenu;

internal class ObjectReferenceManager
{
	Logger _logger;
	public ObjectReferenceManager(Logger logger)
	{
		_logger = logger;
	}


	GameObject _mainMenuVersionLabel;
	TextMeshProUGUI _versionLabelTMP;

	GameObject _mainMenuScreen;
	GameObject _settingsScreen;
	GameObject _mainMenuOptionsBtn;

	// Custom
	GameObject _modMenuBtn;
	GameObject _modMenuScreen;

	string _vanillaVersionText = "UNSET";


	public bool MenuIsActive { get { return _mainMenuScreen.activeInHierarchy; } }

	public void FindMainMenuReferences()
	{
		_mainMenuVersionLabel = GameObject.Find("UI(Clone)/Canvas3D/Wrapper/MainMenuScreen(Clone)/MainMenuScreen/TextMeshPro Text_Version");
		_versionLabelTMP = _mainMenuVersionLabel.GetComponent<TextMeshProUGUI>();


		// Find the options menu
		// Grab existing buttons and screens
		_mainMenuScreen = GameObject.Find("UI(Clone)/Canvas3D/Wrapper/MainMenuScreen(Clone)");
		_settingsScreen = GameObject.Find("UI(Clone)/Canvas3D/Wrapper/SettingsScreen(Clone)");
		_mainMenuOptionsBtn = GameObject.Find("UI(Clone)/Canvas3D/Wrapper/MainMenuScreen(Clone)/MainMenuScreen/Layout_ButtonList/ListButton_Options");
	}

	/// <summary>
	/// Sets the text of the main menu version label.<br></br>
	/// Text is always added after the vanilla contents.
	/// </summary>
	public void SetModdedVersionLabel()
	{
		if (_vanillaVersionText == "UNSET")
		{
			_vanillaVersionText = _versionLabelTMP.text;
		}

		_versionLabelTMP.text = $"{_vanillaVersionText} \n[MODDED v{ModCore.MOD_VERSION}]";
		_versionLabelTMP.GenerateTextMesh();
	}


	public void GenerateModsButton()
	{
		// Clone the existing button
		_modMenuBtn = GameObject.Instantiate(_mainMenuOptionsBtn, _mainMenuOptionsBtn.transform.parent);
		_modMenuBtn.name = "ListButton_Mods";

		GameObject modMenuBtnTextDisplay = _modMenuBtn.transform.Find("Elements/TextMeshPro Text_OptionsButton").gameObject;
		modMenuBtnTextDisplay.name = "TextMeshPro Text_ModsButton";
		TextMeshProUGUI tmpComponent = modMenuBtnTextDisplay.GetComponent<TextMeshProUGUI>();
		tmpComponent.m_text = "Mods";

		// Destroy all Megagon scripts as I have no way of knowing how they work (thanks il2cpp)
		// Probably aren't important ¯\_(ツ)_/¯
		_modMenuBtn.RemoveChildComponents<ListButton>();
		_modMenuBtn.RemoveChildComponents<UIAnimation_FadeContainer>();
		modMenuBtnTextDisplay.RemoveChildComponents<InteractiveUIElement>();

		// TODO:
		// Add onClick function to open the mod menu
		modMenuBtnTextDisplay.GetComponent<UnityEngine.UI.Button>().onClick.AddListener((UnityEngine.Events.UnityAction)OpenModMenuScreen);
	}

	// FIXME:
	void OnClickModMenuButton()
	{
		_logger.LogInfo("Mod menu button was clicked!");
	}

	public void CreateModMenuScreen()
	{
		// TODO: Would be nice to use an assetbundle for this...
		// Create a new menu screen
			_modMenuScreen = GameObject.Instantiate(_settingsScreen, _settingsScreen.transform.parent);
			_modMenuScreen.name = "ModMenuScreen";
			_modMenuScreen.transform.Find("SettingsMenu").gameObject.name = "ModOptionsMenu";

			// Remove useless scripts
			_modMenuScreen.RemoveChildComponents<SettingsScreen>();
			_modMenuScreen.RemoveChildComponents<ControlsDisplayAnimations>();
			_modMenuScreen.RemoveChildComponents<DummyShowHideAnimation>();
			_modMenuScreen.RemoveChildComponents<ScreenElement>();
			_modMenuScreen.RemoveChildComponents<InteractiveUIElement>();
			_modMenuScreen.RemoveChildComponents<UIAnimation_FadeContainer>();
			_modMenuScreen.RemoveChildComponents<UIAnimation_MoveContainer>();
			_modMenuScreen.RemoveChildComponents<TextMeshProLocalized>();

			// Set menu header text
			TextMeshProUGUI menuSubHeader_TMP = _modMenuScreen.transform.Find("ModOptionsMenu/MenuHeader/TextMeshPro Text_SubHeadline").GetComponent<TextMeshProUGUI>();
			TextMeshProUGUI menuHeader_TMP = _modMenuScreen.transform.Find("ModOptionsMenu/MenuHeader/TextMeshPro Text_GameMenuHeadline").GetComponent<TextMeshProUGUI>();
			menuHeader_TMP.m_text = "Mod Options";
			menuSubHeader_TMP.m_text = $"Version {ModCore.MOD_VERSION}";

			// Get the TMP components for each category button
			// These existing buttons are just for debugging and would be replaced in future
			string btnNav = "ModOptionsMenu/Layout_Categories/";
			TextMeshProUGUI categoryBtn_Game_TMP = _modMenuScreen.transform.Find(btnNav+"CategoryButton_Game/TextMeshPro Text_Display").GetComponent<TextMeshProUGUI>();
			TextMeshProUGUI categoryBtn_Display_TMP = _modMenuScreen.transform.Find(btnNav+"CategoryButton_Display/TextMeshPro Text_Display").GetComponent<TextMeshProUGUI>();
			TextMeshProUGUI categoryBtn_Graphics_TextDisp = _modMenuScreen.transform.Find(btnNav+"CategoryButton_Graphics/TextMeshPro Text_Display").GetComponent<TextMeshProUGUI>();
			TextMeshProUGUI categoryBtn_Controls_TextDisp = _modMenuScreen.transform.Find(btnNav+"CategoryButton_Controls/TextMeshPro Text_Display").GetComponent<TextMeshProUGUI>();
			TextMeshProUGUI categoryBtn_Audio_TextDisp = _modMenuScreen.transform.Find(btnNav+"CategoryButton_Audio/TextMeshPro Text_Display").GetComponent<TextMeshProUGUI>();

			TextMeshProUGUI backBtn_TextDisp = _modMenuScreen.transform.Find("ModOptionsMenu/BackButton/Image_ControlsExplanationLeftBackground/TextMeshPro Text_Back").GetComponent<TextMeshProUGUI>();
			TextMeshProUGUI applyBtn_TextDisp = _modMenuScreen.transform.Find("ModOptionsMenu/ApplyButton/Image_ControlsExplanationRightBackground (1)/TextMeshPro Text_Apply").GetComponent<TextMeshProUGUI>();

			// Make sure all labels are visible
			categoryBtn_Game_TMP.alpha = 1;
			categoryBtn_Display_TMP.alpha = 1;
			categoryBtn_Graphics_TextDisp.alpha = 1;
			categoryBtn_Controls_TextDisp.alpha = 1;
			categoryBtn_Audio_TextDisp.alpha = 1;

			backBtn_TextDisp.alpha = 1;
			backBtn_TextDisp.m_text = "Back";   // It seems to default to {0}Back (guessing it's localization related)

			applyBtn_TextDisp.alpha = 1;
			applyBtn_TextDisp.m_text = "Apply Changes";   // Fixes localization default text

			// Add onClick function to return to the main menu
			backBtn_TextDisp.GetComponent<UnityEngine.UI.Button>().onClick.AddListener((UnityEngine.Events.UnityAction)LeaveModMenuScreen);


			// Remove unwanted buttons
			// GameObject.Destroy(_modMenuScreen.transform.Find("SettingsMenu/Layout_Categories/CategoryButton_Display"));
			// GameObject.Destroy(_modMenuScreen.transform.Find("SettingsMenu/Layout_Categories/CategoryButton_Graphics"));
			// GameObject.Destroy(_modMenuScreen.transform.Find("SettingsMenu/Layout_Categories/CategoryButton_Controls"));
			// GameObject.Destroy(_modMenuScreen.transform.Find("SettingsMenu/Layout_Categories/CategoryButton_Audio"));

			// Hide the mod menu screen by default
			_modMenuScreen.SetActive(false);
	}

	private void OpenModMenuScreen()
	{
		_logger.LogInfo("Mod menu button clicked, opening mod menu.");

		_mainMenuScreen.SetActive(false);
		_modMenuScreen.SetActive(true);
	}
	private void LeaveModMenuScreen()
	{
		_logger.LogInfo("Mod menu back button clicked, exiting mod menu.");

		_modMenuScreen.SetActive(false);
		_mainMenuScreen.SetActive(true);
	}


}
