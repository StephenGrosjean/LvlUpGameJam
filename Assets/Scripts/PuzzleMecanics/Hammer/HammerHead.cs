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
    }


}
