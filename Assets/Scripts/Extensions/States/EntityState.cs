using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class EntityState<T>
{
    public virtual void Initialize(T entity) { }

    public virtual void OnEnter(T entity) { }
    public virtual void OnPreUpdate(T entity) { }
    public virtual void OnUpdate(T entity) { }
    public virtual void OnLeave(T entity) { }
}
