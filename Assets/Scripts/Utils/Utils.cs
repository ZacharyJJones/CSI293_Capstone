using System;
using System.Collections;
using UnityEngine;
using HexGrid;

public static class Utils
{
  public static Vector3 HexIn3DSpace(Hex hex)
  {
    var twoDSpaceCoords = hex.InTwoDSpace;
    return new Vector3(twoDSpaceCoords.X, 0f, twoDSpaceCoords.Y);
  }

  public static int GetRandomNoRepeat(int maxExclusive, int last)
  {
    int rand = UnityEngine.Random.Range(0, maxExclusive);
    return (last + 1 + rand) % maxExclusive;
  }

  public static IEnumerator SimpleWait(float waitTime, Action onComplete)
  {
    for (float time = 0; time < waitTime; time += Time.deltaTime)
    {
      yield return null;
    }

    onComplete?.Invoke();
  }

  public static IEnumerator SimpleWaitConditional(Func<bool> completeWhenTrue, Action onComplete)
  {
    while (!completeWhenTrue.Invoke())
    {
      yield return null;
    }

    onComplete.Invoke();
  }

  public static IEnumerator DoOverTime(float timeToComplete, Action<float> whileActive, Action onComplete = null)
  {
    for (float time = 0; time < timeToComplete; time += Time.deltaTime)
    {
      whileActive.Invoke(time / timeToComplete);
      yield return null;
    }

    whileActive.Invoke(1f);
    onComplete?.Invoke();
  }
}