using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadSceneManager : MonoBehaviour
{

    private string scneName;

    public void LoadSceneAsync(string sceneName)
    {
        StartCoroutine(sceneName);
    }




    public IEnumerator loadSceneAsync(string sceneName , Action onCompleted)
    {

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scneName);

        yield return new WaitUntil(() => asyncOperation.isDone); //끝날때 까지 기다림
    
        
    
    }



    
    

}
