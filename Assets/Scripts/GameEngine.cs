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

    public Material FrameMaterial;
    public Material NodeMaterial;
    public Material ColorMissing;

    public int NumberOfDnaPairs = 1;
    public float RotatingSpeed;
    public float YSpeed;
    public float DnaChainInitialPosition;


    private List<Material> Colors = new List<Material>();
    private Material previousMaterial = null;
    private static System.Random random = new System.Random();
    void Start()
    {
        InitColors();
        InitChain();
    }

    private void InitChain()
    {
        var randomNumber = random.Next(0, 4);
        previousMaterial = Colors[randomNumber];

        // root object
        var rootObject = Instantiate(RootObject);

        // falling object
        var fallingObject = Instantiate(FallingObject);
        fallingObject.transform.parent = rootObject.transform;
        
        // dna chain
        var dnaChainObject = Instantiate(DnaChain);
        dnaChainObject.transform.localPosition = new Vector3(0, DnaChainInitialPosition, 0);
        dnaChainObject.transform.parent = fallingObject.transform;

        var counter = 0;
        var rotationY = 0.0f;
        var positionY = 0.0f;
        bool isOddDnaPairPosition = false;
        while (counter < NumberOfDnaPairs)
        {
            isOddDnaPairPosition = !isOddDnaPairPosition;
            GameObject dnaPair = null;
            if (isOddDnaPairPosition)
            {
                dnaPair = InstantiateDnaPair(DnaPairMode.Default);
            }
            else
            {
                var random1to3 = random.Next(0, 3);
                dnaPair = InstantiateDnaPair((DnaPairMode)random1to3);
            }

            dnaPair.transform.localPosition = new Vector3(0, positionY, 0);
            dnaPair.transform.localRotation = Quaternion.Euler(0, rotationY, 0);
            dnaPair.transform.parent = dnaChainObject.transform;

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

    private GameObject InstantiateDnaPair(DnaPairMode pairMode)
    {
        switch (pairMode)
        {
            case DnaPairMode.BothMissing:
                return GenerateBothMissingDnaPair();
            case DnaPairMode.LeftMissing:
                return GenerateLeftMissingDnaPair();
            case DnaPairMode.RightMissing:
                return GenerateRightMissingDnaPair();
            default:
                return GenerateDefaultDnaPair();
        }
    }

    private GameObject GenerateBothMissingDnaPair()
    {
        var dnaPair = Instantiate(DnaPair);
        foreach (var renderer in dnaPair.GetComponentsInChildren<Renderer>())
        {
            if (renderer.gameObject.tag == Constants.Tags.Frame)
                renderer.material = ColorMissing;
            else if (renderer.gameObject.tag == Constants.Tags.NodeLeft)
                renderer.material = ColorMissing;
            else if (renderer.gameObject.tag == Constants.Tags.BridgeLeft)
                renderer.material = ColorMissing;
            else if (renderer.gameObject.tag == Constants.Tags.NodeRight)
                renderer.material = ColorMissing;
            else if (renderer.gameObject.tag == Constants.Tags.BridgeRight)
                renderer.material = ColorMissing;
        }
        return dnaPair;
    }
    private GameObject GenerateLeftMissingDnaPair()
    {
        var dnaPair = Instantiate(DnaPair);
        foreach (var renderer in dnaPair.GetComponentsInChildren<Renderer>())
        {
            var rightColor = GetRandomMaterial();
            if (renderer.gameObject.tag == Constants.Tags.Frame)
                renderer.material = ColorMissing;
            else if (renderer.gameObject.tag == Constants.Tags.NodeLeft)
                renderer.material = ColorMissing;
            else if (renderer.gameObject.tag == Constants.Tags.BridgeLeft)
                renderer.material = ColorMissing;
            else if (renderer.gameObject.tag == Constants.Tags.NodeRight)
                renderer.material = ColorMissing;
            else if (renderer.gameObject.tag == Constants.Tags.BridgeRight)
                renderer.material = ColorMissing;
        }
        return dnaPair;
    }
    private GameObject GenerateRightMissingDnaPair()
    {
        var dnaPair = Instantiate(DnaPair);
        foreach (var renderer in dnaPair.GetComponentsInChildren<Renderer>())
        {
            var leftColor = GetRandomMaterial();
            if (renderer.gameObject.tag == Constants.Tags.Frame)
                renderer.material = ColorMissing;
            else if (renderer.gameObject.tag == Constants.Tags.NodeLeft)
                renderer.material = ColorMissing;
            else if (renderer.gameObject.tag == Constants.Tags.BridgeLeft)
                renderer.material = ColorMissing;
            else if (renderer.gameObject.tag == Constants.Tags.NodeRight)
                renderer.material = ColorMissing;
            else if (renderer.gameObject.tag == Constants.Tags.BridgeRight)
                renderer.material = ColorMissing;
        }
        return dnaPair;
    }
    private GameObject GenerateDefaultDnaPair()
    {
        var dnaPair = Instantiate(DnaPair);
        foreach (var renderer in dnaPair.GetComponentsInChildren<Renderer>())
        {
            var leftColor = GetRandomMaterial();
            var rightColor = GetRandomMaterial();
            if (renderer.gameObject.tag == Constants.Tags.Frame)
                renderer.material = ColorMissing;
            else if (renderer.gameObject.tag == Constants.Tags.NodeLeft)
                renderer.material = leftColor;
            else if (renderer.gameObject.tag == Constants.Tags.BridgeLeft)
                renderer.material = ColorMissing;
            else if (renderer.gameObject.tag == Constants.Tags.NodeRight)
                renderer.material = ColorMissing;
            else if (renderer.gameObject.tag == Constants.Tags.BridgeRight)
                renderer.material = ColorMissing;
        }
        return dnaPair;
    }
}
