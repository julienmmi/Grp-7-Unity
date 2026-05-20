using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

public class MainMenu : MonoBehaviour
{
	private const string FovPrefKey = "player_fov";
	private const string SensitivityPrefKey = "player_sensitivity";
	private const float DefaultFov = 70f;
	private const float DefaultSensitivity = 5f;

    [SerializeField] private GameObject panel_options;
	[SerializeField] private GameObject panel_para1;
	[SerializeField] private GameObject panel_para2;
	[SerializeField] private GameObject panel_para3;

	void Awake(){
		ResolvePanels();
		WireNavigationButtons();
	}
	
	void Start(){
		EnsureDefaultSettings();
		CloseOptions();
	}

	public void OnFovSliderChanged(float value){
		float clampedValue = Mathf.Clamp(value, 70f, 110f);
		PlayerPrefs.SetFloat(FovPrefKey, clampedValue);
		PlayerPrefs.Save();
		ApplySettingsToPlayer();
	}

	public void OnSensitivitySliderChanged(float value){
		float clampedValue = Mathf.Clamp(value, 1f, 10f);
		PlayerPrefs.SetFloat(SensitivityPrefKey, clampedValue);
		PlayerPrefs.Save();
		ApplySettingsToPlayer();
	}

	private void EnsureDefaultSettings(){
		if (!PlayerPrefs.HasKey(FovPrefKey)){
			PlayerPrefs.SetFloat(FovPrefKey, DefaultFov);
		}

		if (!PlayerPrefs.HasKey(SensitivityPrefKey)){
			PlayerPrefs.SetFloat(SensitivityPrefKey, DefaultSensitivity);
		}

		PlayerPrefs.Save();
	}

	private void ApplySettingsToPlayer(){
		PlayerController playerController = FindObjectOfType<PlayerController>();
		if (playerController == null){
			return;
		}

		playerController.ApplySavedSettings();
	}

	private void ResolvePanels(){
		if (panel_options == null || panel_options.name.Trim() != "Button Options"){
			Transform optionsTransform = FindDeepChild(transform, "Button Options");
			if (optionsTransform != null){
				panel_options = optionsTransform.gameObject;
			}
		}

		if (panel_options != null){
			if (panel_para1 == null || panel_para1.name.Trim() != "Para1"){
				Transform para1Transform = FindDeepChild(panel_options.transform, "Para1");
				if (para1Transform != null){
					panel_para1 = para1Transform.gameObject;
				}
			}

			if (panel_para2 == null || panel_para2.name.Trim() != "Para2"){
				Transform para2Transform = FindDeepChild(panel_options.transform, "Para2");
				if (para2Transform != null){
					panel_para2 = para2Transform.gameObject;
				}
			}

			if (panel_para3 == null || panel_para3.name.Trim() != "Para3"){
				Transform para3Transform = FindDeepChild(panel_options.transform, "Para3");
				if (para3Transform != null){
					panel_para3 = para3Transform.gameObject;
				}
			}
		}
	}

	private void WireNavigationButtons(){
		HookButton(panel_para1, "Button para1", ShowPara1);
		HookButton(panel_para1, "Button para2", ShowPara2);
		HookButton(panel_para1, "Button para3", ShowPara3);
		HookButton(panel_para1, "Button Retour", CloseOptions);

		HookButton(panel_para2, "Button para1", ShowPara1);
		HookButton(panel_para2, "Button para2", ShowPara2);
		HookButton(panel_para2, "Button para3", ShowPara3);
		HookButton(panel_para2, "Button Retour", CloseOptions);

		HookButton(panel_para3, "Button para1", ShowPara1);
		HookButton(panel_para3, "Button para2", ShowPara2);
		HookButton(panel_para3, "Button para3", ShowPara3);
		HookButton(panel_para3, "Button Retour", CloseOptions);
	}

	private void HookButton(GameObject root, string buttonName, UnityAction action){
		if (root == null){
			return;
		}

		Button button = FindButton(root.transform, buttonName);
		if (button != null){
			button.onClick.AddListener(action);
		}
	}

	private Button FindButton(Transform root, string buttonName){
		Button[] buttons = root.GetComponentsInChildren<Button>(true);
		for (int i = 0; i < buttons.Length; i++){
			if (buttons[i] != null && buttons[i].name.Trim() == buttonName){
				return buttons[i];
			}
		}

		return null;
	}

	private Transform FindDeepChild(Transform parent, string childName){
		foreach (Transform child in parent){
			if (child.name.Trim() == childName){
				return child;
			}

			Transform result = FindDeepChild(child, childName);
			if (result != null){
				return result;
			}
		}

		return null;
	}
	
	public void PlayGame(){
		SceneManager.LoadScene("Level1");
	}
	
	public void ShowOptions(){
		ResolvePanels();
		SetOptionsButtonLocked(true);
		ShowPara1();
	}
	
	public void UnshowOptions(){
		if (panel_para1 != null){
			panel_para1.SetActive(false);
		}
		if (panel_para2 != null){
			panel_para2.SetActive(false);
		}
		if (panel_para3 != null){
			panel_para3.SetActive(false);
		}
	}

	public void ShowPara1(){
		if (panel_para1 != null){
			panel_para1.SetActive(true);
		}
		if (panel_para2 != null){
			panel_para2.SetActive(false);
		}
		if (panel_para3 != null){
			panel_para3.SetActive(false);
		}
	}

	public void ShowPara2(){
		if (panel_para1 != null){
			panel_para1.SetActive(false);
		}
		if (panel_para2 != null){
			panel_para2.SetActive(true);
		}
		if (panel_para3 != null){
			panel_para3.SetActive(false);
		}
	}

	public void ShowPara3(){
		if (panel_para1 != null){
			panel_para1.SetActive(false);
		}
		if (panel_para2 != null){
			panel_para2.SetActive(false);
		}
		if (panel_para3 != null){
			panel_para3.SetActive(true);
		}
	}

	public void CloseOptions(){
		UnshowOptions();
		SetOptionsButtonLocked(false);
	}

	private void SetOptionsButtonLocked(bool locked){
		if (panel_options == null){
			return;
		}

		Button optionsButton = panel_options.GetComponent<Button>();
		if (optionsButton != null){
			optionsButton.interactable = !locked;
		}
	}
	
	public void QuitGame(){
		Application.Quit();
	}
}
