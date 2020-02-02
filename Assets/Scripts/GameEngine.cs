using System;
using System.Collections.Generic;
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
    public float ActionRangeDown;
    public float ActionRangeUp;


    private List<Material> Colors = new List<Material>();
    private List<DnaPairModel> DnaPairsList = new List<DnaPairModel>();
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
    private void HandleInput()
    {
        var input = CalculateInput();
        HandleKickAction(ActionColor.ColorOne, input);
    }

    private static int CalculateInput()
    {
        var inputOne = Input.GetKeyDown(KeyCode.Z) ? 1 : 0;
        var inputTwo = Input.GetKeyDown(KeyCode.X) ? 2 : 0;
        var inputThree = Input.GetKeyDown(KeyCode.N) ? 4 : 0;
        var inputFour = Input.GetKeyDown(KeyCode.M) ? 8 : 0;
        return inputOne + inputTwo + inputThree + inputFour;
    }

    private void HandleKickAction(ActionColor color, int input)
    {
        var dnaPair = GetElementInActionRange();
        if (dnaPair == null)
            return;
        var renderers = dnaPair.ChildRenderers;
        var leftBridge = renderers.FirstOrDefault(x => x.gameObject.tag == Constants.Tags.BridgeLeft);
        var rightBridge = renderers.FirstOrDefault(x => x.gameObject.tag == Constants.Tags.BridgeRight);

        if (leftBridge.material == ColorMissing || leftBridge.material == ColorMissing)
        {
            Debug.Log("missing");
            var leftNode = renderers.FirstOrDefault(x => x.gameObject.tag == Constants.Tags.NodeLeft);
            var rightNode = renderers.FirstOrDefault(x => x.gameObject.tag == Constants.Tags.NodeRight);
        }
        else
        {
            Debug.Log("not missing");

        }
    }

    private bool RendererHasMaterial(Renderer renderer, ActionColor color)
    {
        return renderer.material == Colors[(int)color - 1];
    }

    private DnaPairModel GetElementInActionRange()
    {
        var element = DnaPairsList.FirstOrDefault(dnaPair => dnaPair.GameObj.transform.position.y > ActionRangeDown
            && dnaPair.GameObj.transform.position.y < ActionRangeUp);
        return element;
    }

    #region Initialize
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

        GenerateObjects(DnaPair, dnaChainObject.transform, NumberOfDnaPairs);

        foreach (var dnaPair in DnaPairsList)
        {
            if (counter % 2 == 0)
            {
                ConfigurePair(DnaPairMode.Default, dnaPair);
            }
            else
            {
                var random1to3 = random.Next(1, 4);
                ConfigurePair((DnaPairMode)random1to3, dnaPair);
            }

            dnaPair.GameObj.transform.localPosition = new Vector3(0, positionY, 0);
            dnaPair.GameObj.transform.localRotation = Quaternion.Euler(0, rotationY, 0);
            dnaPair.GameObj.transform.parent = dnaChainObject.transform;

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
    void ConfigurePair(DnaPairMode pairMode, DnaPairModel pair)
    {
        switch (pairMode)
        {
            case DnaPairMode.BothMissing:
                ConfigureBothMissingDnaPair(pair);
                break;
            case DnaPairMode.LeftMissing:
                ConfigureLeftMissingDnaPair(pair);
                break;
            case DnaPairMode.RightMissing:
                ConfigureRightMissingDnaPair(pair);
                break;
            default:
                ConfigureDefaultDnaPair(pair);
                break;
        }
    }
    private void ConfigureBothMissingDnaPair(DnaPairModel pair)
    {
        var (leftColor, rightColor) = GetRandomColorPair();
        foreach (var renderer in pair.ChildRenderers)
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
    }
    private void ConfigureLeftMissingDnaPair(DnaPairModel pair)
    {
        var (leftColor, rightColor) = GetRandomColorPair();
        foreach (var renderer in pair.ChildRenderers)
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
    }
    private void ConfigureRightMissingDnaPair(DnaPairModel pair)
    {
        var (leftColor, rightColor) = GetRandomColorPair();
        foreach (var renderer in pair.ChildRenderers)
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
    }
    private void ConfigureDefaultDnaPair(DnaPairModel pair)
    {
        var (leftColor, rightColor) = GetRandomColorPair();
        foreach (var renderer in pair.ChildRenderers)
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
    }
    private void GenerateObjects(GameObject go, Transform parent, int count)
    {
        DnaPairsList = new List<DnaPairModel>();
        for (int i = 0; i < count; i++)
        {
            var dnaPair = new DnaPairModel();
            dnaPair.GameObj = Instantiate(go);
            dnaPair.GameObj.transform.parent = parent;
            dnaPair.ChildRenderers = dnaPair.GameObj.GetComponentsInChildren<Renderer>().ToList();
            DnaPairsList.Add(dnaPair);
        }
    }
    #endregion
}
