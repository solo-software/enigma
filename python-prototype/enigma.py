'''
A Python prototype for simulating the Enigma machine.

Author: Zak Mitchell

Last Modified: 25/09/24
'''

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
    turn_position : int
        The position at which, if the rotor advances, its notch will be
        positioned such that its left neighbour also advances.

    Methods
    -------
    get_output(input_index)
        Returns the output obtained from an input signal at the given index.
    '''

    def __init__(self, wiring, turn_position, position=0, ring_offset=0):
        '''
        Constructor for the Rotor class.

        Parameters
        ----------
        wiring : int list
            The wiring configuration of the rotor.
        turn_position : int
            The position at which a rotor will step its neighbour if it steps.
        position : int (optional)
            Offset of the rotor from starting position (default is 0).
        ring_offset : int (optional)
            Offset of alphabet ring from rotor (default is 0).
        '''
        self.wiring = wiring
        self.position = position
        self.ring_offset = ring_offset
        self.turn_position = turn_position

    def get_output(self, input_index):
        '''
        Returns the output obtained from an input signal at the given index.

        Parameters
        ----------
        input_index : int
            The index of the input signal (0=A, 1=B etc.)

        Returns
        -------
        output_index : int
            The index of the output signal (0=A, 1=B etc.)
        '''
        # Calculate which route the signal is entering based on rotor position
        idx = (input_index + self.position) % 26
        # Calculate the output of the signal for given input
        output_index = self.wiring[idx]
        return output_index

    def advance():
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
        # Increment the rotor's position
        self.position += 1
        # Calculate whether the rotor notch is in the correct position
        # To advance its neighbour
        if (self.position - 1) - self.ring_offset == self.turn_position:
            return True
        return False

# Wiring configurations for rotors I-VIII
ROTOR_WIRINGS = [
[ 4, 10, 12,  5, 11,  6,  3, 16, 21, 25, 13, 19, 14, 22, 24,  7, 23, 20, 18, 15,  0,  8,  1, 17,  2,  9],
[ 0,  9,  3, 10, 18,  8, 17, 20, 23,  1, 11,  7, 22, 19, 12,  2, 16,  6, 25, 13, 15, 24,  5, 21, 14,  4],
[ 1,  3,  5,  7,  8, 11,  2, 15, 17, 19, 23, 21, 25, 13, 24,  4,  8, 22,  6,  0, 10, 12, 20, 18, 16, 14],
[ 4, 18, 14, 21, 15, 25,  9,  0, 24, 16, 20,  8, 17,  7, 23, 11, 13,  5, 19,  6, 10,  3,  2, 12, 22,  1],
[21, 25,  1, 17,  6,  8, 19, 24, 20, 15, 18,  3, 13,  7, 11, 23,  0, 22, 12,  9, 16, 14,  5,  4,  2, 10],
[ 9, 15,  6, 21, 14, 20, 12,  5, 24, 16,  1,  4, 13,  7, 25, 17,  3, 10,  0, 18, 23, 11,  8,  2, 19, 22],
[13, 25,  9,  7,  6, 17,  2, 23, 12, 24, 18, 22,  1, 14, 20,  5,  0,  8, 21, 11, 15,  4, 10, 16,  3, 19],
[ 5, 10, 16,  7, 19, 11, 23, 14,  2,  1,  9, 18, 15,  3, 25, 17,  0, 12,  4, 22, 13,  8, 20, 24,  6, 21]
]
