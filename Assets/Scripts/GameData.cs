using System.Collections;
using System.Collections.Generic;
using HexGrid;
using UnityEngine;
using System.Linq;

public class GameData
{
  public Dictionary<Hex, HexTerrainType> HexMap
  {
    get
    {
      return Data.ToDictionary(x => x.Coordinate, y => y.Terrain);
    }
    set
    {
      Data = new List<HexTileWithTerrain>();

      foreach (var pair in value)
      {
        Data.Add(new HexTileWithTerrain()
        {
          Coordinate = pair.Key,
          Terrain = pair.Value
        });
      }
    }
  }


  public List<HexTileWithTerrain> Data;



  [System.Serializable]
  public class HexTileWithTerrain
  {
    public Hex Coordinate;
    public HexTerrainType Terrain;
  }
}
