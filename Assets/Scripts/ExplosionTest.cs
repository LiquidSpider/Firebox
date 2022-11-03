using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionTest : MonoBehaviour
{
    public LayerMask layer;
    public float radius = 20;
    public float power = 30;
    private GameObject camera;
    public GameObject explosion;
    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponentInChildren<Camera>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Primary")) {
            CreateExplosiveForce();
        }
    }

    void CreateExplosiveForce() {
        RaycastHit hit;
        if (Physics.Raycast(camera.transform.position, 
            camera.transform.TransformDirection(Vector3.forward),
            out hit,
            Mathf.Infinity,
            layer)) {

            Vector3 explosionPos = hit.point;
            GameObject exp = Instantiate(explosion, hit.point, Quaternion.identity);
            Destroy(exp, 1);
            Debug.Log("Created explosion at: " + hit.point.ToString());
            Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
            foreach (Collider obj in colliders) {
                Rigidbody rb = obj.GetComponent<Rigidbody>();

                if (rb != null)
                    rb.AddExplosionForce(power, explosionPos, radius, 3.0F);
            }

        }
    }
}
