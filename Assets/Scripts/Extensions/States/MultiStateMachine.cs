using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class MultiStateMachine<TKey, TEntity>
{
    private EntityStateMachine<TKey, TEntity>[] stateMachines;
}
