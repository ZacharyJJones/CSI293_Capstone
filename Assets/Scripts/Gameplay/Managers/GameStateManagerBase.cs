using System;
using System.Collections.Generic;
using HexGrid;
using UnityEngine;

public class GameStateManagerBase : MonoBehaviour
{
  protected Hero _hero;
  protected Dictionary<Hex, MapHex> _map;
  protected Action _onStateCompletion;

  public virtual string GameStateName => "Undefined Game State";

  public virtual void BeginStateManagement(Hero activeHero, Dictionary<Hex, MapHex> map, Action onStateCompletion)
  {
    _hero = activeHero;
    _map = map;
    _onStateCompletion = onStateCompletion;
  }
}
