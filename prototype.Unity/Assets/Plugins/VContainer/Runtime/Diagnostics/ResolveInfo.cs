using System.Collections.Generic;

namespace VContainer.Diagnostics
{
    public sealed class ResolveInfo
    {
        public ResolveInfo(Registration registration)
        {
            Registration = registration;
        }

        public Registration Registration { get; }
        public List<object> Instances { get; } = new();
        public int MaxDepth { get; set; } = -1;
        public int RefCount { get; set; }
        public long ResolveTime { get; set; }
    }
}