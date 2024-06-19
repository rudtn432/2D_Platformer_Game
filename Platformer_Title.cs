using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Platformer_Title : MonoBehaviour
{
    [SerializeField] Button btnStart;

    void Start()
    {
        btnStart.onClick.AddListener(() => {
            SceneManager.LoadScene("2DPlatformer_Main");
        });
    }

    void Update()
    {
        
    }
}
