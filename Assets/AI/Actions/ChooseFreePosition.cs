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

  private float _defaultMoveDistance = 10f;

  public override ActionResult Execute(RAIN.Core.AI ai)
  {
    if (!MoveTargetVariable.IsVariable)
      throw new Exception("The Choose Move Position node requires a valid Move Target Variable");

    float tMoveDistance = 0f;
    if (MoveDistance.IsValid)
      tMoveDistance = MoveDistance.Evaluate<float>(ai.DeltaTime, ai.WorkingMemory);

    if (tMoveDistance <= 0f)
      tMoveDistance = _defaultMoveDistance;

    Vector3 tDirection = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0f, UnityEngine.Random.Range(-1f, 1f));
    tDirection *= tMoveDistance;

    Vector3 tDestination = ai.Kinematic.Position + tDirection;
    if (StayOnGraph.IsValid && (StayOnGraph.Evaluate<bool>(ai.DeltaTime, ai.WorkingMemory)))
    {
      if (NavigationManager.Instance.GraphForPoint(tDestination, ai.Motor.MaxHeightOffset, NavigationManager.GraphType.Navmesh, ((BasicNavigator)ai.Navigator).GraphTags).Count == 0)
        return ActionResult.FAILURE;
    }

    ai.WorkingMemory.SetItem<Vector3>(MoveTargetVariable.VariableName, tDestination);

    return ActionResult.SUCCESS;
  }

}