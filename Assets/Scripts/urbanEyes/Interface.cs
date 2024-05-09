/**
 * Disclaimer: This file is originally created by MVK group 15. 
 * Modified by MVK 2016 group Lambda: 2017
 */
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Interface : MonoBehaviour {

    // A collection of essential geometric values
    public SortedDictionary<string, int> sliderValues = new SortedDictionary<string, int>();
    private ToggleGroup ambientSoundToggleGroup;
    private Dictionary<string, AudioClip> ambientSoundTypes;

    // Define UI Object
    public GameObject UI;

    /**
	 * Instantiates a UI Game Object
     * @param Dictionary<string, int[]> defaultvalues for the different sliders
     * @param string[] listOfAmbientSounds a list containing the names of the ambient sounds, 
     * each name corresponds to an audioClip
	 */
    public void createUI(Dictionary<string, int[]> defaultvalues, Dictionary<string, AudioClip> listOfAmbientSounds) {
        UI = Instantiate((GameObject)Resources.Load("Dynamic_UI_Enlarge"));
        createSliders(defaultvalues);
        createToggleGroupForSoundTypes(defaultvalues.Count, listOfAmbientSounds);
        createListenersForSliders();
    }

    /**
     * Returns a dictionary containing slider names and their corresponding values. 
     * Called by the environmental generator.
     */
    public SortedDictionary<string, int> getAllSliderValues() {
        return sliderValues;
    }

    /**
	 * Wrapper function for the purpose of retrieving slider values from the UI.
     * @param  {[type]} string label
	 * @return {[type]} int
	 */
    public int getValue(string label) {
        return (int)sliderValues[label];
    }

    /**
     * Create toggles for the different sound types. All toggles will belong to the same toggle group, 
     * i.e. only one toggle can be selected simultaneously. 
     * @param int numberOfSliders used to calculate position of the toggles. 
     * @param Dictionary<string, AudioClip> listOfAmbientSounds one toggle is created for each and every element in the list.
     */
    private void createToggleGroupForSoundTypes(int numberOfSliders, Dictionary<string, AudioClip> listOfAmbientSounds) {
        this.ambientSoundTypes = listOfAmbientSounds;
        GameObject OriginalToggle = UI.transform.Find("Toggle").gameObject;
        RectTransform originalTransform = OriginalToggle.GetComponent<RectTransform>();
        this.ambientSoundToggleGroup = GameObject.FindObjectOfType<ToggleGroup>();
        this.ambientSoundToggleGroup.allowSwitchOff = true;

        int i = 0;
        foreach (string soundType in listOfAmbientSounds.Keys) {
            GameObject toggle = (GameObject)Instantiate(OriginalToggle);
            toggle.name = soundType;
            toggle.GetComponent<Toggle>().group = this.ambientSoundToggleGroup;
            toggle.transform.Find("Label").GetComponent<Text>().text = toggle.name;
            toggle.transform.position = getTogglePosition(originalTransform.rect.position, originalTransform.rect.height, i);
            toggle.transform.SetParent(ambientSoundToggleGroup.transform, false);
            i++;
        }
        this.ambientSoundToggleGroup.SetAllTogglesOff();
        Destroy(OriginalToggle);
    }

    /**
	 * Create sliders for the UI
	 * @param  Dictionary<string, int[]> name of the sliders and their corresponding defaultvalues
	 */
    private void createSliders(Dictionary<string, int[]> defaultvalues) {

        GameObject OriginalSlider = UI.transform.Find("Slider").gameObject; // Copy from slider template from prefab
        RectTransform OSRT = OriginalSlider.GetComponent<RectTransform>(); // Reference the RectTransform object in order to retrieve size dimensions

        int i = 0; // Initialize start value of counter.

        // Initialize user interface values from arguments.
        foreach (KeyValuePair<string, int[]> defaultvalue in defaultvalues) {

            GameObject Slider = (GameObject)Instantiate(OriginalSlider);
            Slider SliderComponent = Slider.GetComponent<Slider>();

            SliderComponent.wholeNumbers = true;
            SliderComponent.minValue = (float)defaultvalue.Value[0];
            SliderComponent.value = (float)defaultvalue.Value[1];
            SliderComponent.maxValue = (float)defaultvalue.Value[2];

            sliderValues[defaultvalue.Key] = defaultvalue.Value[1];

            Slider.name = defaultvalue.Key;
            Slider.transform.Find("Label").GetComponent<Text>().text = defaultvalue.Key;
            Slider.transform.Find("Value").GetComponent<Text>().text = sliderValues[defaultvalue.Key].ToString();
            Slider.transform.position = getPosition(defaultvalues.Count - 1, i, OSRT.rect.width, OSRT.rect.height, 2);
            Slider.transform.SetParent(UI.transform, false);


            i++; // Increment counter.
        }

        Destroy(OriginalSlider); // Destroy the slider template object.

    }

    /**
	 * Creates listeners for each slider. 
     * When detects value change it calls changeValue.
	 */
    private void createListenersForSliders() {
        for (int i = 0; i < sliderValues.Count; i++) {
            string element = sliderValues.Keys.ElementAt(i);
            Slider SliderComponent = UI.transform.Find(element).gameObject.GetComponent<Slider>();
            SliderComponent.onValueChanged.AddListener(delegate (float val) { changeValueForSlider(element, val); });
        }
    }

    /**
     * Returns the selected ambient AudioClip by checking which (if any) of the ambient 
     * sound toggles is selected.  
     */
    public AudioClip getSelectedSoundType() {
        Toggle activeToggle = ambientSoundToggleGroup.ActiveToggles().FirstOrDefault();
        if (activeToggle != null) {
            return this.ambientSoundTypes[activeToggle.name];
        } else {
            return null;
        }
    }

    /**
	 * Changes the value label of a slider and stores it in the value dictionary.
	 * @param  string slider
	 * @param  float  val
	 */
    private void changeValueForSlider(string slider, float val) {
        GameObject Slider = UI.transform.Find(slider).gameObject;
        Slider.transform.Find("Value").GetComponent<Text>().text = val.ToString();
        sliderValues[slider] = (int)val;
    }

    /**
	 * Creates a new Vector2 position for a slider.
	 * @param  int   total
	 * @param  int   index
	 * @param  float width
	 * @param  float height
	 * @param  float margin
	 * @return Vector2
	 */
    private Vector2 getPosition(int total, int index, float width, float height, float margin) {
        float xoffset = width / 2; // Move anchor of slider to the right instead of center.
        float yoffset = total * height / 2 + margin / 2; // Center the sliders with correct Y offset.
        float xmargin = (index % 2).Equals(1) ? -1 * margin : 1 * margin; // Correct sign of margin.
        return new Vector2(width * (index % 2) - xmargin - xoffset, (index - (index % 2)) * (height + margin) - yoffset);
    }

    private Vector2 getTogglePosition(Vector2 basePos, float height, int toggleNum) {
        float width = basePos.x;
        height = basePos.y * (toggleNum + 1); //0 indexing
        return new Vector2(width + 500, height + 140);
    }
}