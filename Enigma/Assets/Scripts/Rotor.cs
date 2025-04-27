using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Rotor
{

    private int[] wiring;
    private int position;
    private int ring_offset;
    private int[] turn_positions;
    private Transform rotorTransform;
    private Controller controller;

    private static Vector3 ROTOR_STEP = new Vector3(0, -360f / 26, 0);
    private static int FRAMES_PER_ROTATION = 20;

    public Rotor(int[] wiring, int[] turn_positions, Transform rotorTransform, Controller controller, int position = 0, int ring_offset = 0)
    {
        this.wiring = wiring;
        this.turn_positions = turn_positions;
        this.position = position;
        this.ring_offset = ring_offset;
        this.rotorTransform = rotorTransform;
        this.controller = controller;
    }

    public int GetOutput(int input_index, bool signal_out)
    {
        // Calculate the offset of the entered character according to rotor position
        int actual_index = Controller.mod(input_index + position, 26);
        int output_index;

        // Calculate the output index depending on whether the signal is going "out" or "back" through the rotor
        output_index = signal_out ? wiring[actual_index] : Array.IndexOf(wiring, actual_index);

        // Prevent output index from overflowing
        output_index = Controller.mod(output_index - position, 26);

        return output_index;
    }

    public bool Advance()
    {
        // Increment position and prevent overflow
        position = Controller.mod(position + 1, 26);

        // If there is a model corresponding to the rotor (i.e. it is not being tested)
        // then start a rotation animation
        if (rotorTransform)
        {
            controller.StartCoroutine(RotateRotor());
        }

        // Return whether this step will cause a turnover of adjacent rotors
        return turn_positions.Contains(GetApparentPosition());
    }

    // Get the functional position of the rotor (for encryption)
    public int GetPosition()
    {
        return position;
    }

    // Get the position that the rotor appears to be in according to its alphabet tyre
    public int GetApparentPosition()
    {
        return Controller.mod((position + ring_offset), 26);
    }

    // Rotate the rotor by a small amount each frame to produce a rotation animation
    public IEnumerator RotateRotor()
    {
        for (int i = 0; i < FRAMES_PER_ROTATION; i++)
        {
            rotorTransform.Rotate(ROTOR_STEP / FRAMES_PER_ROTATION);
            yield return null;
        }
    }

    public bool HasTransform() => rotorTransform;
}
