using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class RadialMenu : MonoBehaviour
{
    public GameObject menuRoot;
    public Button bubble1;
    public Button bubble2;
    public Button bubble3;
    public Button bubble4;
    private Vector2Int hexCoordsChosed;
    public HexGrid hexGrid;

    private Camera cam;

    void Awake()
    {
        cam = Camera.main;
        menuRoot.SetActive(false);

        bubble1.onClick.AddListener(() => Action(HexType.Grass));
        bubble2.onClick.AddListener(() => Action(HexType.Sand));
        bubble3.onClick.AddListener(() => Action(HexType.Mountain));
        bubble4.onClick.AddListener(() => Action(HexType.Forest));
    }

    void Update()
    {
        if (menuRoot.activeSelf && Input.GetMouseButtonDown(0))
        {
            if (!IsPointerOverUI())
                Close();
        }
    }

    public void Open(Vector3 worldPosition, Vector2Int hexCoords)
    {
        Vector3 screenPos = cam.WorldToScreenPoint(worldPosition);
        menuRoot.transform.position = screenPos;
        menuRoot.SetActive(true);
        hexCoordsChosed = hexCoords;
    }

    public void Close()
    {
        menuRoot.SetActive(false);
    }

    void Action(HexType actionName)
    {
        hexGrid.NewHex(hexCoordsChosed, actionName);
        Close();
    }

    private bool IsPointerOverUI()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, raycastResults);

        return raycastResults.Count > 0;
    }
}
