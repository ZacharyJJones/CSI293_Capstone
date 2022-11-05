using System.Collections;
using System.Collections.Generic;
using HexGrid;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  public static GameManager Instance;

  private void Awake()
  {
    if (Instance != null)
    {
      Destroy(this);
      return;
    }

    Instance = this;
    DontDestroyOnLoad(this);
  }

  public Dictionary<Hex, HexTerrainType> HexMap;
}
