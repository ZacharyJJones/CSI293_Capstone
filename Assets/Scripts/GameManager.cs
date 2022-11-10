using System.Collections;
using System.Collections.Generic;
using HexGrid;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  public static GameManager Instance;


  // Inspector
  public MapBuilder MapBuilder;
  public int MapSize;

  // Array of players is preferable
  // State-Machine approach to stages within turn. When reaching end of turn, just go to next player. Nice and simple.

  // these would be better as a class to represent player.
  public int ActivePlayerIndex;
  public Player[] Players;
  public GameObject Player;
  public Hex PlayerCoordinate;

  // Runtime
  public Dictionary<Hex, MapHex> HexMap;

  public Player ActivePlayer => Players[ActivePlayerIndex];


  private void Awake()
  {
    if (Instance != null)
    {
      Destroy(this);
      return;
    }

    Instance = this;
    DontDestroyOnLoad(this);
    PlayerCoordinate = Hex.Zero;
  }

  private void Start()
  {
    var map = MapBuilder.GenerateMap(MapSize);
    HexMap = MapBuilder.BuildMap(map);

    var options = GetPlayerMovementOptions();
    PickPlayerMovementOption(options[Random.Range(0, options.Count)]);
  }


  public void SwitchToNextPlayer()
  {
    ActivePlayerIndex++;
    ActivePlayerIndex %= Players.Length;
  }


  public List<Hex> GetPlayerMovementOptions()
  {
    var adjacents = PlayerCoordinate.Adjacents;
    for (int i = adjacents.Count - 1; i >= 0; i--)
    {
      if (!HexMap.ContainsKey(adjacents[i]))
      {
        adjacents.RemoveAt(i);
      }
    }
    return adjacents;
  }

  // could raise them higher to show selectability?
  public void DisplayPlayerMovementOptions(List<Hex> options)
  {
    foreach (var option in options)
    {
      // show tile can be moved to

      // instantiate something
    }
  }

  public void PickPlayerMovementOption(Hex chosen)
  {
    PlayerCoordinate = chosen;
    var xy = PlayerCoordinate.InTwoDSpace;

    var oldPosition = Player.transform.localPosition;
    var newPosition = new Vector3(xy.X, 0f, xy.Y);

    StartCoroutine(Utils.DoOverTime(0.125f,
      (t) =>
      {
        Player.transform.localPosition = Vector3.Lerp(
          oldPosition,
          newPosition,
          Transform.SmoothStepX(t, 2)
        );
      },
      () =>
      {
        var options = GetPlayerMovementOptions();
        PickPlayerMovementOption(options[Random.Range(0, options.Count)]);
      })
    );
  }
}
