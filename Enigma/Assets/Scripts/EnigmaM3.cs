using System;
using System.Collections;
using System.Collections.Generic;
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
        // Ensure input is uppercase (this should be the case anyway)
        inputLetter = Char.ToUpper(inputLetter);

        // Advance any rotors that are due to be turned over
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

        // Get the A0Z25 index of the character
        int active_index = Array.IndexOf(CHARS, inputLetter);

        // Encrypt through the plugboard and entry wheel
        active_index = plugboard[active_index];
        active_index = entryWheel[active_index];

        // Send the signal through the rotors
        active_index = rRotor.GetOutput(active_index, true);
        active_index = mRotor.GetOutput(active_index, true);
        active_index = lRotor.GetOutput(active_index, true);

        // Get the reflector output of the signal
        active_index = reflector[active_index];

        // Send the signal BACKWARDS through the rotors
        active_index = lRotor.GetOutput(active_index, false);
        active_index = mRotor.GetOutput(active_index, false);
        active_index = rRotor.GetOutput(active_index, false);

        // Send the signal BACKWARDS through the entry wheel and plugboard
        active_index = entryWheel[active_index];
        active_index = plugboard[active_index];

        // Return the result as a character
        return CHARS[active_index];
    }

    // Get position of the given rotor
    // 0 = left
    // 1 = middle
    // 2 = right
    public int GetRotorPosition(int rotorIndex)
    {
        if (rotorIndex == 0)
        {
            return lRotor.GetPosition();
        }
        if (rotorIndex == 1)
        {
            return mRotor.GetPosition();
        }
        return rRotor.GetPosition();
    }

    // Get the apparent position of the given rotor according to the uppermost letter
    public int GetApparentRotorPosition(int rotorIndex)
    {
        if (rotorIndex == 0)
        {
            return lRotor.GetApparentPosition();
        }
        if (rotorIndex == 1)
        {
            return mRotor.GetApparentPosition();
        }
        return rRotor.GetApparentPosition();
    }

    // Replace the given rotor with a new rotor
    public void SetRotor(int rotorIndex, Rotor rotor)
    {
        if (rotorIndex == 0)
        {
            lRotor = rotor;
        }
        else if (rotorIndex == 1)
        {
            mRotor = rotor;
        }
        else
        {
            rRotor = rotor;
        }
    }
}
