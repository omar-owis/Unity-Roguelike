using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using DungeonMan.CustomAttributes;

namespace DungeonMan
{
    public class PushCollider : AbilityBehavior
    {
        private Transform playerTransform;
        public bool _drawGizmo;
        public readonly PushObjectLogic _object = default; // DO NOT CHANGE VALUES
        [StaticBool("_positionBool")] public Vector3 _position;
        [StaticBool("_sizeBool")] public Vector3 _size = Vector3.one;
        [StaticBool("_pushForceBool")] public float _pushForce;
        public bool _followPlayer;
        [StaticBool("_durationBool")] public int _duration;

        [HideInInspector] public bool _positionBool;
        [HideInInspector] public bool _sizeBool;
        [HideInInspector] public bool _pushForceBool;
        [HideInInspector] public bool _durationBool;

#if UNITY_EDITOR
        private void OnEnable()
        {
            SceneView.duringSceneGui += OnSceneUpdate;
        }

        private async void Awake()
        {
            await Task.Delay(1000);
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneUpdate;
        }
#endif

        public override async Task Execute(float effectivenessFactor)
        {
            PullObjectLogic spawnedObj = _object.Spawn(playerTransform, _followPlayer) as PullObjectLogic;

            if (_positionBool) spawnedObj.Position = _position * effectivenessFactor;
            else spawnedObj.Position = _position;

            if (_positionBool) spawnedObj.Size = _size * effectivenessFactor;
            else spawnedObj.Size = _size;

            if (_pushForceBool) spawnedObj.PullForce = _pushForce * effectivenessFactor;
            else spawnedObj.PullForce = _pushForce;

            if (_durationBool) await Task.Delay((int)(_duration * effectivenessFactor));
            else await Task.Delay(_duration);

            spawnedObj.Delete();
        }

        public override void Initialize(AbilityController abilityController)
        {
            playerTransform = abilityController.transform;
        }
#if UNITY_EDITOR
        private void OnSceneUpdate(SceneView sceneView)
        {
            if (_drawGizmo)
            {
                Handles.color = Color.magenta;
                Handles.DrawWireCube(_position + playerTransform.position, Vector3.one * 0.05f); // center
                Handles.color = Color.red;
                Handles.DrawWireCube(_position + playerTransform.position, _size); // collider outline
            }
        }
#endif
    }
}
