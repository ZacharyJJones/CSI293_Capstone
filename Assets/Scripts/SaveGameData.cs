using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGameData : MonoBehaviour
{
  public static SaveGameData Instance;

  public void Initialize(GameData data)
  {
    if (Instance != null)
    {
      Destroy(this);
      return;
    }

    Instance = this;
    DontDestroyOnLoad(this);

    Data = data;
  }

  public GameData Data;
}
