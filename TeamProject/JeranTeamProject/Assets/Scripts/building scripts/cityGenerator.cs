using UnityEngine;

public class CityGenerator : MonoBehaviour
{
    public GameObject[] buildingPrefabs;
    public GameObject roadPrefab;
    public Transform parentPos;
    public int width = 10;
    public int height = 10;
    public float spacing = 10f;
    public int roadSpacing = 4;

    public Vector3 offsetRoads;
    public Vector3 offsetBuilding;
    private Vector3 roadSize;

    void Start()
    {
        roadSize = GetPrefabSize(roadPrefab);

        GenerateCity();
    }

    void GenerateCity()
    {


        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                bool isRoad = (x % roadSpacing == 0) || (z % roadSpacing == 0);

                Vector3 pos = new Vector3(x * roadSize.x, 0, z * roadSize.z);

                if (isRoad)
                {
                    Instantiate(roadPrefab, pos + parentPos.transform.position + offsetRoads, Quaternion.identity, transform);
                }
                else
                {
                    Instantiate(RandomBuilding(), pos + parentPos.transform.position + offsetBuilding, Quaternion.identity, transform);
                }
            }
        }
    }

    GameObject RandomBuilding()
    {

        if (buildingPrefabs == null || buildingPrefabs.Length == 0)
        {
            Debug.LogError("No building prefabs assigned!");
            return null;
        }

        int index = GameManager.instance.randomNumberPicker(buildingPrefabs.Length);
        Debug.Log(index);

        return buildingPrefabs[index];
    }

    Vector3 GetPrefabSize(GameObject prefab)
    {
        Renderer rend = prefab.GetComponentInChildren<Renderer>();

        if (rend == null)
        {
            Debug.LogWarning("No Renderer found on prefab: " + prefab.name);
            return Vector3.one * spacing;
        }

        return rend.bounds.size;
    }

}