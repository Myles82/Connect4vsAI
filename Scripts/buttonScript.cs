
using UnityEngine;


public class buttonScript : MonoBehaviour
{
    [SerializeField] GameObject chip;
    bool touching = false;
    [SerializeField] GameObject spawner;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (touching)
        {
            if(Input.GetMouseButtonDown(0)) {
                activate();
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        touching = true;    
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        touching = false;
    }

    private void activate()
    {
        spawner.GetComponent<spawningScript>().SpawnChip(chip);
    }
}
