using System;
using UnityEngine;

namespace UnityStandardAssets._2D {
    public class Camera2DFollow : MonoBehaviour {

        public float damping = 1;
        public float lookAheadFactor = 3;
        public float lookAheadReturnSpeed = 0.5f;
        public float lookAheadMoveThreshold = 0.1f;
        private Player fiya;
        private Transform target;
        private float m_OffsetZ;
        private Vector3 m_LastTargetPosition;
        private Vector3 m_CurrentVelocity;
        private Vector3 m_LookAheadPos;
        private Vector3 m_LookUpPos;
        public float zoomMin = 10.0f;
        public float zoomMax = 30.0f;
        public float zoomSensitivity = 1.0f;
        private float zoomCurrent;
        private Camera camera;

        // Use this for initialization
        private void Start() {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            fiya = GameObject.FindObjectOfType<Player>();
            m_LastTargetPosition = target.position;
            m_OffsetZ = (transform.position - target.position).z;
            transform.parent = null;
            camera = transform.GetComponent<Camera>();
            zoomCurrent = camera.orthographicSize;
        }


        // Update is called once per frame
        private void Update() {
            if (fiya.CameraShouldNOTMove) {
                lookAheadReturnSpeed = 0;
            }
            else
                lookAheadReturnSpeed = .5f;

            follow();

            // Zoom Input
            float zoomInput = Input.GetAxis("Mouse ScrollWheel") * -zoomSensitivity;
            zoomCurrent += zoomInput;
            zoomCurrent = Mathf.Clamp(zoomCurrent, zoomMin, zoomMax);
            camera.orthographicSize = zoomCurrent;

        }

        private void follow() {
            // only update lookahead pos if accelerating or changed direction
            float xMoveDelta = (target.position - m_LastTargetPosition).x;
            float yMoveDelta = (target.position - m_LastTargetPosition).y;

            bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;
            bool updateLookUpTarget = Mathf.Abs(yMoveDelta) > lookAheadMoveThreshold;

            if (updateLookAheadTarget) {
                m_LookAheadPos = lookAheadFactor * Vector3.right * Mathf.Sign(xMoveDelta);
            }
            else {
                m_LookAheadPos = Vector3.MoveTowards(m_LookAheadPos, Vector3.zero, Time.deltaTime * lookAheadReturnSpeed);
            }

            if (updateLookUpTarget) {
                m_LookUpPos = lookAheadFactor * Vector3.up * Mathf.Sign(yMoveDelta);
            }
            else {
                m_LookUpPos = Vector3.MoveTowards(m_LookUpPos, Vector3.zero, Time.deltaTime * lookAheadReturnSpeed);
            }

            Vector3 aheadTargetPos = target.position + m_LookAheadPos + m_LookUpPos + Vector3.forward * m_OffsetZ;
            Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref m_CurrentVelocity, damping);

            transform.position = newPos;

            m_LastTargetPosition = target.position;
        }
    }
}