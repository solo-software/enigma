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

    private static char[] chars = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

    public EnigmaM3(Rotor lRotor, Rotor mRotor, Rotor rRotor, int[] reflector, int[] entry_wheel, int[] plugboard)
    {
        this.lRotor = lRotor;
        this.mRotor = mRotor;
        this.rRotor = rRotor;
        this.reflector = reflector;
        this.entry_wheel = entry_wheel;
        this.plugboard = plugboard;

        rotors_to_advance = new bool[] { false, false, true };
    }

    public char Encrypt(char input_letter)
    {
        input_letter = Char.ToUpper(input_letter);

        if (rotors_to_advance[0])
        {
            rotors_to_advance[0] = false;
            lRotor.Advance();
            mRotor.Advance();
        }
        if (rotors_to_advance[1])
        {
            rotors_to_advance[1] = false;
            rotors_to_advance[0] = mRotor.Advance();
        }
        rotors_to_advance[1] = rRotor.Advance();

        int active_index = Array.IndexOf(chars, input_letter);

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

        return chars[active_index];
    }
}
