using System;
using Unity.VisualScripting;
using UnityEngine;

public class GLBAnimationPlayer: MonoBehaviour
{
    private int _maxFrame = 0;
    private float _maxTime = 0F;
    private float _time;
    private bool _playing = false;
    private bool _paused = false;

    
    public int MaxFrame => _maxFrame;
    public float CurrentTime => _time;
    public int CurrentFrame => (int)(Progress * _maxFrame);
    public float Progress => Mathf.Clamp(_time / _maxTime, 0F, 1F);
    public bool Playing => _playing;
    public bool Paused => _paused;
    
    public float speed = 1F;
    public bool loop = true;

    private GLBScene _gltf;

    public void SetGLTF(GLBScene gltf) {
        Stop();
        Debug.Log($"[GLBAnimaPlayer] 设置GLTF: {gltf?.transform?.name ?? "null"}");
        _gltf = gltf;
        if (gltf == null || gltf.GLTFRoot == null || gltf.Animation == null) {
            _gltf = null;
            return;
        }
    }
    
    private void Update()
    {
        if (_gltf && _gltf.gameObject.IsDestroyed()) SetGLTF(null);
        
        if (_maxTime <= 0F) {
            _time = 0F;
        }else {
            UpdatePlaying();
        }
    }

    private void UpdatePlaying()
    {
        if (_time >= _maxTime) {
            _time = 0F;
            OnPlayEnd();
        }else {
            if (_playing && !_paused) _time += Time.deltaTime * Mathf.Max(speed, 0F);
            if (_gltf && _gltf.Animation)
                foreach (AnimationState state in _gltf.Animation)
                    state.time = _time;
        }
    }

    public void Pause()
    {
        this._paused = true;
    }

    public void Resume()
    {
        this._paused = false;
    }
    
    private void OnPlayEnd()
    {
        if (loop) {
            Stop();
            Play();
        }else {
            Stop();
        }
    }

    public void Stop()
    {
        _playing = false;
        _time = 0F;
        if (_gltf && !_gltf.IsDestroyed())
        {
            foreach (AnimationState state in _gltf.Animation)
                state.time = 0F;
            _gltf.Animation.Stop();
        }
    }

    public void Play()
    {
        _playing = true;
        _time = 0F;
        ResetAnimation();
        _gltf?.Animation?.Play();
    }


    private void ResetAnimation()
    {
        if (_gltf && _gltf.Animation) {
            Debug.Log($"[GLBAnimaPlayer] 开始播放GLB动画: {_gltf.transform.name}");
            int layer = 0;
            foreach (AnimationState state in _gltf.Animation) {
                state.layer = layer++;
                state.weight = 1F;
                state.speed = 0F;
                state.clip.wrapMode = WrapMode.Once;
                state.wrapMode = WrapMode.Once;
                _gltf.Animation.Play(state.name);
                _maxTime = Math.Max(state.length, _maxTime);
                _maxFrame = (int)Math.Max(state.clip.frameRate * state.clip.length, _maxFrame);
            }
            Debug.Log($"[GLBAnimaPlayer] 时长: {_maxTime}s");
        }
    }
}