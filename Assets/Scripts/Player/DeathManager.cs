using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathManager : MonoBehaviour
{
    private SpriteRenderer playerRenderer;
    private PlayerController playerController;
    private Rigidbody2D playerBody;

    void Start() {
        playerRenderer = GetComponentInChildren<SpriteRenderer>();
        playerController = GetComponent<PlayerController>();
        playerBody = GetComponent<Rigidbody2D>();
    }

    public void Respawn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void KillPeaks()
    {
        playerRenderer.color = Color.red;
        playerController.enabled = false;
        StartCoroutine(KillCoolDown());
    }
    public void KillHammer(Vector2 velocity)
    {
        playerRenderer.color = Color.red;
        playerController.enabled = false;
        playerBody.AddForce(velocity * 1000);
        Debug.Log(velocity);
        StartCoroutine(KillCoolDown());
    }
    public void KillRock()
    {
        playerRenderer.color = Color.red;
        playerController.enabled = false;
        transform.localScale = new Vector3(transform.lossyScale.x, 0.2f, transform.lossyScale.z);
        StartCoroutine(KillCoolDown());
    }

    IEnumerator KillCoolDown()
    {
        yield return new WaitForSeconds(1.0f);
        Respawn();
    }
}
