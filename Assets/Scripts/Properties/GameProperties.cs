using UnityEngine;

public class GameProperties : MonoBehaviour
{
    public static GameProperties inst;

    [Header("Player Settings")]
    public bool allowPlayerRotation;
    //public bool autoFire = false;
    //public float autoFireDelay = 1;
    public KeyCode fireKey;
    public float playerMoveSpeed = 3;
    public float projectileSpeed = 7;

    [Header("Grid Settings")]
    [Range(0, 10)] public int blockCount = 5;
    [Range(0, 10)] public int trapTileCount = 8;
    [Range(0, 30)] public int crateCount = 30;

    public int gridColumns = 13;
    public int gridRows = 7;
    public Vector3 gridOriginPosition = new Vector3(-7, -3, 0);

    private void Awake()
    {
        if (inst == null) inst = this;
        else Destroy(gameObject);
    }

}
