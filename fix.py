import re

with open("Assets/Scripts/Menus/MainMenu.cs", "r", encoding="utf-8") as f:
    t = f.read()

t = re.sub(r'private const string SensitivityPrefKey = "player_sensitivity";[ \r\n\t]+private const float DefaultFov = 90f;[ \r\n\t]+private const float DefaultSensitivity = 5f;[ \r\n\t]+private float tempFov;[ \r\n\t]+private float tempSensitivity;', 
'''private const string SensitivityPrefKey = "player_sensitivity";
        private const string VolumePrefKey = "player_volume";
        private const float DefaultFov = 90f;
        private const float DefaultSensitivity = 5f;
        private const float DefaultVolume = 100f;

        private float tempFov;
        private float tempSensitivity;
        private float tempVolume;''', t)

t = re.sub(r'public void OnSensitivitySliderChanged\(float value\)\{.*?UpdateSliderTextFallback\("Slider \(1\)", tempSensitivity\);\s*\}',
'''public void OnSensitivitySliderChanged(float value){
                tempSensitivity = Mathf.Clamp(value, 1f, 10f);
                if (sensitivityText != null){
                        sensitivityText.text = Mathf.RoundToInt(tempSensitivity).ToString();
                }
                UpdateSliderTextFallback("Slider (1)", tempSensitivity);
        }

        public void OnVolumeSliderChanged(float value){
                tempVolume = Mathf.Clamp(value, 0f, 100f);
                UpdateSliderTextFallback("VolumeSlider", tempVolume);
        }''', t, flags=re.DOTALL)


t = re.sub(r'if \(!PlayerPrefs\.HasKey\(SensitivityPrefKey\)\)\{\s*PlayerPrefs\.SetFloat\(SensitivityPrefKey, DefaultSensitivity\);\s*\}', r'''if (!PlayerPrefs.HasKey(SensitivityPrefKey)){
                        PlayerPrefs.SetFloat(SensitivityPrefKey, DefaultSensitivity);
                }
                if (!PlayerPrefs.HasKey(VolumePrefKey)){
                        PlayerPrefs.SetFloat(VolumePrefKey, DefaultVolume);
                }''', t)


t = re.sub(r'// Sliders\s*Slider\[\] sliders = GetComponentsInChildren<Slider>\(true\);\s*foreach \(Slider s in sliders\) \{\s*s\.onValueChanged\.RemoveAllListeners\(\);\s*if \(s\.name\.Trim\(\) == "Slider"\) \{\s*s\.minValue = 70f;\s*s\.maxValue = 110f;\s*s\.onValueChanged\.AddListener\(OnFovSliderChanged\);\s*\} else \{\s*s\.minValue = 1f;\s*s\.maxValue = 10f;\s*s\.onValueChanged\.AddListener\(OnSensitivitySliderChanged\);\s*\}\s*\}', r'''// Sliders
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
                }''', t)

t = re.sub(r'tempFov = PlayerPrefs\.GetFloat\(FovPrefKey, DefaultFov\);\s*tempSensitivity = PlayerPrefs\.GetFloat\(SensitivityPrefKey, DefaultSensitivity\);\s*Slider\[\] sliders = GetComponentsInChildren<Slider>\(true\);\s*foreach \(Slider s in sliders\) \{\s*if \(s\.name\.Trim\(\) == "Slider"\) \{\s*s\.value = tempFov; // FOV\s*\} else \{\s*s\.value = tempSensitivity; // Sensitivity\s*\}\s*\}', r'''tempFov = PlayerPrefs.GetFloat(FovPrefKey, DefaultFov);
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
                }''', t)

t = re.sub(r'UpdateSliderTextFallback\("Slider", tempFov\);\s*UpdateSliderTextFallback\("Slider \(1\)", tempSensitivity\);', r'''UpdateSliderTextFallback("Slider", tempFov);
                UpdateSliderTextFallback("Slider (1)", tempSensitivity);
                UpdateSliderTextFallback("VolumeSlider", tempVolume);''', t)

t = re.sub(r'PlayerPrefs\.SetFloat\(FovPrefKey, tempFov\);\s*PlayerPrefs\.SetFloat\(SensitivityPrefKey, tempSensitivity\);\s*PlayerPrefs\.Save\(\);', r'''PlayerPrefs.SetFloat(FovPrefKey, tempFov);
                PlayerPrefs.SetFloat(SensitivityPrefKey, tempSensitivity);
                PlayerPrefs.SetFloat(VolumePrefKey, tempVolume);
                PlayerPrefs.Save();''', t)


with open("Assets/Scripts/Menus/MainMenu.cs", "w", encoding="utf-8") as f:
    f.write(t)

