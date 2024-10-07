'''
A Python prototype for simulating the Enigma machine.

Author: Zak Mitchell

Last Modified: 07/10/24
'''

import string

class Rotor:
    '''
    A class representing one of the rotors used in Enigma.

    Attributes
    ----------
    wiring : int list
        Represents the wiring inside the rotor. Each index contains the output
        index obtained by sending an input signal to this index.
    position : int
        How offset the rotor is from the start position (A). 0 represents the
        start position, 1 represents position B, etc.
    ring_offset : int
        How offset the the alphabet ring is from the rotor wiring. 0 means that
        the "A" on the alphabet ring is aligned with rotor input "A", 1 means
        "A" is aligned with "B" etc.
    turn_positions : int
        The position at which, if the rotor advances, its notch will be
        positioned such that its left neighbour also advances.

    Methods
    -------
    get_output(input_index)
        Returns the output obtained from an input signal at the given index.
    '''

    def __init__(self, wiring, turn_positions, position=0, ring_offset=0):
        '''
        Constructor for the Rotor class.

        Parameters
        ----------
        wiring : int list
            The wiring configuration of the rotor.
        turn_positions : int list
            The position(s) at which a rotor will step its neighbour if it
            steps.
        position : int (optional)
            Offset of the rotor from starting position (default is 0).
        ring_offset : int (optional)
            Offset of alphabet ring from rotor (default is 0).
        '''
        self.wiring = wiring
        self.position = position
        self.ring_offset = ring_offset
        self.turn_positions = turn_positions

    def get_output(self, input_index, out):
        '''
        Returns the output obtained from an input signal at the given index.

        Parameters
        ----------
        input_index : int
            The index of the input signal (0=A, 1=B etc.)
        out : bool
            Whether the signal is going out to or back from the reflector.

        Returns
        -------
        output_index : int
            The index of the output signal (0=A, 1=B etc.)
        '''
        # Calculate which route the signal is entering based on rotor position
        idx = (input_index + self.position) % 26
        output_index = None
        # Calculate the output of the signal for given input
        if out:
            output_index = self.wiring[idx]
        else:
            output_index = self.wiring.index(idx)
        output_index -= self.position
        return output_index

    def advance(self):
        '''
        Steps the rotor forwards one position.

        Parameters
        ----------
        None

        Returns
        -------
        turn_adjacent : bool
            Whether or not this step should also advance the rotor's neighbour.
        '''
        # Calculate whether the rotor notch is in the correct position
        # To advance its neighbour
        turn_adjacent = False
        if (self.position - self.ring_offset) in self.turn_positions:
            turn_adjacent = True
        # Increment the rotor's position
        self.position = (self.position + 1) % 26
        # Return whether or not the rotor should step its neighbour
        return turn_adjacent

class EnigmaM3:
    def __init__(self, lRotor, mRotor, rRotor, reflector, entry_wheel):
        self.lRotor = lRotor
        self.mRotor = mRotor
        self.rRotor = rRotor
        self.reflector = reflector
        self.entry_wheel = entry_wheel

    def encrypt(self, input_letter, verbose=False):

        input_letter = input_letter.upper()
        if verbose: print("Keyboard input:",input_letter)

        mRotorAdvance = self.rRotor.advance()
        if mRotorAdvance:
            lRotorAdvance = self.mRotor.advance()
            if lRotorAdvance:
                self.mRotor.advance()
                self.lRotor.advance()

        if verbose: print("Rotor position: {0}{1}{2}".format(list(string.ascii_uppercase)[lRotor.position], list(string.ascii_uppercase)[mRotor.position], list(string.ascii_uppercase)[rRotor.position]))

        input_index = list(string.ascii_uppercase).index(input_letter)

        e_out = self.entry_wheel[input_index]
        r_out = self.rRotor.get_output(e_out, True)
        if verbose: print("Right wheel encryption:",list(string.ascii_uppercase)[r_out])
        m_out = self.mRotor.get_output(r_out, True)
        if verbose: print("Middle wheel encryption:",list(string.ascii_uppercase)[m_out])
        l_out = self.lRotor.get_output(m_out, True)
        if verbose: print("Left wheel encryption:",list(string.ascii_uppercase)[l_out])
        reflector_out = self.reflector[l_out]
        if verbose: print("Reflector encryption:",list(string.ascii_uppercase)[reflector_out])
        l_back = self.lRotor.get_output(reflector_out, False)
        if verbose: print("Left wheel encryption:",list(string.ascii_uppercase)[l_back])
        m_back = self.mRotor.get_output(l_back, False)
        if verbose: print("Middle wheel encryption:",list(string.ascii_uppercase)[m_back])
        r_back = self.rRotor.get_output(m_back, False)
        if verbose: print("Right wheel encryption:",list(string.ascii_uppercase)[r_back])
        e_back = self.entry_wheel.index(r_back)

        return list(string.ascii_uppercase)[e_back]

