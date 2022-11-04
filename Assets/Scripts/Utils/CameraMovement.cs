using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
  [SerializeField]
  private Camera _camera;
  private bool _controllable;



  public Vector3 CameraSpeeds; // Vector3 to bundle together as one variable.

  public GameObject PanController;
  public GameObject TiltController;

  public Vector2 MinMaxTiltDegrees;
  public Vector2 MinMaxZoomFOV;


  private float _defaultTilt => (MinMaxTiltDegrees.x + MinMaxTiltDegrees.y) / 2;
  private float _defaultZoom => MinMaxTiltDegrees.y;


  private float _pan;
  public float Pan
  {
    get => _pan;
    set
    {
      value %= 360;
      if (value < 0f)
      {
        value += 360;
      }
      _pan = value;
      PanController.transform.localRotation = Quaternion.Euler(0f, _pan, 0f);
    }
  }

  private float _tilt;
  public float Tilt
  {
    get => _tilt;
    set
    {
      _tilt = Mathf.Clamp(value, MinMaxTiltDegrees.x, MinMaxTiltDegrees.y);
      TiltController.transform.localRotation = Quaternion.Euler(_tilt, 0f, 0f);
    }
  }

  private float _zoom;
  public float Zoom
  {
    get => _zoom;
    set
    {
      _zoom = Mathf.Clamp(value, MinMaxZoomFOV.x, MinMaxZoomFOV.y);
      _camera.fieldOfView = _zoom;
    }
  }



  private void Awake()
  {
    _controllable = true;
  }

  // Start is called before the first frame update
  private void Start()
  {
    // when starting up, set some defaults for pan/tilt/zoom.
    // Pan will be 0 (positive Z away from camera)
    Pan = 0f;
    // Tilt will be in middle of range
    Tilt = _defaultTilt;
    // Zoom will be 100% of max.
    Zoom = _defaultZoom;
  }

  // Update is called once per frame
  private void Update()
  {
    if (!_controllable)
    {
      return;
    }

    float deltaT = Time.deltaTime;

    float pan = Input.GetAxis("Horizontal"); // left/right
    if (pan != 0f)
    {
      Pan += -CameraSpeeds.x * pan * deltaT;
    }

    float tilt = Input.GetAxis("Vertical"); // up/down
    if (tilt != 0f)
    {
      Tilt += CameraSpeeds.y * tilt * deltaT;
    }


    float zoom = Input.GetAxis("Mouse ScrollWheel"); // in/out
    if (zoom != 0f)
    {
      Zoom += -CameraSpeeds.z * zoom * deltaT;
    }

    if (Input.GetKeyDown(KeyCode.R))
    {
      // smoothly interpolate to defaults
      StartCoroutine(Reset(1f));
    }
  }

  private IEnumerator Reset(float interpolationTime)
  {
    _controllable = false;
    float startTilt = Tilt;
    float startZoom = Zoom;
    float goalTilt = _defaultTilt;
    float goalZoom = _defaultZoom;

    float timeElapsed = 0f;
    while (timeElapsed < interpolationTime)
    {
      float t = timeElapsed / interpolationTime;
      float tSmooth = Transform.SmoothStepX(t, 2);
      Tilt = Mathf.Lerp(startTilt, goalTilt, tSmooth);
      Zoom = Mathf.Lerp(startZoom, goalZoom, tSmooth);

      timeElapsed += Time.deltaTime;
      yield return null;
    }

    _controllable = true;
  }
}
