using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Linq;

public class TestEnigmaFunctionality
{
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

    [Test]
    public void TestRotorIsolatedBehaviour()
    {
        // Test that rotors are producing expected output when used individually
        // For both out and back signals
        for (int i = 0; i < ROTORS.Length; i++)
        {
            // For each wiring configuration, set up a test rotor
            Rotor testRotor = new Rotor(ROTORS[i], TURNOVER_POSITIONS[i], null);
            for (int j = 0; j < 26; j++)
            {
                // For each letter, test that it encrypts to the expected letter
                // For both out and back
                Assert.AreEqual(ROTORS[i][j], testRotor.GetOutput(j, true));
                Assert.AreEqual(Array.IndexOf(ROTORS[i], j), testRotor.GetOutput(j, false));
            }
        }
    }

    [Test]
    public void TestStandardSteppingBehaviour()
    {
        // Test that the machine encrypts as expected with no turnovers and no plugboard
        // Set up enigma with rotors I, II and III in state AAA
        Rotor lTestRotor = new Rotor(ROTORS[0], TURNOVER_POSITIONS[0], null);
        Rotor mTestRotor = new Rotor(ROTORS[1], TURNOVER_POSITIONS[1], null);
        Rotor rTestRotor = new Rotor(ROTORS[2], TURNOVER_POSITIONS[2], null);
        EnigmaM3 testEnigma = new EnigmaM3(lTestRotor, mTestRotor, rTestRotor, REFLECTORS[0], ENTRY_WHEELS[0], ENTRY_WHEELS[0]);
        char[] plainText = { 'H', 'E', 'L', 'L', 'O'};
        char[] cipherText = { 'I', 'L', 'B', 'D', 'A'}; // "HELLO" encrypted on these settings
        // Test that "HELLO" encrypts to "ILBDA"
        for (int i=0; i < 5; i++){
            Assert.AreEqual(cipherText[i], testEnigma.Encrypt(plainText[i]));
        }
    }

    [Test]
    public void TestZToA()
    {
        //Test that behaviour is as expected when the right rotor steps from Z back to A
        //Set up enigma with rotors I, II and III in state AAY
        Rotor lTestRotor = new Rotor(ROTORS[0], TURNOVER_POSITIONS[0], null);
        Rotor mTestRotor = new Rotor(ROTORS[1], TURNOVER_POSITIONS[1], null);
        Rotor rTestRotor = new Rotor(ROTORS[2], TURNOVER_POSITIONS[2], null, 24);
        EnigmaM3 testEnigma = new EnigmaM3(lTestRotor, mTestRotor, rTestRotor, REFLECTORS[0], ENTRY_WHEELS[0], ENTRY_WHEELS[0]);
        char[] plainText = { 'H', 'E', 'L', 'L', 'O' };
        char[] cipherText = { 'R', 'B', 'P', 'E', 'N' }; // "HELLO" encrypted on these settings
        // Test that "HELLO" encrypts to "RBPEN"
        for (int i = 0; i < 5; i++)
        {
            Assert.AreEqual(cipherText[i], testEnigma.Encrypt(plainText[i]));
        }
        // Test that the right rotor finishes in position 'D'
        Assert.AreEqual(testEnigma.GetRotorPosition(2), 3);
    }

    [Test]
    public void TestmRotorTurnover()
    {
        // Test that the machine encrypts as expected when the center rotor has a turnover
        // Set up enigma with rotors I, II and III in state AAT
        Rotor lTestRotor = new Rotor(ROTORS[0], TURNOVER_POSITIONS[0], null);
        Rotor mTestRotor = new Rotor(ROTORS[1], TURNOVER_POSITIONS[1], null);
        Rotor rTestRotor = new Rotor(ROTORS[2], TURNOVER_POSITIONS[2], null, 19); // Set right rotor to position T which will cause a turnover of center rotor
        EnigmaM3 testEnigma = new EnigmaM3(lTestRotor, mTestRotor, rTestRotor, REFLECTORS[0], ENTRY_WHEELS[0], ENTRY_WHEELS[0]);
        Assert.AreEqual(testEnigma.GetRotorPosition(1), 0); // Test that the middle rotor starts in the "A" position
        char[] plainText = { 'H', 'E', 'L', 'L', 'O' };
        char[] cipherText = { 'Q', 'Y', 'B', 'S', 'A' }; // "HELLO" encrypted on these settings
        // Test that "HELLO" encrypts to "QYBSA"
        for (int i = 0; i < 5; i++)
        {
            Assert.AreEqual(cipherText[i], testEnigma.Encrypt(plainText[i]));
            Assert.AreEqual(testEnigma.GetRotorPosition(1), i >= 2 ? 1 : 0); // Check that the center rotor turns over at the correct time, while encrypting the first "L", and not before
        }
    }

    [Test]
    public void TestlRotorTurnover()
    {
        // Test that the machine encrypts as expected when the left rotor has a turnover
        // Note that this should also cause "double stepping" of the middle rotor
        // Which we will also check for in this test
        // In this test there will be a turnover of the middle rotor to position "E"
        // Which will then cause a turnover of the left rotor to position "B" on the next encryption
        // This will cause a double step of the middle rotor
        // Set up enigma with rotors I, II and III in state ADT
        Rotor lTestRotor = new Rotor(ROTORS[0], TURNOVER_POSITIONS[0], null);
        Rotor mTestRotor = new Rotor(ROTORS[1], TURNOVER_POSITIONS[1], null, 3);
        Rotor rTestRotor = new Rotor(ROTORS[2], TURNOVER_POSITIONS[2], null, 19);
        EnigmaM3 testEnigma = new EnigmaM3(lTestRotor, mTestRotor, rTestRotor, REFLECTORS[0], ENTRY_WHEELS[0], ENTRY_WHEELS[0]);
        Assert.AreEqual(testEnigma.GetRotorPosition(0), 0); // Test that left rotor starts in the "A" position
        Assert.AreEqual(testEnigma.GetRotorPosition(1), 3); // Test that middle rotor starts in the "D" position
        char[] plainText = { 'H', 'E', 'L', 'L', 'O' };
        char[] cipherText = { 'B', 'A', 'H', 'X', 'V' }; // "HELLO" encrypted on these settings
        for (int i = 0; i < 5; i++)
        {
            Assert.AreEqual(cipherText[i], testEnigma.Encrypt(plainText[i]));
            if (i < 2)
            {
                Assert.AreEqual(testEnigma.GetRotorPosition(1), 3); // Until entry of the first "L" the middle rotor should remain in position "D"
            }
            if (i == 2)
            {
                Assert.AreEqual(testEnigma.GetRotorPosition(1), 4); // After entry of the first "L" the middle rotor should turnover to position "E"
            }
            if (i > 2)
            {
                Assert.AreEqual(testEnigma.GetRotorPosition(0), 1); // Entry of the second "L" should cause the left rotor to turnover
                Assert.AreEqual(testEnigma.GetRotorPosition(1), 5); // It should also cause an additional turnover of the middle rotor which should know be in position "F"
            }
            else
            {
                Assert.AreEqual(testEnigma.GetRotorPosition(0), 0); // Until the second "L" the left rotor should remain in position "A"
            }
        }
    }
}
