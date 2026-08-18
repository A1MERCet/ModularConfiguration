using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

 public class ModelRenderer : SingletonMono<ModelRenderer> 
 {
        [Serializable]
        public class RenderState
        {
            protected internal long index;
            public RenderParameters parameters;
            public Action onRenderClear;
            public Action onRenderComplete;
            public Action onRenderStart;
            public GameObject target;
            public RenderTexture texture;

            public RenderState(long index, RenderParameters parameters, GameObject target)
            {
                this.index = index;
                this.parameters = parameters;
                this.target = target;
            }

            public void ClearContent()
            {
                ModelRenderer.instance.ClearRenderTexture(texture);
                onRenderClear?.Invoke();
            }
            
        }
        
        [Serializable]
        public class RenderParameters
        {
            public int width = 100,height = 100;
            public float size = 0.5F;
            public Vector3 position = new Vector3(0,0,0);
            public Vector3 rotation = new Vector3(0,0,0);
            public Vector3 scale    = new Vector3(1,1,1);
            public bool modifyCamera = false;
            public Vector3 modifyCameraPosition = new Vector3(0,0,0);
            public Vector3 modifyCameraRotation = new Vector3(0,0,0);

            public RenderParameters Copy()
            {
                RenderParameters r = new RenderParameters
                {
                    width = this.width,
                    height = this.height,
                    size = this.size,
                    position = new Vector3(this.position.x, this.position.y, this.position.z),
                    rotation = new Vector3(this.rotation.x, this.rotation.y, this.rotation.z),
                    scale = new Vector3(this.scale.x, this.scale.y, this.scale.z),
                    modifyCamera = this.modifyCamera,
                    modifyCameraPosition = this.modifyCameraPosition,
                    modifyCameraRotation = this.modifyCameraRotation,
                };
                return r;
            }

            public override string ToString() => $"{width}*{height} Size:{size} Pos:{position} Rot:{rotation} Scale:{scale}";
        }
        
        private long index = -1;
        private Queue<RenderState> queue = new();
        private bool locked = false;
        public Camera camera;

        public RenderTexture renderTexturePrefab;
        public Dictionary<long, RenderState> caches = new();

        public IEnumerator RenderAsync(RenderState state)
        {
            if(state.target==null){Debug.LogError("RenderItemNull - "+state.index); queue.Dequeue();locked = false;yield break;}

            GameObject target = state.target;
            
            bool prevActive = target.activeSelf;
            Transform prevRoot = target.transform.parent;
            Vector3 prevPos = target.transform.position;
            Vector3 prevScale = target.transform.localScale;
            Quaternion prevRot = target.transform.rotation;
            int prevLayer = target.layer;
            
            target.SetActive(true);
            target.transform.SetParent(transform,false);
            target.transform.localPosition = state.parameters.position;
            target.transform.localRotation = Quaternion.Euler(state.parameters.rotation);
            target.transform.localScale    = state.parameters.scale;
            UtilUnity.SetLayer(state.target, "model");

            if (state.parameters.modifyCamera) {
                camera.transform.localPosition = state.parameters.modifyCameraPosition;
                camera.transform.localRotation = Quaternion.Euler(state.parameters.modifyCameraRotation);
            }else {
                camera.transform.localPosition = new Vector3(-20F, 0F, 0F);
                camera.transform.localRotation = Quaternion.Euler(0F,90F,0F);
            }
            
            RenderTexture output = state.texture;
            if (state.texture == null)
            {
                output = Instantiate(renderTexturePrefab);
                state.texture = output;
                output.width = state.parameters.width;
                output.height = state.parameters.height;
            }
            ClearRenderTexture(output);
            
            camera.orthographicSize = state.parameters.size;
            camera.targetTexture = output;

            camera.gameObject.SetActive(true);
            state.texture = output;
            yield return new WaitForEndOfFrame();
            yield return new WaitForNextFrameUnit();
            yield return new WaitForNextFrameUnit();
            // Debug.Log(JsonUtility.ToJson(state.parameters));
            // EditorApplication.isPaused = true;

            camera.gameObject.SetActive(false);
            
            caches.Add(state.index, state);
            queue.Dequeue();

            locked = false;

            output.Create();
            RenderTexture.active = output;

            target.transform.SetParent(prevRoot);
            target.transform.position = prevPos;
            target.transform.localScale = prevScale;
            target.transform.rotation = prevRot;
            target.layer = prevLayer;
            target.SetActive(prevActive);
            state.onRenderComplete?.Invoke();
        }

        private RenderTexture ClearRenderTexture(RenderTexture rt)
        {
            RenderTexture.active = rt;
            GL.Clear(true, true, Color.clear);
            RenderTexture.active = null;
            return rt;
        }
            
        void Start()
        {
            camera = GetComponentInChildren<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = Color.clear;
            camera.gameObject.SetActive(false);
        }

        void Update()
        {
            if (!locked && queue.Count > 0)
            {
                locked = true;
                camera.gameObject.SetActive(true);
                camera.enabled = true;
                StartCoroutine(RenderAsync(queue.Peek()));
            }
        }
        
        public RenderState Render(GameObject target,RenderParameters parameters=null)
        {
            if (target == null) { Debug.LogError("Target is null");}
            if (parameters == null) parameters = new RenderParameters() { width = 512, height = 512 };
            index++;
            RenderState state = new RenderState(index, parameters , target);
            queue.Enqueue(state);
            state.onRenderStart?.Invoke();
            return state;
        }

        public RenderState Render(RenderState state)
        {
            if (queue.Contains(state)) return state;
            index++;
            state.index = index;
            queue.Enqueue(state);
            state.onRenderStart?.Invoke();
            return state;
        }

        public RenderState RemoveCache(int index)
        {
            RenderState t = caches.GetValueOrDefault(index);
            caches.Remove(index);
            return t;
        }
    }