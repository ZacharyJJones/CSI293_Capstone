using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HexGrid;
using System.Linq;

public class MapHexConnectable : MapHex
{
  public MapHexConnectionPiece[] ConnectorPieces;

  public override void PostInstantiation(Dictionary<Hex, HexTerrainType> hexes, float delayToRise)
  {
    foreach (var connector in ConnectorPieces)
    {
      connector.GameObject.SetActive(false);

      var direction = connector.Direction;
      var adjacent = Coordinate.GetAdjacentInDirection(direction);
      if (hexes.TryGetValue(adjacent, out var terrainType))
      {
        if (terrainType == this.TerrainType || terrainType == HexTerrainType.Town)
        {
          connector.GameObject.SetActive(true);
        }
      }
    }

    base.PostInstantiation(hexes, delayToRise);
  }


  [System.Serializable]
  public class MapHexConnectionPiece
  {
    public Direction Direction;
    public GameObject GameObject;
  }
}
