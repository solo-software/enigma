using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlugboardSocket : MonoBehaviour
{
    [SerializeField] private Material DEFAULT_MATERIAL;
    [SerializeField] private Material HIGHLIGHT_MATERIAL;
    public Controller ENIGMA_CONTROLLER;
    private char plugCharacter = '0';

    public void OnMouseDown()
    {
        if (plugCharacter == '0')
        {
            plugCharacter = gameObject.name[name.Length - 1];
        }
        Debug.Log(plugCharacter.ToString() + " Clicked!");
        ENIGMA_CONTROLLER.PlugSocketSelected(plugCharacter, gameObject);
    }

    public void OnMouseOver()
    {
        Material[] materials = gameObject.GetComponent<Renderer>().materials;
        materials[0] = HIGHLIGHT_MATERIAL;
        gameObject.GetComponent<Renderer>().materials = materials;
    }

    public void OnMouseExit()
    {
        Material[] materials = gameObject.GetComponent<Renderer>().materials;
        materials[0] = DEFAULT_MATERIAL;
        gameObject.GetComponent<Renderer>().materials = materials;
    }

    public char GetPlugCharacter()
    {
        return plugCharacter;
    }
}
