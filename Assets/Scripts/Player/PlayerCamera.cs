using UnityEngine;
using Utility;

namespace Player
{
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField]
        private Transform m_player_transform;
        [SerializeField]
        [Range(0f, 3f)]
        private float m_smooth_factor;
        [SerializeField]
        private Vector3 m_camera_offset;
        private Vector3 m_velocity = Vector3.zero;

        private void Awake()
        {
            m_player_transform = GameObject.Find(GameObjectNames.PLAYER_GAMEOBJECT_NAME).gameObject.transform;
        }

        private void Start()
        {
            transform.position = m_player_transform.position + m_camera_offset;
        }

        private void LateUpdate()
        {
            Vector3 target_position = m_player_transform.position + m_camera_offset;
            transform.position = Vector3.SmoothDamp(transform.position, target_position, ref m_velocity, m_smooth_factor);
        }
    }
}
