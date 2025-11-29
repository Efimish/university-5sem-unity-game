using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private Transform player;
    private Vector3 pos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!player)
        {
            player=FindObjectOfType<Anonimus>().transform;
        }
        
    }

    // Update is called once per frame
    void Update()
    {

        pos=player.position;
        // Debug.Log(pos);
        pos.z=-5f;
        pos.y+=6f;
        transform.position=Vector3.Lerp(transform.position,pos,0.05f);
        
    }
}
