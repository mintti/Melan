using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class WorldController : MonoBehaviour
{
    public GameObject world;
    public Button _bt;
    

    public void Click()
    {
        _bt.onClick.Invoke();
    }
}
