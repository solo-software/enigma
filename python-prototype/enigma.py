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
        None
        '''
        # Increment the rotor's position
        self.position += 1
        # Calculate whether the rotor notch is in the correct position
        # To advance its neighbour
        if (self.position - 1) - self.ring_offset == self.turn_position:
            return True
        return False
