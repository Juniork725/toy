using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Star star;
    private void Awake()
    {
        if (this.gameObject.name.EndsWith('A')){
            star.portalA = this.gameObject;
        }
        else
        {
            star.portalB = this.gameObject;
        }
    }
}
