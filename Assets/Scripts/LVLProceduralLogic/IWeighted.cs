using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeighted
{
    GameObject Prefab { get; }
    int Weight { get; }
}
