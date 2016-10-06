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

  private float _defaultFleeDistance = 10f;

  private MoveLookTarget _fleeTarget = new MoveLookTarget();

  public override ActionResult Execute(RAIN.Core.AI ai)
  {
    if (!FleeTargetVariable.IsVariable)
      throw new Exception("The Choose Flee Position node requires a valid Flee Target Variable");

    float tFleeDistance = 0f;
    if (FleeDistance.IsValid)
      tFleeDistance = FleeDistance.Evaluate<float>(ai.DeltaTime, ai.WorkingMemory);

    if (tFleeDistance <= 0f)
      tFleeDistance = _defaultFleeDistance;

    if (FleeFrom.IsVariable)
      MoveLookTarget.GetTargetFromVariable(ai.WorkingMemory, FleeFrom.VariableName, ai.Motor.DefaultCloseEnoughDistance, _fleeTarget);
    else
      _fleeTarget.TargetType = MoveLookTarget.MoveLookTargetType.None;

    if (_fleeTarget.IsValid)
    {
      
      Vector3 tAway = ai.Kinematic.Position - _fleeTarget.Position;
      Vector3 tFleeDirection = tAway.normalized * UnityEngine.Random.Range(1f, tFleeDistance);

      Vector3 tFleePosition = ai.Kinematic.Position + tFleeDirection;
      if (ai.Navigator.OnGraph(tFleePosition, ai.Motor.MaxHeightOffset))
      {
        ai.WorkingMemory.SetItem<Vector3>(FleeTargetVariable.VariableName, tFleePosition);
        return ActionResult.SUCCESS;
      }


      Vector3 tFortyFive = Quaternion.Euler(new Vector3(0, 45, 0)) * tFleeDirection;
      tFleePosition = ai.Kinematic.Position + tFortyFive;
      if (ai.Navigator.OnGraph(tFleePosition, ai.Motor.MaxHeightOffset))
      {
        ai.WorkingMemory.SetItem<Vector3>(FleeTargetVariable.VariableName, tFleePosition);
        return ActionResult.SUCCESS;
      }


      tFortyFive = Quaternion.Euler(new Vector3(0, -45, 0)) * tFleeDirection;
      tFleePosition = ai.Kinematic.Position + tFortyFive;
      if (ai.Navigator.OnGraph(tFleePosition, ai.Motor.MaxHeightOffset))
      {
        ai.WorkingMemory.SetItem<Vector3>(FleeTargetVariable.VariableName, tFleePosition);
        return ActionResult.SUCCESS;
      }
    }


    Vector3 tDirection = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0f, UnityEngine.Random.Range(-1f, 1f));
    tDirection *= tFleeDistance;

    Vector3 tDestination = ai.Kinematic.Position + tDirection;
    if (StayOnGraph.IsValid && (StayOnGraph.Evaluate<bool>(ai.DeltaTime, ai.WorkingMemory)))
    {
      if (!ai.Navigator.OnGraph(tDestination, ai.Motor.MaxHeightOffset))
        return ActionResult.FAILURE;
    }

    ai.WorkingMemory.SetItem<Vector3>(FleeTargetVariable.VariableName, tDestination);

    return ActionResult.SUCCESS;
  }
}