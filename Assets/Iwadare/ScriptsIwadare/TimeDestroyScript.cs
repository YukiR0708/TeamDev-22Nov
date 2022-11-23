using UnityEngine;

public class TimeDestroyScript : MonoBehaviour
{
    [SerializeField] float _time = 1.0f;
    void Start()
    {
        Destroy(gameObject, _time);
    }
}
