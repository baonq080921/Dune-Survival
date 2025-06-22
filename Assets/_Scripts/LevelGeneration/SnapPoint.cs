using UnityEngine;


public enum SnapPointType { Enter, Exit}
public class SnapPoint : MonoBehaviour
{

    public SnapPointType snapPointType;

    void OnValidate()
    {
        // auto rename the snap point
        gameObject.name = "SnapPoint" + snapPointType.ToString();
    }
}
