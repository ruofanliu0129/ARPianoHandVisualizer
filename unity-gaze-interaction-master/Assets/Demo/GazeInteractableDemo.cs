#region Includes
using UnityEngine;
#endregion

namespace TS.GazeInteraction.Demo
{
    public class GazeInteractableDemo : MonoBehaviour
    {
        #region Variables

        [Header("Configuration")]
        [SerializeField] private float _speed;
        [SerializeField] private float _radius;

        private Vector3 _initialPosition;
        private Vector3 _targetPosition;

        #endregion

        private void Start()
        {
            _initialPosition = transform.position;

            Reset();
        }
        private void Update()
        {
            //transform.position = Vector3.MoveTowards(transform.position, _targetPosition, Time.deltaTime * _speed);
            //if(transform.position == _targetPosition)
            //{
            //    _targetPosition = _initialPosition + Random.insideUnitSphere * _radius;
            //}
        }

        public void Enable25()
        {
            enabled = true;
            gameObject.GetComponent<MeshRenderer>().material.color = new Color(0.2f, 0.6f, 1.0f, 1.0f);
            PlayTimeLine.I.speedValue = 0.20f;
        }
        public void Enable50()
        {
            enabled = true;
            gameObject.GetComponent<MeshRenderer>().material.color = new Color(0.2f, 0.6f, 1.0f, 1.0f);
            PlayTimeLine.I.speedValue = 0.40f;
        }
        public void Enable75()
        {
            enabled = true;
            gameObject.GetComponent<MeshRenderer>().material.color = new Color(0.2f, 0.6f, 1.0f, 1.0f);
            PlayTimeLine.I.speedValue = 0.60f;
        }
        public void Enable100()
        {
            enabled = true;
            gameObject.GetComponent<MeshRenderer>().material.color = new Color(0.2f, 0.6f, 1.0f, 1.0f);
            PlayTimeLine.I.speedValue = 0.80f;
        }
        public void Reset()
        {
            transform.position = _initialPosition;
            _targetPosition = _initialPosition;

            enabled = false;
            gameObject.GetComponent<MeshRenderer>().material.color = new Color(1.0f, 0.6f, 0f, 1.0f);
        }
    }
}