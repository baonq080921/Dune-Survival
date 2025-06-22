using System.Collections.Generic;
using UnityEngine;

public class LevelPart : MonoBehaviour
{

    [Header("Intersection Check")]
    [SerializeField] private LayerMask IntersectionLayer;
    [SerializeField] private Collider[] intersectionCheckCollider;
    [SerializeField] private Transform intersectionCheckParent;


    public bool IntersectionDetected()
    {
        Physics.SyncTransforms();

        foreach (Collider collider in intersectionCheckCollider)
        {
            Collider[] hitCollider =
                Physics.OverlapBox(collider.bounds.center, collider.bounds.extents, Quaternion.identity, IntersectionLayer);
            foreach (Collider hit in hitCollider)
            {
                Intersection_Check intersection_Check = hit.GetComponentInParent<Intersection_Check>();
                if (intersection_Check != null && intersectionCheckParent != intersection_Check.transform)
                {
                    return true;
                }
            }
        }
        return false;

    }


    public void SnapAndAlignPartTo(SnapPoint targetSnapPoint)
    {
        SnapPoint entrancePoint = GetEntrancePoint();


        AlginTo(entrancePoint, targetSnapPoint);

        SnapTo(entrancePoint, targetSnapPoint);
    }

    private void AlginTo(SnapPoint ownSnapPoint, SnapPoint targetSnapPoint)
    {

        // caculate the rotation offet beetween the level parts current rotation
        // and its own snap point rotation . this is help in turning the aligment
        float rotationoffSet =
        ownSnapPoint.transform.rotation.eulerAngles.y - transform.rotation.eulerAngles.y;

        //set the level part's rotation to mathch the target snap point rotation
        //this is the initial step to align the orientations of the twos parts

        transform.rotation = targetSnapPoint.transform.rotation;

        //Appy the prevously caculated offset. this is step fine-tunes the alignemnt by adjusting the 
        //level part's rotation to accoutn for any initail diff in orientation between the level
        //'spart own snap point and the main body of the part

        transform.Rotate(0, 180, 0);
        transform.Rotate(0, -rotationoffSet, 0);



    }

    public SnapPoint GetEntrancePoint() => GetSnapPointOfType(SnapPointType.Enter);
    public SnapPoint GetExitPoint() => GetSnapPointOfType(SnapPointType.Exit);

    private void SnapTo(SnapPoint ownSnapPoint, SnapPoint targetSnapPoint)
    {
        // caculate the offset beetween the level part current positon
        //and its own snap point position .this offset represent the
        // distance and direction from the level part's pivot to its snap point
        Vector3 offset = transform.position - ownSnapPoint.transform.position;

        // set the new position for the level part. Its/ calculated by
        //adding the previously co, puted offset to the target snap points position
        // this will move the level part so that its snap point's position
        //with the target snap position

        Vector3 newPos = targetSnapPoint.transform.position + offset;

        transform.position = newPos;


    }

    private SnapPoint GetSnapPointOfType(SnapPointType snapPointType)
    {
        SnapPoint[] snapPoints = GetComponentsInChildren<SnapPoint>();
        List<SnapPoint> fillterSnapPonis = new List<SnapPoint>();
        foreach (SnapPoint snapPoint in snapPoints)
        {
            if (snapPoint.snapPointType == snapPointType)
            {
                fillterSnapPonis.Add(snapPoint);
            }
        }

        // if There are matching snap points, choose one at random

        if (fillterSnapPonis.Count > 0)
        {
            int randomIndex = Random.Range(0, fillterSnapPonis.Count);
            return fillterSnapPonis[randomIndex];
        }

        return null;


    }

}
