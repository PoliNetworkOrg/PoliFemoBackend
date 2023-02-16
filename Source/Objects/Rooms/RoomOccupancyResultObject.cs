using PoliFemoBackend.Source.Enums;

namespace PoliFemoBackend.Source.Objects.Rooms;

public class RoomOccupancyResultObject
{
    public readonly TimeOnly _timeOnly;
    public readonly bool inScopeSearch;
    public readonly RoomOccupancyEnum RoomOccupancyEnum;

    public RoomOccupancyResultObject(TimeOnly timeOnly, RoomOccupancyEnum roomOccupancyEnum, bool inScopeSearch)
    {
        _timeOnly = timeOnly;
        RoomOccupancyEnum = roomOccupancyEnum;
        this.inScopeSearch = inScopeSearch;
    }
}