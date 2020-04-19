using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MildlyWackAssMenu : MonoBehaviour
{
    public string firstLevel = "Game 0";

    float switchTime;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (switchTime > Time.time)
        {
            switchTime = Mathf.Infinity;
            SceneManager.LoadScene(firstLevel);
        }
    }

    public void StartGameOrSomthing()
    {
        switchTime = Time.time + 0.6f;
    }
}
