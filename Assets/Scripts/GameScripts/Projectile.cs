using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    [HideInInspector] public float speed;
    [HideInInspector]public Vector3 direction;
    private SpriteRenderer rend;
    private VisualProperties.ProjectileVisuals visuals;
    private Color color;
    private Sprite sprite;
    private RuntimeAnimatorController animator;
    private TrailRenderer trail;


    public void Init()
    {

        rend = GetComponentInChildren<SpriteRenderer>();
        visuals = VisualProperties.inst.projectileVisuals;

        if (visuals.animController != null)
        {
            animator = visuals.animController;
            var anim = gameObject.AddComponent<Animator>();
            anim.runtimeAnimatorController = animator;
        }
        else if (visuals.sprite != null)
        {
            sprite = visuals.sprite;
            rend.sprite = sprite;
            if (visuals.spriteColorOverride)
            {
                color = visuals.color;
                rend.color = color;
            }
        }
        else
        {
            color = visuals.color;
            rend.color = color;
        }

        if (visuals.enableTrail)
        {
            trail = GetComponent<TrailRenderer>();
            trail.enabled = true;
        }
    }


    void Update()
    {
        if (rend != null && rend.isVisible)
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }
        else
        {
            Destroy(gameObject);
        }     
    }
}
