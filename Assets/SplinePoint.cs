using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[ExecuteInEditMode]
public class SplinePoint : MonoBehaviour
{
  private Spline _spline;
  public Spline Spline
  {
    get
    {
      if (_spline == null)
        _spline = GetComponentInParent<Spline>();
      if (_spline == null)
        throw new NullReferenceException("A SplinePoint cannot exist outside of a spline!");

      return _spline;
    }
  }
}
