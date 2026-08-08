using System;
using System.Collections.Generic;

namespace Project
{
    public abstract class StateMachineMono<T> : SingletonMono<StateMachineMono<T>> where T : Enum
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
        public bool log = true;
        
        public void SetState(T type)
        {
            if(Equals(type, curType)) return;
            GetCurrentState()?.OnExit();
            curType = type;
            GetCurrentState()?.OnEnter();
        }

        protected override void Awake()
        {
            base.Awake();
        }

        protected virtual void Start()
        {
            InitStateMachine();
            SetState(StartState());
        }
        
        protected virtual void Update()
        {
            GetCurrentState()?.OnUpdate();
        }
        
        protected virtual void FixedUpdate()
        {
            GetCurrentState()?.OnFixedUpdate();
        }

        protected virtual void InitStateMachine()
        {
            foreach (T v in Enum.GetValues(typeof(T)))
            {
                var state = CreateStateFromType(v);
                states.Add(v, state);
            }

            GetCurrentState()?.OnInitial();
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
        public abstract T StartState();
        protected abstract State<T> CreateStateFromType(T type);
        
        protected virtual State<T> GetState(T type) => states[type];
        protected virtual  State<T> GetCurrentState() => states[curType];
    }
}