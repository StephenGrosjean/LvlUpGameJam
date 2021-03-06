using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonSpawner : MonoBehaviour
{
    [SerializeField] private GameObject moon;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Moon moonObj = Instantiate(moon).GetComponent<Moon>();
            moonObj.layerFadeOut = 0;
        }
    }
}
