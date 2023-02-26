using PoliFemoBackend.Source.Enums;

namespace PoliFemoBackend.Source.Objects.Rooms;

public class RoomOccupancyResultObject
{
    public readonly TimeOnly TimeOnly;
    public readonly RoomOccupancyEnum RoomOccupancyEnum;
    internal readonly string? text;

    public RoomOccupancyResultObject(TimeOnly timeOnly, RoomOccupancyEnum roomOccupancyEnum, string? text)
    {
        TimeOnly = timeOnly;
        RoomOccupancyEnum = roomOccupancyEnum;
        this.text = text;
    }
}