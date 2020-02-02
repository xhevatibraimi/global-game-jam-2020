using System.Collections.Generic;
using UnityEngine;

public class DnaPairModel
{
    public int InstanceId { get; set; }
    public GameObject GameObj { get; set; }
    public List<Renderer> ChildRenderers { get; set; }
    public List<int> ColorsMissing { get; set; }
    public bool IsHandled { get; set; }

    public DnaPairModel()
    {
        ChildRenderers = new List<Renderer>();
        ColorsMissing = new List<int>();
    }
}
