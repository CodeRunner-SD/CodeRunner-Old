using CodeRunner.Diagnostics;
using System.Collections;
using System.Collections.Generic;

namespace CodeRunner.Templates
{
    public class VariableCollection : ICollection<Variable>
    {
        public int Count => ((ICollection<Variable>)Variables).Count;

        public bool IsReadOnly => ((ICollection<Variable>)Variables).IsReadOnly;

        private HashSet<Variable> Variables { get; set; } = new HashSet<Variable>(EqualityComparer<Variable>.Default);

        public void Add(Variable item) => ((ICollection<Variable>)Variables).Add(item);

        public void Clear() => ((ICollection<Variable>)Variables).Clear();

        public bool Contains(Variable item) => ((ICollection<Variable>)Variables).Contains(item);

        public void CopyTo(Variable[] array, int arrayIndex) => ((ICollection<Variable>)Variables).CopyTo(array, arrayIndex);

        public IEnumerator<Variable> GetEnumerator() => ((ICollection<Variable>)Variables).GetEnumerator();

        public bool Remove(Variable item) => ((ICollection<Variable>)Variables).Remove(item);

        IEnumerator IEnumerable.GetEnumerator() => ((ICollection<Variable>)Variables).GetEnumerator();

        public void Collect(BaseTemplate from)
        {
            Assert.IsNotNull(from);

            foreach (Variable v in from.GetVariables())
            {
                Add(v);
            }
        }

        public void Collect(IEnumerable<BaseTemplate> from)
        {
            Assert.IsNotNull(from);

            foreach (BaseTemplate item in from)
            {
                Collect(item);
            }
        }
    }
}
