using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DecalShooter : MonoBehaviour
{
    public GameObject decalPrefab;
    public float range = 100f;
    public float surfaceOffset = 0.01f;
    public float angleOffset = 6f; // degrees left/right

    private HashSet<Collider> hitColliders = new HashSet<Collider>();
    float sharedRotation;
    void Update()
    {
        sharedRotation = Random.Range(0f, 360f);
        if (Input.GetMouseButtonDown(0))
        {
            hitColliders.Clear();
            Shoot();
        }
    }

    void Shoot()
    {
        hitColliders.Clear();

        int sprayRays = 6;          // try 6–10
        float sprayAngle = 8f;      // degrees

        Vector3 origin = transform.position;
        Vector3 forward = transform.forward;

        // Always fire the center ray
        TryShootRay(origin, forward);

        // Spray cone
        for (int i = 0; i < sprayRays; i++)
        {
            Vector2 rand = Random.insideUnitCircle;
            Vector3 sprayDir =
                Quaternion.AngleAxis(rand.x * sprayAngle, transform.up) *
                Quaternion.AngleAxis(rand.y * sprayAngle, transform.right) *
                forward;

            TryShootRay(origin, sprayDir);
        }
    }


    void TryShootRay(Vector3 origin, Vector3 direction)
    {
        if (Physics.Raycast(origin, direction, out RaycastHit hit, range))
        {
            // Prevent duplicate decals on the same collider
            if (hitColliders.Contains(hit.collider))
                return;

            hitColliders.Add(hit.collider);
            SpawnDecal(hit, direction);
        }
    }

    public void SpawnDecal(RaycastHit hit, Vector3 rayDir)
    {
        Vector3 tangent = Vector3.ProjectOnPlane(-rayDir, hit.normal).normalized;

        Vector3 pos =
            hit.point +
            hit.normal * 0.01f +
            tangent * 0.05f;

        Quaternion rot = Quaternion.LookRotation(-hit.normal);

        GameObject decal = Instantiate(decalPrefab, pos, rot);

        var projector = decal.GetComponent<UnityEngine.Rendering.Universal.DecalProjector>();
        float size = Random.Range(1.1f, 1.5f);
        projector.size = new Vector3(size, size, 0.05f);

        decal.transform.Rotate(0f, 0f, sharedRotation);
    }
}
