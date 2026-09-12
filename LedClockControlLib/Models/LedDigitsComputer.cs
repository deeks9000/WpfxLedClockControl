namespace LedClockControlLib.Models;

internal static class LedDigitsComputer
{
    // 5 x 9 seven-segment layout:
    //
    //  AAA
    // F   B
    // F   B
    // F   B
    //  GGG
    // E   C
    // E   C
    // E   C
    //  DDD

    private static readonly byte[] DigitMasks = [
        0b0111111, // 0 = ABCDEF
        0b0000110, // 1 = BC
        0b1011011, // 2 = ABDEG
        0b1001111, // 3 = ABCDG
        0b1100110, // 4 = BCFG
        0b1101101, // 5 = ACDFG
        0b1111101, // 6 = ACDEFG
        0b0000111, // 7 = ABC
        0b1111111, // 8 = ABCDEFG
        0b1101111  // 9 = ABCDFG
    ];

    public static void UpdateLedDigits(DateTime timestamp, byte[] ledDigits)
    {
        ledDigits[5] = DigitMasks[timestamp.Hour / 10];
        ledDigits[4] = DigitMasks[timestamp.Hour % 10];
        ledDigits[3] = DigitMasks[timestamp.Minute / 10];
        ledDigits[2] = DigitMasks[timestamp.Minute % 10];
        ledDigits[1] = DigitMasks[timestamp.Second / 10];
        ledDigits[0] = DigitMasks[timestamp.Second % 10];
    }
}
