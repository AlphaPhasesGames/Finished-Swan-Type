using UnityEngine;

public class PaintTest : MonoBehaviour
{
    public Material m;

    void Start()
    {
        m = GetComponent<Renderer>().material;
       // m.SetFloat("_PaintStength", 1f);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
       {
            m.SetFloat("_PaintStength", 1f);
        }

       // m.SetFloat("_PaintStrength", Mathf.PingPong(Time.time, 1f));
    }
}