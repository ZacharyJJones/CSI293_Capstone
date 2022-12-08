using System;
using System.Collections;
using System.Collections.Generic;
using HexGrid;
using UnityEngine;

public class AdventureManager : GameStateManagerBase
{
  // "Data-First" would be better. Then can have a prefab that gets filled in with relevant data.
  // -- that can wait though, as making the prefab is so straightforward.
  public AdventureChallenge[] T1_GreenEnemies;
  public AdventureChallenge[] T2_YellowEnemies;
  public AdventureChallenge[] T3_BlueEnemies;
  public AdventureChallenge[] T4_RedEnemies;





  public override string GameStateName => "Adventure";


  public override void BeginStateManagement(Action onStateCompletion)
  {
    base.BeginStateManagement(onStateCompletion);

    // 1. Check if player is on an adventure tile (if not, nothing needs to happen here)
    int challengeTier = 1;

    // 2. get challege prefab
    var adventureChallengePrefab = _getChallengePrefab(challengeTier);





    // Step 1: Get the adventure.

    // Step 1: Before Combat Things
    // -- Happens once. Makes sense to do in this method.
  }


  private AdventureChallenge _getChallengePrefab(int tier)
  {
    AdventureChallenge[] options;
    switch (tier)
    {
      case 4:
        options = T4_RedEnemies;
        break;

      case 3:
        options = T3_BlueEnemies;
        break;

      case 2:
        options = T2_YellowEnemies;
        break;

      case 1:
      default:
        options = T1_GreenEnemies;
        break;
    }

    return options[UnityEngine.Random.Range(0, options.Length)];
  }
}
