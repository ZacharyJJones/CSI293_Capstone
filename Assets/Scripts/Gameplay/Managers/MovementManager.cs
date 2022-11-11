using System;
using System.Collections;
using System.Collections.Generic;
using HexGrid;
using UnityEngine;

public class MovementManager : GameStateManagerBase
{
  // Inspector
  public float HeroMoveTime = 0.25f;
  public float MoveOptionSpawnHeight = 1.5f;
  public float MoveOptionSpawnLerpTime = 0.5f;
  public GameObject MovementSelectionPrefab;

  // Private
  protected List<Hex> _validMoves;
  protected List<GameObject> _moveOptionObjects;

  // Etc
  public override string GameStateName => "Movement";

  public override void BeginStateManagement(Hero activeHero, Dictionary<Hex, MapHex> map, Action onStateCompletion)
  {
    // Base State Management
    base.BeginStateManagement(activeHero, map, onStateCompletion);

    // Specific State Management
    _validMoves = GetValidPlayerMoves();
    DisplayPlayerMovementOptions(_validMoves);
    //PickPlayerMovementOption(options[UnityEngine.Random.Range(0, options.Count)]);
  }



  public List<Hex> GetValidPlayerMoves()
  {
    var adjacents = _hero.Coordinate.Adjacents;
    for (int i = adjacents.Count - 1; i >= 0; i--)
    {
      if (!_map.ContainsKey(adjacents[i]))
      {
        adjacents.RemoveAt(i);
      }
    }
    return adjacents;
  }

  // could raise them higher to show selectability?
  public void DisplayPlayerMovementOptions(List<Hex> options)
  {
    _moveOptionObjects = new List<GameObject>();
    foreach (var option in options)
    {
      _spawnMoveOption(option);
    }
  }

  public void PickPlayerMovementOption(Hex chosen)
  {
    _cleanupMoveOptionObjects();

    _hero.Coordinate = chosen;
    var xy = chosen.InTwoDSpace;

    var oldPosition = _hero.transform.localPosition;
    var newPosition = new Vector3(xy.X, 0f, xy.Y);

    StartCoroutine(Utils.DoOverTime(HeroMoveTime,
      (t) =>
      {
        _hero.transform.localPosition = Vector3.Lerp(
          oldPosition,
          newPosition,
          Transform.SmoothStepX(t, 2)
        );
      }, _onStateCompletion)
    );
  }

  private void _cleanupMoveOptionObjects()
  {
    foreach (var moveObj in _moveOptionObjects)
    {
      _despawnMoveOption(moveObj);
    }
  }

  private void _spawnMoveOption(Hex location)
  {
    // show tile can be moved to
    // instantiate something
    var obj = Instantiate(
      MovementSelectionPrefab,
      Utils.HexIn3DSpace(location),
      Quaternion.Euler(0f, 100f, 0f),
      this.transform
    );

    _moveOptionObjects.Add(obj);

    // Set onclick
    var clickable = obj.GetComponent<CameraClickable>();
    clickable.OnClick = () => PickPlayerMovementOption(location);

    // lerp into view!
    var endScale = obj.transform.localScale;
    var startPos = obj.transform.localPosition;
    var endPos = new Vector3(startPos.x, MoveOptionSpawnHeight, startPos.z);
    StartCoroutine(Utils.DoOverTime(
      MoveOptionSpawnLerpTime,
      (t) =>
      {
        obj.transform.localPosition = Vector3.Lerp(startPos, endPos, Transform.SmoothStepX(t, 2));
        obj.transform.localScale = Vector3.Lerp(Vector3.zero, endScale, t);
      }
    ));
  }
  private void _despawnMoveOption(GameObject obj)
  {
    var startScale = obj.transform.localScale;
    var startPos = obj.transform.localPosition;
    var endPos = new Vector3(startPos.x, -MoveOptionSpawnHeight, startPos.z);
    StartCoroutine(Utils.DoOverTime(
      MoveOptionSpawnLerpTime / 2f,
      (t) =>
      {
        obj.transform.localPosition = Vector3.Lerp(startPos, endPos, Transform.SmoothStepX(t, 2));
        obj.transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);
      },
      () => Destroy(obj)
    ));
  }
}
