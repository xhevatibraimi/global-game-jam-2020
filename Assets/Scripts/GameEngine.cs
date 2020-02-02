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
    private int GameScore = 0;
    void Start()
    {
        InitColors();
        InitChain();
    }

    private void Update()
    {
        HandleInput();
    }

    #region Handle Kick Action
    private void HandleInput()
    {
        if (HasInput())
        {
            // check for clicks
            var input = GetInput();
            HandleKickAction(input);
        }
        else
        {
            // check formissing zone
        }
    }
    private bool HasInput()
    {
        return Input.GetKey(KeyCode.Z)
            || Input.GetKey(KeyCode.X)
            || Input.GetKey(KeyCode.N)
            || Input.GetKey(KeyCode.M);
    }
    private static bool[] GetInput()
    {
        return new bool[] {
            Input.GetKeyDown(KeyCode.Z),
            Input.GetKeyDown(KeyCode.X),
            Input.GetKeyDown(KeyCode.N),
            Input.GetKeyDown(KeyCode.M)
        };
    }
    private void HandleKickAction(bool[] input)
    {
        var dnaPair = GetElementInActionRange();
        if (dnaPair == null)
        {
            return;
        }

        var renderers = dnaPair.ChildRenderers;
        var leftBridge = renderers.FirstOrDefault(x => x.gameObject.tag == Constants.Tags.BridgeLeft);
        var rightBridge = renderers.FirstOrDefault(x => x.gameObject.tag == Constants.Tags.BridgeRight);

        if (dnaPair.MissingColors.Any())
        {
            if (dnaPair.IsHandled)
                return;

            // MISSING
            var clickedColors = GetClickedColors(input);
            if (dnaPair.LeftIsMissing && dnaPair.RightIsMissing)
            {
                // both missing
                if (clickedColors.Contains(Colors[dnaPair.MissingColors[0]].color) && clickedColors.Contains(Colors[dnaPair.MissingColors[1]].color) && clickedColors.Count == 2)
                    IncrementScore(2);
                else
                    DecrementScore();
            }
            else if (dnaPair.LeftIsMissing)
            {
                // left missing
                if (clickedColors.Contains(Colors[dnaPair.MissingColors[0]].color) && clickedColors.Count == 1)
                    IncrementScore(1);
                else
                    DecrementScore();
            }
            else if (dnaPair.RightIsMissing)
            {
                // right missing
                if (clickedColors.Contains(Colors[dnaPair.MissingColors[0]].color) && clickedColors.Count == 1)
                    IncrementScore(1);
                else
                    DecrementScore();
            }
            dnaPair.IsHandled = true;
        }
        else
        {
            //NOT MISSING 
            Debug.Log("no missing");
        }
    }
    private void DecrementScore()
    {
        GameScore--;
        UpdateScore();
    }
    private void IncrementScore(int points)
    {
        GameScore += points;
        UpdateScore();
    }
    private void UpdateScore()
    {
        if (GameScore < 0) GameScore = 0;
        Debug.ClearDeveloperConsole();
        Debug.Log(GameScore);
    }
    private List<Color> GetClickedColors(bool[] input)
    {
        var colors = new List<Color>();
        for (int i = 0; i < Colors.Count; i++)
        {
            if (input[i])
            {
                colors.Add(Colors[i].color);
            }
        }
        return colors;
    }
    private DnaPairModel GetElementInActionRange()
    {
        var element = DnaPairsList.FirstOrDefault(dnaPair => dnaPair.GameObj.transform.position.y > ActionRangeDown
            && dnaPair.GameObj.transform.position.y < ActionRangeUp);
        return element;
    }
    #endregion

    #region Initialize
    private void InitChain()
    {
        // root object
        RootObject.transform.position = new Vector3(0, DnaChainInitialPosition, 0);

        // falling object
        var fallingObject = Instantiate(FallingObject);
        fallingObject.transform.parent = RootObject.transform;

        // dna chain
        var dnaChainObject = Instantiate(DnaChain);
        dnaChainObject.transform.localPosition = new Vector3(0, 0, 0);
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
    private (Material Left, Material Right, int leftColorIndex, int rightColorIndex) GetRandomColorPair()
    {
        var materials = Colors.OrderBy(_ => Guid.NewGuid().ToString()).ToList();
        int leftColorIndex = 0;
        int rightColorIndex = 0;
        for (int i = 0; i < Colors.Count; i++)
        {
            if (materials[0].color == Colors[i].color)
            {
                leftColorIndex = i;
            }
            if (materials[1].color == Colors[i].color)
            {
                leftColorIndex = i;
            }
        }
        return (materials[0], materials[1], leftColorIndex, rightColorIndex);
    }
    private void ConfigurePair(DnaPairMode pairMode, DnaPairModel pair)
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
        var (leftColor, rightColor, leftColorIndex, rightColorIndex) = GetRandomColorPair();
        pair.MissingColors.Add(leftColorIndex);
        pair.MissingColors.Add(rightColorIndex);
        pair.LeftIsMissing = true;
        pair.RightIsMissing = true;

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
        var (leftColor, rightColor, leftColorIndex, rightColorMissing) = GetRandomColorPair();
        pair.MissingColors.Add(leftColorIndex);
        pair.LeftIsMissing = true;

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
        var (leftColor, rightColor, leftColorIndex, rightColorIndex) = GetRandomColorPair();
        pair.MissingColors.Add(rightColorIndex);
        pair.RightIsMissing = true;

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
        var (leftColor, rightColor, leftColorIndex, rightColorIndex) = GetRandomColorPair();
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
            dnaPair.InstanceId = dnaPair.GameObj.GetInstanceID();
            dnaPair.GameObj.transform.parent = parent;
            dnaPair.GameObj.transform.localPosition = Vector3.zero;
            dnaPair.ChildRenderers = dnaPair.GameObj.GetComponentsInChildren<Renderer>().ToList();
            DnaPairsList.Add(dnaPair);
        }
    }
    #endregion
}
