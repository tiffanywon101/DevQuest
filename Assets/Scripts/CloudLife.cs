using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloudLife : MonoBehaviour
{
    private int _CloudLife;
    public GameObject scatter_effect;
    public GameObject clouds;
    ParticleSystem scatterParticle;
    public bool scatterisOn;
 
    //public GameObject cloudPrefab;
    void Start()
    {
        
        scatterParticle = GameObject.Find("Scatter_vfx").GetComponent<ParticleSystem>();
        scatterParticle.Stop();
        _CloudLife = 100;
        //newcloud = Instantiate(cloudPrefab, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Bullet(Clone)")
        {
            if (_CloudLife > 0)
            {
                _CloudLife -=10;
                

            }
            else
            {
                scatterParticle.Play();
                Destroy(gameObject, 3.0f);
                
            }
        }
       
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
