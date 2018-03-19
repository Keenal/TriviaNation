using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringToFront : MonoBehaviour
{
    //Draws screen on last.
    public void OnEnable()
    {
        transform.SetAsLastSibling();
    }
}
