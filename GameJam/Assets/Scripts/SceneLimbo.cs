using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLimbo : MonoBehaviour
{
    [SerializeField] PlayerController2 player;
    [SerializeField] GameObject camioneta;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player") && name.Equals("camioneta"))
        {
            SceneManager.LoadScene("Limbo");
        }
        if (other.tag.Equals("Player") && name.Equals("Stoping"))
        {
            player.move = false;
            camioneta.SetActive(true);
        }
    }
}
