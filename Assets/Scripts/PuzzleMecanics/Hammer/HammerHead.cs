using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerHead : MonoBehaviour
{
    [SerializeField] private Hammer hammer;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PushableBloc"))
        {
            hammer.TouchCase();
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            hammer.HeadTouchGround();
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            if (hammer.active)
            {
                collision.gameObject.GetComponent<DeathManager>().KillHammer(-collision.GetContact(0).normal);
                GetComponent<BoxCollider2D>().isTrigger = true;
            }
        }
    }


}
