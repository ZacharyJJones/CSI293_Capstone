using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventureToken : MonoBehaviour
{
  public GameObject GreenGem;
  public GameObject YellowGem;
  public GameObject BlueGem;
  public GameObject RedGem;

  private void Awake()
  {
    GreenGem.SetActive(false);
    YellowGem.SetActive(false);
    BlueGem.SetActive(false);
    RedGem.SetActive(false);
  }

  public void SetGemToDisplay(int adventureTier)
  {
    if (adventureTier == 1)
    {
      GreenGem.SetActive(true);
    }
    else if (adventureTier == 2)
    {
      YellowGem.SetActive(true);
    }
    else if (adventureTier == 3)
    {
      BlueGem.SetActive(true);
    }
    else if (adventureTier == 4)
    {
      RedGem.SetActive(true);
    }
  }
}
