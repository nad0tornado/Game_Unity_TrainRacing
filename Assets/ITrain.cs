using System;

public interface ITrain
{
  public float Speed { get; set; }
  public float MaxSpeed { get; }
  public float Acceleration { get; }
}
