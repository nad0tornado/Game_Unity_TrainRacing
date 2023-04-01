using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainSpeaker : MonoBehaviour
{
  private ITrain _train;
  private ITrain train
  {
    get
    {
      if (_train == null)
        _train = GetComponent<Train>();

      return _train;
    }
    set => _train = value;
  }

  private bool playedBrakeRelease = false, playedBrakeApply = true, playedWheels = false;
  public AudioClip engineSound;
  public AudioClip wheelSound;
  public AudioClip brakeReleaseSound;
  public AudioClip brakeApplySound;
  private AudioSource engineAudio, wheelAudio, brakeAudio;

  // Start is called before the first frame update
  void Start()
  {
    train = GetComponent<Train>();

    if (train == null)
      throw new NullReferenceException("A Train component is required to use TrainSpeaker");

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

    // .. brake soundsspeed
    brakeAudio = gameObject.AddComponent<AudioSource>();
    brakeAudio.clip = brakeReleaseSound;
    brakeAudio.volume = .5f;
    brakeAudio.playOnAwake = false;
  }

  // Update is called once per frame
  void Update()
  {
    // .. brake sounds
    if (train.Speed < train.MaxSpeed * .25f && !playedBrakeApply)
    {
      brakeAudio.clip = brakeApplySound;
      brakeAudio.Play();
      playedBrakeApply = true;
    }
    else if (
        (train.Speed > 0 && train.Speed > train.MaxSpeed * .25f) ||
        (train.Speed < 0 && train.Speed < train.MaxSpeed * -.25f)
    )
      playedBrakeApply = false;

    if (train.Speed != 0 && !playedBrakeRelease)
    {
      brakeAudio.clip = brakeReleaseSound;
      brakeAudio.Play();
      playedBrakeRelease = true;
      Debug.Log("released brakes");
    }
    else if ((train.Speed < 0 && train.Speed > train.MaxSpeed * -.01f) || (train.Speed > 0 && train.Speed < train.MaxSpeed * .01f))
      playedBrakeRelease = false;

    // .. engine sound vs. train train.Speed
    engineAudio.pitch = Mathf.Min(1 + (train.Speed / train.MaxSpeed) * .5f, 1.5f);
  }
}
