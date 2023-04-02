using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

[ExecuteInEditMode]
public class Spline : MonoBehaviour
{
  private List<Vector3> lastPointPos = new List<Vector3>();
  private List<Vector3> lastPointRotation = new List<Vector3>();

  public List<SplinePoint> Points { get; private set; } = new List<SplinePoint>();
  private Vector3 dirStart, dirEnd;

  public GameObject Tie;
  public float TieSeparation = .1f;
  private List<GameObject> ties = new List<GameObject>();
  public bool RegenTies = false;

  void Awake()
  {
    generatePoints();
    if (Tie != null)
      generateTies();
  }

  void generatePoints()
  {
    var colliders = GetComponentsInChildren<BoxCollider>();
    Points = colliders.Select(c => c.GetComponent<SplinePoint>()).Where(s => s != null).ToList();
    lastPointPos = Points.Select(p => p.transform.position).ToList();
    lastPointRotation = Points.Select(p => p.transform.localEulerAngles).ToList();

    if (Points.Count != 3)
      throw new ApplicationException("A curve must have 3 points");
  }

  void generateTies()
  {
    clearTies();
    dirStart = Points[0].transform.forward;
    dirEnd = Points[2].transform.forward;

    for (float i = 0; i < 1; i += TieSeparation)
    {
      var tie = GameObject.Instantiate(Tie);
      tie.transform.parent = transform;
      tie.transform.position = CalculateBezierPoint(i);
      tie.transform.right = CalculateDirection(i);
      ties.Add(tie);
    }
  }

  void clearTies()
  {
    var otherTiesTransforms = transform.GetComponentsInChildren<Transform>().Where(t => t.name.Contains("Tie"));
    var otherTies = otherTiesTransforms.Select(t => t.gameObject);

    ties.Concat(otherTies).ToList().ForEach(t => GameObject.DestroyImmediate(t));
  }

  // Update is called once per frame
  void Update()
  {
    if (Points.Count < 3)
      generatePoints();

    if (Points.Any((p) => p.transform.position != lastPointPos[Points.IndexOf(p)] || p.transform.localEulerAngles != lastPointRotation[Points.IndexOf(p)]))
    {
      lastPointPos = Points.Select(p => p.transform.position).ToList();
      lastPointRotation = Points.Select(p => p.transform.localEulerAngles).ToList();
      generateTies();
    }

    Debug.DrawLine(Points[0].transform.position, Points[2].transform.position, Color.green);
    Debug.DrawRay(Points[0].transform.position, dirStart, Color.magenta);
    Debug.DrawRay(Points[2].transform.position, dirEnd, Color.magenta);
  }

  // Calculate the position of a point on a Bezier curve using parametric equations
  public Vector3 CalculateBezierPoint(float t)
  {
    float u = 1.0f - t;
    float tt = t * t;
    float uu = u * u;

    Vector3 p = uu * Points[0].transform.position;
    p += 2.0f * u * t * Points[1].transform.position;
    p += tt * Points[2].transform.position;

    return p;
  }

  public Vector3 CalculateDirection(float t)
    => Vector3.Lerp(dirStart, dirEnd, t);

  public float GetTFromPosition(Vector3 p)
  {
    var p0 = Points[0].transform.position;
    var p0f = Points[0].transform.forward;

    var p1 = Points[1].transform.position;
    var p1p0f = (p1 - p0).normalized;

    var p2 = Points[2].transform.position;
    var p2f = Points[2].transform.forward;

    var totalDistance = Vector3.Distance(p0, p2);
    var distanceToEnd = Vector3.Distance(p, p2);

    return 1 - (distanceToEnd / totalDistance);
  }

  public SplinePoint GetEndPoint(SplinePoint startPoint)
  {
    if (startPoint != Points.LastOrDefault())
      return Points.LastOrDefault();
    else
      return Points.FirstOrDefault();
  }
}
