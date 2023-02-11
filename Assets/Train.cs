using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
  public float speed = 0;
  private bool goingForward = false, goingBackward = false;
  public float maxSpeed = 30;
  public float acceleration = 1;

  private bool playedBrakeRelease = false, playedBrakeApply = true, playedWheels = false;
  public AudioClip engineSound;
  public AudioClip wheelSound;
  public AudioClip brakeReleaseSound;
  public AudioClip brakeApplySound;

  private AudioSource engineAudio, wheelAudio, brakeAudio;
  // Start is called before the first frame update
  void Start()
  {
    // .. engine sounds to start straight away and loop
    engineAudio = gameObject.AddComponent<AudioSource>();
    engineAudio.clip = engineSound;
    engineAudio.loop = true;
    engineAudio.Play();

    // .. wheel sounds
    wheelAudio = gameObject.AddComponent<AudioSource>();
    wheelAudio.clip = wheelSound;
    wheelAudio.playOnAwake = false;
    wheelAudio.loop = true;

    // .. brake sounds
    brakeAudio = gameObject.AddComponent<AudioSource>();
    brakeAudio.clip = brakeReleaseSound;
    brakeAudio.volume = .5f;
    brakeAudio.playOnAwake = false;
  }

  // Update is called once per frame
  void Update()
  {
    transform.Translate(transform.forward * speed * Time.deltaTime * .1f);
    goingForward = speed > 0;
    goingBackward = speed < 0;

    // .. brake sounds
    if (speed < maxSpeed * .25f && !playedBrakeApply)
    {
      brakeAudio.clip = brakeApplySound;
      brakeAudio.Play();
      playedBrakeApply = true;
    }
    else if ((goingForward && speed > maxSpeed * .25f) || (goingBackward && speed < maxSpeed * -.25f))
      playedBrakeApply = false;

    if (speed != 0 && !playedBrakeRelease)
    {
      brakeAudio.clip = brakeReleaseSound;
      brakeAudio.Play();
      playedBrakeRelease = true;
      Debug.Log("released brakes");
    }
    else if ((goingBackward && speed > maxSpeed * -.01f) || (goingForward && speed < maxSpeed * .01f))
      playedBrakeRelease = false;


    // .. wheel sounds
    /* if ((goingBackward && speed < maxSpeed * -.25f || goingForward && speed > maxSpeed * .25f) && !playedWheels)
    {
      wheelAudio.Play();
      playedWheels = true;
    }

    if ((goingForward && speed < maxSpeed * -.25f) || (goingBackward && speed > maxSpeed * -.25f))
    {
      playedWheels = false;
      wheelAudio.Stop();
    }

    wheelAudio.volume = .1f + speed / maxSpeed; */

    if (Input.GetAxis("Vertical") == 0)
    {
      if (speed > 0)
      {
        // .. deceleration
        var multiplier = 1 + (1f / speed * 20);
        speed = Mathf.Lerp(speed, speed * .9f, Time.deltaTime * multiplier);

        if ((speed > 0 && speed < .01f) || (speed < 0 && speed > -.01f))
          speed = 0;
      }
    }
    else
    {


      // .. train accelerates gradually up to max speed
      if (speed < maxSpeed)
        speed += Input.GetAxis("Vertical") * acceleration * Time.deltaTime;
    }

    // .. engine sound vs. train speed
    engineAudio.pitch = Mathf.Min(1 + (speed / maxSpeed) * .5f, 1.5f);


  }
}
