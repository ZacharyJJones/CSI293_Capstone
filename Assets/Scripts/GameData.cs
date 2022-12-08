using System.Collections;
using System.Collections.Generic;
using HexGrid;
using UnityEngine;
using System.Linq;
using System;

public class GameData
{
  public Dictionary<Hex, HexTerrainType> HexMap
  {
    get
    {
      return Data.ToDictionary(x => new Hex(x.xCoord, x.yCoord), y => y.Terrain);
    }
    set
    {
      Data = new List<HexTileWithTerrain>();

      foreach (var pair in value)
      {
        Data.Add(new HexTileWithTerrain()
        {
          xCoord = pair.Key.X,
          yCoord = pair.Key.Y,
          Terrain = pair.Value
        });
      }
    }
  }


  public List<HexTileWithTerrain> Data;



  [System.Serializable]
  public class HexTileWithTerrain
  {
    public int xCoord;
    public int yCoord;
    public HexTerrainType Terrain;
  }
}
