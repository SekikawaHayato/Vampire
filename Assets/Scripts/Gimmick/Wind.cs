using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vampire.Gimmick
{
    public class Wind : MonoBehaviour
    {
        float _moveSpeed;
        float _lifeTime;

        public void SetParameter(float moveSpeed = 5, float lifeTime = 2)
        {
            this._moveSpeed = moveSpeed;
            this._lifeTime = lifeTime;
        }

        // Update is called once per frame
        void Update()
        {
            transform.Translate(Vector3.left * _moveSpeed * Time.deltaTime);
            _lifeTime -= Time.deltaTime;
            if (_lifeTime <= 0) Destroy(this.gameObject);
        }
    }
}
