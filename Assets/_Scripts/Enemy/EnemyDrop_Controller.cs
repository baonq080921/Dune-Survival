using UnityEngine;

public class EnemyDrop_Controller : MonoBehaviour
{
    [SerializeField] private GameObject missionObjectKey;
    private void CreateItem(GameObject go)
    {
        GameObject newItem = Instantiate(go, transform.position + Vector3.up, Quaternion.identity);

    }



    public void DropItems()
    {
        if (missionObjectKey != null)
            CreateItem(missionObjectKey);
    }

    public void GiveKey(GameObject newKey) => missionObjectKey = newKey;
}
