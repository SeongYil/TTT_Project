using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets;

public class Environment
{
	public List<EnumData.EBoardState> Board = new List<EnumData.EBoardState>();
	public EnumData.EBoardResult current_state = EnumData.EBoardResult.Continue;

	public void Initilize()
	{
		for (int i = 0; i < 9; ++i)
		{
			Board.Add(0);
		}
	}

	public void ResetBoard()
	{
		for (int i = 0; i < 9; ++i)
		{
			Board[i] = EnumData.EBoardState.E;
		}
	}


	public string GetBoardState()
	{
		string board_state = string.Join("", Board.ToArray());
		return board_state;

	}

	


	public List<bool> GetAvailableIndex()
	{
		List<bool> available_list = new List<bool>();

		for (int i = 0; i < Board.Count; ++i)
		{
			if (Board[i] == EnumData.EBoardState.E)
			{
				available_list.Add(true);
			}
			else
			{
				available_list.Add(false);
			}

		}
		return available_list;
	}

	public EnumData.EBoardResult GetCurrentBoardState()
	{
		return current_state;

	}
	public EnumData.EBoardResult Move(EnumData.EBoardState maker, int Position)
	{
		Board[Position] = maker;
		bool result = is_check_end();
		if (result == true)
		{
			if (maker == EnumData.EBoardState.O)
			{
				current_state = EnumData.EBoardResult.O_Win;
				return EnumData.EBoardResult.O_Win;
			}
			else
			{
				current_state = EnumData.EBoardResult.X_Win;
				return EnumData.EBoardResult.X_Win;
			}
		}

		foreach( EnumData.EBoardState state in Board)
		{
			if(state == EnumData.EBoardState.E)
			{
				current_state = EnumData.EBoardResult.Continue;
				return EnumData.EBoardResult.Continue;
			}
		}

		current_state = EnumData.EBoardResult.Draw;
		return EnumData.EBoardResult.Draw;
		


	}

	public bool is_check_end()
	{
		if ((Board[0] == Board[1]) && (Board[1] == Board[2]) && (Board[0] != EnumData.EBoardState.E) ||
			(Board[3] == Board[4]) && (Board[4] == Board[5]) && (Board[3] != EnumData.EBoardState.E) ||
			(Board[6] == Board[7]) && (Board[7] == Board[8]) && (Board[6] != EnumData.EBoardState.E) ||
			(Board[0] == Board[3]) && (Board[3] == Board[6]) && (Board[0] != EnumData.EBoardState.E) ||
			(Board[1] == Board[4]) && (Board[4] == Board[7]) && (Board[1] != EnumData.EBoardState.E) ||
			(Board[2] == Board[5]) && (Board[5] == Board[8]) && (Board[2] != EnumData.EBoardState.E) ||
			(Board[0] == Board[4]) && (Board[4] == Board[8]) && (Board[0] != EnumData.EBoardState.E) ||
			(Board[6] == Board[4]) && (Board[4] == Board[2]) && (Board[6] != EnumData.EBoardState.E))
		{

			return true;
		}
		return false;

	}

}
