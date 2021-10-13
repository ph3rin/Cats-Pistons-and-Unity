using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CatProcessingUnit
{
    public class WorkshopData
    {
        private readonly int _width;
        private readonly int _height;

        private WorkshopTile[,] _tiles;

        public WorkshopData(WorkshopTile[,] tiles)
        {
            _width = tiles.GetLength(0);
            _height = tiles.GetLength(1);
            _tiles = tiles;
        }

        public IEnumerable<WorkshopTile> Tiles
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

        public WorkshopTile GetTileAt(Vector2Int position)
        {
            if (position.x < 0 || position.x >= _width || position.y < 0 || position.y >= _height) return null;
            return _tiles[position.x, position.y];
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

        public WorkshopData Clone()
        {
            var newTiles = _tiles.Clone();
            return new WorkshopData(newTiles as WorkshopTile[,]);
        }
    }
}