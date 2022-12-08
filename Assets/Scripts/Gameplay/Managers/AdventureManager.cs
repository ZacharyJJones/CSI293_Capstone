using System;
using System.Collections;
using System.Collections.Generic;
using HexGrid;
using UnityEngine;

public class AdventureManager : GameStateManagerBase
{
  // "Data-First" would be better. Then can have a prefab that gets filled in with relevant data.
  // -- that can wait though, as making the prefab is so straightforward.
  public AdventureChallenge[] TestEnemies;

  public override string GameStateName => "Adventure";
  public override void BeginStateManagement(Action onStateCompletion)
  {
    base.BeginStateManagement(onStateCompletion);

    // Step 1: Get the adventure.

    // Step 1: Before Combat Things
    // -- Happens once. Makes sense to do in this method.
  }

}
