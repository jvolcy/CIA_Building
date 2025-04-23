using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningCredits : MonoBehaviour
{
    [SerializeField] GameObject OpeningCreditsUIPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        OpeningCreditsUIPanel.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        OpeningCreditsUIPanel.SetActive(false);
    }
}
