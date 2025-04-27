using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class spawningScript : MonoBehaviour
{
    public GameObject lastChip;
    bool inPlace = false;
    private Vector3 spawnPosition;
    public Vector2 velocity;
    public Vector2 settled = new Vector2(0f,0f);
    Rigidbody2D lastChipRb;
    Transform tran;
    [SerializeField] Vector2 position1;
    [SerializeField] Vector2 position2;
    [SerializeField] Vector2 position3;
    [SerializeField] Vector2 position4;
    [SerializeField] Vector2 position5;
    [SerializeField] Vector2 position6;
    [SerializeField] Vector2 position7;
    // Start is called before the first frame update
    void Start()
    {
      tran = GetComponent<Transform>();
      spawnPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
       
        
    }

    public bool isSettled()
    {
        if (lastChip != null)
        {
            velocity = lastChipRb.velocity;
            return velocity == settled;
        }
        return true;
        
    }

    public void dropAt(GameObject chip,int pos)
    {
        switch (pos)
        {
            case 1:
                spawnPosition = position1;
                break;
            case 2:
                spawnPosition = position2;
                break;
            case 3:
                spawnPosition = position3;
                break;
            case 4:
                spawnPosition = position4;
                break;
            case 5:
                spawnPosition = position5;
                break;
            case 6:
                spawnPosition = position6;
                break;
            case 7:
                spawnPosition = position7;
                break;
        }
        SpawnChip(chip);
    }

    public void SpawnChip(GameObject chip)
    {
        tran.position = spawnPosition;
        chip.transform.position = spawnPosition;
        lastChip = Instantiate(chip);
        lastChipRb = lastChip.GetComponent<Rigidbody2D>();
        inPlace = true;
    }

    
}
