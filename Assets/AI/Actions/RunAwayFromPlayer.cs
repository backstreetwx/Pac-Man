using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;
using RAIN.Representation;
using RAIN.Motion;

[RAINAction("Run away from Player")]
public class RunAwayFromPlayer : RAINAction
{
  public Expression FleeDistance = new Expression();

  public Expression FleeFrom = new Expression();

  public Expression StayOnGraph = new Expression();

  public Expression FleeTargetVariable = new Expression();

  private float defaultFleeDistance = 10f;

  private MoveLookTarget fleeTarget = new MoveLookTarget();

  public override ActionResult Execute(RAIN.Core.AI ai)
  {
    if (!FleeTargetVariable.IsVariable)
      throw new Exception("The Choose Flee Position node requires a valid Flee Target Variable");

    float _tFleeDistance = 0f;
    if (FleeDistance.IsValid)
      _tFleeDistance = FleeDistance.Evaluate<float>(ai.DeltaTime, ai.WorkingMemory);

    if (_tFleeDistance <= 0f)
      _tFleeDistance = defaultFleeDistance;

    if (FleeFrom.IsVariable)
      MoveLookTarget.GetTargetFromVariable(ai.WorkingMemory, FleeFrom.VariableName, ai.Motor.DefaultCloseEnoughDistance, fleeTarget);
    else
      fleeTarget.TargetType = MoveLookTarget.MoveLookTargetType.None;

    if (fleeTarget.IsValid)
    {
      
      Vector3 _tAway = ai.Kinematic.Position - fleeTarget.Position;
      Vector3 _tFleeDirection = _tAway.normalized * UnityEngine.Random.Range(1f, _tFleeDistance);

      Vector3 _tFleePosition = ai.Kinematic.Position + _tFleeDirection;
      if (ai.Navigator.OnGraph(_tFleePosition, ai.Motor.MaxHeightOffset))
      {
        ai.WorkingMemory.SetItem<Vector3>(FleeTargetVariable.VariableName, _tFleePosition);
        return ActionResult.SUCCESS;
      }


      Vector3 tFortyFive = Quaternion.Euler(new Vector3(0, 45, 0)) * _tFleeDirection;
      _tFleePosition = ai.Kinematic.Position + tFortyFive;
      if (ai.Navigator.OnGraph(_tFleePosition, ai.Motor.MaxHeightOffset))
      {
        ai.WorkingMemory.SetItem<Vector3>(FleeTargetVariable.VariableName, _tFleePosition);
        return ActionResult.SUCCESS;
      }


      tFortyFive = Quaternion.Euler(new Vector3(0, -45, 0)) * _tFleeDirection;
      _tFleePosition = ai.Kinematic.Position + tFortyFive;
      if (ai.Navigator.OnGraph(_tFleePosition, ai.Motor.MaxHeightOffset))
      {
        ai.WorkingMemory.SetItem<Vector3>(FleeTargetVariable.VariableName, _tFleePosition);
        return ActionResult.SUCCESS;
      }
    }


    Vector3 _tDirection = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0f, UnityEngine.Random.Range(-1f, 1f));

    Vector3 _tDestination = ai.Kinematic.Position + _tDirection.normalized*_tFleeDistance;
    if (StayOnGraph.IsValid && (StayOnGraph.Evaluate<bool>(ai.DeltaTime, ai.WorkingMemory)))
    {
      if (!ai.Navigator.OnGraph(_tDestination, ai.Motor.MaxHeightOffset))
        return ActionResult.FAILURE;
    }

    ai.WorkingMemory.SetItem<Vector3>(FleeTargetVariable.VariableName, _tDestination);

    return ActionResult.SUCCESS;
  }
}