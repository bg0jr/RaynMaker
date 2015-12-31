using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Plainion.Logging;
using RaynMaker.Import.Spec;
using RaynMaker.Import.Spec.v2.Locating;

namespace RaynMaker.Import.Documents
{
    class DocumentCache
    {
        private static readonly ILogger myLogger = LoggerFactory.GetLogger( typeof( DocumentCache ) );

        private string myCacheFolder;
        private string myIndexFile;
        private CacheIndex myIndex;

        public DocumentCache()
            : this( new CacheSettings() )
        {
        }

        public DocumentCache( CacheSettings settings )
        {
            Settings = settings;

            myCacheFolder = Path.Combine( Path.GetTempPath(), "RaynMaker.Import.Cache" );
            if( !Directory.Exists( myCacheFolder ) )
            {
                Directory.CreateDirectory( myCacheFolder );
            }
            myIndexFile = Path.Combine( myCacheFolder, "cache.idx" );

            myIndex = CacheIndex.LoadOrCreate( myIndexFile );
        }

        public CacheSettings Settings { get; private set; }

        internal Uri TryGet( DocumentLocator key )
        {
            var entry = myIndex.TryGet( key.FragmentsHashCode );
            if( entry == null )
            {
                return null;
            }

            if( entry.IsExpired )
            {
                // found but live time of entry expired
                myIndex.Remove( key.FragmentsHashCode );
                return null;
            }

            // found and live time of entry not expired
            myLogger.Info( "DocumentCache CacheHit for {0}", key );

            return entry.Uri;
        }

        /// <summary>
        /// Adds the document specified by the given URL and the given navigation as key to the cache
        /// </summary>
        internal Uri Add( DocumentLocator key, Uri document )
        {
            var entry = CreateCacheEntry( key, document );

            ShrinkCacheIfRequired( entry );

            myIndex.Add( entry );

            myIndex.Store( myIndexFile );

            return entry.Uri;
        }

        private CacheEntryBase CreateCacheEntry( DocumentLocator key, Uri document )
        {
            var expirationTime = DateTime.Now.Add( Settings.MaxEntryLiveTime );

            if( document.IsFile )
            {
                return new CacheEntryBase( key.FragmentsHashCode, expirationTime, document );
            }
            else
            {
                var cacheFile = Path.Combine( myCacheFolder, key.FragmentsHashCode + ".dat" );
                WebUtil.DownloadTo( document, cacheFile );

                return new ValueCacheEntry( key.FragmentsHashCode, expirationTime, new Uri( cacheFile ) );
            }
        }

        private void ShrinkCacheIfRequired( CacheEntryBase entry )
        {
            while( myIndex.CacheSizeInKB + entry.SizeInKB > Settings.MaxCacheSizeInKB )
            {
                var entryToRemove = myIndex.Entries
                    .OrderBy( e => e.ExpirationTime )
                    .First();

                myIndex.Remove( entryToRemove );
            }
        }

        [Serializable]
        private class CacheIndex
        {
            private IDictionary<int, CacheEntryBase> myCacheEntries;

            private CacheIndex()
            {
                myCacheEntries = new Dictionary<int, CacheEntryBase>();
            }

            internal void Add( CacheEntryBase entry )
            {
                myCacheEntries[ entry.Id ] = entry;
            }

            internal CacheEntryBase TryGet( int id )
            {
                if( !myCacheEntries.ContainsKey( id ) )
                {
                    return null;
                }

                return myCacheEntries[ id ];
            }

            internal void Remove( int id )
            {
                var entry = TryGet( id );
                if( entry == null )
                {
                    return;
                }

                entry.Destroy();
                myCacheEntries.Remove( id );
            }

            public IEnumerable<CacheEntryBase> Entries
            {
                get
                {
                    return myCacheEntries.Values;
                }
            }

            public long CacheSizeInKB
            {
                get
                {
                    return myCacheEntries.Values.Sum( e => e.SizeInKB );
                }
            }

            internal static CacheIndex LoadOrCreate( string indexFile )
            {
                if( !File.Exists( indexFile ) )
                {
                    return new CacheIndex();
                }

                try
                {
                    using( var fileStream = new FileStream( indexFile, FileMode.Open, FileAccess.Read ) )
                    {
                        using( var stream = new BufferedStream( fileStream ) )
                        {
                            var formatter = new BinaryFormatter();
                            return ( CacheIndex )formatter.Deserialize( stream );
                        }
                    }
                }
                catch( Exception ex )
                {
                    myLogger.Warning( "Failed to load cache (skipping): {0}", ex.Message );
                    return new CacheIndex();
                }
            }

            internal void Store( string indexFile )
            {
                try
                {
                    using( var fileStream = new FileStream( indexFile, FileMode.Create, FileAccess.Write ) )
                    {
                        using( var stream = new BufferedStream( fileStream ) )
                        {
                            var formatter = new BinaryFormatter();
                            formatter.Serialize( stream, this );
                        }
                    }
                }
                catch( Exception ex )
                {
                    myLogger.Warning( "Failed to store cache (skipping): {0}", ex.Message );
                }
            }


            internal void Remove( CacheEntryBase entry )
            {
                Remove( entry.Id );
            }
        }

        [Serializable]
        private class CacheEntryBase
        {
            public CacheEntryBase( int id, DateTime expirationTime, Uri uri )
            {
                Id = id;
                ExpirationTime = expirationTime;
                Uri = uri;

                SizeInKB = new FileInfo( Uri.LocalPath ).Length / 1024;
            }

            public int Id
            {
                get;
                private set;
            }

            public DateTime ExpirationTime
            {
                get;
                private set;
            }

            public Uri Uri
            {
                get;
                private set;
            }

            public long SizeInKB
            {
                get;
                private set;
            }

            public bool IsExpired
            {
                get
                {
                    return ExpirationTime <= DateTime.Now;
                }
            }

            public virtual void Destroy()
            {
            }
        }

        [Serializable]
        private class ValueCacheEntry : CacheEntryBase
        {
            internal ValueCacheEntry( int id, DateTime expirationTime, Uri uri )
                : base( id, expirationTime, uri )
            {
            }

            public override void Destroy()
            {
                base.Destroy();

                // cache might be broken
                if( File.Exists( Uri.LocalPath ) )
                {
                    File.Delete( Uri.LocalPath );
                }
            }
        }

        internal void Clear()
        {
            foreach( var file in Directory.GetFiles( myCacheFolder ) )
            {
                File.Delete( file );
            }

            myIndex = CacheIndex.LoadOrCreate( myIndexFile );
        }
    }
}
