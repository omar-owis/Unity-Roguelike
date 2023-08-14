using Unity.VisualScripting;
using UnityEngine;

public abstract class SpawnableObject : MonoBehaviour
{
    public Vector3 Position { get { return transform.position; } set { transform.position = value; } }

    public virtual SpawnableObject Spawn(Transform parent, bool onParent = false)
    {
        SpawnableObject obj = Instantiate(this, Position + parent.position, Quaternion.identity);

        if (onParent)
        {
            FixedJoint joint = obj.AddComponent<FixedJoint>();
            joint.connectedBody = parent.GetComponent<Rigidbody>();
            joint.enablePreprocessing = false;
            joint.massScale = 0;
            joint.connectedMassScale = 0;
            return obj;
        }
        return obj;
    }

    public virtual void Delete()
    {
        Destroy(gameObject);
    }
}
