﻿using UnityEngine;

namespace Bipolar
{
    public abstract class GenericRandomizer<T> : MonoBehaviour where T : Component
    {
        private T _randomizedComponent;
        public T RandomizedComponent
        {
            get
            {
                if (_randomizedComponent == null)
                    _randomizedComponent = GetComponent<T>();
                return _randomizedComponent;
            }
        }

        [SerializeField]
        private bool randomizeOnAwake = true;
        public bool RandomizeOnAwake
        {
            get => randomizeOnAwake;
            set => randomizeOnAwake = value;
        }

        public abstract void Randomize();

		protected virtual void Reset()
		{
            if (TryGetComponent<T>(out _) == false)
                gameObject.AddComponent<T>();
		}

		private void Awake()
        {
            if (randomizeOnAwake)
                Randomize();
        }
    }
}
