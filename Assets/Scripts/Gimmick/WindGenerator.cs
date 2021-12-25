using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vampire.Gimmick
{
    public class WindGenerator : MonoBehaviour
    {
        [SerializeField] GameObject _prefab;
        [SerializeField] float _interval;
        float _timer;

        private void Update()
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                GenerateWind();
                _timer = _interval;
            }
        }

        void GenerateWind() {
            GameObject obj = Instantiate(_prefab, RandomPosition(), Quaternion.identity);
            obj.GetComponent<Wind>().SetParameter();
        }

        Vector3 RandomPosition()
        {
            float x = Random.Range(-10, 10);
            float y = Random.Range(-10, 10);
            Vector3 position = Camera.main.transform.position + Vector3.right * x + Vector3.up * y;
            position.z = 0;
            return position;
        }
    }
}