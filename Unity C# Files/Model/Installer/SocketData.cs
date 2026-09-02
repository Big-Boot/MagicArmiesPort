using System;
using UnityEngine;
using static RelicModel;

[Serializable]
public class SocketData
{
    public int row;
    public int column;
    public PlacementRestriction placementRestriction;

    public SocketData(int row, int column, PlacementRestriction placementRestriction)
    {
        this.row = row;
        this.column = column;
        this.placementRestriction = placementRestriction;
    }

    public Vector2 GetPosition()
    {
        return new Vector2(row, column);
    }
}
