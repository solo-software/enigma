using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    private EnigmaM3 enigma;
    private GameObject lastLamp = null;
    private static char[] CHARS = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
    private static char[] QWERTY = new char[] { 'Q', 'W', 'E', 'R', 'T', 'Y', 'U', 'I', 'O', 'P', 'A', 'S', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'Z', 'X', 'C', 'V', 'B', 'N', 'M' };

    [SerializeField] private RotorNumber lRotorNumber = RotorNumber.I;
    [SerializeField] private RotorNumber mRotorNumber = RotorNumber.II;
    [SerializeField] private RotorNumber rRotorNumber = RotorNumber.III;

    [SerializeField] private int lRotorPosition = 0;
    [SerializeField] private int mRotorPosition = 0;
    [SerializeField] private int rRotorPosition = 0;

    [SerializeField] private Character lRotorRingSetting = 0;
    [SerializeField] private Character mRotorRingSetting = 0;
    [SerializeField] private Character rRotorRingSetting = 0;

    [SerializeField] private EntryWheel entryWheel = EntryWheel.STANDARD;
    [SerializeField] private Reflector reflector = Reflector.B;

    [SerializeField] private GameObject ROTOR_MODEL;
    [SerializeField] private List<GameObject> ALPHABET_TYRES;
    [SerializeField] private GameObject LAMP_MODEL;
    [SerializeField] private GameObject LAMP_LABEL;

    [SerializeField] private Material LAMP_UNLIT;
    [SerializeField] private Material LAMP_LIT;

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

    private enum Character
    {
        A,
        B,
        C,
        D,
        E,
        F,
        G,
        H,
        I,
        J,
        K,
        L,
        M,
        N,
        O,
        P,
        Q,
        R,
        S,
        T,
        U,
        V,
        W,
        X,
        Y,
        Z
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 26; i++)
        {
            float xCoord = -0.065f;
            float yCoord = 0.04f;
            float zCoord = 0.1f - (0.025f * (i % 9));
            if (i > 8)
            {
                xCoord -= 0.02f;
                zCoord -= 0.0125f;
            }
            if (i > 16)
            {
                xCoord -= 0.02f;
                zCoord = 0.1f - (0.025f * ((i + 1) % 9));
            }

            GameObject thisLamp = GameObject.Instantiate(LAMP_MODEL, new Vector3(xCoord, yCoord, zCoord), Quaternion.Euler(-90, 0, 180));
            thisLamp.name = "Lamp" + QWERTY[i].ToString();

            GameObject lampLabelCanvas = GameObject.Instantiate(LAMP_LABEL, thisLamp.GetComponent<Transform>());
            lampLabelCanvas.GetComponentInChildren<TextMeshProUGUI>().text = QWERTY[i].ToString();
        }

        GameObject rRotorObject = GameObject.Instantiate(ROTOR_MODEL, new Vector3(0, 0, 0f), Quaternion.Euler(90, 0, 180));
        GameObject mRotorObject = GameObject.Instantiate(ROTOR_MODEL, new Vector3(0, 0, 0.02f), Quaternion.Euler(90, 0, 180));
        GameObject lRotorObject = GameObject.Instantiate(ROTOR_MODEL, new Vector3(0, 0, 0.04f), Quaternion.Euler(90, 0, 180));

        Transform rRotorTransform = rRotorObject.GetComponent<Transform>();
        Transform mRotorTransform = mRotorObject.GetComponent<Transform>();
        Transform lRotorTransform = lRotorObject.GetComponent<Transform>();

        GameObject rRotorAlphabetRing = GameObject.Instantiate(ALPHABET_TYRES[(int)rRotorNumber], new Vector3(0, 0, 0.005f), Quaternion.Euler(90, 0, 180), rRotorObject.GetComponent<Transform>());
        GameObject mRotorAlphabetRing = GameObject.Instantiate(ALPHABET_TYRES[(int)mRotorNumber], new Vector3(0, 0, 0.025f), Quaternion.Euler(90, 0, 180), mRotorObject.GetComponent<Transform>());
        GameObject lRotorAlphabetRing = GameObject.Instantiate(ALPHABET_TYRES[(int)lRotorNumber], new Vector3(0, 0, 0.045f), Quaternion.Euler(90, 0, 180), lRotorObject.GetComponent<Transform>());

        rRotorAlphabetRing.GetComponent<Transform>().Rotate(new Vector3(0, 180 - (360 / 26) * (int)rRotorRingSetting, 0));
        mRotorAlphabetRing.GetComponent<Transform>().Rotate(new Vector3(0, 180 - (360 / 26) * (int)mRotorRingSetting, 0));
        lRotorAlphabetRing.GetComponent<Transform>().Rotate(new Vector3(0, 180 - (360 / 26) * (int)lRotorRingSetting, 0));

        rRotorTransform.Rotate(new Vector3(0, (360 / 26) * (int)rRotorRingSetting, 0));
        mRotorTransform.Rotate(new Vector3(0, (360 / 26) * (int)mRotorRingSetting, 0));
        lRotorTransform.Rotate(new Vector3(0, (360 / 26) * (int)lRotorRingSetting, 0));

        rRotorTransform.Rotate(new Vector3(0, -(360 / 26) * rRotorPosition, 0));
        mRotorTransform.Rotate(new Vector3(0, -(360 / 26) * mRotorPosition, 0));
        lRotorTransform.Rotate(new Vector3(0, -(360 / 26) * lRotorPosition, 0));

        rRotorPosition -= (int)rRotorRingSetting;
        mRotorPosition -= (int)mRotorRingSetting;
        lRotorPosition -= (int)lRotorRingSetting;

        Rotor lRotor = new Rotor(wiring: ROTORS[(int)lRotorNumber], turn_positions: TURNOVER_POSITIONS[(int)lRotorNumber], rotorTransform: lRotorTransform, position: lRotorPosition, ring_offset: (int)lRotorRingSetting);
        Rotor mRotor = new Rotor(wiring: ROTORS[(int)mRotorNumber], turn_positions: TURNOVER_POSITIONS[(int)mRotorNumber], rotorTransform: mRotorTransform, position: mRotorPosition, ring_offset: (int)mRotorRingSetting);
        Rotor rRotor = new Rotor(wiring: ROTORS[(int)rRotorNumber], turn_positions: TURNOVER_POSITIONS[(int)rRotorNumber], rotorTransform: rRotorTransform, position: rRotorPosition, ring_offset: (int)rRotorRingSetting);
        enigma = new EnigmaM3(lRotor, mRotor, rRotor, REFLECTORS[(int)reflector], ENTRY_WHEELS[(int)entryWheel], ENTRY_WHEELS[0]);
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
                int outputIndex = CHARS.ToList().IndexOf(output_char);
                if (lastLamp != null)
                {
                    Material[] lastTempMats = lastLamp.GetComponent<Renderer>().materials;
                    lastTempMats[0] = LAMP_UNLIT;
                    lastLamp.GetComponent<Renderer>().materials = lastTempMats;
                }
                lastLamp = GameObject.Find("Lamp" + output_char.ToString());
                Material[] tempMats = lastLamp.GetComponent<Renderer>().materials;
                tempMats[0] = LAMP_LIT;
                lastLamp.GetComponent<Renderer>().materials = tempMats;
            }
        }
    }
}
