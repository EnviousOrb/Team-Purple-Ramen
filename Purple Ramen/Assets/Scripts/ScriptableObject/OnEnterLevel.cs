using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class OnEnterLevel : MonoBehaviour
{

    public GameObject entrance;
    public GameObject exit;

    [SerializeField] public SceneInfo sceneInfo;
    public Vector3 offsset= new Vector3(1,0.5f,0);  
    public Rigidbody rb;
    // Start is called before the first frame update
    void Awake()
    {
        rb= GetComponent<Rigidbody>(); 
    }

    // Update is called once per frame
    void Start()
    {

       GameObject target = sceneInfo.isNextScene ? gameObject : exit;
        
       
    }
}
