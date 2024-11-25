using System.Collections.Generic;

namespace VContainer.Diagnostics
{
    public sealed class DiagnosticsInfo
    {
        public DiagnosticsInfo(string scopeName, RegisterInfo registerInfo)
        {
            ScopeName = scopeName;
            RegisterInfo = registerInfo;
        }

        public string ScopeName { get; }
        public RegisterInfo RegisterInfo { get; }
        public ResolveInfo ResolveInfo { get; set; }
        public List<DiagnosticsInfo> Dependencies { get; } = new();
    }
}