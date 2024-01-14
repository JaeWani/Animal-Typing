using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMovementManager : MonoBehaviour
{
    public static SceneMovementManager instance;
    private Transitioner transitioner;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        transitioner = GetComponent<Transitioner>();

        transitioner._transitionCamera = Camera.main;
    }
    public static void SceneMovement(string sceneName) => instance.transitioner.TransitionToScene(sceneName);
}
