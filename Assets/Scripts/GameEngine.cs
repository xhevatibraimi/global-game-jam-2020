using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEngine : MonoBehaviour
{
    //static GameEngine Instance = new GameEngine();
    public GameObject FallingObject;
    public GameObject RootObject;
    public GameObject DnaChain;
    public GameObject DnaPair;
    public Material RedMaterial;
    public Material GreenMaterial;
    public Material BlueMaterial;
    public Material YellowMaterial;
    public Material FrameMaterial;
    public int NumberOfDnaPairs = 1;
    public float RotatingSpeed;
    public float YSpeed;
    public float DnaChainInitialPosition;


    private List<Material> Materials = new List<Material>();
    private Material previousMaterial = null;
    private System.Random random = new System.Random();
    void Start()
    {
        InitChain();
    }


    private void InitChain()
    {
        Materials.Add(RedMaterial);
        Materials.Add(GreenMaterial);
        Materials.Add(BlueMaterial);
        Materials.Add(YellowMaterial);
        var randomNumber = random.Next(0, 4);
        previousMaterial = Materials[randomNumber];

        var rootObject = Instantiate(RootObject);
        var fallingObject = Instantiate(FallingObject);
        fallingObject.transform.parent = rootObject.transform;
        var dnaChainObject = Instantiate(DnaChain, new Vector3(0, DnaChainInitialPosition, 0), new Quaternion(), fallingObject.transform);

        var counter = 0;
        var rotationY = 0.0f;
        var positionY = 0.0f;
        bool isOddDnaPairPosition = false;
        while (counter < NumberOfDnaPairs)
        {
            isOddDnaPairPosition = !isOddDnaPairPosition;
            var dnaPair = InstantiateDnaPair();
            dnaPair.transform.localPosition = new Vector3(0, positionY, 0);
            dnaPair.transform.localRotation = Quaternion.Euler(0, rotationY, 0);
            dnaPair.transform.parent = dnaChainObject.transform;

            var childRenderers = dnaPair.GetComponentsInChildren<Renderer>();

            foreach (var renderer in childRenderers)
            {
                bool isLeftBridge = renderer.gameObject.tag =="BridgeLeft";
                bool isRightBridge = renderer.gameObject.tag =="BridgeRight";

                // isBridge
                if (isLeftBridge || isRightBridge)
                {
                    if (isOddDnaPairPosition)
                    {
                        renderer.material = GetRandomMaterial();
                    }
                    else
                    {
                        #region Create Variation
                        var randomNumber1to3 = random.Next(0, 3);

                        // left missing
                        if (randomNumber == 1)
                        {
                            renderer.material = GetRandomMaterial();
                            if (isLeftBridge)
                            {
                                renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 127);
                            }
                        }
                        // right missing
                        else if (randomNumber == 2)
                        {
                            renderer.material = GetRandomMaterial();
                            if (isRightBridge)
                            {
                                renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 127);
                            }
                        }
                        // both missing
                        else if (randomNumber == 3)
                        {
                            renderer.material = GetRandomMaterial();
                            renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 127);
                        }
                        #endregion
                    }
                }
                else
                {
                    // frame
                    renderer.material = FrameMaterial;
                }
            }

            counter++;
            rotationY -= RotatingSpeed;
            positionY += YSpeed;
        }
    }

    private Material GetRandomMaterial()
    {
        Material material = null;
        do
        {
            var randomNumber = random.Next(0, 4);
            material = Materials[randomNumber];
        } while (material == previousMaterial);
        previousMaterial = material;
        return material;
    }

    private GameObject InstantiateDnaPair()
    {
        var dnaPair = Instantiate(DnaPair);


        return dnaPair;
    }
}
