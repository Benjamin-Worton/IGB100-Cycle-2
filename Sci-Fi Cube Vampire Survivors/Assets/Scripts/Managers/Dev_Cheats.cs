using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dev_Cheats : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Player.instance.scrap += 10;
            ScrapCounter.instance.AddScrap(10);
        }
    }
}
