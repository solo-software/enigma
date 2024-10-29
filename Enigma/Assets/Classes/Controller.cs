using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private EnigmaM3 enigma;
    private static char[] chars = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

    [SerializeField] private RotorNumber lRotor_number = RotorNumber.I;
    [SerializeField] private RotorNumber mRotor_number = RotorNumber.II;
    [SerializeField] private RotorNumber rRotor_number = RotorNumber.III;

    [SerializeField] private int lRotor_position = 0;
    [SerializeField] private int mRotor_position = 0;
    [SerializeField] private int rRotor_position = 0;

    [SerializeField] private int lRotor_ring_offset = 0;
    [SerializeField] private int mRotor_ring_offset = 0;
    [SerializeField] private int rRotor_ring_offset = 0;

    [SerializeField] private EntryWheel entry_wheel = EntryWheel.STANDARD;
    [SerializeField] private Reflector reflector = Reflector.B;

    private static int[][] ROTORS = 
    {
        new int[] {4, 10, 12, 5, 11, 6, 3, 16, 21, 25, 13, 19, 14, 22, 24, 7, 23, 20, 18, 15, 0, 8, 1, 17, 2, 9},
        new int[] {0, 9, 3, 10, 18, 8, 17, 20, 23, 1, 11, 7, 22, 19, 12, 2, 16, 6, 25, 13, 15, 24, 5, 21, 14, 4},
        new int[] {1, 3, 5, 7, 9, 11, 2, 15, 17, 19, 23, 21, 25, 13, 24, 4, 8, 22, 6, 0, 10, 12, 20, 18, 16, 14},
        new int[] {4, 18, 14, 21, 15, 25, 9, 0, 24, 16, 20, 8, 17, 7, 23, 11, 13, 5, 19, 6, 10, 3, 2, 12, 22, 1},
        new int[] {21, 25, 1, 17, 6, 8, 19, 24, 20, 15, 18, 3, 13, 7, 11, 23, 0, 22, 12, 9, 16, 14, 5, 4, 2, 10},
        new int[] {9, 15, 6, 21, 14, 20, 12, 5, 24, 16, 1, 4, 13, 7, 25, 17, 3, 10, 0, 18, 23, 11, 8, 2, 19, 22},
        new int[] {13, 25, 9, 7, 6, 17, 2, 23, 12, 24, 18, 22, 1, 14, 20, 5, 0, 8, 21, 11, 15, 4, 10, 16, 3, 19},
        new int[] {5, 10, 16, 7, 19, 11, 23, 14, 2, 1, 9, 18, 15, 3, 25, 17, 0, 12, 4, 22, 13, 8, 20, 24, 6, 21}
    };

    private static int[][] TURNOVER_POSITIONS =
    {
        new int[] {16},
        new int[] {4},
        new int[] {21},
        new int[] {9},
        new int[] {25},
        new int[] {25, 12},
        new int[] {25, 12},
        new int[] {25, 12}
    };

    private static int[][] REFLECTORS =
    {
        new int[] {24, 17, 20, 7, 16, 18, 11, 3, 15, 23, 13, 6, 14, 10, 12, 8, 4, 1, 5, 25, 2, 22, 21, 9, 0, 19 },
        new int[] {5, 21, 15, 9, 8, 0, 14, 24, 4, 3, 17, 25, 23, 22, 6, 2, 19, 10, 20, 16, 18, 1, 13, 12, 7, 11 }
    };

    private static int[][] ENTRY_WHEELS =
    {
        new int[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25 },
        new int[] {10, 23, 21, 12, 2, 13, 14, 15, 7, 16, 17, 18, 25, 24, 8, 9, 0, 3, 11, 4, 6, 22, 1, 20, 5, 19}
    };

    private enum RotorNumber
    {
        I,
        II,
        III,
        IV,
        V,
        VI,
        VII,
        VIII
    };

    private enum Reflector
    {
        B,
        C
    };

    private enum EntryWheel
    {
        STANDARD,
        QWERTY
    };

    // Start is called before the first frame update
    void Start()
    {
        Rotor lRotor = new Rotor(wiring: ROTORS[(int)lRotor_number], turn_positions: TURNOVER_POSITIONS[(int)lRotor_number], position: lRotor_position, ring_offset: lRotor_ring_offset);
        Rotor mRotor = new Rotor(wiring: ROTORS[(int)mRotor_number], turn_positions: TURNOVER_POSITIONS[(int)mRotor_number], position: mRotor_position, ring_offset: mRotor_ring_offset);
        Rotor rRotor = new Rotor(wiring: ROTORS[(int)rRotor_number], turn_positions: TURNOVER_POSITIONS[(int)rRotor_number], position: rRotor_position, ring_offset: rRotor_ring_offset);
        enigma = new EnigmaM3(lRotor, mRotor, rRotor, REFLECTORS[(int) reflector], ENTRY_WHEELS[(int) entry_wheel], ENTRY_WHEELS[0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        Event e = Event.current;
        if (e.isKey && e.type == EventType.KeyDown)
        {
            char[] key_code = e.keyCode.ToString().ToCharArray();
            if (key_code.Length == 1) {
                char entered_char = key_code[0];
                Debug.Log("Encrypting: " + entered_char.ToString());
                char output_char = enigma.Encrypt(entered_char);
                Debug.Log("Encrypted character: " +  output_char.ToString());
            }
        }
    }
}
