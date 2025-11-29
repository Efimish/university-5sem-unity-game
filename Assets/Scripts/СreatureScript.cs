using UnityEngine;

public class Creature : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public virtual void GetDamage()
    {


    }

    public virtual void Die()
    {
        Destroy(this.gameObject);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
