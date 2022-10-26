using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HexGrid;

public class MapHex : MonoBehaviour
{
  public HexTerrainType TerrainType;

  [HideInInspector]
  public Hex Coordinate;

  public virtual void PostInstantiation(Dictionary<Hex, HexTerrainType> hexes, float delayToRise)
  {
    // determine resting position
    var TwoDHexCoords = Coordinate.InTwoDSpace;
    Vector3 endPosition = new Vector3(TwoDHexCoords.X, 0, TwoDHexCoords.Y);
    Vector3 startPosition = new Vector3(endPosition.x, endPosition.y - 3, endPosition.z);

    transform.localPosition = startPosition;
    StartCoroutine(Utils.SimpleWait(delayToRise, () =>
    {
      StartCoroutine(Utils.DoOverTime(0.5f, (t) =>
          {
            transform.localPosition = Vector3.Lerp(startPosition, endPosition, Transform.SmoothStopX(t, 2));
          }));
    }));
  }
}
