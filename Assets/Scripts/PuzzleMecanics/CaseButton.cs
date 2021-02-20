using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaseButton : MonoBehaviour
{
    bool keepPressed = true;

    private Animator myAnimator;

    void Start() {
        myAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PushableBloc"))
        {
            OnButtonActionDown();
            myAnimator.SetBool("Pressed", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PushableBloc") && !keepPressed)
        {
            OnButtonActionUp();
            myAnimator.SetBool("Pressed", false);
        }
    }

    protected virtual void OnButtonActionDown()
    {

    }

    protected virtual void OnButtonActionUp()
    {

    }

}
