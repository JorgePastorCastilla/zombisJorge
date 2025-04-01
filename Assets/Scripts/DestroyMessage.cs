using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMessage : MonoBehaviour
{
    public float messageLifeTime = 30f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, messageLifeTime);
    }
}
