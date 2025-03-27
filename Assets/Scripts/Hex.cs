using System.Collections;
using UnityEngine;

public enum HexType { Grass, Sand, Mountain, Forest }

public class Hex : MonoBehaviour
{
	public HexType      groundType;
	public Vector2Int   hexCoordinates;
	[SerializeField]
	private Outline     outline;
	[SerializeField]
	private GameObject  mesh;
	private HexGrid     grid;

	public bool    isActive;
	public bool     isClose = false;

	//move
	private bool isMoving = false;
	private bool isMovingUp = false;
	private float moveTimeAux = 0;
	private Vector3 inicio, fin;
	private float moveTime;

	public void InitializeHex(Vector2Int coordinates, HexType hexType, bool active, HexGrid grid)
	{
		hexCoordinates = coordinates;
		SetGround(hexType);
		this.grid = grid;
		SetActive(active);
	}

	// unity functions
	void Start()
	{
		outline.enabled = false;
		outline.OutlineWidth = 10f;
	}

	private void Update()
	{
		MoveUpDown();
	}

	void OnMouseEnter()
	{
        if (isClose || isActive)
		{
			outline.enabled = true;
			UpDown(0.2f, 0.1f);
		}
	}

	void OnMouseUp()
	{
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1))
            return;

        if (isClose || isActive)
		{
			outline.enabled = true;
			UpDown(0.2f, 0.1f);
		}
		if (isClose)
		{
			grid.radialMenu.Open(transform.position, hexCoordinates);
		}
	}

    void OnMouseExit()
    {
        outline.enabled = false;
    }

	// color del fondo
	public void SetGround(HexType hexType)
	{ 
		Renderer renderer = mesh.GetComponent<Renderer>();
    	Color32 color;
		groundType = hexType;
		switch (groundType)
		{
			case HexType.Grass:
				color = new Color32(0xC7, 0xDF, 0x4D, 0xFF);
				break;
			case HexType.Sand:
				color = new Color32(0xFE, 0xE4, 0x7E, 0xFF);
				break;
			case HexType.Mountain:
				color = new Color32(0xF2, 0x95, 0x44, 0xFF);
				break;
			case HexType.Forest:
				color = new Color32(0x5B, 0x6F, 0xAB, 0xFF);
				break;
			default:
				color = Color.white;
				break;
		}
		renderer.material = new Material(renderer.material);
		renderer.material.color = color;
	}

	// Active
	public void SetActive(bool active)
	{
		isActive = active;
		VisibleMesh();
		if(active)
		{
			isClose = false;
			grid.CheckIfCloseToActive(this);
		}
	}

	private void VisibleMesh()
	{
		mesh.SetActive(isActive);
	}

	// move
	public void UpDown(float altura, float tiempo)
	{
		if (!isMoving && isActive)
		{
			isMoving = true;
			moveTimeAux = 0;
			isMovingUp = true;
			inicio = mesh.transform.localPosition;
			fin = inicio + new Vector3(0, altura, 0);
			moveTime = tiempo;
		}
	}

	private void MoveUpDown()
	{
		if (!isMoving) return;

		moveTimeAux += Time.deltaTime / (isMovingUp ? moveTime : moveTime / 2);
		mesh.transform.localPosition = Vector3.Lerp(inicio, fin, moveTimeAux);

		if (moveTimeAux >= 1f)
		{
			if (isMovingUp)
			{
				(inicio, fin) = (fin, inicio);
				moveTimeAux = 0;
				isMovingUp = false;
			}
			else
				isMoving = false;
		}
	}

	// New panel
	public void SetNewPanel(HexType hexType)
	{
		SetActive(true);
		SetGround(hexType);
		grid.ShockWave(hexCoordinates);
	}
}
