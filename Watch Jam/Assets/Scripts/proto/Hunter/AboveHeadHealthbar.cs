using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AboveHeadHealthbar : MonoBehaviour
    
{
    public GameObject HealthUI;
    public Image HealthBar;
    public Sprite[] BarColors;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateUI(float Life)
    {
        StartCoroutine(FlashHealth());
        float Percentage = Life / Settings.s.totalLife;
        HealthBar.rectTransform.anchoredPosition = Vector3.Lerp(new Vector3(-194, -79.9f, 0), new Vector3(-2.3f, 5.6f, 0), Percentage);

        if (Percentage > .67)
        {
            HealthBar.sprite = BarColors[0];
        }

        else if (Percentage > .34)
        {
            HealthBar.sprite = BarColors[1];
        }

        else if (Percentage < .34)
        {
            HealthBar.sprite = BarColors[2];
        }

        

    }

    IEnumerator FlashHealth()
    {

        HealthUI.SetActive(true);
        yield return new WaitForSeconds(1);
        HealthUI.SetActive(false);
    }
}
