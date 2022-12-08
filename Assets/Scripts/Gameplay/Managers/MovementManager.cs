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
  public override void BeginStateManagement(Action onStateCompletion)
  {
    base.BeginStateManagement(onStateCompletion);

    _validMoves = GetValidPlayerMoves();
    DisplayPlayerMovementOptions(_validMoves);
  }



  public List<Hex> GetValidPlayerMoves()
  {
    var heroCoord = _gameManager.ActiveHero.Coordinate;
    var moveOptions = heroCoord.Adjacents;
    for (int i = moveOptions.Count - 1; i >= 0; i--)
    {
      if (!_gameManager.HexMap.ContainsKey(moveOptions[i]))
      {
        moveOptions.RemoveAt(i);
      }
    }
    moveOptions.Add(heroCoord);
    return moveOptions;
  }

  public void DisplayPlayerMovementOptions(List<Hex> options)
  {
    _moveOptionObjects = new List<GameObject>();
    foreach (var option in options)
    {
      var obj = _spawnMoveOption(option);
      _moveOptionObjects.Add(obj);
    }
  }

  public void PickPlayerMovementOption(Hex chosen)
  {
    _cleanupMoveOptionObjects();

    _gameManager.ActiveHero.Coordinate = chosen;
    var xy = chosen.InTwoDSpace;

    var heroTransform = _gameManager.ActiveHero.transform;

    var oldPosition = heroTransform.localPosition;
    var newPosition = new Vector3(xy.X, 0f, xy.Y);

    StartCoroutine(Utils.DoOverTime(HeroMoveTime,
      (t) =>
      {
        heroTransform.localPosition = Vector3.Lerp(
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

  private GameObject _spawnMoveOption(Hex location)
  {
    var obj = Instantiate(
      MovementSelectionPrefab,
      Utils.HexIn3DSpace(location),
      Quaternion.Euler(0f, 100f, 0f),
      this.transform
    );

    var clickable = obj.GetComponent<CameraClickable>();
    clickable.OnClick = () => PickPlayerMovementOption(location);

    var startPos = obj.transform.localPosition;
    var endPos = new Vector3(startPos.x, MoveOptionSpawnHeight, startPos.z);
    _moveOptionLerpCommon(obj, MoveOptionSpawnLerpTime, startPos, endPos, Vector3.zero, obj.transform.localScale);

    return obj;
  }
  private void _despawnMoveOption(GameObject obj)
  {
    float time = MoveOptionSpawnLerpTime / 2f;
    var startPos = obj.transform.localPosition;
    var endPos = new Vector3(startPos.x, startPos.y - MoveOptionSpawnHeight, startPos.z);
    _moveOptionLerpCommon(obj, time, startPos, endPos, obj.transform.localScale, Vector3.zero);

  }
  private void _moveOptionLerpCommon(GameObject obj, float lerpTime, Vector3 startPos, Vector3 endPos, Vector3 startScale, Vector3 endScale)
  {
    StartCoroutine(Utils.DoOverTime(
      lerpTime,
      (t) =>
      {
        obj.transform.localPosition = Vector3.Lerp(startPos, endPos, Transform.SmoothStepX(t, 2));
        obj.transform.localScale = Vector3.Lerp(startScale, endScale, t);
      },
      () =>
      {
        Destroy(obj);
      }
    ));
  }
}
