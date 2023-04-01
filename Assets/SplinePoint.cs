using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[ExecuteInEditMode]
public class SplinePoint : MonoBehaviour
{
  void Start()
  {
    if (transform.GetComponentInParent<Spline>() == null)
      throw new ApplicationException("A Spline point must be a direct child of a Spline component");
  }
}
