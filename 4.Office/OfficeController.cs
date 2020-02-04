using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeController : MonoBehaviour
{
    public OfficeGage officeGage;

    public void Click()
    {
        officeGage.SetGage();
    }
}
