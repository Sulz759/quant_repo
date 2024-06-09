
namespace _Project.Architecture
{ 
	public abstract class Repository
	{
		public bool isInitialized { get; private set; }
		
		public virtual void Initialize() { isInitialized = true; }
		public virtual void OnStart() { }
		public virtual void OnCreate() { }
		
		
	}
}

