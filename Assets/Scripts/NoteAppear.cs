using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NoteAppear : MonoBehaviour
{
    [SerializeField]
    private Image noteImage;
    [SerializeField]
    private TextMeshProUGUI text;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            text.enabled = true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
        {
            noteImage.enabled = !noteImage.enabled;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            text.enabled = false;
            noteImage.enabled = false;
        }
    }
}
