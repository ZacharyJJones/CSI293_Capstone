using System;
using System.Collections.Generic;
using HexGrid;
using UnityEngine;

public class GameStateManagerBase : MonoBehaviour
{
  protected GameManager _gameManager;
  protected Action _onStateCompletion;

  public virtual string GameStateName => "Undefined Game State";

  public virtual void BeginStateManagement(Action onStateCompletion)
  {
    _onStateCompletion = onStateCompletion;
  }

  public void SetGameManager(GameManager toSet) => _gameManager = toSet;
}
