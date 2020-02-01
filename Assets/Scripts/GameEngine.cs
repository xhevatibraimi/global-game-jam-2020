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
        var dnaPair = InstantiateDnaPair();
        dnaPair.transform.parent = rootObject.transform;

    }

    private GameObject InstantiateDnaPair()
    {
        var dnaPair = Instantiate(DnaPair);
        var childrenComponents = dnaPair.GetComponentsInChildren<Material>();
        
        // modify materials

        return dnaPair;
    }
}
