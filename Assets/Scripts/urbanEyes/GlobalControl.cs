using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GlobalControl : MonoBehaviour {

    //Settings made by the user through the GUI
    public static GlobalControl Instance;
    public SortedDictionary<string, int> SliderValues;
    public AudioClip AmbientSound;
    public float SoundVolume;

    public Interface UserInterface;
    private const string __AUDIOVOLUME__ = "Sound volume slider";
    private const string __BUILDINGHEIGHT__ = "Building height slider";
    private const string __ROADWIDTH__ = "Road width slider";
    public Dictionary<string, int[]> DefaultSliderValues;
    public Dictionary<string, AudioClip> ListOfAmbientSounds;

    // Use this for initialization
    // Always keep the original SceneSetting object
    void Awake () {
		if (Instance == null) {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        } 
        else if (Instance != this) {
            Destroy(gameObject);
        }
	}

    void Start() {
        UserInterface = gameObject.AddComponent<Interface>();
        DefaultSliderValues = createUISliderDefaultValues();
        ListOfAmbientSounds = createListOfAmbientSoundTypes();
        UserInterface.createUI(DefaultSliderValues, ListOfAmbientSounds);
        addListnersToStartButton();
    }

    /**
    * Add listners to StartButton and ResetButton. 
    * When start/reset is clicked, method createWorld will be called. 
    */
    private void addListnersToStartButton() {
        GameObject.Find("StartButton").GetComponent<Button>().
            onClick.AddListener(delegate () { loadScene(); });
    }

    /**
     * Read input from GUI and load the correct scene. 
     */
    private void loadScene() {
        SliderValues = UserInterface.getAllSliderValues();
        AmbientSound = UserInterface.getSelectedSoundType();
        SoundVolume = (float)SliderValues[__AUDIOVOLUME__] / 100;
        int buildingHeight = SliderValues[__BUILDINGHEIGHT__];
        switch(buildingHeight) {
            case 1:
                loadSceneHelper("High");
                break;
            default:
                loadSceneHelper("Low");
                break;
        }
    }

    private void loadSceneHelper(string buildingHeight) {
        int roadWidth = SliderValues[__ROADWIDTH__];
        switch (roadWidth) {
            case 1:
                SceneManager.LoadScene(buildingHeight + "_Medium");
                break;
            case 2:
                SceneManager.LoadScene(buildingHeight + "_Large");
                break;
            default:
                SceneManager.LoadScene(buildingHeight + "_Small");
                break;
        }
    }

    /**
     * Build a collection of default values for the UI.
     * @return Dictionary<string, int[]>
     */
    private Dictionary<string, int[]> createUISliderDefaultValues() {

        Dictionary<string, int[]> defaultvalues = new Dictionary<string, int[]>();

        // default slider value: {min, value, max}
        defaultvalues[__AUDIOVOLUME__] = new int[3] { 0, 0, 100 };
        defaultvalues[__BUILDINGHEIGHT__] = new int[3] { 0, 0, 1 };
        defaultvalues[__ROADWIDTH__] = new int[3] { 0, 0, 2 };

        return defaultvalues;
    }

    /**
     * Build a collection of available ambient sound clips. 
     * @return Dictionary<string, AudioClip>
     */
    private Dictionary<string, AudioClip> createListOfAmbientSoundTypes() {
        AudioClip[] ambientSounds = Resources.LoadAll<AudioClip>("Sound\\City Audio");
        Dictionary<string, AudioClip> listOfAmbientSounds = new Dictionary<string, AudioClip>();
        foreach (AudioClip c in ambientSounds) {
            listOfAmbientSounds.Add(c.name, c);
        }
        return listOfAmbientSounds;
    }
}
