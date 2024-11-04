using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class EnigmaM3
{
    private Rotor lRotor;
    private Rotor mRotor;
    private Rotor rRotor;
    private int[] reflector;
    private int[] entry_wheel;
    private int[] plugboard;
    private bool[] rotors_to_advance;
    private GameObject lRotorObject;
    private GameObject mRotorObject;
    private GameObject rRotorObject;

    private static char[] CHARS = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

    private static Vector3 ROTOR_STEP = new Vector3(0, -360 / 26, 0);

    public EnigmaM3(Rotor lRotor, Rotor mRotor, Rotor rRotor, int[] reflector, int[] entry_wheel, int[] plugboard, GameObject lRotorObject, GameObject mRotorObject, GameObject rRotorObject)
    {
        this.lRotor = lRotor;
        this.mRotor = mRotor;
        this.rRotor = rRotor;
        this.reflector = reflector;
        this.entry_wheel = entry_wheel;
        this.plugboard = plugboard;
        this.lRotorObject = lRotorObject;
        this.mRotorObject = mRotorObject;
        this.rRotorObject = rRotorObject;

        rotors_to_advance = new bool[] { false, false, true };
    }

    public char Encrypt(char input_letter)
    {
        input_letter = Char.ToUpper(input_letter);

        if (rotors_to_advance[0])
        {
            rotors_to_advance[0] = false;
            lRotor.Advance();
            lRotorObject.GetComponent<Transform>().Rotate(ROTOR_STEP);
            mRotor.Advance();
            mRotorObject.GetComponent<Transform>().Rotate(ROTOR_STEP);
        }
        if (rotors_to_advance[1])
        {
            rotors_to_advance[1] = false;
            rotors_to_advance[0] = mRotor.Advance();
            mRotorObject.GetComponent<Transform>().Rotate(ROTOR_STEP);
        }
        rotors_to_advance[1] = rRotor.Advance();
        rRotorObject.GetComponent<Transform>().Rotate(ROTOR_STEP);

        int active_index = Array.IndexOf(CHARS, input_letter);

        active_index = plugboard[active_index];
        active_index = entry_wheel[active_index];

        active_index = rRotor.GetOutput(active_index, true);
        active_index = mRotor.GetOutput(active_index, true);
        active_index = lRotor.GetOutput(active_index, true);

        active_index = reflector[active_index];

        active_index = lRotor.GetOutput(active_index, false);
        active_index = mRotor.GetOutput(active_index, false);
        active_index = rRotor.GetOutput(active_index, false);

        active_index = entry_wheel[active_index];
        active_index = plugboard[active_index];

        return CHARS[active_index];
    }
}
