using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Assets;

public class DQN : Agent
{
	public EnumData.EMaker maker;
	public EnumData.EBoardState eBoardMaker;
	public delegate void TurnChangeEventHandler(int tile_index, EnumData.EMaker maker);

	public TurnChangeEventHandler eventTurnChange;


	public bool RandomAgent = false;

	public GameManager manager;


	void Start()
	{
		if (maker == EnumData.EMaker.O)
			eBoardMaker = EnumData.EBoardState.O;
		else if (maker == EnumData.EMaker.X)
		{
			eBoardMaker = EnumData.EBoardState.X;
		}
	}

	public override void OnEpisodeBegin()
	{
		manager.env.ResetBoard();
		manager.ResetRender();
		manager.current_turn = EnumData.EMaker.O;
	}


	public override void CollectObservations(VectorSensor sensor)
	{



		for (int i = 0; i < 9; ++i)
		{
			sensor.AddObservation((int)manager.env.Board[i]);
		}

	}




	public override void OnActionReceived(ActionBuffers actionBuffers)
	{


		int action = actionBuffers.DiscreteActions[0];

		if (action == 0)
		{
			return;
		}


		if (manager.current_turn == maker)
		{
			if (manager.env.Board[action - 1] == EnumData.EBoardState.E)
			{
				manager.ChangeTurn(action - 1, eBoardMaker, maker);
			}
		}

	}

	public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
	{
		var available_list = manager.env.GetAvailableIndex();

		actionMask.SetActionEnabled(0, 0, false);

		for (int i = 1; i <= available_list.Count; ++i)
		{
			actionMask.SetActionEnabled(0, i, available_list[i - 1]);
		}




	}

	public override void Heuristic(in ActionBuffers actionsOut)
	{
		var discreteActionsOut = actionsOut.DiscreteActions;

		if (RandomAgent == true)
        {
			var available = manager.env.GetAvailableIndex();

			List<int> available_index = new List<int>();

			for(int i = 0;  i < available.Count; ++i )
            {
				if(available[i] == true)
                {
					available_index.Add(i);
				}
				
            }

			var select = UnityEngine.Random.Range(0, available_index.Count);

			discreteActionsOut[0] = available_index[select] + 1;
			return;

		}

		for (int i = 0; i < 9; ++i)
		{
			if (manager.env.Board[i] == EnumData.EBoardState.E)
			{
				if (Input.GetKey(KeyCode.Keypad1 + i) == true)
				{

					discreteActionsOut[0] = i + 1;

				}

			}

		}


	}
}