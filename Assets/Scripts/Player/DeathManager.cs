using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathManager : MonoBehaviour
{
    private SpriteRenderer playerRenderer;

    void Start() {
        playerRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void Respawn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void KillPeaks()
    {
        playerRenderer.color = Color.red;
        StartCoroutine(KillCoolDown());
    }
    
    IEnumerator KillCoolDown()
    {
        yield return new WaitForSeconds(1.0f);
        Respawn();
    }
}
