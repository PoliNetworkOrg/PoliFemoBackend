namespace PoliFemoBackend.Source.Utils.Rooms;

public static class TimeRoomUtil
{
    internal static int GetShiftSlotFromTime(DateTime time)
    {
        var shiftSlot = (time.Hour - 8) * 4;
        shiftSlot += time.Minute / 15;
        return shiftSlot;
    }
}