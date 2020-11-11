using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Splash : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LeaderboardManager.instance();
        StartCoroutine(LoadAsynchronously());
    }

    IEnumerator LoadAsynchronously()
    {

        float timer = 0f;
        float minLoadTime = 0f;

        AsyncOperation operation = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            timer += Time.deltaTime;

            if (timer > minLoadTime)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }

        yield return null;

    }
}
