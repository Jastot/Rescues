using System.Collections.Generic;
using System.Linq;
using Rescues.NPC.Models;
using UnityEngine;


namespace Rescues
{
	public class CurveWayController
	{
		#region Fildes

		private const float CURVES_RESOLUTION = 0.001f;
		private List<CurveWay> _curveWays;

		#endregion


		#region Private

		public CurveWayController(List<CurveWay> curveWays)
		{
			_curveWays = curveWays;

			foreach (var curve in _curveWays)
			{
				CalculateCurveData(curve);
				curve.GetScaleAction += GetScaleByCurve;
			}
		}

		#endregion
		
		
		#region Methods

		private void GetScaleByCurve(Vector3 position, CurveWay curve)
		{
			curve.Scale = Vector3.one * (curve.KKoef * position.y + curve.MKoef);
		}

		private void CalculateCurveData(CurveWay curveWay)
		{
			var backPoint = curveWay.PathCreator.transform.position.y + curveWay.PathCreator.bezierPath.PathBounds.extents.y;
			var frontPoint = curveWay.PathCreator.transform.position.y - curveWay.PathCreator.bezierPath.PathBounds.extents.y;
			
			//Вычисление коэфицентов линейной функции для скейла персонажей на Curve
			curveWay.KKoef = (curveWay.BackScale - curveWay.FrontScale) / (backPoint - frontPoint);
			curveWay.MKoef = curveWay.FrontScale - curveWay.KKoef * frontPoint;
		}
		
		public CurveWay GetCurve(Gate enterGate, WhoCanUseCurve type)
		{
			var chosenCurves = _curveWays.FindAll(x => x.WhoCanUseWay == type);
            
			if (chosenCurves.Count == 0)
				chosenCurves = _curveWays.FindAll(x => x.WhoCanUseWay == WhoCanUseCurve.All);

			var result = chosenCurves.First();
			
			var closestPoints = new Dictionary<CurveWay, Vector3>();
			foreach (var curve in chosenCurves)
				closestPoints.Add(curve, curve.PathCreator.path.GetClosestPointOnPath(enterGate.transform.position));
			
			//Поиск ближайшей Curve к точке enterGate
			var startDistance = float.MaxValue;
			foreach (var curve in closestPoints)
			{
				var newDistance = Vector3.Distance(curve.Value, enterGate.transform.position);
				if (newDistance < startDistance)
				{
					startDistance = newDistance;
					curve.Key.StartCharacterPosition = curve.Value;
					curve.Key.LeftmostPoint = curve.Key.PathCreator.transform.position + 
					                          curve.Key.PathCreator.path.localPoints[0];;
					result = curve.Key;
				}
			}
			return result;
		}

		public CurveWay GetCurve(NPCWayPoints point, WhoCanUseCurve type)
		{
			var chosenCurves = _curveWays.FindAll(x => x.WhoCanUseWay == type);
            
			if (chosenCurves.Count == 0)
				chosenCurves = _curveWays.FindAll(x => x.WhoCanUseWay == WhoCanUseCurve.All);

			var result = chosenCurves.First();
			
			var closestPoints = new Dictionary<CurveWay, Vector3>();
			foreach (var curve in chosenCurves)
				closestPoints.Add(curve, curve.PathCreator.path.GetClosestPointOnPath(point.transform.position));
			
			//Поиск ближайшей Curve к точке enterGate
			var startDistance = float.MaxValue;
			foreach (var curve in closestPoints)
			{
				var newDistance = Vector3.Distance(curve.Value, point.transform.position);
				if (newDistance < startDistance)
				{
					startDistance = newDistance;
					curve.Key.StartCharacterPosition = curve.Value;
					curve.Key.LeftmostPoint = curve.Key.PathCreator.transform.position + 
					                          curve.Key.PathCreator.path.localPoints[0];;
					result = curve.Key;
				}
			}
			return result;
		}
		
		public void UnloadData()
		{
			foreach (var curve in _curveWays)
			{
				curve.GetScaleAction = null;
			}
		}

		#endregion
	}
}