﻿using UnityEngine;
using System.Collections;

public class CameraFollowScript : MonoBehaviour
{
    GameObject Camera;
    GameObject DsplitManager;
    DSplitScript DsplitManagerScript;
    public GameObject Partner;
      GameObject CameraPath;
    bool CamMove = false;
     float CamMoveTime = 1;

    private void Start()
    {
        CameraPath = GameObject.FindGameObjectWithTag("CamPath");
        DsplitManager = GameObject.FindGameObjectWithTag("DSplitManager");

        DsplitManagerScript = DsplitManager.GetComponent<DSplitScript>();
        Camera = GameObject.FindGameObjectWithTag("MainCamera");
    }
    private void Update()
    {
         if (CamMove)
        {
           if(DsplitManagerScript.LEVELTRACK <= CameraPath.transform.childCount)
            {
                CamMoveTime -= Time.deltaTime;
                Partner.transform.position = transform.position + new Vector3 (-1.5f ,0,0);
                // Camera.transform.position = Vector3.MoveTowards(Camera.transform.position, CameraPath.GetComponentInChildren[DsplitManagerScript.LEVELTRACK].transform.position, 0.5f);
                Camera.transform.position = Vector3.MoveTowards(Camera.transform.position, CameraPath.transform.GetChild(DsplitManagerScript.LEVELTRACK).transform.position, 0.5f);
            }
           if(CamMoveTime <= 0)
            {
                CamMove = false;
                CamMoveTime = 1;
            }
        }
     
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "EndLevel")
        {
            Destroy(other.gameObject);
            DsplitManagerScript.LEVELTRACK++;

            CamMove = true;
         }

    }
}