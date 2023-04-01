using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour, ITrain
{
  public float Speed { get; set; } = 10;

  [SerializeField]
  private float maxSpeed = 30;
  public float MaxSpeed { get => maxSpeed; set => maxSpeed = value; }

  [SerializeField]
  private float acceleration = 1;
  public float Acceleration { get => acceleration; set => acceleration = value; }

  private Spline currentSpline = null;
  private SplinePoint endPoint = null;

  void OnTriggerEnter(Collider col)
  {
    var splinePoint = col.gameObject.GetComponent<SplinePoint>();

    if (splinePoint == null)
      return;

    currentSpline = splinePoint.GetComponentInParent<Spline>();
  }

  // Update is called once per frame
  void Update()
  {
    transform.Translate(Vector3.forward * (Speed * Time.deltaTime * .1f));

    if (currentSpline != null)
    {
      var t = currentSpline.GetTFromPosition(transform.position);
      transform.forward = currentSpline.CalculateDirection(t);
    }

    Debug.DrawRay(transform.position, transform.forward * .125f, Color.red, 30);

    if (Input.GetAxis("Vertical") == 0 && false)
    {
      if (Speed > 0)
      {
        // .. deceleration
        var multiplier = 1 + (1f / Speed * 20);
        Speed = Mathf.Lerp(Speed, Speed * .9f, Time.deltaTime * multiplier);

        if ((Speed > 0 && Speed < .01f) || (Speed < 0 && Speed > -.01f))
          Speed = 0;
      }
    }
    else
    {
      // .. train accelerates gradually up to max Speed
      if (Speed < MaxSpeed)
        Speed += Input.GetAxis("Vertical") * Acceleration * Time.deltaTime;
    }
  }


}
