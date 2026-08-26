namespace WpfxCustomControls;

internal sealed class SegmentModel
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

    private static readonly string[] Decoder = [
        "ABCDEF",   // 0
        "BC",       // 1
        "ABDEG",    // 2
        "ABCDG",    // 3
        "BCFG",     // 4
        "ACDFG",    // 5
        "ACDEFG",   // 6
        "ABC",      // 7
        "ABCDEFG",  // 8
        "ABCDFG"    // 9
    ];

    public SegmentModel(DigitUnit unit, LedSegment segment)
    {
        Unit = unit;
        Segment = segment;
    }

    private DigitUnit Unit { get; }

    private LedSegment Segment { get; }

    public bool IsSegmentLit(DateTime timestamp)
    {
        int digit = Unit switch
        {
            DigitUnit.OneSeconds => timestamp.Second % 10,
            DigitUnit.TenSeconds => timestamp.Second / 10,

            DigitUnit.OneMinutes => timestamp.Minute % 10,
            DigitUnit.TenMinutes => timestamp.Minute / 10,

            DigitUnit.OneHours => timestamp.Hour % 10,
            DigitUnit.TenHours => timestamp.Hour / 10,

            _ => throw new ArgumentOutOfRangeException(nameof(Unit), Unit, null)
        };

        return Decoder[digit].Contains(Segment.ToString());
    }

    public bool IsSeconds()
    {
        return Unit is DigitUnit.TenSeconds or DigitUnit.OneSeconds;
    }
}