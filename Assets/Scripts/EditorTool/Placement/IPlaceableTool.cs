using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlaceableTool
{
    void StartPlacing();
    void CancelPlacing();
    void UpdatePlacing();
    bool TryPlace();

}
