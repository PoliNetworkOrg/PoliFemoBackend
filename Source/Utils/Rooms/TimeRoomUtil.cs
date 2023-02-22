namespace PoliFemoBackend.Source.Utils.Rooms;

public class TimeRoomUtil
{
    internal static int GetShiftSlotFromTime(DateTime time)
    {
        var shiftSlot = (time.Hour - 8) * 4;
        shiftSlot += time.Minute / 15;
        return shiftSlot;
    }

    internal static TimeOnly GetTimeFromShiftSlot(int shiftSlot)
    {
        var hour = shiftSlot / 4 + 8;
        var minute = shiftSlot % 4 * 15;
        return new TimeOnly(hour, minute, 0);
    }
}