using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetAreaCol : MonoBehaviour
{
    public TargetArea TA;
    private void OnTriggerEnter2D(Collider2D other) {
        TA.ColIsIn(true);
    }
    private void OnTriggerExit2D(Collider2D other) {
        TA.ColIsIn(false);    
    }
}
