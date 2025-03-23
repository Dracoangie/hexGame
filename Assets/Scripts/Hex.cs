using System.Collections;
using UnityEngine;


public enum HexType { Grass, Water, Mountain }

public class Hex : MonoBehaviour
{
    public HexType      type;
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
        type = hexType;
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
        if (isClose || isActive)
        {
            outline.enabled = true;
            UpDown(0.2f, 0.1f);
        }
        if (isClose)
        {
            SetActive(true);
            SetNewPanel();
        }
    }

    void OnMouseExit()
    {
        outline.enabled = false;
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

    void SetNewPanel()
    {
        SetActive(true);
        grid.ShockWave(hexCoordinates);
    }
}
