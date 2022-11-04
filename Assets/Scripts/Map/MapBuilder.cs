using System.Collections.Generic;
using UnityEngine;
using HexGrid;
using System.Linq;

public class MapBuilder : MonoBehaviour
{
  public GameObject MapRoot;

  public MapHexPrefabsListScriptable MapPrefabs;

  public Dictionary<HexTerrainType, HexTile> MapPrefabsByTerrainType;

  // =====

  private void Awake()
  {
    MapPrefabsByTerrainType = MapPrefabs.HexTiles.ToDictionary(x => x.Type, y => y);
  }

  private void Start()
  {
    int size = 20;
    var hexes = Hex.Zero.HexesInRadius(size);

    var hexMap = hexes.ToDictionary(x => x, y => MapPrefabs.GetRandom().Type);

    // Mountain Ring(s)
    int numRings = 1 + size / 5;
    for (int i = 1; i <= numRings; i++)
    {
      var ring = Hex.Zero.HexesAtRadius(size + i);
      foreach (var hex in ring)
      {
        hexMap.Add(hex, HexTerrainType.Mountain);
      }
    }

    BuildFeatures(hexMap, size);
    BuildMap(hexMap, size + numRings);
  }


  private void BuildFeatures(Dictionary<Hex, HexTerrainType> hexes, int mapSize)
  {
    int mapHalf = mapSize / 2;
    int numRoads = 1 + mapHalf;
    int numRivers = mapHalf;

    Hex zeroHex = Hex.Zero;


    for (int i = 0; i < mapHalf; i++)
    {
      int ring = mapSize - i;
      var ringHexes = zeroHex.HexesAtRadius(ring);

      int range = ringHexes.Count;
      int random1 = Random.Range(0, range);
      int random2 = Utils.GetRandomNoRepeat(range, random1);

      var riverLine = ringHexes[random1].GetLineToPoint(ringHexes[random2]);
      foreach (var hex in riverLine)
      {
        hexes[hex] = HexTerrainType.River;
      }
    }


    hexes[zeroHex] = HexTerrainType.Town;
    for (int i = 2; i <= mapSize; i += 2)
    {
      var ring = zeroHex.HexesAtRadius(i);
      var rand = ring[Random.Range(0, ring.Count)];

      hexes[rand] = HexTerrainType.Town;
    }

    var townHexes = hexes.Where(x => x.Value == HexTerrainType.Town).Select(x => x.Key).ToList();
    for (int i = 0; i < townHexes.Count; i++)
    {
      int rand = Utils.GetRandomNoRepeat(townHexes.Count, i);

      var roadLine = townHexes[i].GetLineToPoint(townHexes[rand]);
      for (int j = 1; j < roadLine.Count - 1; j++)
      {
        hexes[roadLine[j]] = HexTerrainType.Road;
      }
    }
  }

  private void BuildMap(Dictionary<Hex, HexTerrainType> hexes, int maxDist)
  {
    float waitTime = 0.5f;
    float buildingWaitTime = 0f;

    var flood = Hex.Zero.FloodFill(maxDist);
    foreach (var subList in flood)
    {
      // raise each from this list, then increase wait time

      foreach (var hex in subList)
      {
        var type = hexes[hex];

        // var mapHex = Instantiate(_hexPrefabsByTerrainType[terrainType], this.transform);
        var instantiatedObject = Instantiate(MapPrefabsByTerrainType[type].Prefab, this.transform);

        if (MapPrefabsByTerrainType[type].IsRotatable)
        {
          instantiatedObject.transform.Rotate(Vector3.up, Random.Range(0f, 360f));
        }

        var mapHex = instantiatedObject.GetComponent<MapHex>();

        mapHex.Coordinate = hex;
        mapHex.gameObject.name += $" ({hex.X}, {hex.Y})";
        mapHex.PostInstantiation(hexes, buildingWaitTime);
      }


      buildingWaitTime += waitTime;
    }
  }
}
