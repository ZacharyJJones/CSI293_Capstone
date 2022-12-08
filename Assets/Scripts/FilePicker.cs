using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class FilePicker : MonoBehaviour
{
  public SaveGameData SaveGameDataObj;
  public SceneLoader SceneLoader;

  private string path;

  public void OpenFileExplorer()
  {
    path = EditorUtility.OpenFilePanel("Pick a Save File", Application.dataPath, "json");

    //Debug.Log(path + $" IsNullOrEmpty:{string.IsNullOrEmpty(path)}");

    if (string.IsNullOrEmpty(path))
    {
      // didn't work, bad path, etc.
      return;
    }



    // use filepath, get file data, load into game data object, use that to load scene.
    var json = File.ReadAllText(path);
    GameData saveGame = JsonUtility.FromJson<GameData>(json);
    SaveGameDataObj.Initialize(saveGame);
    SceneLoader.LoadScene();
  }
}
