using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuigiAnimScript : MonoBehaviour
{
    // Maybe make the animations loop thru exit & entry
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown("space")) {
            Jump();
        }

        if (Input.GetKeyDown("w")) {
            Run();
        }

        if (Input.GetKeyUp("w")) {
            StopRun();
        }

        if (Input.GetKeyDown("c")) {
            Crouch();
        }

        if (Input.GetKeyUp("c")) {
            StopCrouch();
        }
    }

    [ContextMenu("Do Run")]
    void Run()
    {
        animator.SetFloat("Speed", 1f);
    }

    [ContextMenu("Stop Run")]
    void StopRun()
    {
        animator.SetFloat("Speed", 0f);
    }

    [ContextMenu("Crouch")]
    void Crouch()
    {
        animator.SetBool("Crouching", true);
    }

    [ContextMenu("StopCrouch")]
    void StopCrouch()
    {
        animator.SetBool("Crouching", false);
    }

    [ContextMenu("Jump")]
    void Jump()
    {
        animator.SetTrigger("Jump");
    }
}
