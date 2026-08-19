using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GLBAnimationPlayer: MonoBehaviour
{
    private LoggerProxy logger = new LoggerProxy("GLB动画播放器");
    
    private int _maxFrame = 0;
    private float _maxTime = 0F;
    private float _time;
    private bool _playing = false;
    private bool _paused = false;

    
    public int MaxFrame => _maxFrame;
    public float CurrentTime => _time;
    public int CurrentFrame => (int)(Progress * _maxFrame);
    public float Progress => _maxTime == 0F ? 0F : Mathf.Clamp(_time / _maxTime, 0F, 1F);
    public bool Playing => _playing;
    public bool Paused => _paused;
    
    public float speed = 1F;
    public bool loop = true;

    private BehaviourGLB _behaviourGLB;
    public BehaviourGLB BehaviourGLB => (_behaviourGLB == null || _behaviourGLB.IsDestroyed()) ? null : _behaviourGLB;
    public Animation Animation => BehaviourGLB?.Animation;

    public Action<BehaviourGLB> onSetBehaviourGLB;
    
    public void SetBehaviourGLB(BehaviourGLB behaviour) {
        if (_behaviourGLB == behaviour) return;
        Stop();
        this._behaviourGLB = behaviour;
        ResetAnimation();
        onSetBehaviourGLB?.Invoke(behaviour);
        Debug.Log($"[GLBAnimaPlayer] 设置BehaviourGLB: {BehaviourGLB?.name ?? "null"}");
    }

    public Dictionary<string, GLBAnimationStage> GetAnimaStates() => BehaviourGLB?.RenderGLB?.Animations ?? new Dictionary<string, GLBAnimationStage>();
    
    private void Update()
    {
        if (!BehaviourGLB) SetBehaviourGLB(null);
        
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
            if (Animation)
                foreach (AnimationState state in Animation)
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
        if (Animation)
        {
            foreach (AnimationState state in Animation)
                state.time = 0F;
            Animation.Stop();
        }
    }

    public void Play()
    {
        _playing = true;
        _time = 0F;
        ResetAnimation();
        if (Animation) Animation.Play();
    }


    private void ResetAnimation()
    {
        if (Animation) {
            var fps = _behaviourGLB.RenderGLB.FPS;
            logger.Info($"开始播放GLB动画: {BehaviourGLB?.name ?? "null"}");
            int layer = 0;
            foreach (AnimationState state in Animation) {
                state.layer = layer++;
                state.weight = 1F;
                state.speed = 0F;
                state.clip.wrapMode = WrapMode.Once;
                state.wrapMode = WrapMode.Once;
                Animation.Play(state.name);
                _maxFrame = (int)Math.Max(fps * state.clip.length, _maxFrame);
                _maxTime = Math.Max(state.clip.length, _maxTime);
                // logger.Info($"        Clip: {state.name} TF: {fps * state.clip.length} FR: {fps}/{state.clip.frameRate} L: {state.clip.length}");
            }
            logger.Info($"时长: {_maxTime}s 最大帧: {_maxFrame}");
        }
    }
}