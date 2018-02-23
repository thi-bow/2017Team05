using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtLaser : Enemy_bullet
{
    public GameObject m_atmic;

    [Header("Prefabs")]
    public GameObject beamLineRendererPrefab;
    public GameObject beamStartPrefab;
    public GameObject beamEndPrefab;

    private GameObject beamStart;
    private GameObject beamEnd;
    private GameObject beam;
    private LineRenderer line;

    [Header("Adjustable Variables")]
    public float beamEndOffset = 1f; //How far from the raycast hit point the end effect is positioned
    public float textureScrollSpeed = 8f; //How fast the texture scrolls along the beam
    public float textureLengthScale = 3; //Length of the beam texture

    private void Start()
    {
        beamStart = Instantiate(beamStartPrefab, new Vector3(0, 0, 0), Quaternion.identity, transform) as GameObject;
        beamEnd = Instantiate(beamEndPrefab, new Vector3(0, 0, 0), Quaternion.identity, transform) as GameObject;
        beam = Instantiate(beamLineRendererPrefab, new Vector3(0, 0, 0), Quaternion.identity, transform) as GameObject;
        line = beam.GetComponent<LineRenderer>();
    }

    public override void OnRent()
    {
        base.OnRent();
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        ShootBeamInDir(transform.position, transform.forward);
    }

    void ShootBeamInDir(Vector3 start, Vector3 dir)
    {
        line.SetVertexCount(2);
        line.SetPosition(0, start);
        beamStart.transform.position = start;

        Vector3 end = Vector3.zero;
        RaycastHit hit;
        if (Physics.Raycast(start, dir, out hit))
        {
            end = hit.point - (dir.normalized * beamEndOffset);
            // Instantiate(m_atmic, transform.position, Quaternion.identity);
        }
        else
            end = transform.position + (dir * 100);

        beamEnd.transform.position = end;
        line.SetPosition(1, end);

        beamStart.transform.LookAt(beamEnd.transform.position);
        beamEnd.transform.LookAt(beamStart.transform.position);

        float distance = Vector3.Distance(start, end);
        line.sharedMaterial.mainTextureScale = new Vector2(distance / textureLengthScale, 1);
        line.sharedMaterial.mainTextureOffset -= new Vector2(Time.deltaTime * textureScrollSpeed, 0);
    }
}
