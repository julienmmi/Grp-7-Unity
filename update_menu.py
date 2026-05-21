import re

with open("Assets/Scripts/Menus/MainMenu.cs", "r") as f:
    text = f.read()

# Add Volume config
text = text.replace(
'''        private const string SensitivityPrefKey = "player_sensitivity";
        private const float DefaultFov = 90f;
        private const float DefaultSensitivity = 5f;

        private float tempFov;
        private float tempSensitivity;''',
'''        private const string SensitivityPrefKey = "player_sensitivity";
        private const string VolumePrefKey = "player_volume";
        private const float DefaultFov = 90f;
        private const float DefaultSensitivity = 5f;
        private const float DefaultVolume = 100f;

        private float tempFov;
        private float tempSensitivity;
        private float tempVolume;'''
)

# Add OnVolumeSliderChanged
text = text.replace(
'''        public void OnSensitivitySliderChanged(float value){
                tempSensitivity = Mathf.Clamp(value, 1f, 10f);
                if (sensitivityText != null){
                        sensitivityText.text = Mathf.RoundToInt(tempSensitivity).ToString();
                }
                UpdateSliderTextFallback("Slider (1)", tempSensitivity);
        }''',
'''        public void OnSensitivitySliderChanged(float value){
                tempSensitivity = Mathf.Clamp(value, 1f, 10f);
                if (sensitivityText != null){
                        sensitivityText.text = Mathf.RoundToInt(tempSensitivity).ToString();
                }
                UpdateSliderTextFallback("Slider (1)", tempSensitivity);
        }

        public void OnVolumeSliderChanged(float value){
                tempVolume = Mathf.Clamp(value, 0f, 100f);
                UpdateSliderTextFallback("VolumeSlider", tempVolume);
        }'''
)

# Ensure Default Settings
text = text.replace(
'''                if (!PlayerPrefs.HasKey(SensitivityPrefKey)){
                        PlayerPrefs.SetFloat(SensitivityPrefKey, DefaultSensitivity);
                }''',
'''                if (!PlayerPrefs.HasKey(SensitivityPrefKey)){
                        PlayerPrefs.SetFloat(SensitivityPrefKey, DefaultSensitivity);
                }
                if (!PlayerPrefs.HasKey(VolumePrefKey)){
                        PlayerPrefs.SetFloat(VolumePrefKey, DefaultVolume);
                }'''
)

# Sliders setup
text = text.replace(
'''                // Sliders
                Slider[] sliders = GetComponentsInChildren<Slider>(true);
                foreach (Slider s in sliders) {
                        s.onValueChanged.RemoveAllListeners();
                        if (s.name.Trim() == "Slider") {
                                s.minValue = 70f;
                                s.maxValue = 110f;
                                s.onValueChanged.AddListener(OnFovSliderChanged);
                        } else {
                                s.minValue = 1f;
                                s.maxValue = 10f;
                                s.onValueChanged.AddListener(OnSensitivitySliderChanged);
                        }
                }''',
'''                // Sliders
                Slider[] sliders = GetComponentsInChildren<Slider>(true);
                foreach (Slider s in sliders) {
                        s.onValueChanged.RemoveAllListeners();
                        if (s.transform.parent != null && s.transform.parent.name.Trim() == "Para2") {
                                // Volume slider
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
                }'''
)

# Show Options
text = text.replace(
'''                tempFov = PlayerPrefs.GetFloat(FovPrefKey, DefaultFov);
                tempSensitivity = PlayerPrefs.GetFloat(SensitivityPrefKey, DefaultSensitivity);

                Slider[] sliders = GetComponentsInChildren<Slider>(true);
                foreach (Slider s in sliders) {
                        if (s.name.Trim() == "Slider") {
                                s.value = tempFov; // FOV
                        } else {
                                s.value = tempSensitivity; // Sensitivity
                        }
                }''',
'''                tempFov = PlayerPrefs.GetFloat(FovPrefKey, DefaultFov);
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
                }'''
)

# Show Options Update Text
text = text.replace(
'''                UpdateSliderTextFallback("Slider", tempFov);
                UpdateSliderTextFallback("Slider (1)", tempSensitivity);''',
'''                UpdateSliderTextFallback("Slider", tempFov);
                UpdateSliderTextFallback("Slider (1)", tempSensitivity);
                UpdateSliderTextFallback("VolumeSlider", tempVolume);'''
)

# Close Options
text = text.replace(
'''                tempFov = PlayerPrefs.GetFloat(FovPrefKey, DefaultFov);
                tempSensitivity = PlayerPrefs.GetFloat(SensitivityPrefKey, DefaultSensitivity);

                Slider[] sliders = GetComponentsInChildren<Slider>(true);
                foreach (Slider s in sliders) {
                        if (s.name.Trim() == "Slider") {
                                s.value = tempFov; // FOV
                        } else {
                                s.value = tempSensitivity; // Sensitivity
                        }
                }''',
'''                tempFov = PlayerPrefs.GetFloat(FovPrefKey, DefaultFov);
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
                }'''
)

# Save Options
text = text.replace(
'''        public void SaveSettings(){
                PlayerPrefs.SetFloat(FovPrefKey, tempFov);
                PlayerPrefs.SetFloat(SensitivityPrefKey, tempSensitivity);
                PlayerPrefs.Save();''',
'''        public void SaveSettings(){
                PlayerPrefs.SetFloat(FovPrefKey, tempFov);
                PlayerPrefs.SetFloat(SensitivityPrefKey, tempSensitivity);
                PlayerPrefs.SetFloat(VolumePrefKey, tempVolume);
                PlayerPrefs.Save();'''
)

with open("Assets/Scripts/Menus/MainMenu.cs", "w") as f:
    f.write(text)

