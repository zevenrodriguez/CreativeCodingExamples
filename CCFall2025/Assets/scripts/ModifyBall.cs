using UnityEngine;

public class ModifyBall : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Renderer sphereMaterial;
    [SerializeField] Material red;
    [SerializeField] Material green;
    [SerializeField] Material blue;

    float currentYPos = 0.0f;

    void Start()
    {
        sphereMaterial = GetComponent<Renderer>();
        currentYPos = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (currentYPos < transform.position.y)
        {
            Debug.Log("Falling");
            sphereMaterial.material = red;
        }else if (currentYPos > transform.position.y)
        {
            Debug.Log("Rising");
            sphereMaterial.material = blue;
        }
        else
        {
            //Debug.Log("Apex");
            //sphereMaterial.material = green;
        }

        currentYPos = transform.position.y;

        if (transform.position.y < -5.0f)
        {
            Destroy(this.gameObject);
        }
    }
}
