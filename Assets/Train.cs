using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour, ITrain
{
  public float Speed { get; set; } = 0;

  [SerializeField]
  private float maxSpeed = 30;
  public float MaxSpeed { get => maxSpeed; set => maxSpeed = value; }

  [SerializeField]
  private float acceleration = 1;
  public float Acceleration { get => acceleration; set => acceleration = value; }

  // Update is called once per frame
  void Update()
  {
    transform.Translate(transform.forward * Speed * Time.deltaTime * .1f);

    if (Input.GetAxis("Vertical") == 0)
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
