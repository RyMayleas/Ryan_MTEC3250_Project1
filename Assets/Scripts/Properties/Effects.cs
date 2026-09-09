using UnityEngine;
using System.Collections.Generic;
//using System.Numerics;

public class Effects : MonoBehaviour
{
    private PlayerControl player;
    public LineRenderer pathRenderer;
    public Gradient pathRendererColor; 
    [Space(10)]
    public ParticleSystem crateDestroyedParticles;
    public int crateParticleCountMin;
    public int crateParticleCountMax;
    [Space(10)]
    public ParticleSystem projectileFireParticles;
    public int projectileFireParticleCountMin;
    public int projectileFireParticleCountMax;

    [Space(10)]
    public ParticleSystem projectileImpactParticles;
    public int projectileParticleCountMin;
    public int projectileParticleCountMax;
    [Space(10)]
    public ParticleSystem playerHitParticles;
    public int playerHitParticleCountMin;
    public int playerHitParticleCountMax;

    private List<Vector3> playerTilePath = new List<Vector3>();


    private void OnEnable()
    {
        Tile.CrateDestroyed += TriggerParticlesOnCreateDestroyed;
        Tile.ProjectileHit += TriggerProjectileExplosion;
        PlayerControl.PlayerHit += TriggerPlayerHit;
        PlayerControl.PlayerMoved += DrawPlayerPath;
        PlayerControl.ProjectileFired += TriggerProjectileFire;


    }

    private void OnDisable()
    {
        Tile.CrateDestroyed -= TriggerParticlesOnCreateDestroyed;
        Tile.ProjectileHit -= TriggerProjectileExplosion;
        PlayerControl.PlayerHit -= TriggerPlayerHit;
        PlayerControl.PlayerMoved -= DrawPlayerPath;
        PlayerControl.ProjectileFired -= TriggerProjectileFire;
    }

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerControl>();

        if (pathRenderer != null)
        {
            DrawPlayerPath();
        }
        
    }

    void Update()
    {


    }

    private void TriggerProjectileFire(Vector3 pos)
    {
        if (projectileFireParticles == null) return;

        projectileFireParticles.transform.position = pos;


        if (projectileFireParticleCountMax > projectileFireParticleCountMin)
        {
            projectileFireParticles.Emit(Random.Range(projectileFireParticleCountMin, projectileFireParticleCountMax));

        }
        else
        {
            projectileFireParticles.Emit(projectileFireParticleCountMin);

        }


    }

    private void TriggerParticlesOnCreateDestroyed(Tile tile)
    {
        if (crateDestroyedParticles == null) return;

        crateDestroyedParticles.transform.position = tile.transform.position;

        if (crateParticleCountMax > crateParticleCountMin)
        {
            crateDestroyedParticles.Emit(Random.Range(crateParticleCountMin, crateParticleCountMax));

        }
        else
        {
            crateDestroyedParticles.Emit(crateParticleCountMin);

        }

    }

    private void TriggerProjectileExplosion(Vector3 pos)
    {
        if (projectileImpactParticles == null) return;
  
        projectileImpactParticles.transform.position = pos;
        if (projectileParticleCountMax > projectileParticleCountMin)
        {
            projectileImpactParticles.Emit(Random.Range(projectileParticleCountMin, projectileParticleCountMax));

        }
        else
        {
            projectileImpactParticles.Emit(projectileParticleCountMin);
            
        }
       
    }

    private void TriggerPlayerHit()
    {
        if (playerHitParticles == null) return;
        playerHitParticles.transform.SetParent(player.transform);
        playerHitParticles.transform.localPosition = Vector3.zero;
        //playerHitParticles.transform.position = player.transform.position;

        if (playerHitParticleCountMax > playerHitParticleCountMin)
        {
            playerHitParticles.Emit(Random.Range(playerHitParticleCountMin, playerHitParticleCountMax));

        }
        else
        {
            playerHitParticles.Emit(playerHitParticleCountMin);

        }

    }

    private void DrawPlayerPath()
    {
        if (pathRenderer == null) return;

        pathRenderer.colorGradient = pathRendererColor;

        if (!playerTilePath.Contains(player.CurrentTile.transform.position))
        {

            playerTilePath.Add(player.CurrentTile.transform.position);
            pathRenderer.positionCount = playerTilePath.Count;
        }


        pathRenderer.SetPositions(playerTilePath.ToArray());


    }


}
