using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{

    [SerializeField] AudioClip coinPickUpSFX;
    bool wasCollected = false;
    [SerializeField] int pointsForCoinPickup = 100;
    void Start()
    {

    }


    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!wasCollected)
        {
            wasCollected = true;
            AudioSource.PlayClipAtPoint(coinPickUpSFX, Camera.main.transform.position);
            FindObjectOfType<GameSession>().IncreaseScore(pointsForCoinPickup);
            // ask for which audio clip and the position from where audio clip plays
            gameObject.SetActive(false);
            Destroy(gameObject);
        }

    }
}
