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
    private int[] entryWheel;
    private int[] plugboard;
    private bool[] rotorsToAdvance;

    private static char[] CHARS = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

    public EnigmaM3(Rotor lRotor, Rotor mRotor, Rotor rRotor, int[] reflector, int[] entryWheel, int[] plugboard)
    {
        this.lRotor = lRotor;
        this.mRotor = mRotor;
        this.rRotor = rRotor;
        this.reflector = reflector;
        this.entryWheel = entryWheel;
        this.plugboard = plugboard;

        rotorsToAdvance = new bool[] { false, false, true };
    }

    public char Encrypt(char inputLetter)
    {
        inputLetter = Char.ToUpper(inputLetter);

        if (rotorsToAdvance[0])
        {
            rotorsToAdvance[0] = false;
            lRotor.Advance();
            mRotor.Advance();
        }
        if (rotorsToAdvance[1])
        {
            rotorsToAdvance[1] = false;
            rotorsToAdvance[0] = mRotor.Advance();
        }
        rotorsToAdvance[1] = rRotor.Advance();

        int active_index = Array.IndexOf(CHARS, inputLetter);

        active_index = plugboard[active_index];
        active_index = entryWheel[active_index];

        active_index = rRotor.GetOutput(active_index, true);
        active_index = mRotor.GetOutput(active_index, true);
        active_index = lRotor.GetOutput(active_index, true);

        active_index = reflector[active_index];

        active_index = lRotor.GetOutput(active_index, false);
        active_index = mRotor.GetOutput(active_index, false);
        active_index = rRotor.GetOutput(active_index, false);

        active_index = entryWheel[active_index];
        active_index = plugboard[active_index];

        return CHARS[active_index];
    }
}
