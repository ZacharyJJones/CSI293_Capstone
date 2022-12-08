using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
  public int SceneBuildIndex;

  public void LoadScene()
  {
    SceneManager.LoadScene(SceneBuildIndex);
  }
}
