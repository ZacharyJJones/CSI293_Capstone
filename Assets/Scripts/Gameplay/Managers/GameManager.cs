using System.Collections;
using System.Collections.Generic;
using HexGrid;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  public static GameManager Instance;


  // Inspector
  public int MapSize;
  public MapBuilder MapBuilder;

  public int NumTestHeroesToSpawn;
  public GameObject HeroesRoot;
  public GameObject HeroPrefab;


  public Hero[] Heroes;
  public GameStateManagerBase[] GameStateManagers;



  // Runtime
  public Dictionary<Hex, int> MapAdventureNodes;
  public Dictionary<Hex, MapHex> HexMap;
  private int _activeHeroIndex;

  private int _activeGameStateIndex;

  public Hero ActiveHero => Heroes[_activeHeroIndex];
  public GameStateManagerBase ActiveGameState => GameStateManagers[_activeGameStateIndex];



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

  private void Start()
  {
    _activeHeroIndex = 0;

    var map = MapBuilder.GenerateMap(MapSize);
    HexMap = MapBuilder.BuildMap(map, out float buildingTime);

    // Put players in starting location(s).
    // -> Would be interesting if start town was randomized, or if players could start from different towns.
    Heroes = new Hero[NumTestHeroesToSpawn];
    for (int i = 0; i < Heroes.Length; i++)
    {
      var newHero = Instantiate(HeroPrefab, Vector3.zero, Quaternion.identity, HeroesRoot.transform);
      var heroComponent = newHero.GetComponent<Hero>();
      Heroes[i] = heroComponent;
    }
    for (int i = 0; i < Heroes.Length; i++)
    {
      Heroes[i].Coordinate = Hex.Zero;
    }

    // Initialize all GameStateManagers
    for (int i = 0; i < GameStateManagers.Length; i++)
    {
      GameStateManagers[i].SetGameManager(this);
    }

    // Begin state management
    _activeGameStateIndex = -1;
    StartCoroutine(Utils.SimpleWait(buildingTime, AdvanceGameState));
  }


  public void AdvanceGameState()
  {
    _activeGameStateIndex++;
    if (_activeGameStateIndex >= GameStateManagers.Length)
    {
      _activeGameStateIndex = 0;

      // Advance active player
      _activeHeroIndex++;
      _activeHeroIndex %= Heroes.Length;
    }

    //Debug.Log($"GameState Advanced. Player {_activeHeroIndex}/{Heroes.Length - 1}, State {_activeGameStateIndex}/{GameStateManagers.Length - 1} ({ActiveGameState.GameStateName})");
    ActiveGameState.BeginStateManagement(AdvanceGameState);
  }
}