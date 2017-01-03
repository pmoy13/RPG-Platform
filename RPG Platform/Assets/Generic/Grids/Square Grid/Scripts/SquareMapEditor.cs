using UnityEngine;
using UnityEngine.EventSystems;

public class SquareMapEditor : MonoBehaviour
{
    // The map we will be manipulating.
    public SquareGrid SquareGrid;

    // All the possible colors we can paint.
    // TODO: Make these textures instead!
    public Color[] Colors;

    private Color _activeColor;

    private void Awake()
    {
        // We need to have a default color selected on program load.
        SelectColor(0);
    }

    // TODO: Description.
    private void Update()
    {
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            HandleInput();
        }
    }

    private void HandleInput()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit))
        {
            SquareGrid.TouchCell(hit.point, _activeColor);
        }
    }

    public void SelectColor(int index)
    {
        _activeColor = Colors[index];
    }
}
