using UnityEngine;

public class PlaceObjects : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject[] prefabs = new GameObject[3];
    public int numberOfObjects = 5;
    void Start()
    {
        for (int i = 0; i < numberOfObjects; i++)
        {
            Vector3 position = new Vector3(Random.Range(-10f, 10f), 1.0f, Random.Range(-10f, 10f));
            int randomIndex = Random.Range(0, prefabs.Length);
            Instantiate(prefabs[randomIndex], position, Quaternion.identity);
        } 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
