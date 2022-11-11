using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraClickRaycast : MonoBehaviour
{
  public Camera Camera;

  private void Update()
  {
    if (Input.GetMouseButtonDown(0))
    {
      Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
      if (Physics.Raycast(ray, out RaycastHit hit))
      {
        var hitObject = hit.collider.gameObject;
        var clickable = hitObject.GetComponentInParent<CameraClickable>();

        if (clickable != null)
        {
          clickable.OnClick.Invoke();
        }
      }
    }
  }
}
