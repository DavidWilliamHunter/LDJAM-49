using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dance.Utils.Curves
{
    public struct BezierCurve
    {
        public Vector3[] points;

        public BezierCurve(Vector3 [] _points)
        {
            points = _points;
        }

        public BezierCurve(BezierCurve other)
        {
            points = other.points;
        }        

        public Vector3 GetPoint(float t)
        {
            if (points.Length == 3)
                return (Bezier.GetPoint(points[0], points[1], points[2], t));
            return (Bezier.GetPoint(points[0], points[1], points[2], points[3], t));
        }

        public Vector3 GetPoint(Transform transform, float t)
        {
            if(points.Length==3)
                return transform.TransformPoint(Bezier.GetPoint(points[0], points[1], points[2], t));
            return transform.TransformPoint(Bezier.GetPoint(points[0], points[1], points[2], points[3], t));
        }

        public Vector3 GetVelocity(float t)
        {
            if (points.Length == 3)
                return Bezier.GetFirstDerivative(points[0], points[1], points[2], t);
            return Bezier.GetFirstDerivative(points[0], points[1], points[2], points[3], t);
        }

        public Vector3 GetVelocity(Transform transform, float t)
        {
            if (points.Length == 3)
                return transform.TransformPoint(Bezier.GetFirstDerivative(points[0], points[1], points[2], t)) -
                transform.position;
            return transform.TransformPoint(Bezier.GetFirstDerivative(points[0], points[1], points[2], points[3], t)) -
                transform.position;
        }

        public Vector3 GetDirection(float t)
        {
            return GetVelocity(t).normalized;
        }

        public Vector3 GetDirection(Transform transform, float t)
        {
            return GetVelocity(transform, t).normalized;
        }
    }
}
