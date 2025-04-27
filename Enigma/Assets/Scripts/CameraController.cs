using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Animator cameraAnimator;
    [SerializeField] private TextMeshProUGUI upArrowText;
    [SerializeField] private TextMeshProUGUI downArrowText;
    [SerializeField] private GameObject upArrow;
    [SerializeField] private GameObject downArrow;
    [SerializeField] private CanvasGroup gameCanvasGroup;
    [SerializeField] private CanvasGroup startCanvasGroup;
    [SerializeField] private CanvasGroup rotorSettingsCanvasGroup;

    // -1 = plugboard
    //  0 = keyboard
    //  1 = rotors
    private int cameraState;

    private static int START_FRAMES_TO_FADE = 240;
    private static int SETTINGS_FRAMES_TO_FADE = 60;

    private float startAlphaStep;
    private float settingsAlphaStep;
    // Start is called before the first frame update
    void Start()
    {
        cameraState = 0;
        startAlphaStep = 1f / START_FRAMES_TO_FADE;
        settingsAlphaStep = 1f / SETTINGS_FRAMES_TO_FADE;
    }

    public void UpArrowClicked()
    {
        // Animate the camera from the keyboard to the rotors
        // Change the UI arrows accordingly
        if (cameraState == 0)
        {
            cameraState = 1;
            cameraAnimator.SetBool("AtRotors", true);
            downArrowText.text = "Keyboard";
            upArrow.SetActive(false);
            StartCoroutine(FadeInSettings());
        }
        // Plugboard to keyboard
        else if (cameraState == -1)
        {
            cameraState = 0;
            cameraAnimator.SetBool("AtPlugboard", false);
            downArrowText.text = "Plugboard";
            upArrowText.text = "Rotors";
            downArrow.SetActive(true);
        }
    }

    public void DownArrowClicked()
    {
        // Keyboard to plugboard
        if (cameraState == 0)
        {
            cameraState = -1;
            cameraAnimator.SetBool("AtPlugboard", true);
            upArrowText.text = "Keyboard";
            downArrow.SetActive(false);
        }
        // Rotors to keyboard
        else if (cameraState == 1)
        {
            cameraState = 0;
            cameraAnimator.SetBool("AtRotors", false);
            upArrowText.text = "Rotors";
            downArrowText.text = "Plugboard";
            upArrow.SetActive(true);
            StartCoroutine(FadeOutSettings());
        }
    }

    public void StartButtonClicked()
    {
        // Trigger the initial camera zoom
        cameraAnimator.SetTrigger("StartButtonClicked");
        // Fade out the title screen and fade in the UI
        StartCoroutine(FadeCanvases());
    }

    // Fade out the title screen and fade in the UI
    IEnumerator FadeCanvases()
    {
        for (int i = 0; i < START_FRAMES_TO_FADE; i++)
        {
            startCanvasGroup.alpha -= startAlphaStep;
            yield return null;
        }
        for (int i = 0; i < START_FRAMES_TO_FADE; i++)
        {
            gameCanvasGroup.alpha += startAlphaStep;
            yield return null;
        }   
    }

    // Fade in the rotor settings interface
    IEnumerator FadeInSettings()
    {
        for (int i = 0; i < SETTINGS_FRAMES_TO_FADE; i++)
        {
            rotorSettingsCanvasGroup.alpha += settingsAlphaStep;
            yield return null;
        }
    }

    // Fade out the rotor settings interface
    IEnumerator FadeOutSettings()
    {
        for (int i = 0; i < SETTINGS_FRAMES_TO_FADE; i++)
        {
            rotorSettingsCanvasGroup.alpha -= settingsAlphaStep;
            yield return null;
        }
    }
}
