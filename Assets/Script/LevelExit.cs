using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelExit : MonoBehaviour
{
    [SerializeField] float loadLevelDelay = 1f;

    [SerializeField] AudioClip levelCompleteSFX;
    [SerializeField] AudioClip levelStartSFX;

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine("nextLevel");
    }

    IEnumerator nextLevel()
    {
        yield return new WaitForSeconds(loadLevelDelay);
        int nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if(nextLevelIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextLevelIndex = 0;
        }
        AudioSource.PlayClipAtPoint(levelCompleteSFX, Camera.main.transform.position);
        FindObjectOfType<ScenePersists>().ResetScenePersist();
        SceneManager.LoadScene(nextLevelIndex);
        AudioSource.PlayClipAtPoint(levelStartSFX, Camera.main.transform.position);
    }
}
