using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class EntityStateMachine<TKey, TEntity>
{
    Dictionary<TKey, EntityState<TEntity>> states;
    
    TEntity entity;
    public EntityState<TEntity> CurrentState { get; private set; }
    public TKey CurrentStateKey { get; private set; }

    public EntityStateMachine(TEntity entity)
    {
        this.entity = entity;
        states = new Dictionary<TKey, EntityState<TEntity>>();
    }

    public void RegisterInitialState(TKey key, EntityState<TEntity> state)
    {
        if (initialStateWasSet)
            throw new Exception("two inits!");

        initialStateWasSet = true;
        RegisterState(key, state);
        SetState(key);
    }
    bool initialStateWasSet;

    public void RegisterState(TKey key, EntityState<TEntity> state)
    {
        state.Initialize(entity);
        states[key] = state;
    }

    public void SetState(TKey key)
    {
        CurrentStateKey = key;
        var newState = states[key];

        if (newState == CurrentState)
            return;

        if (CurrentState != null)
            CurrentState.OnLeave(entity);

        CurrentState = newState;
        CurrentState.OnEnter(entity);
    }

    public void Update()
    {
        CurrentState.OnPreUpdate(entity);
        CurrentState.OnUpdate(entity);
    }
}
