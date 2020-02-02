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
    public Material ColorOne;
    public Material ColorTwo;
    public Material ColorThree;
    public Material ColorFour;

    public Material ColorMissing;
    public Material NodeMaterial;
    public int NumberOfDnaPairs = 1;
    public float RotatingSpeed;
    public float YSpeed;
    public float DnaChainInitialPosition;


    private List<Material> Colors = new List<Material>();
    private Material previousMaterial = null;
    private System.Random random = new System.Random();
    void Start()
    {
        InitColors();
        InitChain();
    }

    private void InitChain()
    {
        var randomNumber = random.Next(0, 4);
        previousMaterial = Colors[randomNumber];

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
                bool isLeftBridge = renderer.gameObject.tag == "BridgeLeft";
                bool isRightBridge = renderer.gameObject.tag == "BridgeRight";

                // isBridge
                if (isLeftBridge || isRightBridge)
                {
                    if (isOddDnaPairPosition)
                    {
                        renderer.material = GetRandomMaterial();
                    }
                    else
                    {
                        // create missing
                        var randomNumber1to3 = random.Next(0, 3);

                        // left missing
                        if (randomNumber == 0)
                        {
                            renderer.material = ColorMissing;
                            if (isLeftBridge)
                                renderer.material = ColorMissing;
                            else
                                renderer.material = GetRandomMaterial();
                        }
                        // right missing
                        else if (randomNumber == 1)
                        {
                            if (isRightBridge)
                                renderer.material = ColorMissing;
                            else
                                renderer.material = GetRandomMaterial();
                        }
                        // both missing
                        else if (randomNumber == 2)
                        {
                            renderer.material = ColorMissing;
                        }
                    }
                }
                else
                {
                    bool isFrame = renderer.gameObject.tag == "Frame";
                    bool isNode = renderer.gameObject.tag == "node";
                    // frame
                    if (isFrame)
                    {
                        renderer.material = ColorFour;
                    }
                    else if (isNode)
                    {
                        renderer.material = NodeMaterial;
                    }
                }
            }

            counter++;
            rotationY -= RotatingSpeed;
            positionY += YSpeed;
        }
    }

    private void InitColors()
    {
        Colors.Add(ColorOne);
        Colors.Add(ColorTwo);
        Colors.Add(ColorThree);
        Colors.Add(ColorFour);
    }

    private Material GetRandomMaterial()
    {
        Material material = null;
        do
        {
            var randomNumber = random.Next(0, 4);
            material = Colors[randomNumber];
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
