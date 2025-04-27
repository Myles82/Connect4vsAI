using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class chipScript : MonoBehaviour
{
    [SerializeField] public int playerNum = 0;
    public bool Loose = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

 
    public (int,int) getGridPosition()
    {
        float radius = GetComponent<CircleCollider2D>().radius;
        Vector2 center = transform.position;

        Collider2D[] hits = Physics2D.OverlapCircleAll(center, radius);
        
        int row = 0;
        int col = 0;
        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject.CompareTag("row")){
                row = hit.gameObject.GetComponent<detectorScript>().getNum();
            }
            else if (hit.gameObject.CompareTag("column"))
            {
                col = hit.gameObject.GetComponent<detectorScript>().getNum();
            }
        }
        return (row, col);
        
       
    }

}
