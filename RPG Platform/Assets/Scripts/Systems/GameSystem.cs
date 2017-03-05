using UnityEngine;

public class GameSystem : MonoBehaviour
{
    private const int DD5E = 1;
    private const int PATHFINDER = 2;

    [SerializeField] private DD5eSystem dd5ESystem;
    [SerializeField] private PathfinderSystem pathfinderSystem;
    [SerializeField] private int currSystem;

	// Use this for initialization
    void Start()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public RPGSystem GetCurrentSystem()
    {
        switch (currSystem)
        {
            case DD5E:
            {
                return dd5ESystem;
            }
            case PATHFINDER:
            {
                return pathfinderSystem;
            }
            default:
                return null;
        }
    }
}
