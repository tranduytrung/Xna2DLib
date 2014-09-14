using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Dovahkiin.Model.Core;
using Dovahkiin.Model.TileModel;

namespace Dovahkiin.Maps
{
    public abstract class Map
    {
        private Collection<IMapObject> _mapObjects;
        private Collection<ICanvasObject> _canvasObjects;

        private Tile[,] TileMatrix { get; set; }
        public string Name { get; private set; }
        public int RowCount { get; private set; }
        public int ColumnCount { get; private set; }
        public IEnumerable<IMapObject> MapObjects
        {
            get { return _mapObjects; }
        }
        public IEnumerable<ICanvasObject> CanvasObjects
        {
            get { return _canvasObjects; }
        }

        public event EventHandler<CollectionChangeEventArgs> MapObjectChanged;
        public event EventHandler<CollectionChangeEventArgs> CanvasObjectChanged;

        protected virtual void OnMapObjectChanged(CollectionChangeEventArgs e)
        {
            EventHandler<CollectionChangeEventArgs> handler = MapObjectChanged;
            if (handler != null) handler(this, e);
        }

        protected virtual void OnCanvasObjectChanged(CollectionChangeEventArgs e)
        {
            EventHandler<CollectionChangeEventArgs> handler = CanvasObjectChanged;
            if (handler != null) handler(this, e);
        }

        protected Map(int rowCount, int columnCount, string name)
        {
            Name = name;
            ColumnCount = columnCount;
            RowCount = rowCount;
        }

        public IEnumerable<IMapObject> this[int row, int column]
        {
            get { return GetObjectsAt(row, column).Concat(new[] {GetTileAt(row, column)}); }
        }

        protected abstract Tile ExtractTileAt(int row, int column);

        protected void Load()
        {
            TileMatrix = new Tile[ColumnCount / 2 + ColumnCount % 2, RowCount];
            _mapObjects = new ObservableCollection<IMapObject>();
            _canvasObjects = new Collection<ICanvasObject>();

            for (var row = 0; row < RowCount; row++)
            {
                var y = row;
                for (var col = row % 2; col < ColumnCount; col += 2)
                {
                    var x = col / 2;
                    TileMatrix[y, x] = ExtractTileAt(row, col);
                }
            }
        }

        public Tile GetTileAt(int row, int column)
        {
            int x, y;
            IsometricToMatrix(row, column, out x, out y);
            return TileMatrix[y, x];
        }

        public IEnumerable<IMapObject> GetObjectsAt(int row, int column)
        {
            return from obj in MapObjects
                where obj.Deployment.Formation.Any(f => f.X == column && f.Y == row)
                select obj;
        }

        internal void AddObject(IMapObject obj)
        {
            _mapObjects.Add(obj);
            OnMapObjectChanged(new CollectionChangeEventArgs(CollectionChangeAction.Add, obj));
        }

        internal void AddObject(ICanvasObject obj)
        {
            _canvasObjects.Add(obj);
            OnCanvasObjectChanged(new CollectionChangeEventArgs(CollectionChangeAction.Add, obj));
        }

        internal void RemoveObject(IMapObject obj)
        {
            var disposableObj = obj as IDisposable;
            if (disposableObj != null)
                disposableObj.Dispose();

            _mapObjects.Remove(obj);
            OnMapObjectChanged(new CollectionChangeEventArgs(CollectionChangeAction.Remove, obj));
        }

        internal void RemoveObject(ICanvasObject obj)
        {
            var disposableObj = obj as IDisposable;
            if (disposableObj != null)
                disposableObj.Dispose();

            _canvasObjects.Remove(obj);
            OnCanvasObjectChanged(new CollectionChangeEventArgs(CollectionChangeAction.Remove, obj));
        }

        private static void IsometricToMatrix(int row, int column, out int x, out int y)
        {
            if (row % 2 != column % 2)
                throw new IndexOutOfRangeException("No cell likes this");

            x = column / 2;
            y = row;
        }
    }
}
