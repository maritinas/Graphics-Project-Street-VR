using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour {
    GameObject FPSController;

    // Use this for initialization
    void Start () {
        FPSController = GameObject.Find("FPSController");
        FPSController.SetActive(false);
        setFPSControllerPosition();
        setSoundVolume();
        setSoundType();
        FPSController.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene("GUI");
        }
	}

    /**
     * Position the FPS controller to the start of the path specified 
     * by the movement script. 
     */
     private void setFPSControllerPosition() {
        GameObject path = GameObject.Find("MovementScript");
        Transform startPoint = path.transform.GetChild(0);
        Vector3 FPSControllerNewPosition = startPoint.transform.position;
        FPSController.transform.position = FPSControllerNewPosition;
        Debug.Log("FPS = " + FPSController.transform.position);
    }

    /**
     * Set sound volume in the scene. 
     */
    private void setSoundVolume() {
        AudioListener.volume = GlobalControl.Instance.SoundVolume;
        Debug.Log("Volume = " + AudioListener.volume);
    }

    /**
     * Set ambient sound to the the AudioClip given in the field AmbientSoundAudioClip.
     */
    private void setSoundType() {
        Transform firstPersonCharacter = FPSController.transform.GetChild(0);
        Transform ambientSound = firstPersonCharacter.transform.GetChild(0);
        AudioSource ambientSoundSource = ambientSound.GetComponent<AudioSource>();
        ambientSoundSource.clip = GlobalControl.Instance.AmbientSound;
    }
}
