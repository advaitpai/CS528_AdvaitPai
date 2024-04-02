using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetSolScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject person_camera1;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void resetPostion()
    {
        person_camera1.transform.position = new Vector3(0,1,0);
    }
}
