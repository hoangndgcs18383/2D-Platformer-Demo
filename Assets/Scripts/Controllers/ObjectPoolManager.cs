using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace platformer_2d.demo
{
    public class ObjectPoolManager
    {
        private Dictionary<string, Queue<GameObject>> _objectPoolDictionary;
        private static ObjectPoolManager _instance;

        public static ObjectPoolManager Instance
        {
            get { return _instance ??= new ObjectPoolManager(); }
            private set => _instance = value;
        }

        public void Initialize()
        {
            _objectPoolDictionary = new Dictionary<string, Queue<GameObject>>();
        }

        public GameObject GetOrCreateObject(string tag, Vector3 position, Quaternion rotation)
        {
            if (!_objectPoolDictionary.ContainsKey(tag))
            {
                _objectPoolDictionary.Add(tag, new Queue<GameObject>());
            }

            GameObject objectToSpawn = null;
            if (_objectPoolDictionary[tag].Count > 0)
            {
                objectToSpawn = _objectPoolDictionary[tag].Dequeue();
                objectToSpawn.transform.position = position;
                objectToSpawn.transform.rotation = rotation;
            }
            else
            {
                objectToSpawn = Object.Instantiate(Resources.Load<GameObject>(tag), position, rotation);
            }

            objectToSpawn.SetActive(true);

            return objectToSpawn;
        }

        public void ReturnObjectToPool(string tag, GameObject objectToReturn)
        {
            if (!_objectPoolDictionary.ContainsKey(tag))
            {
                _objectPoolDictionary.Add(tag, new Queue<GameObject>());
            }

            objectToReturn.SetActive(false);
            _objectPoolDictionary[tag].Enqueue(objectToReturn);
        }
    }
}