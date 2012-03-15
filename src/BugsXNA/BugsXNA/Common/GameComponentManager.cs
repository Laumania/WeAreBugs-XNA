using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BugsXNA.Common
{
	/// <summary>
	/// A class to assist with being able to nest game components inside of each other, provides support for all of the
	/// same functionality the game object performs on components with the addition of being neutral to where it resides
	/// in the hierarchy.
	/// </summary>
	public class GameComponentManager : Collection<IGameComponent>, IGameComponent, IUpdateable, IDrawable, IDisposable
	{
		#region Fields

		private Game myGame;
		private IGraphicsDeviceService myGraphicsDeviceService;

		private List<IGameComponent> myUninitializedComponents;
		private List<IUpdateable> myUpdateableComponents;
		private List<IDrawable> myDrawableComponents;

		#endregion

		#region Properties

		/// <summary>
		/// The game that is associated with this manager.
		/// </summary>
		public Game Game { get { return myGame; } }
		/// <summary>
		/// The graphics device that this manager will use to draw with.
		/// </summary>
		public GraphicsDevice GraphicsDevice { get { return myGraphicsDeviceService.GraphicsDevice; } }

		#endregion

		#region Events

		/// <summary>
		/// Raised when a component is added to this manager.
		/// </summary>
		public event EventHandler<GameComponentCollectionEventArgs> ComponentAdded;
		/// <summary>
		/// Raised when a component is removed from this manager.
		/// </summary>
		public event EventHandler<GameComponentCollectionEventArgs> ComponentRemoved;

		#endregion

		#region Constructor

		/// <summary>
		/// Create a new GameComponentManager.
		/// </summary>
		/// <param name="game">The game this manager should be associated with.</param>
		public GameComponentManager(Game game)
		{
			myGame = game;

			myUninitializedComponents = new List<IGameComponent>();
			myUpdateableComponents = new List<IUpdateable>();
			myDrawableComponents = new List<IDrawable>();

			isEnabled = true;
			isVisible = true;
			isInitialized = false;
		}

		#endregion

		#region Collection Methods

		protected override void ClearItems()
		{
			for (int i = 0; i < Count; i++)
				OnComponentRemoved(new GameComponentCollectionEventArgs(this[i]));

			base.ClearItems();
		}

		protected override void InsertItem(int index, IGameComponent item)
		{
			if (IndexOf(item) != -1)
				throw new ArgumentException("Duplicate components are not allowed in the same GameComponentManager.");

			base.InsertItem(index, item);

			if (item != null)
				OnComponentAdded(new GameComponentCollectionEventArgs(item));
		}

		protected override void RemoveItem(int index)
		{
			IGameComponent gameComponent = this[index];
			base.RemoveItem(index);

			if (gameComponent != null)
				OnComponentRemoved(new GameComponentCollectionEventArgs(gameComponent));
		}

		protected override void SetItem(int index, IGameComponent item)
		{
			throw new NotSupportedException("The GameComponentManager does not support directly setting components into the collection.");
		}

		private void OnComponentAdded(GameComponentCollectionEventArgs args)
		{
			if (isInitialized)
				args.GameComponent.Initialize();
			else
				myUninitializedComponents.Add(args.GameComponent);

			// If the new component impliments IUpdateable find a spot for it on the updateable list 
			//  and hook it's UpdateOrderChanged event
			IUpdateable uComponent = args.GameComponent as IUpdateable;
			if (uComponent != null)
			{
				int index = myUpdateableComponents.BinarySearch(uComponent, IUpdateableComparer.Default);
				if (index < 0)
				{
					index = ~index;
					while (index < myUpdateableComponents.Count && myUpdateableComponents[index].UpdateOrder == uComponent.UpdateOrder)
						index++;
					myUpdateableComponents.Insert(index, uComponent);
					uComponent.UpdateOrderChanged += new EventHandler<EventArgs>(ChildUpdateOrderChanged);
				}
			}

			// If the new component impliments IDrawable find a spot for it on the drawable list 
			//  and hook it's DrawOrderChanged event
			IDrawable dComponent = args.GameComponent as IDrawable;
			if (dComponent != null)
			{
				int index = myDrawableComponents.BinarySearch(dComponent, IDrawableComparer.Default);
				if (index < 0)
				{
					index = ~index;
					while (index < myDrawableComponents.Count && myDrawableComponents[index].DrawOrder == dComponent.DrawOrder)
						index++;
					myDrawableComponents.Insert(index, dComponent);
					dComponent.DrawOrderChanged += new EventHandler<EventArgs>(ChildDrawOrderChanged);
				}
			}

			EventHandler<GameComponentCollectionEventArgs> componentAdded = ComponentAdded;
			if (componentAdded != null)
				componentAdded(this, args);
		}

		private void OnComponentRemoved(GameComponentCollectionEventArgs args)
		{
			if (!isInitialized)
				myUninitializedComponents.Remove(args.GameComponent);

			IUpdateable uComponent = args.GameComponent as IUpdateable;
			if (uComponent != null)
			{
				myUpdateableComponents.Remove(uComponent);
				uComponent.UpdateOrderChanged -= new EventHandler<EventArgs>(ChildUpdateOrderChanged);
			}

			IDrawable dComponent = args.GameComponent as IDrawable;
			if (dComponent != null)
			{
				myDrawableComponents.Remove(dComponent);
				dComponent.DrawOrderChanged -= new EventHandler<EventArgs>(ChildDrawOrderChanged);
			}

			EventHandler<GameComponentCollectionEventArgs> componentRemoved = ComponentRemoved;
			if (componentRemoved != null)
				componentRemoved(this, args);
		}

		#endregion

		#region IGameComponent Members

		private bool isInitialized;

		/// <summary>
		/// Initialize the GameComponentManager and attach the event handlers required to allow content loading and unload,
		/// afterward initialize any IGameComponents that haven't been initialized yet.
		/// </summary>
		public virtual void Initialize()
		{
			// If the manager hasn't been initialized, grab the graphics device service from the game service container and
			//  hook device created and disposed events
			if (!isInitialized)
			{
				myGraphicsDeviceService = myGame.Services.GetService(typeof(IGraphicsDeviceService)) as IGraphicsDeviceService;
				if (myGraphicsDeviceService == null)
					throw new InvalidOperationException("Components that impliment IDrawable require a graphics device service in the game service container.");

				myGraphicsDeviceService.DeviceCreated += new EventHandler<EventArgs>(GraphicsDeviceCreated);
				myGraphicsDeviceService.DeviceDisposing += new EventHandler<EventArgs>(GraphicsDeviceDisposing);

				if (myGraphicsDeviceService.GraphicsDevice != null)
					LoadContent();

				isInitialized = true;
			}

			// Initialize any un-initialized game components
			while (myUninitializedComponents.Count > 0)
			{
				myUninitializedComponents[0].Initialize();
				myUninitializedComponents.RemoveAt(0);
			}
		}

		#endregion

		#region IUpdateable Members

		private int myUpdateOrder;
		private bool isEnabled;

		/// <summary>
		/// Indicates the order in which the components should be updated relative to other components in this component's parent. Lower values are updated first. 
		/// </summary>
		public int UpdateOrder
		{
			get { return myUpdateOrder; }
			set
			{
				if (myUpdateOrder != value)
				{
					myUpdateOrder = value;
					OnUpdateOrderChanged(this, EventArgs.Empty);
				}
			}
		}
		/// <summary>
		/// Indicates whether Update should be called when this component's parent Update is called.
		/// </summary>
		public bool Enabled
		{
			get { return isEnabled; }
			set
			{
				if (isEnabled != value)
				{
					isEnabled = value;
					OnEnabledChanged(this, EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// Raised when the UpdateOrder property is changed.
		/// </summary>
		public event EventHandler<EventArgs> UpdateOrderChanged;
		/// <summary>
		/// Raised when the Enabled property is changed.
		/// </summary>
		public event EventHandler<EventArgs> EnabledChanged;

		/// <summary>
		/// Update all game components in this manager that impliment the IUpdateable interface and have their
		/// Enabled property set to true.
		/// </summary>
		/// <param name="gameTime">Snapshot of the game's time when update was started.</param>
		public virtual void Update(GameTime gameTime)
		{
			for (int i = 0; i < myUpdateableComponents.Count; i++)
			{
				IUpdateable uComponent = myUpdateableComponents[i];
				if (uComponent.Enabled)
					uComponent.Update(gameTime);
			}
		}

		/// <summary>
		/// Called when UpdateOrder property changes, this will raise the UpdateOrderChanged event.
		/// </summary>
		protected virtual void OnUpdateOrderChanged(object sender, EventArgs args)
		{
			EventHandler<EventArgs> updateOrderChanged = UpdateOrderChanged;
			if (updateOrderChanged != null)
				updateOrderChanged(sender, args);
		}
		/// <summary>
		/// Called when the Enabled property changes, this will raise the EnabledChanged event.
		/// </summary>
		protected virtual void OnEnabledChanged(object sender, EventArgs args)
		{
			EventHandler<EventArgs> enabledChanged = EnabledChanged;
			if (enabledChanged != null)
				enabledChanged(sender, args);
		}

		/// <summary>
		/// When the update order of a component in this manager changes, will need to find a new place for it
		/// on the list of updateable components.
		/// </summary>
		private void ChildUpdateOrderChanged(object sender, EventArgs e)
		{
			IUpdateable uComponent = sender as IUpdateable;
			myUpdateableComponents.Remove(uComponent);

			int index = myUpdateableComponents.BinarySearch(uComponent, IUpdateableComparer.Default);
			if (index < 0)
			{
				index = ~index;
				while (index < myUpdateableComponents.Count && myUpdateableComponents[index].UpdateOrder == uComponent.UpdateOrder)
					index++;
				myUpdateableComponents.Insert(index, uComponent);
			}
		}

		
		#endregion

		#region IDrawable Members

		private int myDrawOrder;
		private bool isVisible;

		/// <summary>
		/// Indicates the order in which the components should be drawn relative to other components in this component's parent. Lower values are drawn first. 
		/// </summary>
		public int DrawOrder
		{
			get { return myDrawOrder; }
			set
			{
				if (myDrawOrder != value)
				{
					myDrawOrder = value;
					OnDrawOrderChanged(this, EventArgs.Empty);
				}
			}
		}
		/// <summary>
		/// Indicates whether Draw should be called when this component's parent Draw is called.
		/// </summary>
		public bool Visible
		{
			get { return isVisible; }
			set
			{
				if (isVisible != value)
				{
					isVisible = value;
					OnVisibleChanged(this, EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// Raised when the DrawOrder property is changed.
		/// </summary>
		public event EventHandler<EventArgs> DrawOrderChanged;
		/// <summary>
		/// Raised when the Visible property is changed.
		/// </summary>
		public event EventHandler<EventArgs> VisibleChanged;

		/// <summary>
		/// Draw all game components in this manager that impliment IDrawable and have their Visible
		/// property set to true.
		/// </summary>
		/// <param name="gameTime">Snapshot of the game's time when Draw was started.</param>
		public virtual void Draw(GameTime gameTime)
		{
			for (int i = 0; i < myDrawableComponents.Count; i++)
			{
				IDrawable dComponent = myDrawableComponents[i];
				if (dComponent.Visible)
				{
					dComponent.Draw(gameTime);
				}
			}
		}

		/// <summary>
		/// Called when graphical content needs to be loaded.
		/// </summary>
		protected virtual void LoadContent() { }
		/// <summary>
		/// Called when graphical content needs to be unloaded.
		/// </summary>
		protected virtual void UnloadContent() { }

		/// <summary>
		/// Called when DrawOrder property changes, this will raise the DrawOrderChanged event.
		/// </summary>
		protected virtual void OnDrawOrderChanged(object sender, EventArgs args)
		{
			EventHandler<EventArgs> drawOrderChanged = DrawOrderChanged;
			if (drawOrderChanged != null)
				drawOrderChanged(sender, args);
		}
		/// <summary>
		/// Called when the Visible property changes, this will raise the VisibleChanged event.
		/// </summary>
		protected virtual void OnVisibleChanged(object sender, EventArgs args)
		{
			EventHandler<EventArgs> visibleChanged = VisibleChanged;
			if (visibleChanged != null)
				visibleChanged(sender, args);
		}

		/// <summary>
		/// When the draw order of a component in this manager changes, will need to find a new place for it
		/// on the list of drawable components.
		/// </summary>
		private void ChildDrawOrderChanged(object sender, EventArgs e)
		{
			IDrawable dComponent = sender as IDrawable;
			myDrawableComponents.Remove(dComponent);

			int index = myDrawableComponents.BinarySearch(dComponent, IDrawableComparer.Default);
			if (index < 0)
			{
				index = ~index;
				while (index < myDrawableComponents.Count && myDrawableComponents[index].DrawOrder == dComponent.DrawOrder)
					index++;
				myDrawableComponents.Insert(index, dComponent);
			}
		}

		/// <summary>
		/// Catch when a graphics device is created to allow the component to load graphical content.
		/// </summary>
		private void GraphicsDeviceCreated(object sender, EventArgs e) { LoadContent(); }
		/// <summary>
		/// Catch when a graphics device is disposed to allow the component to unload graphical content.
		/// </summary>
		private void GraphicsDeviceDisposing(object sender, EventArgs e) { UnloadContent(); }

		#endregion

		#region IDisposable Members

		~GameComponentManager() { Dispose(false); }

		/// <summary>
		/// Raised when this manager has been disposed of.
		/// </summary>
		public event EventHandler Disposed;

		/// <summary>
		/// Dispose of this manager and all of the game components currently in it that impliment the IDisposable interface.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Called when it's time to dispose of this manager and it's children.
		/// </summary>
		/// <param name="disposing">Is the manager being disposed of?</param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				// Dispose of components in this manager
				for (int i = 0; i < Count; i++)
				{
					IDisposable dComponent = this[i] as IDisposable;
					if (dComponent != null)
						dComponent.Dispose();
				}

				// Unhook the graphics device service events
				if (myGraphicsDeviceService != null)
				{
					myGraphicsDeviceService.DeviceCreated -= new EventHandler<EventArgs>(GraphicsDeviceCreated);
					myGraphicsDeviceService.DeviceDisposing -= new EventHandler<EventArgs>(GraphicsDeviceDisposing);
				}

				// Raise the disposed event if it's available
				EventHandler disposed = Disposed;
				if (disposed != null)
					disposed(this, EventArgs.Empty);
			}
		}

		#endregion
	}

	#region Comparer classes
	/// <summary>
	/// A comparer designed to assist with sorting IUpdateable interfaces.
	/// </summary>
	public sealed class IUpdateableComparer : IComparer<IUpdateable>
	{
		/// <summary>
		/// A static copy of the comparer to circumvent the GC.
		/// </summary>
		public static readonly IUpdateableComparer Default;

		static IUpdateableComparer() { Default = new IUpdateableComparer(); }
		public IUpdateableComparer() { }

		#region IComparer<IUpdateable> Members

		public int Compare(IUpdateable x, IUpdateable y)
		{
			if (x == null && y == null)
				return 0;
			else if (x != null)
			{
				if (y == null)
					return -1;
				else if (x.Equals(y))
					return 0;
				else if (x.UpdateOrder < y.UpdateOrder)
					return -1;
			}

			return 1;
		}

		#endregion
	}

	/// <summary>
	/// A comparer designed to assist with sorting IDrawable interfaces.
	/// </summary>
	public sealed class IDrawableComparer : IComparer<IDrawable>
	{
		/// <summary>
		/// A static copy of the comparer to circumvent the GC.
		/// </summary>
		public static readonly IDrawableComparer Default;

		static IDrawableComparer() { Default = new IDrawableComparer(); }
		public IDrawableComparer() { }

		#region IComparer<IDrawable> Members

		public int Compare(IDrawable x, IDrawable y)
		{
			if (x == null && y == null)
				return 0;
			else if (x != null)
			{
				if (y == null)
					return -1;
				else if (x.Equals(y))
					return 0;
				else if (x.DrawOrder < y.DrawOrder)
					return -1;
			}

			return 1;
		}

		#endregion
	}


	#endregion Comparer classes
}
