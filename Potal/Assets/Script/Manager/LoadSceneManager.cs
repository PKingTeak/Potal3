using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadSceneManager : MonoBehaviour
{

    private static LoadSceneManager instance;

    public static LoadSceneManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LoadSceneManager>();
                if (instance == null)
                {
                    instance = new GameObject("LoadSceneManager").AddComponent<LoadSceneManager>();
                }
                
            }
            return instance;
        }
    }



    private void Awake()
    {
        if (instance == null)
        {

            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }



    public void LoadSceneAsync(string sceneName , Action onCompleted)
    {
        StartCoroutine(loadSceneAsync(sceneName,onCompleted));
        
    }




    public IEnumerator loadSceneAsync(string sceneName , Action onCompleted)
    {

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        yield return new WaitUntil(() => asyncOperation.isDone); //끝날때 까지 기다림
        onCompleted?.Invoke();



    }

    public void LoadSceneNormalMap(string scenName)
    {
        SceneManager.LoadScene(scenName);
    }



  
    

}
