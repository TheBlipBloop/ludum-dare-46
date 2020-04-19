using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MildlyWackAssMenu : MonoBehaviour
{
    public string firstLevel = "Game 0";

    float switchTime = -1;

    bool switching;

    float switchTimer = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (switching)
        {
            switchTimer += Time.deltaTime;
        }

        if (switchTimer > 2f)
        {
            switchTime = Mathf.Infinity;
            SceneManager.LoadScene(firstLevel);
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(firstLevel);
        }
    }

    public void StartGameOrSomthing()
    {
        switching = true;

        // switchTime = Time.time + 5f;
    }
}
