using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UI;
using UnityEngine;

public class Rotor
{

    private int[] wiring;
    private int position;
    private int ring_offset;
    private int[] turn_positions;
    private Transform rotorTransform;

    private static Vector3 ROTOR_STEP = new Vector3(0, -360 / 26, 0);

    private static int mod(int x, int y)
    {
        return (x % y + y) % y;
    }

    public Rotor(int[] wiring, int[] turn_positions, Transform rotorTransform, int position = 0, int ring_offset = 0)
    {
        this.wiring = wiring;
        this.turn_positions = turn_positions;
        this.position = position;
        this.ring_offset = ring_offset;
        this.rotorTransform = rotorTransform;
    }

    public int GetOutput(int input_index, bool signal_out)
    {
        int actual_index = mod(input_index + position, 26);
        int output_index;

        output_index = signal_out ? wiring[actual_index] : Array.IndexOf(wiring, actual_index);

        output_index = mod(output_index - position, 26);

        return output_index;
    }

    public bool Advance()
    {
        position = mod(position + 1, 26);

        if (rotorTransform)
        {
            rotorTransform.Rotate(ROTOR_STEP);
        }

        return turn_positions.Contains(position - ring_offset) ? true : false;
    }

    public int GetPosition()
    {
        return position;
    }
}
