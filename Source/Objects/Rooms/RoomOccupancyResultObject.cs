using PoliFemoBackend.Source.Enums;

namespace PoliFemoBackend.Source.Objects.Rooms;

public class RoomOccupancyResultObject
{
    public readonly TimeOnly _timeOnly;
    public readonly RoomOccupancyEnum RoomOccupancyEnum;
    public readonly bool inScopeSearch;

    public RoomOccupancyResultObject(TimeOnly timeOnly, RoomOccupancyEnum roomOccupancyEnum, bool inScopeSearch)
    {
        this._timeOnly = timeOnly;
        this.RoomOccupancyEnum = roomOccupancyEnum;
        this.inScopeSearch = inScopeSearch;
    }
}