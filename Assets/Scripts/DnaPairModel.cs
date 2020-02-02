using System.Collections.Generic;
using UnityEngine;

public class DnaPairModel
{
    public int InstanceId { get; set; }
    public GameObject GameObj { get; set; }
    public List<Renderer> ChildRenderers { get; set; }
    public List<int> MissingColors { get; set; }
    public bool IsHandled { get; set; }
    public bool LeftIsMissing { get; set; }
    public bool RightIsMissing { get; set; }
    public DnaPairModel()
    {
        ChildRenderers = new List<Renderer>();
        MissingColors = new List<int>();
    }
}
