using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<TextMeshProUGUI>().text = FindObjectOfType<Player>().GetHealth().ToString();
    }

    public void UpdateHealth()
    {
        gameObject.GetComponent<TextMeshProUGUI>().text = FindObjectOfType<Player>().GetHealth().ToString();
    }
    
}
