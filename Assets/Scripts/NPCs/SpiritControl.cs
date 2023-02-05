using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritControl : MonoBehaviour
{

    // Variables
    public float x_offset;
    public float y_offset;

    
    public GameObject player;
    public BoxCollider2D boxCollider;
    public SpriteRenderer spriteRenderer;
    public Sprite spiritSprite;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        // Get components and player
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
    }

    // On trigger enter
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Change sprite to spirit sprite
            changeSprite();
            this.transform.parent = player.transform;
            // Remove box collider
            boxCollider.enabled = false;
            // Disable animator
            animator.enabled = false;
            reposition();
        }
    }

    // Change sprite method
    void changeSprite() {
        spriteRenderer.sprite = spiritSprite;
    }

    void reposition()
    {
        // Move spirit to player and make it hover over the player
        this.transform.position = player.transform.position;
        this.transform.position = new Vector3(this.transform.position.x +x_offset, this.transform.position.y + y_offset, this.transform.position.z);
    }

}
