using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Data;
using System.Linq;
using TMPro;
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

    [SerializeField] private Character lRotorRingSetting = Character.A;
    [SerializeField] private Character mRotorRingSetting = Character.A;
    [SerializeField] private Character rRotorRingSetting = Character.A;

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

    [SerializeField] private TextMeshProUGUI notesText;

    [SerializeField] private AudioSource keyAudio;

    [SerializeField] private TMP_Dropdown lRotorNumberDropdown;
    [SerializeField] private TMP_Dropdown lRotorRingSettingDropdown;
    [SerializeField] private TMP_Dropdown lRotorPositionDropdown;
    [SerializeField] private TMP_Dropdown mRotorNumberDropdown;
    [SerializeField] private TMP_Dropdown mRotorRingSettingDropdown;
    [SerializeField] private TMP_Dropdown mRotorPositionDropdown;
    [SerializeField] private TMP_Dropdown rRotorNumberDropdown;
    [SerializeField] private TMP_Dropdown rRotorRingSettingDropdown;
    [SerializeField] private TMP_Dropdown rRotorPositionDropdown;

    private List<TMP_Dropdown> dropdowns;

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

    private int groupLength = 0;

    private static Vector3 PLUG_WIRE_OFFSET = new Vector3(-0.005f, -0.006f, 0);

    private GameObject lRotorObject;
    private GameObject mRotorObject;
    private GameObject rRotorObject;
    private GameObject lRotorAlphabetRing;
    private GameObject mRotorAlphabetRing;
    private GameObject rRotorAlphabetRing;

    private bool enableRotorChanges = false; // Prevents rotors from being forcibly recreated when dropdowns changed from script

    void Start()
    {
        // Initialise dropdown values to starting values set in inspector
        dropdowns = new List<TMP_Dropdown>() { lRotorNumberDropdown, lRotorRingSettingDropdown, lRotorPositionDropdown, mRotorNumberDropdown, mRotorRingSettingDropdown, mRotorPositionDropdown, rRotorNumberDropdown, rRotorRingSettingDropdown, rRotorPositionDropdown};
        dropdowns[0].value = (int)lRotorNumber;
        dropdowns[1].value = (int)lRotorRingSetting;
        dropdowns[2].value = lRotorPosition;

        dropdowns[3].value = (int)mRotorNumber;
        dropdowns[4].value = (int)mRotorRingSetting;
        dropdowns[5].value = mRotorPosition;

        dropdowns[6].value = (int)rRotorNumber;
        dropdowns[7].value = (int)rRotorRingSetting;
        dropdowns[8].value = rRotorPosition;

        enableRotorChanges = true;

        // Generate keys, lamps and plug sockets
        // Each has a starting set of coordinates, then an offset is added
        for (int i = 0; i < 26; i++)
        {
            // Calculate object coordinates

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

            // Instantiate lamp and corresponding text label
            GameObject thisLamp = Instantiate(LAMP_MODEL, new Vector3(xCoordLamp, yCoordLamp, zCoordLamp), Quaternion.Euler(-90, 0, 180));
            thisLamp.name = "Lamp" + QWERTY[i].ToString();
            GameObject lampLabelCanvas = Instantiate(LAMP_LABEL, thisLamp.GetComponent<Transform>());
            lampLabelCanvas.GetComponentInChildren<TextMeshProUGUI>().text = QWERTY[i].ToString();

            // Instantiate plug socket and corresponding text label
            GameObject thisPlugSocket = Instantiate(PLUG_SOCKET_MODEL, new Vector3(xCoordPlugSocket, yCoordPlugSocket, zCoordPlugSocket), Quaternion.Euler(-90, 0, 180));
            thisPlugSocket.name = "PlugSocket" + QWERTY[i].ToString();
            thisPlugSocket.GetComponent<PlugboardSocket>().ENIGMA_CONTROLLER = this;
            GameObject plugSocketLabelCanvas = Instantiate(PLUG_SOCKET_LABEL, new Vector3(thisPlugSocket.GetComponent<Transform>().position.x - 0.0033f, thisPlugSocket.GetComponent<Transform>().position.y + 0.0115f, thisPlugSocket.GetComponent<Transform>().position.z), Quaternion.Euler(0, 90, 0), thisPlugSocket.GetComponent<Transform>()); 
            plugSocketLabelCanvas.GetComponentInChildren<TextMeshProUGUI>().text = QWERTY[i].ToString();

            // Instantiate key and corresponding text label
            GameObject thisKey = Instantiate(KEY_MODEL, new Vector3(xCoordKey, yCoordKey, zCoordKey), Quaternion.Euler(180, 0, 180));
            thisKey.name = "Key" + QWERTY[i].ToString();
            GameObject keyLabelCanvas = Instantiate(KEY_LABEL, new Vector3(thisKey.GetComponent<Transform>().position.x, thisKey.GetComponent<Transform>().position.y + 0.00121f, thisKey.GetComponent<Transform>().position.z), Quaternion.Euler(90, 90, 0), thisKey.GetComponent<Transform>());
            keyLabelCanvas.GetComponentInChildren<TextMeshProUGUI>().text = QWERTY[i].ToString();
        }

        // Instantiate rotor models
        rRotorObject = Instantiate(ROTOR_MODEL, new Vector3(0, 0, 0f), Quaternion.Euler(90, 0, 180));
        mRotorObject = Instantiate(ROTOR_MODEL, new Vector3(0, 0, 0.02f), Quaternion.Euler(90, 0, 180));
        lRotorObject = Instantiate(ROTOR_MODEL, new Vector3(0, 0, 0.04f), Quaternion.Euler(90, 0, 180));

        // Get a reference to each rotor's transform so we don't have to keep using getcomponent
        Transform rRotorTransform = rRotorObject.GetComponent<Transform>();
        Transform mRotorTransform = mRotorObject.GetComponent<Transform>();
        Transform lRotorTransform = lRotorObject.GetComponent<Transform>();

        // Instantiate each rotor's corresponding alphabet ring
        rRotorAlphabetRing = Instantiate(ALPHABET_TYRES[(int)rRotorNumber], new Vector3(0, 0, 0.005f), Quaternion.Euler(90, 0, 180), rRotorTransform);
        mRotorAlphabetRing = Instantiate(ALPHABET_TYRES[(int)mRotorNumber], new Vector3(0, 0, 0.025f), Quaternion.Euler(90, 0, 180), mRotorTransform);
        lRotorAlphabetRing = Instantiate(ALPHABET_TYRES[(int)lRotorNumber], new Vector3(0, 0, 0.045f), Quaternion.Euler(90, 0, 180), lRotorTransform);

        // Rotate alphabet ring according to desired start position
        rRotorAlphabetRing.GetComponent<Transform>().Rotate(new Vector3(0, 180 - (360f / 26) * (int)rRotorRingSetting, 0));
        mRotorAlphabetRing.GetComponent<Transform>().Rotate(new Vector3(0, 180 - (360f / 26) * (int)mRotorRingSetting, 0));
        lRotorAlphabetRing.GetComponent<Transform>().Rotate(new Vector3(0, 180 - (360f / 26) * (int)lRotorRingSetting, 0));

        // Rotate each rotor to desired start position
        rRotorTransform.Rotate(new Vector3(0, (360f / 26) * (int)rRotorRingSetting, 0));
        mRotorTransform.Rotate(new Vector3(0, (360f / 26) * (int)mRotorRingSetting, 0));
        lRotorTransform.Rotate(new Vector3(0, (360f / 26) * (int)lRotorRingSetting, 0));
        rRotorTransform.Rotate(new Vector3(0, -(360f / 26) * rRotorPosition, 0));
        mRotorTransform.Rotate(new Vector3(0, -(360f / 26) * mRotorPosition, 0));
        lRotorTransform.Rotate(new Vector3(0, -(360f / 26) * lRotorPosition, 0));

        // Calculate each rotor's "actual" position including ring setting offset for encryption
        rRotorPosition = mod(rRotorPosition - (int)rRotorRingSetting, 26);
        mRotorPosition = mod(mRotorPosition - (int)mRotorRingSetting, 26);
        lRotorPosition = mod(lRotorPosition - (int)lRotorRingSetting, 26);

        // Create each Rotor object with the provided settings
        Rotor lRotor = new Rotor(wiring: ROTORS[(int)lRotorNumber], turn_positions: TURNOVER_POSITIONS[(int)lRotorNumber], rotorTransform: lRotorTransform, controller: this, position: lRotorPosition, ring_offset: (int)lRotorRingSetting);
        Rotor mRotor = new Rotor(wiring: ROTORS[(int)mRotorNumber], turn_positions: TURNOVER_POSITIONS[(int)mRotorNumber], rotorTransform: mRotorTransform, controller: this, position: mRotorPosition, ring_offset: (int)mRotorRingSetting);
        Rotor rRotor = new Rotor(wiring: ROTORS[(int)rRotorNumber], turn_positions: TURNOVER_POSITIONS[(int)rRotorNumber], rotorTransform: rRotorTransform, controller: this, position: rRotorPosition, ring_offset: (int)rRotorRingSetting);
        // Create an enigma machine object
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
        // Check that the event was a keypress
        Event e = Event.current;
        if (e.isKey && e.type == EventType.KeyDown)
        {
            // Get the corresponding key's character
            char[] key_code = e.keyCode.ToString().ToCharArray();
            // Check that it is a valid key
            if (key_code.Length == 1) {
                // Convert key code to character
                char entered_char = key_code[0];
                // Get the corresponding key model and trigger its pressed animation
                GameObject thisKey = GameObject.Find("Key" + entered_char.ToString());
                thisKey.GetComponent<Animator>().SetTrigger("KeyPressed");
                // Send the entered character through the plugboard
                entered_char = (char)(plugboardConfig[entered_char - 65] + 65);
                // Send the character through the rotors to get the encrypted value
                char output_char = enigma.Encrypt(entered_char);
                // Send the character through the plugboard in reverse
                output_char = (char)(plugboardConfig[output_char - 65] + 65);
                // Convert the character to A0Z25 index
                int outputIndex = output_char - 65;
                // Unlight the last lamp
                if (lastLamp != null)
                {
                    Material[] lastTempMats = lastLamp.GetComponent<Renderer>().materials;
                    lastTempMats[0] = LAMP_UNLIT;
                    lastLamp.GetComponent<Renderer>().materials = lastTempMats;
                }
                // Get the lamp corresponding to the new output
                lastLamp = GameObject.Find("Lamp" + output_char.ToString());
                // Light the new lamp
                Material[] tempMats = lastLamp.GetComponent<Renderer>().materials;
                tempMats[0] = LAMP_LIT;
                lastLamp.GetComponent<Renderer>().materials = tempMats;
                // Increment the group length and output the character to the notes page
                groupLength += 1;
                notesText.text += output_char.ToString();
                // If the group has reached four characters, add a space
                // And reset the group counter
                if (groupLength == 5)
                {
                    notesText.text += " ";
                    groupLength = 0;
                }
                // Play the key press sound effect
                keyAudio.Play();

                // Get the new rotor positions
                int lPosition = enigma.GetApparentRotorPosition(0);
                int mPosition = enigma.GetApparentRotorPosition(1);
                int rPosition = enigma.GetApparentRotorPosition(2);

                Debug.Log(enigma.GetRotorPosition(0).ToString() + " " + enigma.GetRotorPosition(1).ToString() + " " + enigma.GetRotorPosition(2).ToString());

                // Change the rotor settings dropdown menus according to the new positions
                enableRotorChanges = false;
                dropdowns[2].value = lPosition;
                dropdowns[5].value = mPosition;
                dropdowns[8].value = rPosition;
                enableRotorChanges = true;
            }
        }
    }

    public void PlugSocketSelected(char plugChar, GameObject selectedPlugSocket)
    {
        // Check that the clicked plug does not already have a plug in it
        if (plugboardConfig[plugChar - 65] == plugChar - 65)
        {
            // If there is no plug currently selected
            if (!rootPlugSocket)
            {
                // Start a connection at this plug
                rootPlugSocket = selectedPlugSocket;
                // Instantiate a plug model in this plug socket, and one which follows the mouse
                rootPlug = Instantiate(PLUG_MODEL, rootPlugSocket.GetComponent<Transform>().position, Quaternion.Euler(new Vector3(-90, 0, 180)), rootPlugSocket.GetComponent<Transform>());
                rootPlug.name = "Plug" + plugChar.ToString();
                mousePlug = Instantiate(PLUG_MODEL, rootPlugSocket.GetComponent<Transform>().position, Quaternion.Euler(new Vector3(-90, 0, 180)));
                // Set up the line renderer used to render the wire between plugs
                LineRenderer rootPlugWireRenderer = rootPlug.AddComponent<LineRenderer>();
                rootPlugWireRenderer.startWidth = wireThickness;
                rootPlugWireRenderer.endWidth = wireThickness;
                rootPlugWireRenderer.material = PLUG_WIRE;
                rootPlugWireRenderer.positionCount = numWireSegments;
            }
            // If the user re-clicks on the root socket, remove the root plug
            else if (rootPlugSocket == selectedPlugSocket)
            {
                Destroy(GameObject.Find("Plug" + plugChar.ToString()));
                Destroy(mousePlug);
                mousePlug = null;
                rootPlugSocket = null;
            }
            // If the user is completing a connection, make that connection
            else
            {
                leafPlugSocket = selectedPlugSocket;
                // Instantiate a plug at the leaf socket
                GameObject leafPlug = Instantiate(PLUG_MODEL, leafPlugSocket.GetComponent<Transform>().position, Quaternion.Euler(new Vector3(-90, 0, 180)), leafPlugSocket.GetComponent<Transform>());
                leafPlug.name = "Plug" + plugChar.ToString();
                // Remove the plug tracking the mouse
                Destroy(mousePlug);
                mousePlug = null;
                // Call the connection method
                MakePlugboardConnection();
            }
        }
        // If the selected plug already has a connection
        else
        {
            // Replace the connection with the new one
            if (!leafPlugSocket) {
                // Remove the old plug models
                leafPlugSocket = selectedPlugSocket;
                rootPlugSocket = GameObject.Find("PlugSocket" + ((char)(plugboardConfig[plugChar - 65] + 65)).ToString());
                GameObject leafPlug = GameObject.Find("Plug" + plugChar.ToString());
                rootPlug = GameObject.Find("Plug" + ((char)(plugboardConfig[plugChar - 65] + 65)).ToString());
                LineRenderer lr = leafPlug.GetComponent<LineRenderer>();
                // Remove the old line renderer
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
                mousePlug = Instantiate(PLUG_MODEL, rootPlugSocket.GetComponent<Transform>().position, Quaternion.Euler(new Vector3(-90, 0, 180)));
                BreakPlugboardConnection();
                leafPlugSocket = null;
            }
            // If the user is cancelling a connection, destroy the associated plugs
            else
            {
                Destroy(GameObject.Find("Plug" + plugChar.ToString()));
                Destroy(mousePlug);
                mousePlug = null;
                rootPlugSocket = null;
                leafPlugSocket = null;
            }
        }
    }

    // Make a connection between two letters on the plugboard
    private void MakePlugboardConnection()
    {
        // Convert the characters of the plugs to be connected to A0Z25 indexes
        int rootIndex = rootPlugSocket.GetComponent<PlugboardSocket>().GetPlugCharacter() - 65;
        int leafIndex = leafPlugSocket.GetComponent<PlugboardSocket>().GetPlugCharacter() - 65;

        // Connect them in the plugboard array
        plugboardConfig[rootIndex] = leafIndex;
        plugboardConfig[leafIndex] = rootIndex;

        // Clear the root and leaf plug socket objects
        rootPlugSocket = null;
        leafPlugSocket = null;
    }

    // Remove a connection between two letters on the plugboard
    private void BreakPlugboardConnection()
    {
        // Convert the characters of the plugs to be connected to A0Z25 indexes
        int rootIndex = rootPlugSocket.GetComponent<PlugboardSocket>().GetPlugCharacter() - 65;
        int leafIndex = leafPlugSocket.GetComponent<PlugboardSocket>().GetPlugCharacter() - 65;

        // Connect them in the plugboard array
        plugboardConfig[rootIndex] = rootIndex;
        plugboardConfig[leafIndex] = leafIndex;
    }

    // Clear the text off the notes page
    public void ClearNotes()
    {
        notesText.text = "";
        groupLength = 0;
    }

    // Print the current enigma settings to the notes page
    public void PasteSettings()
    {
        // Rotor numbers
        string walzenlage = lRotorNumber + " " + mRotorNumber + " " + rRotorNumber;
        // Ring settings
        string ringstellung = lRotorRingSetting.ToString() + mRotorRingSetting.ToString() + rRotorRingSetting.ToString();
        // Plugboard connections
        string steckerverbindungen = "";
        // Keep track of plugboard connections that have already been recorded
        List<int> checkedPairs = new List<int>();
        // For each letter, if the letter does not connect to itself, record its connection and note that this has been recorded
        for (int i = 0; i < 26; i++)
        {
            if (!checkedPairs.Contains(i))
            {
                if (plugboardConfig[i] != i)
                {
                    checkedPairs.Add(plugboardConfig[i]);
                    steckerverbindungen += $"{(char)(i + 65)}{(char)(plugboardConfig[i] + 65)} ";
                }
            }
        }
        // Starting positions of the rotors
        string grundstellung = ((char)(enigma.GetApparentRotorPosition(0) + 65)).ToString() + ((char)(enigma.GetApparentRotorPosition(1) + 65)).ToString() + ((char)(enigma.GetApparentRotorPosition(2) + 65)).ToString();
        // If the notes page is not empty, start a new line
        if (notesText.text != "")
        {
            notesText.text += "\n";
        }
        // Print the settings
        notesText.text += $"-----------------------------\nWALZENLAGE: {walzenlage}\nRINGSTELLUNG: {ringstellung}\nSTECKERVERBINDUNGEN: {steckerverbindungen}\nGRUNDSTELLUNG: {grundstellung}\n-----------------------------\n";
        // Reset group length counter
        groupLength = 0;
    }

    // Calls the ChangeRotorSettings method with the appropriate index
    public void ChangeLRotorSetting()
    {
        ChangeRotorSetting(0);
    }

    public void ChangeMRotorSetting()
    {
        ChangeRotorSetting(1);
    }

    public void ChangeRRotorSetting()
    {
        ChangeRotorSetting(2);
    }

    // Changes the rotor model and the enigma's functionality according to the newly selected settings
    private void ChangeRotorSetting(int rotorIndex)
    {
        // Ignore dropdown menu changes if they are disabled
        if (!enableRotorChanges)
        {
            return;
        }
        // Get the new rotor settings for the appropriate rotor
        int newRotorNumber = dropdowns[rotorIndex * 3].value;
        int newRotorRingSetting = dropdowns[rotorIndex * 3 + 1].value;
        int newRotorPosition = dropdowns[rotorIndex * 3 + 2].value;
        // Declare gameobjects for the new rotor and its alphabet ring
        GameObject newRotorObject;
        GameObject newAlphabetRing;
        // For each of the possible rotors
        if (rotorIndex == 0)
        {
            // Remove the old rotor and its ring
            Destroy(lRotorObject);
            Destroy(lRotorAlphabetRing);
            // Create new objects for rotor and ring
            lRotorObject = Instantiate(ROTOR_MODEL, new Vector3(0, 0, 0.04f), Quaternion.Euler(90, 0, 180));
            lRotorAlphabetRing = Instantiate(ALPHABET_TYRES[newRotorNumber], new Vector3(0, 0, 0.045f), Quaternion.Euler(90, 0, 180), lRotorObject.GetComponent<Transform>());
            newRotorObject = lRotorObject;
            newAlphabetRing = lRotorAlphabetRing;
            lRotorNumber = (RotorNumber)newRotorNumber;
            lRotorRingSetting = (Character)newRotorRingSetting;
            lRotorPosition = newRotorPosition;
        }
        else if (rotorIndex == 1)
        {
            Destroy(mRotorObject);
            Destroy(mRotorAlphabetRing);
            mRotorObject = Instantiate(ROTOR_MODEL, new Vector3(0, 0, 0.02f), Quaternion.Euler(90, 0, 180));
            mRotorAlphabetRing = Instantiate(ALPHABET_TYRES[newRotorNumber], new Vector3(0, 0, 0.025f), Quaternion.Euler(90, 0, 180), mRotorObject.GetComponent<Transform>());
            newRotorObject = mRotorObject;
            newAlphabetRing = mRotorAlphabetRing;
            mRotorNumber = (RotorNumber)newRotorNumber;
            mRotorRingSetting = (Character)newRotorRingSetting;
            mRotorPosition = newRotorPosition;
        }
        else
        {
            Destroy(rRotorObject);
            Destroy(rRotorAlphabetRing);
            rRotorObject = Instantiate(ROTOR_MODEL, new Vector3(0, 0, 0f), Quaternion.Euler(90, 0, 180));
            rRotorAlphabetRing = Instantiate(ALPHABET_TYRES[newRotorNumber], new Vector3(0, 0, 0.005f), Quaternion.Euler(90, 0, 180), rRotorObject.GetComponent<Transform>());
            newRotorObject = rRotorObject;
            newAlphabetRing = rRotorAlphabetRing;
            rRotorNumber = (RotorNumber)newRotorNumber;
            rRotorRingSetting = (Character)newRotorRingSetting;
            rRotorPosition = newRotorPosition;
        }
        // Rotate the alphabet ring according to the chosen ringstellung
        newAlphabetRing.GetComponent<Transform>().Rotate(new Vector3(0, 180 - (360f / 26) * newRotorRingSetting, 0));
        Transform newRotorTransform = newRotorObject.GetComponent<Transform>();
        // Rotate the rotor according to the chosen settings
        newRotorTransform.Rotate(new Vector3(0, (360f / 26) * newRotorRingSetting, 0));
        newRotorTransform.Rotate(new Vector3(0, -(360f / 26) * newRotorPosition, 0));

        // Calculate the rotor's position from ringstellung
        newRotorPosition = mod(newRotorPosition - newRotorRingSetting, 26);

        // Create a new rotor object and apply it to the enigma
        Rotor newRotor = new Rotor(wiring: ROTORS[newRotorNumber], turn_positions: TURNOVER_POSITIONS[newRotorNumber], rotorTransform: newRotorObject.GetComponent<Transform>(), controller: this, position: newRotorPosition, ring_offset: newRotorRingSetting);
        enigma.SetRotor(rotorIndex, newRotor);
    }

    // Custom modulo that works for negative numbers
    public static int mod(int a, int b)
    {
        return (a % b + b) % b;
    }
}
