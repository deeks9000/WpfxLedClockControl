namespace WpfxCustomControls;

internal sealed class SegmentModel
{
    public SegmentModel(DigitUnit unit, LedSegment segment)
    {
        Unit = unit;
        SegmentMask = GetSegmentBit(segment);
    }

    public DigitUnit Unit { get; }

    public byte SegmentMask { get; }

    private byte GetSegmentBit(LedSegment segment) => segment switch
    {
        LedSegment.A => 1 << 0,
        LedSegment.B => 1 << 1,
        LedSegment.C => 1 << 2,
        LedSegment.D => 1 << 3,
        LedSegment.E => 1 << 4,
        LedSegment.F => 1 << 5,
        LedSegment.G => 1 << 6,
        _ => throw new ArgumentOutOfRangeException(nameof(segment))
    };

    public bool IsSeconds()
    {
        return Unit is DigitUnit.TenSeconds or DigitUnit.OneSeconds;
    }
}