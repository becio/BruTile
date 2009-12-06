﻿// Copyright 2008 - Paul den Dulk (Geodan)
// 
// This file is part of SharpMap.
// SharpMap is free software; you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// SharpMap is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.

// You should have received a copy of the GNU Lesser General Public License
// along with SharpMap; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA 

using System;
using System.Net;
using BruTile;
using System.Threading;
using BruTile.Cache;

namespace BruTile.Web
{
    public class WebTileProvider : ITileProvider
    {
        IRequestBuilder requestBuilder;
        ITileCache<byte[]> fileCache;

        public WebTileProvider(IRequestBuilder requestBuilder)
            : this(requestBuilder, new NullCache())
        {
        }

        public WebTileProvider(IRequestBuilder requestBuilder, ITileCache<byte[]> fileCache)
        {
            if (requestBuilder == null) throw new ArgumentException("RequestBuilder can not be null");
            this.requestBuilder = requestBuilder;

            if (fileCache == null) throw new ArgumentException("File can not be null");
            this.fileCache = fileCache;
        }

        #region TileProvider Members

        public byte[] GetTile(TileInfo tileInfo)
        {
            byte[] bytes = null;

            bytes = fileCache.Find(tileInfo.Key);
            if (bytes == null)
            {
                bytes = ImageRequest.GetImageFromServer(requestBuilder.GetUri(tileInfo));
                if (bytes != null)
                    fileCache.Add(tileInfo.Key, bytes);
            }
            return bytes;
        }

        #endregion

        #region Private classes

        private class NullCache : ITileCache<byte[]>
        {
            public NullCache()
            {
            }

            public void Add(TileKey key, byte[] image)
            {
                //do nothing
            }

            public void Remove(TileKey key)
            {
                throw new NotImplementedException(); //and should not
            }

            public byte[] Find(TileKey key)
            {
                return null;
            }
        }

        #endregion
    }
}