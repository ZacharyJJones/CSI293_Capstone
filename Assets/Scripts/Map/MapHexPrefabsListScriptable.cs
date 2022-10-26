using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "MapHexPrefabsList", menuName = "EDU_Q6_Capstone/MapHexPrefabsList", order = 0)]
public class MapHexPrefabsListScriptable : ScriptableObject
{

  public HexTile[] HexTiles;





  public HexTile GetRandom()
  {
    int weightSum = HexTiles.Sum(x => x.Weight);
    int[] indexToChoose = new int[weightSum];

    int i = 0;
    for (int j = 0; j < HexTiles.Length; j++)
    {
      for (int k = 0; k < HexTiles[j].Weight; k++)
      {
        indexToChoose[i] = j;
        i++;
      }
    }

    int rand = Random.Range(0, indexToChoose.Length);
    return HexTiles[indexToChoose[rand]];
  }
}


[System.Serializable]
public class HexTile
{
  public HexTerrainType Type;
  public int Weight;
  public GameObject Prefab;
  public bool IsRotatable;
}