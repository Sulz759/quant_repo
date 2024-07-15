namespace _Project.Architecture
{
	public abstract class Interactor
	{
		public virtual void OnCreate() { } // когда все репо и интеракторы созданы
		public virtual void Initialize() { } // когда все репо и интеракторы сделали OnCreate();

		public virtual void OnStart() { } // когда все репо и интеракторы проинициализированы
	}
}