# Wiring configurations for rotors I-VIII
ROTOR_WIRINGS = [
[ 4, 10, 12,  5, 11,  6,  3, 16, 21, 25, 13, 19, 14, 22, 24,  7, 23, 20, 18, 15,  0,  8,  1, 17,  2,  9],
[ 0,  9,  3, 10, 18,  8, 17, 20, 23,  1, 11,  7, 22, 19, 12,  2, 16,  6, 25, 13, 15, 24,  5, 21, 14,  4],
[ 1,  3,  5,  7,  9, 11,  2, 15, 17, 19, 23, 21, 25, 13, 24,  4,  8, 22,  6,  0, 10, 12, 20, 18, 16, 14],
[ 4, 18, 14, 21, 15, 25,  9,  0, 24, 16, 20,  8, 17,  7, 23, 11, 13,  5, 19,  6, 10,  3,  2, 12, 22,  1],
[21, 25,  1, 17,  6,  8, 19, 24, 20, 15, 18,  3, 13,  7, 11, 23,  0, 22, 12,  9, 16, 14,  5,  4,  2, 10],
[ 9, 15,  6, 21, 14, 20, 12,  5, 24, 16,  1,  4, 13,  7, 25, 17,  3, 10,  0, 18, 23, 11,  8,  2, 19, 22],
[13, 25,  9,  7,  6, 17,  2, 23, 12, 24, 18, 22,  1, 14, 20,  5,  0,  8, 21, 11, 15,  4, 10, 16,  3, 19],
[ 5, 10, 16,  7, 19, 11, 23, 14,  2,  1,  9, 18, 15,  3, 25, 17,  0, 12,  4, 22, 13,  8, 20, 24,  6, 21]
]

# Initialise rotor objects
rotorI    = Rotor(ROTOR_WIRINGS[0], [16])
rotorII   = Rotor(ROTOR_WIRINGS[1], [4])
rotorIII  = Rotor(ROTOR_WIRINGS[2], [21])
rotorIV   = Rotor(ROTOR_WIRINGS[3], [9])
rotorV    = Rotor(ROTOR_WIRINGS[4], [25])
rotorVI   = Rotor(ROTOR_WIRINGS[5], [25, 12])
rotorVII  = Rotor(ROTOR_WIRINGS[6], [25, 12])
rotorVIII = Rotor(ROTOR_WIRINGS[7], [25, 12])

entry_wheel = [10, 23, 21, 12, 2, 13, 14, 15, 7, 16, 17, 18, 25, 24, 8, 9, 0, 3, 11, 4, 6, 22, 1, 20, 5, 19]
sample_entry_wheel = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25]
reflectorB = [24, 17, 20, 7, 16, 18, 11, 3, 15, 23, 13, 6, 14, 10, 12, 8, 4, 1, 5, 25, 2, 22, 21, 9, 0, 19]
reflectorC = [5, 21, 15, 9, 8, 0, 14, 24, 4, 3, 17, 25, 23, 22, 6, 2, 19, 10, 20, 16, 18, 1, 13, 12, 7, 11]

rotors = [rotorI, rotorII, rotorIII, rotorIV, rotorV, rotorVI, rotorVII, rotorVIII]

NUMERALS = ["I", "II", "III", "IV", "V", "VI", "VII", "VIII"]

if __name__ == '__main__':
    print("---ENIGMA SIMULATOR---")

    rotor_indexes = input("Please enter three rotor numbers (1-8) separated by a space: ").split(" ")
    for i in range(len(rotor_indexes)):
        rotor_indexes[i] = int(rotor_indexes[i]) - 1
    lRotor = rotors[rotor_indexes[0]]
    mRotor = rotors[rotor_indexes[1]]
    rRotor = rotors[rotor_indexes[2]]
    print("Continuing with rotor configuration: {0} {1} {2}".format(NUMERALS[rotor_indexes[0]], NUMERALS[rotor_indexes[1]], NUMERALS[rotor_indexes[2]]))

    starting_positions = input("Please enter starting positions in the form of letters A-Z separated by a space: ").upper().split(" ")
    lRotor.position = list(string.ascii_uppercase).index(starting_positions[0])
    mRotor.position = list(string.ascii_uppercase).index(starting_positions[1])
    rRotor.position = list(string.ascii_uppercase).index(starting_positions[2])

    reflector_choice = input("Please select a reflector (B or C): ").upper()
    reflector = reflectorB if reflector_choice == "B" else reflectorC

    enigma = EnigmaM3(lRotor, mRotor, rRotor, reflector, sample_entry_wheel)

    while True:
        user_input = input("in > ").upper()
        print("out > {0}".format(enigma.encrypt(user_input, verbose=True)))
