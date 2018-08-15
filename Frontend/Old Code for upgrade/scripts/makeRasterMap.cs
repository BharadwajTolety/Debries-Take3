using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mapbox.Map;
using Mapbox.Scripts.UI;
using Mapbox.Scripts.Utilities;
using UnityEngine.UI;
using System.Linq;
using Mapbox;


public class makeRasterMap : MonoBehaviour, IObserver<RasterTile>
	{
	[SerializeField]
	RawImage _imageContainer;
	Map<RasterTile> _map;
	//=$$dW0735800$$eW0735605$$fN0404721$$gN0404547
	GeoCoordinate _startLoc = new GeoCoordinate(40.777405f, -73.943530f);
	//GeoCoordinate _startLoc = new GeoCoordinate(0, 0);
	GeoCoordinate geo1 = new GeoCoordinate(40.4547f, -73.56f);
	int _mapstyle = 1;
	void Start () {
		//GeoCoordinate _startLoc = new GeoCoordinate(40.787901, -73.957360);
		_map = new Map<RasterTile>(MapboxConvenience.Instance.FileSource);
		_map.MapId = "mapbox://styles/mapbox/dark-v9";
		//_map.Center = _startLoc;
		//GeoCoordinate sw;
		//GeoCoordinate ne;
		//sw.Latitude = 40.780f;
		//sw.Longitude =	-73.949760f;
		//ne.Latitude = 40.781f;
		//ne.Latitude = -73.949760f;



		//Bnds.West = -73.949760f;
		//Bounds.East = -73.949760f;
		//Bounds.North=40.781;
		//Bounds.South=40.781;
		//_map.GeoCoordinateBounds=new GeoCoordinateBounds(geo1,_startLoc);//geo1; //=$$dW0735800$$eW0735605$$fN0404721$$gN0404547
		//_map.SetGeoCoordinateBoundsZoom(
		_map.Center=_startLoc;
		_map.Zoom = 13;
		_map.Subscribe (this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
	public void OnNext(RasterTile tile)
	{
		if (tile.CurrentState != Tile.State.Loaded || tile.Error != null)
		{
			return;
		}

		// Can we utility this? Should users have to know source size?
		var texture = new Texture2D(6000,6000, TextureFormat.Alpha8,true);
		texture.LoadImage(tile.Data);
        try
        {
            _imageContainer.texture = texture;
        }
         
        catch {

        }
        
	}



}
