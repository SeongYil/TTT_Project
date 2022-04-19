using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets;
using Unity.MLAgents.Actuators;

public class GameManager : MonoBehaviour
{
	private GameObject TilePrefab;
	private GameObject O_Prefab;
	private GameObject X_Prefab;

	public EnumData.EMaker current_turn;

	public EnumData.EPlayer eO_Player;
	public EnumData.EPlayer eX_Player;

	private DQN O_PlayerAgent;
	private DQN X_PlayerAgent;

	public Environment env;

	public List<Tile> tile_renderer_list = new List<Tile>();

	private GameObject O_PlayerObject;
	private GameObject X_PlayerObject;

	public EnumData.EMode Mode;

	// Start is called before the first frame update

	private void Initialize()
	{
		env = new Environment();
		env.Initilize();

		if(eO_Player == EnumData.EPlayer.DQNAgent)
		{
            O_PlayerObject = GameObject.Find("O_Player");
            O_PlayerAgent = O_PlayerObject.GetComponent<DQN>();
            DQN dqn_player = (O_PlayerAgent as DQN);
            if (dqn_player != null)
            {
                //do something with it
                dqn_player.manager = this;
            }

        }





        if (eX_Player == EnumData.EPlayer.DQNAgent)
		{
			X_PlayerObject = GameObject.Find("X_Player");
			X_PlayerAgent = X_PlayerObject.GetComponent<DQN>();
			DQN dqn_player = (X_PlayerAgent as DQN);
			if (dqn_player != null)
			{
				//do something with it
				dqn_player.manager = this;
			}

		}
	



	}

	public void Start()
    {
		Initialize();
		LoadResources();
		RenderBackground();

		current_turn = EnumData.EMaker.O;



	}

	public void ChangeTurn()
	{
		if(current_turn == EnumData.EMaker.O)
		{
			current_turn = EnumData.EMaker.X;
		}
		else
		{
			current_turn = EnumData.EMaker.O;
		}
	}


    public void Update()
    {
		for (int i = 0; i < 9; ++i)
		{
			tile_renderer_list[i].Renderer(env.Board[i]);
		}



	}

	public void ChangeTurn(int tile_index , EnumData.EBoardState eBoardMaker, EnumData.EMaker eMaker)
	{
		EnumData.EBoardResult result = env.Move(eBoardMaker, tile_index);

		
		switch(result)
		{
			case EnumData.EBoardResult.Continue:
				{
					if(eMaker == EnumData.EMaker.O)
                    {
						O_PlayerAgent.AddReward(-0.01f);
						
					}
                    else
                    {
						X_PlayerAgent.AddReward(-0.01f);
					}
					
					ChangeTurn();
					break;
				}
			case EnumData.EBoardResult.Draw:
				{
					current_turn = EnumData.EMaker.O;
					O_PlayerAgent.SetReward(0.5f);
					X_PlayerAgent.SetReward(0.5f);
					O_PlayerAgent.EndEpisode();
					X_PlayerAgent.EndEpisode();
					env.ResetBoard();
					ResetRender();
					break;
				}
			case EnumData.EBoardResult.O_Win:
				{
					current_turn = EnumData.EMaker.O;
					O_PlayerAgent.SetReward(1.0f);
					X_PlayerAgent.SetReward(-1.0f);
					O_PlayerAgent.EndEpisode();
					X_PlayerAgent.EndEpisode();
					env.ResetBoard();
					ResetRender();
					break;
				}
			case EnumData.EBoardResult.X_Win:
				{
					current_turn = EnumData.EMaker.O;
					O_PlayerAgent.SetReward(-1.0f);
					X_PlayerAgent.SetReward(1.0f);
					O_PlayerAgent.EndEpisode();
					X_PlayerAgent.EndEpisode();
					env.ResetBoard();
					ResetRender();
					break;
				}
		}
		
		
		
		
		

	}

	private void LoadResources()
	{
		TilePrefab = Resources.Load("Tile") as GameObject;
		O_Prefab = Resources.Load("O") as GameObject;
		X_Prefab = Resources.Load("X") as GameObject;
	}

	public void ResetRender()
	{
		foreach( Tile tile in tile_renderer_list)
		{
			tile.Renderer(EnumData.EBoardState.E);
		}
	}
	private void RenderBackground()
	{
		int index = 0;
		GameObject board = new GameObject();
		board.name = "board";
		for (int y = 0; y < 3; ++y)
		{
			for (int x = 0; x < 3; ++x)
			{
				Vector3 Pos = new Vector3(x, y, 0);
				GameObject tile = GameObject.Instantiate(TilePrefab, Pos, transform.rotation);
				tile.name = index.ToString();
				Tile tile_script = tile.GetComponent<Tile>();
				tile_renderer_list.Add(tile_script);
				tile_script.PosX = x;
				tile_script.PosY = y;
				tile_script.index = index;
				index += 1;
				tile.transform.parent = board.transform;
			}
		}
	}
}
