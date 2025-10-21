using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSpawner : MonoBehaviour
{
    [System.Serializable]
    public class LootItem
    {
        public GameObject itemObj;
        [Range(0,1)]
        public float weight;
    }

    public LootItem[] lootItems;

    public void SpwanLoot()
    {
        float currVal = Random.value;
        for (int i = 0; i < lootItems.Length; i++)
        {
            if (currVal <= lootItems[i].weight)
            {
                var obj = Instantiate(lootItems[i].itemObj);
                obj.transform.position = transform.position + Vector3.up * 2;
            }
        }
    }
}
