using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerControl : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////////
    /// Animation Controlling Methods   ////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////
    


    private void MovementAnimationControl(Vector3 _direction)
    {
        RotatePlayer(_direction, rotationEnabled);

        if (animatorController == null) return;

        if (!rotationEnabled)
        {
            if (_direction == Vector3.down)
            {
                //Animator code goes here for this state

            }

            if (_direction == Vector3.up)
            {
                //Animator code goes here for this state
            }

            if (_direction == Vector3.left)
            {
                //Animator code goes here for this state

            }

            if (_direction == Vector3.right)
            {
                //Animator code goes here for this state

            }
        }
        else //ONLY IF USING PLAYER ROTATION 
        {
            //Animator code goes here for this state

        }
    }


    private void MovingFinished()
    {
        if (animatorController == null) return;

        //Code for resetting animator bools go here
    }

    private void ChangeFacingDirection(Vector3 _direction)
    {
        if (animatorController == null) return;

        if (_direction == Vector3.down)
        {
            //Animator code goes here for this state
        }

        if (_direction == Vector3.up)
        {
            //Animator code goes here for this state
        }

        if (_direction == Vector3.left)
        {
            //Animator code goes here for this state
        }

        if (_direction == Vector3.right)
        {
            //Animator code goes here for this state
        }
    }

    private void FiringAnimationControl(Vector3 _direction)
    {
        if (animatorController == null) return;

        if (!rotationEnabled)
        {
            //Animator code goes here if not using Player Rotation

        }
        else
        {
            //Animator code goes here if using Player Rotation

        }
    }



    private void EnteredTrap(Vector3 _direction)
    {
        if (animatorController == null) return;

        if (!rotationEnabled)
        {
            //Animator code goes here if not using Player Rotation 
        }
        else
        {
            //Animator code goes here if using Player Rotation
        }
    }





    ///////////////////////////////////////////////////////////////////////////////////////////
    /// Rest of Player Control Script below this point ////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////


    private float moveSpeed;
    private float projectileSpeed;

    private KeyCode fireKey;
    //private float timeSinceFired;
    private GridGenerator grid;
    public GameObject projectilePrefab;
    private bool isMovingToTarget;
    private bool isMoving;
    private Vector3 direction;
    private VisualProperties.PlayerVisuals visuals;
    private Color color;
    private Sprite sprite;
    private Animator animator;
    private RuntimeAnimatorController animatorController;
    private GameObject arrow;
    private bool rotationEnabled;
    private Color impactColor;

    private SpriteRenderer rend;

    private AudioClip playerMoveSFX;
    private AudioClip enteringTrapSFX;
    private AudioClip firingSFX;
    private AudioClip goalReachedSFX;

    private int rIndex = 0;
    private int lastRIndex = 0;
    private int lastCIndex = 0;
    private int cIndex = 0;


    public Tile CurrentTile {get{return currentTile;}}
    private Tile currentTile;
    private Tile lastTile;
    private Tile targetTile;

    private WaitForSeconds blinkDuration = new WaitForSeconds(0.04f);

    public static event Action PlayerMoved;
    public static event Action PlayerHit;

    public static event Action<Vector3> ProjectileFired;


    private void Start()
    {
        visuals = VisualProperties.inst.playerVisuals;

        rend = GetComponentInChildren<SpriteRenderer>();
        grid = GridGenerator.inst;

        //This sets the player to grid place 0,0 at start
        transform.position = grid.GetTilePosition(rIndex, cIndex);

        fireKey = GameProperties.inst.fireKey;

        //We make sure that the current tile and target tile are set to our current 0,0 tile at start
        currentTile = grid.tiles[rIndex, cIndex];
        targetTile = currentTile;

        moveSpeed = GameProperties.inst.playerMoveSpeed;
        projectileSpeed = GameProperties.inst.projectileSpeed;
        //timeSinceFired = GameProperties.inst.autoFireDelay;
        impactColor = visuals.impactColor;

        direction = GetStartDirection();
        Init();

        playerMoveSFX = Sounds.inst.playerMove;
        enteringTrapSFX = Sounds.inst.enterTrap;
        firingSFX = Sounds.inst.fireProjectile;
        goalReachedSFX = Sounds.inst.goalReached;
    }

    private void Init()
    {   

        rotationEnabled = GameProperties.inst.allowPlayerRotation;
        color = rend.color;
        


        if (visuals.animController != null)
        {

            animatorController = visuals.animController;
            animator = rend.gameObject.AddComponent<Animator>();
            animator.runtimeAnimatorController = animatorController;
        }

        else if (visuals.defaultSprite != null)
        {
            sprite = visuals.defaultSprite;
            rend.sprite = sprite;

            if (visuals.spriteColorOverride)
            {
                color = visuals.color;
                rend.color = color;
            }
        }

        else
        {
            rend.sprite = Resources.Load<Sprite>("Circle");
            color = visuals.color;
            rend.color = color;

            var prefab = Resources.Load<GameObject>("Arrow");
            var pos = rend.transform.position + new Vector3(0, 0.2f, 0);
            arrow = Instantiate(prefab, pos, Quaternion.identity, rend.transform);
        }
    }


    private void Update()
    {
        //Player will then move a single space in that direction
        if (!isMovingToTarget)
        {
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                MovementAnimationControl(Vector3.right);
                SetTargetTile(Vector3.right);
            }
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                MovementAnimationControl(Vector3.left);
                SetTargetTile(Vector3.left);
            }
            else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                MovementAnimationControl(Vector3.up);
                SetTargetTile(Vector3.up);
            }
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) 
            {
                MovementAnimationControl(Vector3.down);
                SetTargetTile(Vector3.down);
            }

            if (Input.GetKeyDown(fireKey))
            {
                FireProjectile();
            }


        }
        
        //here is what triggers the player to move, once the targettile and currentle are not the same
        //We run the coroutine that moves the player from their current position to their target position
        if (targetTile != currentTile)
        {
            StartCoroutine(MovePlayer(grid.GetTilePosition(currentTile), grid.GetTilePosition(targetTile)));

            //We then make a referenc to the last tile in case we need it again later -See: Traps
            lastTile = currentTile;
            //And we make sure to set currentTile to the targettile so that we dont run the coroutine endlessly
            currentTile = targetTile;
        }   
    }

    private void RotatePlayer(Vector3 _direction, bool rotateVisual )
    {
        direction = _direction;
        ChangeFacingDirection(_direction);
        if (rotateVisual)
        {
            float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        }
    }

    //These two functions do the same thing but take in different arguments.
    //The first sets the target tile based on cardinal direction  
    private void SetTargetTile(Vector3 dir)
    {
        if (rIndex + dir.x >= 0 && rIndex + dir.x < grid.rows && cIndex + dir.y >= 0 && cIndex + dir.y < grid.columns)
        {
            var t = grid.tiles[rIndex + (int)dir.x, cIndex + (int)dir.y];

            if (!t.isInaccessible)
            {
                targetTile = t;
                lastRIndex = rIndex;
                lastCIndex = cIndex;
                rIndex += (int)dir.x;
                cIndex += (int)dir.y;
            }
            else MovingFinished();        
        }
        else MovingFinished();     
    }

    //The second let's us direct pass a tile as target tile.
    //In this case we also need to remeber to update the row and column indices since we'll need them 
    //to keep track of where we are in the grid
    private void SetTargetTile(Tile t)
    {
        if (!t.isInaccessible)
        {
            targetTile = t;
            rIndex = lastRIndex;
            cIndex = lastCIndex;
        }
    }

    private void FireProjectile()
    {
        AudioManager.inst.PlaySound(firingSFX, Sounds.inst.fireProjectileVolume);
        float offsetScaler = 0.8f;
        var pos = transform.position + (direction * offsetScaler);
        GameObject go = Instantiate(projectilePrefab, pos, Quaternion.identity);
        var rend = go.GetComponentInChildren<SpriteRenderer>();
        if (rotationEnabled) { rend.transform.localRotation = transform.rotation; }
       
        var pjt = go.GetComponent<Projectile>();
        pjt.speed = projectileSpeed;
        pjt.direction = direction;
        pjt.Init();
        FiringAnimationControl(direction);

        ProjectileFired?.Invoke(pos);

    }

    //We move the player in a coroutine by sending a start and an end pos and just lerping between then
    //for the duration set. The duration is currently 1/ moveSpeed 
    private IEnumerator MovePlayer(Vector3 startPos, Vector3 endPos)
    {
        isMovingToTarget = true;

        float timeElapsed = 0;
        float duration = 1 / moveSpeed;

        AudioManager.inst.PlaySound(playerMoveSFX, Sounds.inst.playerMoveVolume);
        while (timeElapsed < duration)
        {
            //The lerping happens in the while loop
            transform.position = Vector3.Lerp(startPos, endPos, timeElapsed / duration);
            timeElapsed += Time.deltaTime;

            yield return null;
        }


        //Once the player has completely shifted to the target location, we're gonna run a function that process tile effects
        //We also make sure to set the position of the object to the exact endPos in case it's off by a very tiny amount
        //This tiny amounts can compound over time so this is necessary
        transform.position = endPos;
        ProcessTileEvents();
    }

    private void ProcessTileEvents()
    {
        //this function checks if the current tile is a trap tile
        if (currentTile.isTrap)
        {
            PlayerHit?.Invoke();
            AudioManager.inst.PlaySound(enteringTrapSFX, Sounds.inst.enterTrapVolume);
            //If it is, move the player back to the last tile they were on
            StartCoroutine(FlashPlayer());
            //And we call the Camera shake function, passing it the shake duration we want (it's currently 1/4 a sec) 
            CameraShake.inst.Shake(0.25f);

            EnteredTrap(direction);

            //This step is also necessary to make sure out place on the grid gets properly updated
            SetTargetTile(lastTile);
        }
        else if (currentTile.isGoal)
        {
            isMovingToTarget = true;

            //Fire an event notifying any script that has registered for this event that the Player finished moving
            PlayerMoved?.Invoke();

            //When we reach the goal, stop the music and play the goal reached sound
            AudioManager.inst.StopMusic();
            AudioManager.inst.PlaySound(goalReachedSFX, Sounds.inst.goalReachedVolume);

            //If there is no goal reached sound then just restart immediately, otherwise invoke a restart with a delay the goal clip length
            if (goalReachedSFX == null) GameManager.inst.RestartGame();
            else
            {
                var gm = GameManager.inst;
                gm.Invoke("RestartGame", goalReachedSFX.length);
            } 
        }
        else
        {
            isMovingToTarget = false;
            //Fire an event notifying any script that has registered for this event that the Player finished moving
            PlayerMoved?.Invoke();
            if (!IsMoving())
            {
                MovingFinished();
            }
            
        }
    }


    //This coroutine just flashs the player red when they are hit by a trap
    private IEnumerator FlashPlayer()
    {
        rend.color = impactColor;
        yield return blinkDuration;
        rend.color = color;
        yield return blinkDuration;
        rend.color = impactColor;
        yield return blinkDuration;
        rend.color = color;
        yield return blinkDuration;
        rend.color = impactColor;
        yield return blinkDuration;
        rend.color = color;

    }

    private bool IsMoving()
    {
        return Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || 
               Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) ||
               Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) ||
               Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);

    }

    private Vector3 GetStartDirection()
    {
        Vector3 result = Vector3.zero;
        switch (visuals.playerStartingDirection)
        {
            case Direction.UP:
                result = Vector3.up;
                break;
            case Direction.DOWN:
                result = Vector3.down;
                break;
            case Direction.LEFT:
                result = Vector3.left;
                break;
            case Direction.RIGHT:
                result = Vector3.right;
                break;
        }
        return result;
    }
}
