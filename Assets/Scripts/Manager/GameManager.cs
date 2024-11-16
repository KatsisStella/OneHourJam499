using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OneHourJam.Manager
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { private set; get; }

        private readonly List<Transform> _boxes = new();

        private float _timeBeforeNextSpawn = 2f;

        [SerializeField]
        private GameObject _boxPrefab;

        private void Awake()
        {
            Instance = this;

            StartCoroutine(SpawnBoxes());
        }

        private void Register(Transform box)
        {
            _boxes.Add(box);
        }

        private IEnumerator SpawnBoxes()
        {
            var bounds = CalculateBounds(Camera.main);
            while (_timeBeforeNextSpawn > .25f)
            {
                yield return new WaitForSeconds(_timeBeforeNextSpawn);
                var go = Instantiate(_boxPrefab, new Vector3(Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y)), Quaternion.identity);
                Register(go.transform);

                _timeBeforeNextSpawn -= .5f;
            }

            // You won
        }

        public void FightBoxes(Vector2Int v)
        {
            for (int i = _boxes.Count - 1; i >= 0; i--)
            {
                var b = _boxes[i];
                Vector2Int dir;
                if (Mathf.Abs(b.position.x) > Mathf.Abs(b.position.y))
                {
                    dir = b.position.x > 0f ? Vector2Int.right : Vector2Int.left;
                }
                else
                {
                    dir = b.position.y > 0f ? Vector2Int.up : Vector2Int.down;
                }

                if (dir == v)
                {
                    Destroy(b.gameObject);
                    _boxes.RemoveAt(i);
                }
            }
        }
        
        // http://answers.unity.com/answers/502236/view.html
        public static Bounds CalculateBounds(Camera cam)
        {
            float screenAspect = Screen.width / (float)Screen.height;
            float cameraHeight = cam.orthographicSize * 2;
            Bounds bounds = new(
                cam.transform.position,
                new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
            return bounds;
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
