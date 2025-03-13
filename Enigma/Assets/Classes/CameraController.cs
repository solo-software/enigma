using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Animator cameraAnimator;
    [SerializeField] private TextMeshProUGUI upArrowText;
    [SerializeField] private TextMeshProUGUI downArrowText;
    [SerializeField] private GameObject upArrow;
    [SerializeField] private GameObject downArrow;

    private int cameraState;
    // Start is called before the first frame update
    void Start()
    {
        cameraState = 0;
    }

    public void UpArrowClicked()
    {
        if (cameraState == 0)
        {
            cameraState = 1;
            //upArrow.SetActive(false);
            cameraAnimator.SetBool("AtRotors", true);
            downArrowText.text = "Keyboard";
            upArrow.SetActive(false);
            
        }
        else if (cameraState == -1)
        {
            cameraState = 0;
            //downArrow.SetActive(true);
            cameraAnimator.SetBool("AtPlugboard", false);
            downArrowText.text = "Plugboard";
            downArrow.SetActive(true);
        }
    }

    public void DownArrowClicked()
    {
        if (cameraState == 0)
        {
            cameraState = -1;
            //downArrow.SetActive(false);
            cameraAnimator.SetBool("AtPlugboard", true);
            upArrowText.text = "Keyboard";
            downArrow.SetActive(false);
        }
        else if (cameraState == 1)
        {
            cameraState = 0;
            //upArrow.SetActive(true);
            cameraAnimator.SetBool("AtRotors", false);
            upArrowText.text = "Rotors";
            upArrow.SetActive(true);
        }
    }
}
