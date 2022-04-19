using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets;
public class Tile : MonoBehaviour
{
	public GameObject Background;
	public GameObject O;
	public GameObject X;


	public int PosX = 0;
	public int PosY = 0;
	public int index = -1;

	private void Awake()
	{

		O.SetActive(false);
		X.SetActive(false);



	}

	public void Renderer(EnumData.EBoardState state)
	{
		switch (state)
		{
			case EnumData.EBoardState.E:
				{
					O.SetActive(false);
					X.SetActive(false);
					break;
				}
			case EnumData.EBoardState.O:
				{
					O.SetActive(true);
					X.SetActive(false);
					break;
				}
			case EnumData.EBoardState.X:
				{
					O.SetActive(false);
					X.SetActive(true);
					break;
				}
		}

	}


}
