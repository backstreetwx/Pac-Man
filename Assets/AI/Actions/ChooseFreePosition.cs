using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;
using RAIN.Representation;
using RAIN.Navigation;

[RAINAction("Choose Free Position")]
public class ChooseFreePosition : RAINAction
{

  public Expression MoveDistance = new Expression();

  public Expression StayOnGraph = new Expression();

  public Expression MoveTargetVariable = new Expression();

  private float defaultMoveDistance = 10f;

  public override ActionResult Execute(RAIN.Core.AI ai)
  {
    if (!MoveTargetVariable.IsVariable)
      throw new Exception("The Choose Move Position node requires a valid Move Target Variable");

    float _moveDistance = 0f;
    if (MoveDistance.IsValid)
      _moveDistance = MoveDistance.Evaluate<float>(ai.DeltaTime, ai.WorkingMemory);

    if (_moveDistance <= 0f)
      _moveDistance = defaultMoveDistance;

    Vector3 _tempDirection = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0f, UnityEngine.Random.Range(-1f, 1f));

    Vector3 tempDestination = ai.Kinematic.Position + _tempDirection.normalized*_moveDistance;

    if (!StayOnGraph.IsValid || !(StayOnGraph.Evaluate<bool> (ai.DeltaTime, ai.WorkingMemory))) 
    {
      ai.WorkingMemory.SetItem<Vector3>(MoveTargetVariable.VariableName, tempDestination);
      return ActionResult.SUCCESS;
    }
    if (NavigationManager.Instance.GraphForPoint(tempDestination, ai.Motor.MaxHeightOffset, NavigationManager.GraphType.Navmesh, null).Count == 0)
    {
      return ActionResult.FAILURE;
    }

    ai.WorkingMemory.SetItem<Vector3>(MoveTargetVariable.VariableName, tempDestination);

    return ActionResult.SUCCESS;
  }

}