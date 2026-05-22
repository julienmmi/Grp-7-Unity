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
        private const string VolumePrefKey = "player_volume";
        private const float DefaultFov = 90f;
        private const float DefaultSensitivity = 5f;
        private const float DefaultVolume = 100f;

        private float tempFov;
        private float tempSensitivity;
        private float tempVolume;

	public bool isStartMenu = false;

    [SerializeField] private GameObject panel_options;
	[SerializeField] private GameObject panel_para1;
	[SerializeField] private GameObject panel_para2;
	[SerializeField] private GameObject panel_para3;
	[SerializeField] private GameObject panel_credits;
	[SerializeField] private GameObject panel_missions;

	[Header("UI References (Optionnel)")]
	[SerializeField] private Slider fovSlider;
	[SerializeField] private Slider sensitivitySlider;
	[SerializeField] private TMPro.TextMeshProUGUI fovText;
	[SerializeField] private TMPro.TextMeshProUGUI sensitivityText;

	void Awake(){
		ResolvePanels();
		WireNavigationButtons();
	}
	
	void Start(){
		EnsureDefaultSettings();
		CloseOptions();
		CloseCredits();
	}

	void Update(){
		if (Input.GetKeyDown(KeyCode.Escape)){
			if (panel_credits != null && panel_credits.activeSelf) {
				CloseCredits();
			} else {
				ToggleOptions();
			}
		}

		if (Input.GetKeyDown(KeyCode.H)){
			if (panel_missions != null){
				panel_missions.SetActive(!panel_missions.activeSelf);
			}
		}
	}

	public void OnFovSliderChanged(float value){
		tempFov = Mathf.Clamp(value, 70f, 110f);
		if (fovText != null){
			fovText.text = Mathf.RoundToInt(tempFov).ToString();
		}
		UpdateSliderTextFallback("Slider", tempFov);
	}

	public void OnSensitivitySliderChanged(float value){
                tempSensitivity = Mathf.Clamp(value, 1f, 10f);
                if (sensitivityText != null){
                        sensitivityText.text = Mathf.RoundToInt(tempSensitivity).ToString();
                }
                UpdateSliderTextFallback("Slider (1)", tempSensitivity);
        }

        public void OnVolumeSliderChanged(float value){
                tempVolume = Mathf.Clamp(value, 0f, 100f);
                UpdateSliderTextFallback("VolumeSlider", tempVolume);
        }

	private void UpdateSliderTextFallback(string sliderName, float val){
		Slider[] sliders = GetComponentsInChildren<Slider>(true);
		foreach (Slider s in sliders) {
			if (s.name.Trim() == sliderName) {
				// Essaie de trouver un texte enfant (TextMeshPro)
				TMPro.TextMeshProUGUI txt = s.GetComponentInChildren<TMPro.TextMeshProUGUI>(true);
				if (txt != null) {
					txt.text = Mathf.RoundToInt(val).ToString();
					continue;
				}
				
				// Ou essaie de chercher juste au dessus (sur le parent) si le texte est à côté
				if (s.transform.parent != null) {
					TMPro.TextMeshProUGUI parentTxt = s.transform.parent.GetComponentInChildren<TMPro.TextMeshProUGUI>(true);
					if (parentTxt != null) {
						parentTxt.text = Mathf.RoundToInt(val).ToString();
					}
				}
			}
		}
	}

	private void EnsureDefaultSettings(){
		if (!PlayerPrefs.HasKey(FovPrefKey)){
			PlayerPrefs.SetFloat(FovPrefKey, DefaultFov);
		}

		if (!PlayerPrefs.HasKey(SensitivityPrefKey)){
                        PlayerPrefs.SetFloat(SensitivityPrefKey, DefaultSensitivity);
                }
                if (!PlayerPrefs.HasKey(VolumePrefKey)){
                        PlayerPrefs.SetFloat(VolumePrefKey, DefaultVolume);
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

		if (panel_credits == null || panel_credits.name.Trim() != "creditfen"){
			Transform creditTransform = FindDeepChild(transform, "creditfen");
			if (creditTransform != null){
				panel_credits = creditTransform.gameObject;
			}
		}
	}

	private void WireNavigationButtons(){
		// Para 1
		HookButton(panel_para1, "Button para1", ShowPara1);
		HookButton(panel_para1, "Button para2", ShowPara2);
		HookButton(panel_para1, "Button para3", ShowPara3);
		HookButton(panel_para1, "Button Retour", CloseOptions);
		HookButton(panel_para1, "Button Quitter", QuitGame);
		HookButton(panel_para1, "Button Relancer", RestartGame);
		HookButton(panel_para1, "Button Sauvegarder", SaveSettings);

		// Para 2
		HookButton(panel_para2, "Button para1", ShowPara1);
		HookButton(panel_para2, "Button para2", ShowPara2);
		HookButton(panel_para2, "Button para3", ShowPara3);
		HookButton(panel_para2, "Button Retour", CloseOptions);
		HookButton(panel_para2, "Button Quitter", QuitGame);
		HookButton(panel_para2, "Button Relancer", RestartGame);
		HookButton(panel_para2, "Button Sauvegarder", SaveSettings);

		// Para 3
		HookButton(panel_para3, "Button para1", ShowPara1);
		HookButton(panel_para3, "Button para2", ShowPara2);
		HookButton(panel_para3, "Button para3", ShowPara3);
		HookButton(panel_para3, "Button Retour", CloseOptions);
		HookButton(panel_para3, "Button Quitter", QuitGame);
		HookButton(panel_para3, "Button Relancer", RestartGame);
		HookButton(panel_para3, "Button Sauvegarder", SaveSettings);

		// Credits
		HookButton(gameObject, "Credit", OpenCredits);
		if (panel_credits != null){
			HookButton(panel_credits, "Button Retour", CloseCredits);
		}

		// Sliders
                Slider[] sliders = GetComponentsInChildren<Slider>(true);
                foreach (Slider s in sliders) {
                        s.onValueChanged.RemoveAllListeners();
                        
                        if (s.transform.parent != null && s.transform.parent.name.Trim() == "Para2") {
                                s.name = "VolumeSlider";
                                s.minValue = 0f;
                                s.maxValue = 100f;
                                s.onValueChanged.AddListener(OnVolumeSliderChanged);
                        } else if (s.name.Trim() == "Slider") {
                                s.minValue = 70f;
                                s.maxValue = 110f;
                                s.onValueChanged.AddListener(OnFovSliderChanged);
                        } else {
                                s.minValue = 1f;
                                s.maxValue = 10f;
                                s.onValueChanged.AddListener(OnSensitivitySliderChanged);
                        }
                }
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
		SceneManager.LoadScene("Acte 1");
	}
	
	public void ShowOptions(){
		ResolvePanels();
		
		tempFov = PlayerPrefs.GetFloat(FovPrefKey, DefaultFov);
                tempSensitivity = PlayerPrefs.GetFloat(SensitivityPrefKey, DefaultSensitivity);
                tempVolume = PlayerPrefs.GetFloat(VolumePrefKey, DefaultVolume);

                Slider[] sliders = GetComponentsInChildren<Slider>(true);
                foreach (Slider s in sliders) {
                        if (s.transform.parent != null && s.transform.parent.name.Trim() == "Para2") {
                                s.value = tempVolume;
                        } else if (s.name.Trim() == "Slider") {
                                s.value = tempFov; // FOV
                        } else {
                                s.value = tempSensitivity; // Sensitivity
                        }
                }

		if (fovText != null) fovText.text = Mathf.RoundToInt(tempFov).ToString();
		if (sensitivityText != null) sensitivityText.text = Mathf.RoundToInt(tempSensitivity).ToString();

		UpdateSliderTextFallback("Slider", tempFov);
                UpdateSliderTextFallback("Slider (1)", tempSensitivity);
                UpdateSliderTextFallback("VolumeSlider", tempVolume);

		SetOptionsButtonLocked(true);
		ShowPara1();
		SetPaused(true);
	}

	public void ToggleOptions(){
		if (AreOptionsVisible()){
			CloseOptions();
		} else {
			ShowOptions();
		}
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
		SetPaused(false);

		// Toujours réinitialiser les valeurs sur les vraies sauvegardes si on quitte
		tempFov = PlayerPrefs.GetFloat(FovPrefKey, DefaultFov);
                tempSensitivity = PlayerPrefs.GetFloat(SensitivityPrefKey, DefaultSensitivity);
                tempVolume = PlayerPrefs.GetFloat(VolumePrefKey, DefaultVolume);

                Slider[] sliders = GetComponentsInChildren<Slider>(true);
                foreach (Slider s in sliders) {
                        if (s.transform.parent != null && s.transform.parent.name.Trim() == "Para2") {
                                s.value = tempVolume;
                        } else if (s.name.Trim() == "Slider") {
                                s.value = tempFov; // FOV
                        } else {
                                s.value = tempSensitivity; // Sensitivity
                        }
                }
	}

	private bool AreOptionsVisible(){
		return (panel_para1 != null && panel_para1.activeSelf)
			|| (panel_para2 != null && panel_para2.activeSelf)
			|| (panel_para3 != null && panel_para3.activeSelf);
	}

	private void SetPaused(bool paused){
		HudManager.pause = paused;
		Time.timeScale = paused ? 0f : 1f;

		if(isStartMenu){
			return;
		}
		
		Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
		Cursor.visible = paused;
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

	public void RestartGame(){
		string currentSceneName = SceneManager.GetActiveScene().name;
		if (currentSceneName != "MainMenu"){
			SceneManager.LoadScene(currentSceneName);
		}
	}

	public void OpenCredits(){
		ResolvePanels();
		if (panel_credits != null){
			panel_credits.SetActive(true);
		}
	}

	public void CloseCredits(){
		ResolvePanels();
		if (panel_credits != null){
			panel_credits.SetActive(false);
		} 
		
		// Fallback puissant qui ferme absolument TOUS les crédits trouvés
		Transform[] allTransforms = GetComponentsInChildren<Transform>(true);
		foreach (Transform t in allTransforms){
			if (t.name.Trim() == "creditfen"){
				t.gameObject.SetActive(false);
			}
		}
	}

	public void SaveSettings(){
		PlayerPrefs.SetFloat(FovPrefKey, tempFov);
                PlayerPrefs.SetFloat(SensitivityPrefKey, tempSensitivity);
                PlayerPrefs.SetFloat(VolumePrefKey, tempVolume);
                PlayerPrefs.Save();
		ApplySettingsToPlayer();
		CloseOptions();
	}
}
