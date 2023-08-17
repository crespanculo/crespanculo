using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;

    public ContactFilter2D movementFilter;

    Vector2 movementInput;
    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer spriteRenderer;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if(movementInput != Vector2.zero)
        {
            bool success = TryMove(movementInput);

            if(!success)
            {
                success = TryMove(new Vector2(movementInput.x, 0));

                if(!success)
                {
                    TryMove(new Vector2(movementInput.x, 0));
                }
            }

            if(movementInput.x < 0)
            {
                animator.SetBool("isMovingLeft", success);
                animator.SetBool("isMoving", false);
                animator.SetBool("isMovingUp", false);
                animator.SetBool("isMovingRight", false);
                spriteRenderer.flipX = true;
            }
            else if(movementInput.x > 0)
            {
                animator.SetBool("isMovingRight", success);
                animator.SetBool("isMoving", false);
                animator.SetBool("isMovingUp", false);
                animator.SetBool("isMovingLeft", false);

                spriteRenderer.flipX = false;
            }
            else if(movementInput.y > 0)
            {
                animator.SetBool("isMovingUp", success);
                animator.SetBool("isMoving", false);
                animator.SetBool("isMovingLeft", false);
                animator.SetBool("isMovingRight", false);

                spriteRenderer.flipX = false;
            }
            else if(movementInput.y < 0){
                animator.SetBool("isMoving", success);
                animator.SetBool("isMovingUp", false);
                animator.SetBool("isMovingLeft", false);
                animator.SetBool("isMovingRight", false);
                spriteRenderer.flipX = false;
            }
        }else
        {
            animator.SetBool("isMoving", false);
            animator.SetBool("isMovingUp", false);
            animator.SetBool("isMovingLeft", false);
            animator.SetBool("isMovingRight", false);
        }
    }

    private bool TryMove(Vector2 direction)
    {
        int count = rb.Cast(movementInput, movementFilter, castCollisions, moveSpeed * Time.fixedDeltaTime + collisionOffset);
            
        if(count == 0)
        {
            rb.MovePosition(rb.position + movementInput * moveSpeed * Time.fixedDeltaTime);
            return true;
        }else return false;
            
    }

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }
}
