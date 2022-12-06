using Interfaces;
using UnityEngine;
using Utility;

namespace Managers
{
    public class MapGenerator : MonoBehaviour, IMapGeneratorInterface
    {
        private static IMapGeneratorInterface instance;
        [SerializeField]
        [Range(5, 100)]
        private float m_map_width;
        [SerializeField]
        private float m_map_max_height;
        [SerializeField]
        private float m_underground_depth;
        [SerializeField]
        [Range(0, 5)]
        private float m_start_end_offset;
        [SerializeField]
        private float m_ground_level_from_spawn_point;
        private Transform m_player_spawn_transform;
        private Transform m_map_transform;
        private GameObject m_map_block;
        [SerializeField]
        private AnimationCurve m_map_curve;
        [SerializeField]
        private Material[] m_block_possible_material;
        [SerializeField]
        private GameObject m_begin_boundary;
        [SerializeField]
        private GameObject m_end_boundary;
        [SerializeField]
        private GameObject m_victory;
        [SerializeField]
        private bool m_generate_map_procedurally;

        private void Awake()
        {
            instance = this;
            m_player_spawn_transform = GameObject.Find(GameObjectNames.PLAYER_SPAWN_GAMEOBJECT_NAME).gameObject.transform;
            m_map_transform = GameObject.Find(GameObjectNames.MAP_GAMEOBJECT_NAME).gameObject.transform;
            m_map_block = Resources.Load<GameObject>(ResourceNames.MAP_BLOCK_PREFAB_NAME);
            m_begin_boundary = GameObject.Find(GameObjectNames.BEGIN_BOUNDARY_GAMEOBJECT_NAME);
            m_end_boundary = GameObject.Find(GameObjectNames.END_BOUNDARY_GAMEOBJECT_NAME);
            m_victory = GameObject.Find(GameObjectNames.VICTORY_GAMEOBJECT_NAME);
        }

        public static IMapGeneratorInterface GetInstance()
        {
            return instance;
        }

        public void GenerateMap()
        {
            short block_num = 0;
            Vector3 ground_position = m_player_spawn_transform.position + Vector3.down * m_ground_level_from_spawn_point;
            GameObject map_block;
            Material block_material;
            Vector3 horizontal_offset;

            //Begin boundary
            m_begin_boundary.transform.position = ground_position + Vector3.left * m_start_end_offset + Vector3.left;
            //End boundary
            m_end_boundary.transform.position = ground_position + Vector3.right * m_map_width + Vector3.right * m_start_end_offset;
            //Victory gameobject
            m_victory.transform.position = ground_position + Vector3.right * m_map_width + Vector3.right;

            //Generates ground and underground
            for (int j = 0; j < m_underground_depth; j++)
            {
                horizontal_offset = Vector3.left * m_start_end_offset + Vector3.down * j;
                for (int i = 0; i < m_map_width + m_start_end_offset * 2; i++)
                {
                    map_block = GameObject.Instantiate(m_map_block);
                    block_material = RandomizeMaterial();
                    if (block_material != null)
                        map_block.GetComponent<Renderer>().material = block_material;
                    map_block.transform.SetParent(m_map_transform);
                    map_block.transform.position = ground_position + horizontal_offset;
                    map_block.name = string.Format("{0}_{1}", ResourceNames.MAP_BLOCK_PREFAB_NAME, block_num++);
                    horizontal_offset += Vector3.right;
                }
            }

            if(m_generate_map_procedurally)
                RandomizeMapCurve();

            float step = 1.0f/m_map_width;
            float curve_value;
            horizontal_offset = Vector3.zero;
            int vertical_offset;

            for(float t=step; t<1f; t+=step)
            {
                curve_value =  m_map_curve.Evaluate(t);
                //No overlapping geometry
                if (curve_value <= 0)
                    continue;
                curve_value *= m_map_max_height;
                vertical_offset = 1;
                for(int i=0; i<=curve_value; i++)
                {
                    map_block = GameObject.Instantiate(m_map_block);
                    block_material = RandomizeMaterial();
                    if (block_material != null)
                        map_block.GetComponent<Renderer>().material = block_material;
                    map_block.transform.SetParent(m_map_transform);
                    map_block.transform.position = ground_position + horizontal_offset + Vector3.up * vertical_offset;
                    map_block.name = string.Format("{0}_{1}", ResourceNames.MAP_BLOCK_PREFAB_NAME, block_num++);
                    vertical_offset++;
                }
                horizontal_offset += Vector3.right;
            }
        }

        private void RandomizeMapCurve()
        {
            //Generating min and max number of keyframes of the curve (+2 for begin and end)
            int num_keyframe = Random.Range(RandomParameters.MIN_NUMBER_KEYFRAME_NUMBER, RandomParameters.MAX_NUMBER_KEYFRAME_NUMBER)+2;
            Keyframe[] keyframes = new Keyframe[num_keyframe];
            float distance = 0;
            float height;
            bool up = true;
            //Min and max hill distance dictate min and max distance between two keyframes.
            float max_hill_distance = 0.8f / (num_keyframe - 2);
            float min_hill_distance = max_hill_distance - 0.05f;

            for (int i=0; i< num_keyframe; i++)
            {
                if (i == 0)
                {
                    //This force the first keyframe to be at 0
                    distance = 0;
                    height = 0;
                }
                else if(i== num_keyframe - 1)
                {
                    //This force the last keyframe to be at 0
                    distance = 1f;
                    height = 0;
                }
                else
                {
                    distance = distance + Random.Range(min_hill_distance, max_hill_distance);
                    //This step forces the curve to have up and down points
                    if (up)
                        height = Random.Range(0.5f, 1f);
                    else
                        height = 0f;
                    up = !up;
                }

                Keyframe keyframe = new Keyframe(distance, height);
                keyframes[i] = keyframe;
            }

            m_map_curve.keys  = keyframes;
        }
    
        private Material RandomizeMaterial()
        {
            if (m_block_possible_material.Length == 0)
                return null;
            return m_block_possible_material[Random.Range(0, m_block_possible_material.Length)];
        }
    }
}
