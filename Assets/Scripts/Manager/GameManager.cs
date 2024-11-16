using OneHourJam.Map;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OneHourJam.Manager
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { private set; get; }

        private List<Box> _boxes = new();

        private void Awake()
        {
            Instance = this;
        }

        public void Register(Box box)
        {
            _boxes.Add(box);
        }

        public void FightBoxes(Vector2Int v)
        {
            for (int i = _boxes.Count - 1; i >= 0; i--)
            {
                var b = _boxes[i];
                Vector2Int dir;
                if (Mathf.Abs(b.transform.position.x) > Mathf.Abs(b.transform.position.y))
                {
                    dir = b.transform.position.x > 0f ? Vector2Int.right : Vector2Int.left;
                }
                else
                {
                    dir = b.transform.position.y > 0f ? Vector2Int.up : Vector2Int.down;
                }

                if (dir == v)
                {
                    Destroy(b.gameObject);
                    _boxes.RemoveAt(i);
                }
            }
        }

        public void OnFightUp(InputAction.CallbackContext value)
        {
            if (value.phase == InputActionPhase.Started) FightBoxes(Vector2Int.up);
        }
        public void OnFightRight(InputAction.CallbackContext value)
        {
            if (value.phase == InputActionPhase.Started) FightBoxes(Vector2Int.right);
        }
        public void OnFightLeft(InputAction.CallbackContext value)
        {
            if (value.phase == InputActionPhase.Started) FightBoxes(Vector2Int.left);
        }
        public void OnFightDown(InputAction.CallbackContext value)
        {
            if (value.phase == InputActionPhase.Started) FightBoxes(Vector2Int.down);
        }
    }
}
