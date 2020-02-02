﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameEngine : MonoBehaviour
{
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
    private static System.Random random = new System.Random();
    void Start()
    {
        InitColors();
        InitChain();
    }

    private void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {

        }
        if (Input.GetKeyDown(KeyCode.X))
        {

        }
        if (Input.GetKeyDown(KeyCode.N))
        {

        }
        if (Input.GetKeyDown(KeyCode.M))
        {

        }
    }

    private void InitChain()
    {
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

        var gameObjects = GenerateObjects(DnaPair, dnaChainObject.transform, NumberOfDnaPairs);

        foreach (var pair in gameObjects)
        {
            GameObject dnaPair = null;
            if (counter % 2 == 0)
            {
                dnaPair = ConfigurePair(DnaPairMode.Default, pair);
            }
            else
            {
                var random1to3 = random.Next(1, 4);
                dnaPair = ConfigurePair((DnaPairMode)random1to3, pair);
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

    private (Material Left, Material Right) GetRandomColorPair()
    {
        var materials = Colors.OrderBy(_ => System.Guid.NewGuid().ToString()).ToList();
        return (materials[0], materials[1]);
    }

    private GameObject ConfigurePair(DnaPairMode pairMode, GameObject pair)
    {
        switch (pairMode)
        {
            case DnaPairMode.BothMissing:
                return ConfigureBothMissingDnaPair(pair);
            case DnaPairMode.LeftMissing:
                return ConfigureLeftMissingDnaPair(pair);
            case DnaPairMode.RightMissing:
                return ConfigureRightMissingDnaPair(pair);
            default:
                return ConfigureDefaultDnaPair(pair);
        }
    }

    private GameObject ConfigureBothMissingDnaPair(GameObject pair)
    {
        var (leftColor, rightColor) = GetRandomColorPair();
        foreach (var renderer in pair.GetComponentsInChildren<Renderer>())
        {
            if (renderer.gameObject.tag == Constants.Tags.Frame)
                renderer.material = FrameMaterial;

            else if (renderer.gameObject.tag == Constants.Tags.NodeLeft)
                renderer.material = leftColor;
            else if (renderer.gameObject.tag == Constants.Tags.BridgeLeft)
                renderer.material = ColorMissing;

            else if (renderer.gameObject.tag == Constants.Tags.NodeRight)
                renderer.material = rightColor;
            else if (renderer.gameObject.tag == Constants.Tags.BridgeRight)
                renderer.material = ColorMissing;
        }
        return pair;
    }
    private GameObject ConfigureLeftMissingDnaPair(GameObject pair)
    {
        var (leftColor, rightColor) = GetRandomColorPair();
        foreach (var renderer in pair.GetComponentsInChildren<Renderer>())
        {

            if (renderer.gameObject.tag == Constants.Tags.Frame)
                renderer.material = FrameMaterial;

            else if (renderer.gameObject.tag == Constants.Tags.NodeLeft)
                renderer.material = leftColor;
            else if (renderer.gameObject.tag == Constants.Tags.BridgeLeft)
                renderer.material = ColorMissing;

            else if (renderer.gameObject.tag == Constants.Tags.NodeRight)
                renderer.material = rightColor;
            else if (renderer.gameObject.tag == Constants.Tags.BridgeRight)
                renderer.material = rightColor;
        }
        return pair;
    }
    private GameObject ConfigureRightMissingDnaPair(GameObject pair)
    {
        var (leftColor, rightColor) = GetRandomColorPair();
        foreach (var renderer in pair.GetComponentsInChildren<Renderer>())
        {
            if (renderer.gameObject.tag == Constants.Tags.Frame)
                renderer.material = FrameMaterial;

            else if (renderer.gameObject.tag == Constants.Tags.NodeLeft)
                renderer.material = leftColor;
            else if (renderer.gameObject.tag == Constants.Tags.BridgeLeft)
                renderer.material = leftColor;

            else if (renderer.gameObject.tag == Constants.Tags.NodeRight)
                renderer.material = rightColor;
            else if (renderer.gameObject.tag == Constants.Tags.BridgeRight)
                renderer.material = ColorMissing;
        }
        return pair;
    }
    private GameObject ConfigureDefaultDnaPair(GameObject pair)
    {
        var (leftColor, rightColor) = GetRandomColorPair();
        foreach (var renderer in pair.GetComponentsInChildren<Renderer>())
        {
            if (renderer.gameObject.tag == Constants.Tags.Frame)
                renderer.material = FrameMaterial;

            else if (renderer.gameObject.tag == Constants.Tags.NodeLeft)
                renderer.material = leftColor;
            else if (renderer.gameObject.tag == Constants.Tags.BridgeLeft)
                renderer.material = leftColor;

            else if (renderer.gameObject.tag == Constants.Tags.NodeRight)
                renderer.material = rightColor;
            else if (renderer.gameObject.tag == Constants.Tags.BridgeRight)
                renderer.material = rightColor;
        }
        return pair;
    }

    static IEnumerable<GameObject> GenerateObjects(GameObject go, Transform parent, int count)
    {
        var list = new List<GameObject>();
        for (int i = 0; i < count; i++)
        {
            var obj = Instantiate(go);
            obj.transform.parent = parent;
            list.Add(obj);
        }
        return list;
    }
}
