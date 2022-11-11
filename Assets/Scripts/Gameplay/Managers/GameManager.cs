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

  public Hero[] Heroes;
  public GameStateManagerBase[] GameStateManagers;

  // Array of players is preferable
  // State-Machine approach to stages within turn. When reaching end of turn, just go to next player. Nice and simple.


  // Runtime
  private Dictionary<Hex, MapHex> _hexMap;
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
    _hexMap = MapBuilder.BuildMap(map, out float buildingTime);

    // Put players in starting location(s).
    // -> Would be interesting if start town was randomized, or if players could start from different towns.
    var defaultHero = Heroes[0];
    Heroes = new Hero[Heroes.Length + NumTestHeroesToSpawn - 1];
    Heroes[0] = defaultHero;
    for (int i = 1; i < Heroes.Length; i++)
    {
      var newHero = Instantiate(defaultHero, Vector3.zero, Quaternion.identity, HeroesRoot.transform);
      Heroes[i] = newHero;
    }
    for (int i = 0; i < Heroes.Length; i++)
    {
      Heroes[i].Coordinate = Hex.Zero;
    }


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
    ActiveGameState.BeginStateManagement(ActiveHero, _hexMap, AdvanceGameState);
  }
}