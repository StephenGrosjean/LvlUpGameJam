using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerHandle : MonoBehaviour
{
    [SerializeField] private Hammer hammer;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            hammer.HandleTouchGround();
        }
    }
}
