using OneHourJam.Manager;
using UnityEngine;

namespace OneHourJam.Map
{
    public class Box : MonoBehaviour
    {
        private void Start()
        {
            GameManager.Instance.Register(this);
        }
    }
}
