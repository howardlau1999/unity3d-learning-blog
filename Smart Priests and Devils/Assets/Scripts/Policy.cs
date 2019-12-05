using System.Collections.Generic;
using System;
using System.Linq;

public enum Action
{
    P,
    D,
    PP,
    PD,
    DD
}

public struct State
{
    public readonly int priestsOnLeft, devilsOnLeft, priestsOnRight, devilsOnRight, boat;
    public Dictionary<Action, State> nextStates;

    public State(int priestsOnLeft, int devilsOnLeft, int priestsOnRight, int devilsOnRight, int boat)
    {
        this.priestsOnLeft = priestsOnLeft;
        this.devilsOnLeft = devilsOnLeft;
        this.priestsOnRight = priestsOnRight;
        this.devilsOnRight = devilsOnRight;
        this.boat = boat;
        nextStates = new Dictionary<Action, State>();
    }

    public override string ToString()
    {
        return String.Format("({0:g}, {1:g}, {2:g}, {3:g}, {4:g})", priestsOnLeft, devilsOnLeft, priestsOnRight,
            devilsOnRight, boat);
    }

    public override bool Equals(object obj)
    {
        State state = (State) obj;
        return this.priestsOnLeft == state.priestsOnLeft &&
               this.devilsOnLeft == state.devilsOnLeft &&
               this.priestsOnRight == state.priestsOnRight &&
               this.devilsOnRight == state.devilsOnRight && this.boat == state.boat;
    }

    public override int GetHashCode()
    {
        return ((this.boat & 0x1) << 8) | ((this.priestsOnLeft & 0x3) << 6) | ((this.devilsOnLeft & 0x3) << 4) |
               ((this.priestsOnRight & 0x3) << 2) |
               (this.devilsOnRight & 0x3);
    }
}

public class Policy
{
    public HashSet<State> validStates;
    public Dictionary<State, bool> visited;

    bool isValid(State state)
    {
        return !(state.priestsOnLeft != 0 && state.priestsOnLeft < state.devilsOnLeft ||
                 state.priestsOnRight != 0 && state.priestsOnRight < state.devilsOnRight);
    }

    bool isLegal(State state)
    {
        return state.priestsOnLeft >= 0 && state.priestsOnRight >= 0 && state.devilsOnLeft >= 0 &&
               state.devilsOnRight >= 0;
    }

    public Policy()
    {
        validStates = new HashSet<State>();
        visited = new Dictionary<State, bool>();
        Queue<State> queue = new Queue<State>();
        State initial = new State(0, 0, 3, 3, 1);
        Action[] actions = new Action[]
        {
            Action.P,
            Action.D,
            Action.PP,
            Action.PD,
            Action.DD
        };
        queue.Enqueue(initial);
        validStates.Add(initial);
        visited[initial] = true;
        while (queue.Count > 0)
        {
            State current = queue.Dequeue();
            int boat = current.boat;

            State P = new State(current.priestsOnLeft + 1 * boat, current.devilsOnLeft,
                current.priestsOnRight - 1 * boat,
                current.devilsOnRight, -boat);

            State D = new State(current.priestsOnLeft, current.devilsOnLeft + 1 * boat, current.priestsOnRight,
                current.devilsOnRight - 1 * boat, -boat);

            State PD = new State(current.priestsOnLeft + 1 * boat, current.devilsOnLeft + 1 * boat,
                current.priestsOnRight - 1 * boat,
                current.devilsOnRight - 1 * boat, -boat);

            State PP = new State(current.priestsOnLeft + 2 * boat, current.devilsOnLeft,
                current.priestsOnRight - 2 * boat,
                current.devilsOnRight, -boat);

            State DD = new State(current.priestsOnLeft, current.devilsOnLeft + 2 * boat, current.priestsOnRight,
                current.devilsOnRight - 2 * boat, -boat);

            State[] states = new State[] {P, D, PP, PD, DD};
            foreach (var (action, state) in actions.Zip(states, (action, state) => (action, state)))
            {
                if (isLegal(state))
                {
                    if (isValid(state))
                    {
                        current.nextStates[action] = state;
                        validStates.Add(state);
                        if (!visited.ContainsKey(state))
                        {
                            visited[state] = true;
                            queue.Enqueue(state);
                        }
                    }
                }
            }
        }
    }
}