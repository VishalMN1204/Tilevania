using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenePersists : MonoBehaviour
{
    void Awake()
    {
        int numScenesPersists = FindObjectsOfType<ScenePersists>().Length;
        if (numScenesPersists > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ResetScenePersist()
    {
        Destroy(gameObject);
    }
}
