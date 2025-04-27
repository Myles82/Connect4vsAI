using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class HandScript : MonoBehaviour
{
    bool touching = false;  
    bool holding = false;
    GameObject chip = null;
    // Start is called before the first frame update
    void Start()
    {
       Collider2D collider = GetComponent<Collider2D>(); 
    }

    // Update is called once per frame
    void Update()
    {
        FollowMouse();
        
        
        
        
       
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            if (touching)
            {
                Vector2 direction = new Vector2(transform.position.x - chip.transform.position.x, transform.position.y - chip.transform.position.y);
                chip.GetComponent<Rigidbody2D>().velocity = direction*20;            
            }
        }
    }

    private void FollowMouse()
    {
        Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        position = Camera.main.ScreenToWorldPoint(position);
        transform.position = new Vector3(position.x, position.y, 0);
    }

    private void Grab()
    {
        if (touching)
        {
            if (chip.GetComponent<Rigidbody2D>() != null)
            {
               holding = true;
            }
        }
    }

    private void UnGrab()
    {
        holding = false;   
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("chip")&&collision.GetComponent<chipScript>().Loose)
        {
            touching = true;
            chip = collision.gameObject;
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        touching = false;
        chip = null;
        holding = false;
    }
}
