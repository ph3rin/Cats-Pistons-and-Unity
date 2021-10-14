using System;
using System.Collections.Generic;
using CatProcessingUnit.TileDataNS;
using UnityEngine;

namespace CatProcessingUnit
{
    public class WorkshopData
    {
        private readonly int _width;
        private readonly int _height;

        private TileData[,] _tiles;

        public Workshop Workshop { get; }

        public WorkshopData(Workshop workshop)
        {
            Workshop = workshop;
            _width = workshop.Width;
            _height = workshop.Height;
            _tiles = new TileData[_width, _height];
        }

        public IEnumerable<TileData> Tiles
        {
            get
            {
                for (var x = 0; x < _width; ++x)
                {
                    for (var y = 0; y < _height; ++y)
                    {
                        var tile = GetTileAt(new Vector2Int(x, y));
                        if (tile != null)
                        {
                            yield return tile;
                        }
                    }
                }
            }
        }

        public TileData GetTileAt(Vector2Int position)
        {
            if (position.x < 0 || position.x >= _width || position.y < 0 || position.y >= _height) return null;
            return _tiles[position.x, position.y];
        }

        public void AddTile(TileData tileData)
        {
            Debug.Assert(tileData != null, "tileData != null");
            var pos = tileData.Position;
            Debug.Assert(GetTileAt(pos) == null);
            Debug.Assert(tileData.WorkshopData == null);
            _tiles[pos.x, pos.y] = tileData;
            tileData.WorkshopData = this;
        }

        public WorkshopTileCoordinates ToCoordinates()
        {
            return new WorkshopTileCoordinates(_tiles);
        }

        public void ApplyCoordinates(WorkshopTileCoordinates tileCoordinates)
        {
            var coordinates = tileCoordinates.Coordinates;
            Array.Clear(_tiles, 0, _tiles.Length);
            foreach (var pair in coordinates)
            {
                var tile = pair.Key;
                var pos = pair.Value;
                _tiles[pos.x, pos.y] = tile;
                tile.Position = pos;
            }
        }

        public void OnActivate()
        {
            foreach (var tile in Tiles)
            {
                tile.OnActivate();
            }
        }

        public void OnDeactivate()
        {
            foreach (var tile in Tiles)
            {
                tile.OnDeactivate();
            }
        }

        public WorkshopData Clone()
        {
            var clone = new WorkshopData(Workshop);
            foreach (var tile in Tiles)
            {
                clone.AddTile(tile.Clone());
            }

            return clone;
        }

        public void PushToWorkshopHistory()
        {
            Workshop.PushToHistory(this);
        }
    }
}