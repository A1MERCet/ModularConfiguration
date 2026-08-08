using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public abstract class StateMachine<T> where T : Enum
    {
        public class State<T> where T : Enum
        {
            public readonly StateMachineMono<T> stateMachine;
            public readonly T type;

            public State(StateMachineMono<T> stateMachine , T type)
            {
                this.stateMachine = stateMachine;
                this.type = type;
            }

            public virtual void OnInitial()
            {
                
            }
            
            public virtual void OnEnter()
            {
                if(stateMachine.log) Debug.Log("Enter: "+type.ToString());
            }

            public virtual void OnUpdate()
            {
                
            }
            
            public virtual void OnFixedUpdate()
            {
                
            }
            
            public virtual void OnExit()
            {
                
            }
        }
        
        public T curType { get; private set; }
        public Dictionary<T, State<T>> states { get; private set; } = new Dictionary<T, State<T>>();

        public void SetState(T type)
        {
            GetCurrentState()?.OnExit();
            curType = type;
            GetCurrentState()?.OnEnter();
        }

        protected virtual void InitStateMachine()
        {
            foreach (T v in Enum.GetValues(typeof(T)))
            {
                var state = CreateStateFromType(v);
                states.Add(v, state);
            }

            var initialState = InitialState();
            if(initialState!=null) SetState(initialState);
        }
        
        protected virtual void UpdateStateMachine()
        {
            GetCurrentState()?.OnUpdate();
        }
        
        protected virtual void FixedUpdateStateMachine()
        {
            GetCurrentState()?.OnFixedUpdate();
        }

        public abstract T InitialState();
        protected abstract State<T> CreateStateFromType(T type);
        
        private State<T> GetState(T type) => states[type];
        private State<T> GetCurrentState() => states[curType];
    }
}