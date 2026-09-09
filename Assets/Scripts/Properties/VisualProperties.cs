using UnityEngine;
using System;

public class VisualProperties : MonoBehaviour
{

    public static VisualProperties inst;

    public Sprite backgroundImage;

    [Serializable]
    public class PlayerVisuals
    {
        public Direction playerStartingDirection;
        [Space(10)]
        public RuntimeAnimatorController animController;
        public Sprite defaultSprite;
        public Color color;
        public bool spriteColorOverride = false;

        public Color impactColor;
    }

    [Serializable]
    public class ProjectileVisuals
    {
        public RuntimeAnimatorController animController;
        public Sprite sprite;
        public Color color;
        public bool spriteColorOverride = false;
        [Space(10)]
        public bool enableTrail;
        

    }

    [Serializable]
    public class TileVisuals
    {
        public RuntimeAnimatorController animController;
        public Sprite sprite;
        public Color color;
        public bool spriteColorOverride = false;

    }

    [Serializable]
    public class CrateTileVisuals
    {
        public RuntimeAnimatorController animController;
        public Sprite sprite;
        public Color color;
        public bool spriteColorOverride = false;

    }

    [Serializable]
    public class TrapTileVisuals
    {
        public RuntimeAnimatorController animController;
        public Sprite sprite;
        public Color color;
        public bool spriteColorOverride = false;

    }

    [Serializable]
    public class BlockTileVisuals
    {
        public RuntimeAnimatorController animController;
        public Sprite sprite;
        public Color color;
        public bool spriteColorOverride = false;

    }

    [Serializable]
    public class GoalTileVisuals
    {
        public RuntimeAnimatorController animController;
        public Sprite sprite;
        public Color color;
        public bool spriteColorOverride = false;

    }

    [Header("Player")]
    public PlayerVisuals playerVisuals;
    [Header("Projectile")]
    public ProjectileVisuals projectileVisuals;
    [Header("Tiles")]
    public TileVisuals tileVisuals;
    public CrateTileVisuals crateVisuals;
    public TrapTileVisuals trapVisuals;
    public BlockTileVisuals blockVisuals;
    public GoalTileVisuals goalVisuals;

    private void Awake()
    {
        if (inst == null) inst = this;
        else Destroy(gameObject);
    }


}
public enum Direction
{
    UP,
    DOWN,
    LEFT,
    RIGHT
}
