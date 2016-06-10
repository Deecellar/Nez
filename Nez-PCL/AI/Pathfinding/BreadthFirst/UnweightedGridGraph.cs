﻿using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Nez.Tiled;


namespace Nez.AI.Pathfinding
{
	/// <summary>
	/// basic unweighted grid graph for use with the BreadthFirstPathfinder
	/// </summary>
	public class UnweightedGridGraph : IUnweightedGraph<Point>
	{
		static readonly Point[] DIRS = new []
		{
			new Point( 1, 0 ),
			new Point( 0, -1 ),
			new Point( -1, 0 ),
			new Point( 0, 1 )
		};

		public HashSet<Point> walls = new HashSet<Point>();

		int _width, _height;


		public UnweightedGridGraph( int width, int height )
		{
			this._width = width;
			this._height = height;
		}


		public UnweightedGridGraph( TiledTileLayer tiledLayer )
		{
			_width = tiledLayer.width;
			_height = tiledLayer.height;

			for( var y = 0; y < tiledLayer.tilemap.height; y++ )
			{
				for( var x = 0; x < tiledLayer.tilemap.width; x++ )
				{
					if( tiledLayer.getTile( x, y ) != null )
						walls.Add( new Point( x, y ) );
				}
			}
		}


		public bool isNodeInBounds( Point id )
		{
			return 0 <= id.X && id.X < _width && 0 <= id.Y && id.Y < _height;
		}


		public bool isNodePassable( Point id )
		{
			return !walls.Contains( id );
		}


		IEnumerable<Point> IUnweightedGraph<Point>.getNeighbors( Point node )
		{
			foreach( var dir in DIRS )
			{
				var next = new Point( node.X + dir.X, node.Y + dir.Y );
				if( isNodeInBounds( next ) && isNodePassable( next ) )
					yield return next;
			}
		}
	

		/// <summary>
		/// convenience shortcut for clling BreadthFirstPathfinder.search
		/// </summary>
		/// <param name="start">Start.</param>
		/// <param name="goal">Goal.</param>
		public List<Point> search( Point start, Point goal )
		{
			return BreadthFirstPathfinder.search( this, start, goal );
		}

	}
}