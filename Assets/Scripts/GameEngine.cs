using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEngine : MonoBehaviour
{
    //static GameEngine Instance = new GameEngine();
    public GameObject RootObject;
    public GameObject DnaChain;
    public GameObject DnaPair;

    void Start()
    {
        InitChain();
    }

    private void InitChain()
    {
        var rootObject = Instantiate(RootObject);
        var dnaChainObject = Instantiate(DnaChain);
        dnaChainObject.transform.parent = rootObject.transform;

        var counter = 0;
        var rotationY = 0.0f;
        var positionY = 0.0f;
        while (counter < 5)
        {
            var dnaPair = InstantiateDnaPair();
            dnaPair.transform.localPosition = new Vector3(0, positionY, 0);
            dnaPair.transform.localRotation = new Quaternion(0, rotationY, 0, 0);
            dnaPair.transform.parent = dnaChainObject.transform;
            counter++;
            rotationY += 90.0f;
            positionY += 2.5f;
        }
    }

    private GameObject InstantiateDnaPair()
    {
        var dnaPair = Instantiate(DnaPair);
        //var childrenComponents = dnaPair.GetComponentsInChildren<Material>();

        // modify materials

        return dnaPair;
    }
}
