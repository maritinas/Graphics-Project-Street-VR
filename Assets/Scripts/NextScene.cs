using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    public Color blackScreen;
    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        Debug.Log("Current Scene: " + SceneManager.GetActiveScene().name + "\n" + " Scene Index: #" + SceneManager.GetActiveScene().buildIndex);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            //StartCoroutine(BlackScreen());
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            //StartCoroutine(BlackScreen());
            //StartCoroutine(BlackScreen(-1));
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            rend.material.SetColor("_Color", blackScreen);
        }

    }

    /* public IEnumerator BlackScreen(float x)
     {
         float timer = 0;

         while (timer < 5)
         {
             rend.material.SetColor("_Color", fadeColor);

             timer += Time.deltaTime;
             yield return null;
         }

         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + x);

     }*/

    public IEnumerator BlackScreen()
    {
        float timer = 0;

        while (timer < 5)
        {
            rend.material.SetColor("_Color", blackScreen);

            timer += Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }


}

