using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadSceneManager : MonoBehaviour
{
    private static LoadSceneManager instance;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    public static LoadSceneManager Instance
    {
        get 
        {
            if (instance == null)
            {
                instance = new GameObject("LoadSceneManger").AddComponent<LoadSceneManager>();
            }
            return instance;
        }

    }

   
    public void LoadSceneAsync(string sceneName , Action onCompleted)
    {
        StartCoroutine(loadSceneAsync(sceneName, () =>  onCompleted?.Invoke()));
        
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
