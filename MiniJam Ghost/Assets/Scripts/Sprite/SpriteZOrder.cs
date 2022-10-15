using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/*
    Use to properly choose what's in front and what's in the back when moving up or down
    Need object o be in the SAME sorting layer and order
*/

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteZOrder : MonoBehaviour
{
    [Header("Optional: set sprite origin offset y (normally to the \"feet\" of sprite)")]
    //[SerializeField] private Transform optionalSpriteOrigin;
    [SerializeField] private float originOffsetY;
    public static float ratioZPerY = 0.5f;

    [Header("Optional: Child sprites will have the same Z order as parent")] 
    [SerializeField] private SpriteRenderer[] optionalChildSprites;
    List<SpriteRenderer> sprites;

    [Header("Optional: Objects goes translucent when player is behind")]
    [SerializeField] private bool activateAlphaAdjustment = false;
    [Range(0f,1f)][SerializeField] private float alphaDecreaseFactor = 0.5f;
    private float playerOriginYOffset = -1.04f;
    private float[] defaultAlpha;
    private PlayerMovement player;

    // child z offset (so child that is initially in front of parent still is infront)
    public static float ratioZPerZInitial = 0.01f;
    private float[] spritesZInitial;

    [Header("Optional: Invert Z offset (specially made for easy shotgun Z offset)")]
    public bool invertZOffset = false;
    
    void Start() {
        // find player object to get player position in update
        player = FindObjectOfType<PlayerMovement>();

        // add itself and child to sprites list
        // stores all current alpha as default for all SR in sprites
        sprites = new List<SpriteRenderer> (optionalChildSprites);
        sprites.Insert(0, GetComponent<SpriteRenderer>());
        defaultAlpha = new float[sprites.Count];
        spritesZInitial = new float[sprites.Count];
        for (int i = 0; i < sprites.Count; i++) {
            defaultAlpha[i] = sprites[i].color.a;
            spritesZInitial[i] = sprites[i].transform.position.z;
        }
    }

    void Update() {
        // set origin
        float originY = transform.position.y + originOffsetY;
        //if (optionalSpriteOrigin != null) originY = optionalSpriteOrigin.position.y;
        
        // apply Z transforms
        for (int i = 0; i < sprites.Count; i++) {
            SpriteRenderer sr = sprites[i];
            Vector3 _cpos = sr.transform.position;
            _cpos.z = ratioZPerY * originY;
            if (invertZOffset) {
                _cpos.z -= ratioZPerZInitial * spritesZInitial[i];
            } else {
                _cpos.z += ratioZPerZInitial * spritesZInitial[i];
            }
            sr.transform.position = _cpos;
        }

        // adjust alpha
        AdjustAlphaUpdate();
        Debug.Log(activateAlphaAdjustment);
    }

    void OnDrawGizmosSelected() {
        // set color
        Gizmos.color = Color.yellow;

        // get center
        Vector3 _pos = transform.position;
        _pos.y += originOffsetY;

        // draw
        Gizmos.DrawLine(_pos + Vector3.right * -0.4f, _pos + Vector3.right * 0.4f);
        Gizmos.DrawLine(_pos + Vector3.up * -0.1f, _pos);
    }

    private void AdjustAlphaUpdate() {
        // check null and amount
        if (player != null && activateAlphaAdjustment) {
            Debug.Log("c1");
            
            if (player.transform.position.y + playerOriginYOffset > transform.position.y + originOffsetY) {
                // case player is behind

                // check if the PlayerMovement component have BoxCollider2D with it to check for collision
                BoxCollider2D collider = player.GetComponent<BoxCollider2D>();
                if (collider == null) return;
                Debug.Log("c2");

                // get player's Bounds
                Bounds b1 = collider.bounds;

                // check if player is actually collide with ANY of the objects
                bool collide = false;
                foreach (SpriteRenderer sr in sprites) {
                    Bounds b2 = sr.bounds;

                    // rect rect collision check
                    if (b1.min.x < b2.max.x && b1.max.x > b2.min.x &&
                        b1.min.y < b2.max.y && b1.max.y > b2.min.y) {
                        // collide
                        collide = true;
                        break;
                    }
                }
                // collide result
                if (collide) {
                    // if collide set transparentcy to all objects
                    SetAlphaFactor(alphaDecreaseFactor);
                    Debug.Log("c3");

                    // then return so it don't continue to another case
                    return;
                } else {
                    // if don't collide, do nothing and go to case player's in the front
                }
            }

            // case player is in front
            // set alpha to default
            SetAlphaFactor(1);
        }
    }

    private void SetAlphaFactor(float fac) {
        for (int i = 0; i < sprites.Count; i++) {
            SpriteRenderer sr = sprites[i];
            Color _col = sr.color;
            _col.a = defaultAlpha[i] * fac;
            sr.color = _col;
        }
    }
}
