using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Controller : MonoBehaviour
{
    private EnigmaM3 enigma;
    private GameObject lastLamp = null;
    private static char[] QWERTY = new char[] { 'Q', 'W', 'E', 'R', 'T', 'Y', 'U', 'I', 'O', 'A', 'S', 'D', 'F', 'G', 'H', 'J', 'K', 'P', 'L', 'Z', 'X', 'C', 'V', 'B', 'N', 'M' };

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

    [SerializeField] private int numWireSegments = 10;
    [SerializeField] private float wireParabolaHeight = 0.01f;
    [SerializeField] private float wireThickness = 0.00001f;

    [SerializeField] private GameObject ROTOR_MODEL;
    [SerializeField] private List<GameObject> ALPHABET_TYRES;
    [SerializeField] private GameObject LAMP_MODEL;
    [SerializeField] private GameObject LAMP_LABEL;
    [SerializeField] private GameObject PLUG_SOCKET_MODEL;
    [SerializeField] private GameObject PLUG_SOCKET_LABEL;
    [SerializeField] private GameObject PLUG_MODEL;
    [SerializeField] private GameObject KEY_MODEL;
    [SerializeField] private GameObject KEY_LABEL;

    [SerializeField] private Material LAMP_UNLIT;
    [SerializeField] private Material LAMP_LIT;
    [SerializeField] private Material PLUG_WIRE;

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

    private int[] plugboardConfig = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25 };

    private GameObject rootPlugSocket;
    private GameObject leafPlugSocket;
    private GameObject mousePlug;
    private GameObject rootPlug;

    private static Vector3 PLUG_WIRE_OFFSET = new Vector3(-0.005f, -0.006f, 0);

    void Start()
    {
        for (int i = 0; i < 26; i++)
        {
            float xCoordLamp = -0.065f;
            float yCoordLamp = 0.04f;
            float zCoordLamp = 0.1f - (0.025f * (i % 9));
            float xCoordPlugSocket = -0.181f;
            float yCoordPlugSocket = -0.007f;
            float zCoordPlugSocket = 0.1f - (0.025f * (i % 9));
            float xCoordKey = -0.13f;
            float yCoordKey = 0.04f;
            float zCoordKey = 0.1f - (0.025f *  (i % 9));
            if (i > 8)
            {
                xCoordLamp -= 0.02f;
                zCoordLamp -= 0.0125f;
                yCoordPlugSocket -= 0.016f;
                zCoordPlugSocket -= 0.0125f;
                xCoordKey -= 0.02f;
                yCoordKey -= 0.008f;
                zCoordKey -= 0.0125f;
            }
            if (i > 16)
            {
                xCoordLamp -= 0.02f;
                zCoordLamp = 0.1f - (0.025f * ((i + 1) % 9));
                yCoordPlugSocket -= 0.016f;
                zCoordPlugSocket = 0.1f - (0.025f * ((i + 1) % 9));
                xCoordKey -= 0.02f;
                yCoordKey -= 0.008f;
                zCoordKey = 0.1f - (0.025f * ((i + 1) % 9));
            }

            GameObject thisLamp = GameObject.Instantiate(LAMP_MODEL, new Vector3(xCoordLamp, yCoordLamp, zCoordLamp), Quaternion.Euler(-90, 0, 180));
            thisLamp.name = "Lamp" + QWERTY[i].ToString();

            GameObject lampLabelCanvas = GameObject.Instantiate(LAMP_LABEL, thisLamp.GetComponent<Transform>());
            lampLabelCanvas.GetComponentInChildren<TextMeshProUGUI>().text = QWERTY[i].ToString();

            GameObject thisPlugSocket = GameObject.Instantiate(PLUG_SOCKET_MODEL, new Vector3(xCoordPlugSocket, yCoordPlugSocket, zCoordPlugSocket), Quaternion.Euler(-90, 0, 180));
            thisPlugSocket.name = "PlugSocket" + QWERTY[i].ToString();
            thisPlugSocket.GetComponent<PlugboardSocket>().ENIGMA_CONTROLLER = this;

            GameObject plugSocketLabelCanvas = GameObject.Instantiate(PLUG_SOCKET_LABEL, new Vector3(thisPlugSocket.GetComponent<Transform>().position.x - 0.0033f, thisPlugSocket.GetComponent<Transform>().position.y + 0.0115f, thisPlugSocket.GetComponent<Transform>().position.z), Quaternion.Euler(0, 90, 0), thisPlugSocket.GetComponent<Transform>()); 
            plugSocketLabelCanvas.GetComponentInChildren<TextMeshProUGUI>().text = QWERTY[i].ToString();

            GameObject thisKey = GameObject.Instantiate(KEY_MODEL, new Vector3(xCoordKey, yCoordKey, zCoordKey), Quaternion.Euler(180, 0, 180));
            thisKey.name = "Key" + QWERTY[i].ToString();

            GameObject keyLabelCanvas = GameObject.Instantiate(KEY_LABEL, new Vector3(thisKey.GetComponent<Transform>().position.x, thisKey.GetComponent<Transform>().position.y + 0.00121f, thisKey.GetComponent<Transform>().position.z), Quaternion.Euler(90, 90, 0), thisKey.GetComponent<Transform>());
            keyLabelCanvas.GetComponentInChildren<TextMeshProUGUI>().text = QWERTY[i].ToString();
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

    void Update()
    {
        if (mousePlug)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                mousePlug.transform.position = hit.point;
            }
            LineRenderer lineRenderer = rootPlug.GetComponent<LineRenderer>();
            Vector3 start = rootPlug.transform.position;
            Vector3 end = mousePlug.transform.position;
            for (int i = 0; i < numWireSegments; i++)
            {
                float t = (float)i / (numWireSegments - 1);

                // Linear interpolation between start and end points
                Vector3 point = Vector3.Lerp(start, end, t);

                // Add parabolic height based on t
                point.y -= wireParabolaHeight * Mathf.Sin(Mathf.PI * t);

                point += PLUG_WIRE_OFFSET;

                lineRenderer.SetPosition(i, point);
            }
        }
    }

    private void OnGUI()
    {
        Event e = Event.current;
        if (e.isKey && e.type == EventType.KeyDown)
        {
            char[] key_code = e.keyCode.ToString().ToCharArray();
            if (key_code.Length == 1) {
                char entered_char = key_code[0];
                entered_char = (char)(plugboardConfig[entered_char - 65] + 65);
                Debug.Log("Encrypting: " + entered_char.ToString());
                char output_char = enigma.Encrypt(entered_char);
                output_char = (char)(plugboardConfig[output_char - 65] + 65);
                Debug.Log("Encrypted character: " +  output_char.ToString());
                int outputIndex = output_char - 65;
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

    public void PlugSocketSelected(char plugChar, GameObject selectedPlugSocket)
    {
        if (plugboardConfig[plugChar - 65] == plugChar - 65)
        {
            if (!rootPlugSocket)
            {
                rootPlugSocket = selectedPlugSocket;
                rootPlug = GameObject.Instantiate(PLUG_MODEL, rootPlugSocket.GetComponent<Transform>().position, Quaternion.Euler(new Vector3(-90, 0, 180)), rootPlugSocket.GetComponent<Transform>());
                rootPlug.name = "Plug" + plugChar.ToString();
                mousePlug = GameObject.Instantiate(PLUG_MODEL, rootPlugSocket.GetComponent<Transform>().position, Quaternion.Euler(new Vector3(-90, 0, 180)));
                LineRenderer rootPlugWireRenderer = rootPlug.AddComponent<LineRenderer>();
                rootPlugWireRenderer.startWidth = wireThickness;
                rootPlugWireRenderer.endWidth = wireThickness;
                rootPlugWireRenderer.material = PLUG_WIRE;
                rootPlugWireRenderer.positionCount = numWireSegments;
            }
            else if (rootPlugSocket == selectedPlugSocket)
            {
                Destroy(GameObject.Find("Plug" + plugChar.ToString()));
                Destroy(mousePlug);
                mousePlug = null;
                rootPlugSocket = null;
            }
            else
            {
                leafPlugSocket = selectedPlugSocket;
                GameObject leafPlug = GameObject.Instantiate(PLUG_MODEL, leafPlugSocket.GetComponent<Transform>().position, Quaternion.Euler(new Vector3(-90, 0, 180)), leafPlugSocket.GetComponent<Transform>());
                leafPlug.name = "Plug" + plugChar.ToString();
                Destroy(mousePlug);
                mousePlug = null;
                MakePlugboardConnection();
            }
        }
        else
        {
            if (!leafPlugSocket) {
                leafPlugSocket = selectedPlugSocket;
                rootPlugSocket = GameObject.Find("PlugSocket" + ((char)(plugboardConfig[plugChar - 65] + 65)).ToString());
                GameObject leafPlug = GameObject.Find("Plug" + plugChar.ToString());
                rootPlug = GameObject.Find("Plug" + ((char)(plugboardConfig[plugChar - 65] + 65)).ToString());
                LineRenderer lr = leafPlug.GetComponent<LineRenderer>();
                if (lr)
                {
                    Destroy(lr);
                    LineRenderer rootPlugWireRenderer = rootPlug.AddComponent<LineRenderer>();
                    rootPlugWireRenderer.startWidth = wireThickness;
                    rootPlugWireRenderer.endWidth = wireThickness;
                    rootPlugWireRenderer.material = PLUG_WIRE;
                    rootPlugWireRenderer.positionCount = numWireSegments;
                }
                Destroy(GameObject.Find("Plug" + plugChar.ToString()));
                mousePlug = GameObject.Instantiate(PLUG_MODEL, rootPlugSocket.GetComponent<Transform>().position, Quaternion.Euler(new Vector3(-90, 0, 180)));
                BreakPlugboardConnection();
            }
            else
            {
                Destroy(GameObject.Find("Plug" + plugChar.ToString()));
                Destroy(mousePlug);
                mousePlug = null;
                rootPlugSocket = null;
            }
        }
    }

    private void MakePlugboardConnection()
    {
        int rootIndex = rootPlugSocket.GetComponent<PlugboardSocket>().GetPlugCharacter() - 65;
        int leafIndex = leafPlugSocket.GetComponent<PlugboardSocket>().GetPlugCharacter() - 65;

        plugboardConfig[rootIndex] = leafIndex;
        plugboardConfig[leafIndex] = rootIndex;

        Debug.Log("Characters connected: " + ((char)(rootIndex + 65)).ToString() + " and " + ((char)(leafIndex + 65)).ToString());

        rootPlugSocket = null;
        leafPlugSocket = null;
    }

    private void BreakPlugboardConnection()
    {
        int rootIndex = rootPlugSocket.GetComponent<PlugboardSocket>().GetPlugCharacter() - 65;
        int leafIndex = leafPlugSocket.GetComponent<PlugboardSocket>().GetPlugCharacter() - 65;

        plugboardConfig[rootIndex] = rootIndex;
        plugboardConfig[leafIndex] = leafIndex;
    }
}
