using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void MoveToMain()
    {
        SceneManager.LoadScene("1.Main");
    }
   
    
}
