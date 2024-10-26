using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    [SerializeField]
    private Transform thing;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
            thing.Translate(0, 0, 0.05f);

        if (Input.GetKey(KeyCode.S))
            thing.Translate(0, 0, -0.05f);
    }
}
