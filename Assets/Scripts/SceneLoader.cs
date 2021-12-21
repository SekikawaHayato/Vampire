using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : SingletonMonoBehaviour<SceneLoader>
{

    // 非同期動作で使用するAsyncOperation
    AsyncOperation async;
    // シーンロード中に表示するUI画面
    [SerializeField]
    Transform maskImage;
    // 読み込み率を表示するスライダー
    [SerializeField]
    GameObject parent;

    [SerializeField]
    float maxScale;
    [SerializeField]
    float scaleTime;

    private void Start()
    {
       
        //NextScene("Main");
    }
    public void NextScene()
    {
        if (async == null) StartCoroutine(NextSceneCoroutine("Main"));
    }
    public void NextScene(string name){
        if(async==null)StartCoroutine(NextSceneCoroutine(name));
    }

    IEnumerator NextSceneCoroutine(string name)
    {
        float currentScale = 0;
        float scaleSpeed =  maxScale/scaleTime;
        parent.SetActive(true);
        while (currentScale <= maxScale)
        {
            currentScale += scaleSpeed*Time.deltaTime;
            maskImage.localScale = Vector3.one * currentScale;
            yield return null;
        }

        async = SceneManager.LoadSceneAsync(name);
        while (!async.isDone)
        {
            // LoadingEffect
            yield return null;
        }

        while (currentScale > 0 )
        {
            currentScale -= scaleSpeed * Time.deltaTime;
            if (currentScale < 0) currentScale = 0;
            maskImage.localScale = Vector3.one * currentScale;
            yield return null;
        }
        parent.SetActive(false);
        async=null;
    }
}
