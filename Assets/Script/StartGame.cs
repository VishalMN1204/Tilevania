using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    [SerializeField] AudioClip levelStartSFX;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadStartGame()
    {
        SceneManager.LoadScene(1);
        AudioSource.PlayClipAtPoint(levelStartSFX, Camera.main.transform.position);
    }
}
