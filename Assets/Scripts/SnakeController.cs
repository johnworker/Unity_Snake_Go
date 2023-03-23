using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    public float speed = 1.0f;
    public Vector3 direction = Vector3.right;

    private List<Transform> bodyParts = new List<Transform>();
    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        bodyParts.Add(transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            Move();
            HandleInput();
            CheckDeath();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Restart();
            }
        }
    }

    void Move()
    {
        for (int i = bodyParts.Count - 1; i > 0; i--)
        {
            bodyParts[i].position = bodyParts[i - 1].position;
        }
        transform.position += direction * speed * Time.deltaTime;
    }

    void HandleInput()
    {
        if (Input.GetKey(KeyCode.UpArrow) && direction != Vector3.down)
        {
            direction = Vector3.up;
        }
        else if (Input.GetKey(KeyCode.DownArrow) && direction != Vector3.up)
        {
            direction = Vector3.down;
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && direction != Vector3.right)
        {
            direction = Vector3.left;
        }
        else if (Input.GetKey(KeyCode.RightArrow) && direction != Vector3.left)
        {
            direction = Vector3.right;
        }
    }

    void CheckDeath()
    {
        // Check if snake hits its own body
        for (int i = 1; i < bodyParts.Count; i++)
        {
            if (transform.position == bodyParts[i].position)
            {
                isDead = true;
            }
        }

        // Check if snake hits the wall or obstacle
        if (transform.position.x < -10.0f || transform.position.x > 10.0f ||
            transform.position.z < -10.0f || transform.position.z > 10.0f)
        {
            isDead = true;
        }
    }

    void Restart()
    {
        // Destroy all body parts except the head
        for (int i = 1; i < bodyParts.Count; i++)
        {
            Destroy(bodyParts[i].gameObject);
        }
        bodyParts.Clear();
        bodyParts.Add(transform);

        // Reset position and direction
        transform.position = Vector3.zero;
        direction = Vector3.right;

        // Reset death flag
        isDead = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            // Destroy the food object
            Destroy(other.gameObject);

            // Add a new body part to the snake
            GameObject bodyPart = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bodyPart.transform.position = bodyParts[bodyParts.Count - 1].position - direction;
            bodyPart.transform.localScale = transform.localScale;
            bodyPart.GetComponent<Renderer>().material.color = Color.green;
            bodyPart.transform.parent = transform;
            bodyParts.Add(bodyPart.transform);
        }
    }
}
