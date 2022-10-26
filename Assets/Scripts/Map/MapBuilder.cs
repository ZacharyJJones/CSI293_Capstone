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
    int size = 4;

    // var hexes = Hex.Zero.FloodFill(4).Select(x => (x.Key, MapPrefabs.GetRandom().Type)).ToList();
    var hexes = Hex.Zero.FloodFill(size).ToDictionary(x => x.Key, y => MapPrefabs.GetRandom().Type);
    BuildConnectables(hexes, size);
    // CleanMap(hexes);


    // Mountain Ring
    var ring = Hex.Zero.HexesAtDistance(size + 1);
    foreach (var hex in ring)
    {
      hexes.Add(hex, HexTerrainType.Mountain);
    }

    // Create GameObjects
    BuildMap(hexes);
  }

  private void BuildConnectables(Dictionary<Hex, HexTerrainType> hexes, int mapSize)
  {
    int mapHalf = mapSize / 2;
    int numRoads = 1 + mapHalf;
    int numRivers = mapHalf;

    Hex zeroHex = Hex.Zero;


    for (int i = 0; i < mapHalf; i++)
    {
      int ring = mapSize - i;
      var ringHexes = zeroHex.HexesAtDistance(ring);

      int range = ringHexes.Count;
      int random1 = Random.Range(0, range);
      int random2 = Utils.GetRandomNoRepeat(range, random1);

      var riverLine = ringHexes[random1].GetLineToPoint(ringHexes[random2]);
      foreach (var hex in riverLine)
      {
        hexes[hex] = HexTerrainType.River;
      }
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

    // roads between all towns - each town picks another town to build a road to.

  }
  private void _buildConnectable(Dictionary<Hex, HexTerrainType> hexes, HexTerrainType type, int length)
  {
    // var mapHexes = hexes.Keys.ToList();

    // var start = mapHexes[Random.Range(0, mapHexes.Count)];
    // var excludedHexes = new List<Hex>() { start };

    // hexes[start] = type;
    // var adjacents = start.GetAdjacents();
    // int random1 = Random.Range(0, 6);
    // int random2 = Utils.GetRandomNoRepeat(6, random1);

    // hexes[adjacents[random1]] = type;
    // hexes[adjacents[random2]] = type;


    // var currentHex = start;
    // for (int i = 0; i < length; i++)
    // {
    //   hexes[currentHex] = type;

    //   Debug.Log(string.Join(", ", excludedHexes.Select(x => x.ToString())));
    //   Debug.Log(string.Join(", ", currentHex.GetAdjacents().Select(x => x.ToString())));

    //   var adjacents = currentHex.GetAdjacents();
    //   var inMapAndValidAdjacents = adjacents.Where(x => hexes.ContainsKey(x) && !excludedHexes.Contains(x)).ToList();

    //   if (inMapAndValidAdjacents.Count == 0)
    //   {
    //     break;
    //   }

    //   var next = inMapAndValidAdjacents[Random.Range(0, inMapAndValidAdjacents.Count)];
    //   excludedHexes.AddRange(inMapAndValidAdjacents);
    // }
  }

  private void CleanMap(Dictionary<Hex, HexTerrainType> hexes)
  {


    // Road across the X plane
    // var line = new Hex(-mapSize, 0).GetLineToPoint(new Hex(mapSize, 0));
    // foreach (var hex in line)
    // {
    //   if (hexes.ContainsKey(hex))
    //   {
    //     hexes[hex] = HexTerrainType.Road;
    //   }
    // }

    // Town in center
    // if (hexes.ContainsKey(Hex.Zero))
    // {
    //   hexes[Hex.Zero] = HexTerrainType.Town;
    // }

    // // No orphan roads
    // var hexMapList = hexes.Keys.ToList();
    // foreach (var key in hexMapList)
    // {
    //   var value = hexes[key];

    //   // only want to check roads
    //   if (value != HexTerrainType.Road)
    //   {
    //     continue;
    //   }

    //   // check this road's adjacent tiles
    //   bool hasRoadAdjacent = false;
    //   var adjacentHexes = key.GetAdjacents();
    //   foreach (var hex in adjacentHexes)
    //   {
    //     if (hexes.ContainsKey(hex) && hexes[hex] == HexTerrainType.Road)
    //     {
    //       hasRoadAdjacent = true;
    //       break;
    //     }
    //   }

    //   if (!hasRoadAdjacent)
    //   {
    //     hexes[key] = HexTerrainType.Plains;
    //   }
    // }
  }

  private void BuildMap(Dictionary<Hex, HexTerrainType> hexes)
  {
    if (hexes == null)
    {
      Debug.LogError("No hexes were supplied to build map data with.", this);
      return;
    }


    float waitTime = 0.05f;
    float buildingWaitTime = 0f;
    foreach (var pair in hexes)
    {
      buildingWaitTime += waitTime;


      var hex = pair.Key;
      var type = pair.Value;

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
  }
}
