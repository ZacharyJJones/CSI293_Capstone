using System.Collections.Generic;
using UnityEngine;
using HexGrid;
using System.Linq;

public class MapBuilder : MonoBehaviour
{
  public GameObject MapRoot;

  public MapHexPrefabsListScriptable MapPrefabs;


  public Dictionary<Hex, HexTerrainType> GenerateMap(int mapRadius)
  {
    var hexes = Hex.Zero.HexesInRadius(mapRadius);

    var hexMap = hexes.ToDictionary(x => x, y => MapPrefabs.GetRandom().Type);


    _buildFeatures(hexMap, mapRadius);



    // Add Mountain Ring Border
    int numRings = 1 + mapRadius / 5;
    for (int i = 1; i <= numRings; i++)
    {
      var ring = Hex.Zero.HexesAtRadius(mapRadius + i);
      foreach (var hex in ring)
      {
        hexMap.Add(hex, HexTerrainType.Mountain);
      }
    }

    return hexMap;
  }

  private void Start()
  {
  }


  private void _buildFeatures(Dictionary<Hex, HexTerrainType> hexes, int mapRadius)
  {
    Hex hexZero = Hex.Zero;

    // Make some rivers
    int numRivers = mapRadius / 2;
    for (int i = 0; i < numRivers; i++)
    {
      int ring = mapRadius - i;
      var ringHexes = hexZero.HexesAtRadius(ring);

      int range = ringHexes.Count;
      int random1 = Random.Range(0, range);
      int random2 = Utils.GetRandomNoRepeat(range, random1);

      var riverLine = ringHexes[random1].GetLineToPoint(ringHexes[random2]);
      foreach (var hex in riverLine)
      {
        hexes[hex] = HexTerrainType.River;
      }
    }

    // Get town eventual locations
    var townLocations = new List<Hex> { Hex.Zero };
    for (int i = 2; i <= mapRadius; i += 2)
    {
      var ring = hexZero.HexesAtRadius(i);
      var rand = ring[Random.Range(0, ring.Count)];
      townLocations.Add(rand);
    }

    // Connect towns w/ roads
    // -> Note that roads can pave over top of towns if you are not careful.
    for (int i = 0; i < townLocations.Count; i++)
    {
      int rand = Utils.GetRandomNoRepeat(townLocations.Count, i);
      var roadLine = townLocations[i].GetLineToPoint(townLocations[rand]);

      foreach (var hex in roadLine)
      {
        hexes[hex] = HexTerrainType.Road;
      }
    }

    // Place towns last, to ensure they aren't paved over.
    foreach (var hex in townLocations)
    {
      hexes[hex] = HexTerrainType.Town;
    }
  }

  public Dictionary<Hex, MapHex> BuildMap(Dictionary<Hex, HexTerrainType> hexes, float buildInterval = 0.25f)
  {
    float buildingWaitTime = 0f;
    var mapPrefabsByTerrainType = MapPrefabs.HexTiles.ToDictionary(x => x.Type, y => y);

    var gameobjectMap = new Dictionary<Hex, MapHex>();

    int dist = 0;
    var hexesAtDist = new List<Hex> { Hex.Zero };
    while (hexesAtDist.Any(x => hexes.ContainsKey(x)))
    {
      foreach (var hex in hexesAtDist)
      {
        var gameobjectInfo = mapPrefabsByTerrainType[hexes[hex]];

        var instantiatedObject = Instantiate(gameobjectInfo.Prefab, this.transform);

        if (gameobjectInfo.IsRotatable)
        {
          instantiatedObject.transform.Rotate(Vector3.up, Random.Range(0f, 360f));
        }

        var mapHex = instantiatedObject.GetComponent<MapHex>();
        mapHex.Coordinate = hex;
        mapHex.gameObject.name += $" ({hex.X}, {hex.Y})";
        mapHex.PostInstantiation(hexes, buildingWaitTime);

        gameobjectMap.Add(hex, mapHex);
      }

      dist++;
      hexesAtDist = Hex.Zero.HexesAtRadius(dist);
      buildingWaitTime += buildInterval;
    }

    return gameobjectMap;
  }
}
