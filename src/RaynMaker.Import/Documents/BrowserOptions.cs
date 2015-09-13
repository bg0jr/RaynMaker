using System;
using System.Collections.Generic;
using System.Text;

namespace RaynMaker.Import.Documents
{
    [Flags]
    public enum BrowserOptions : long
    {
        /// <summary>
        /// No flags are set.
        /// </summary>
        None = 0,
        /// <summary>
        /// The browser will operate in offline mode. Equivalent to DLCTL_FORCEOFFLINE.
        /// </summary>
        AlwaysOffline = 0x10000000,
        /// <summary>
        /// The browser will play background sounds. Equivalent to DLCTL_BGSOUNDS.
        /// </summary>
        BackgroundSounds = 0x00000040,
        /// <summary>
        /// Specifies that the browser will not run Active-X controls. Use this setting
        /// to disable Flash movies. Equivalent to DLCTL_NO_RUNACTIVEXCTLS.
        /// </summary>
        NotRunActiveX = 0x00000200,
        /// <summary>
        /// Specifies that the browser should fetch the content from the server. If the server's
        /// content is the same as the cache, the cache is used.Equivalent to DLCTL_RESYNCHRONIZE.
        /// </summary>
        IgnoreCache = 0x00002000,
        /// <summary>
        /// The browser will force the request from the server, and ignore the proxy, even if the
        /// proxy indicates the content is up to date.Equivalent to DLCTL_PRAGMA_NO_CACHE.
        /// </summary>
        IgnoreProxy = 0x00004000,
        /// <summary>
        /// Specifies that the browser should download and display images. This is set by default.
        /// Equivalent to DLCTL_DLIMAGES.
        /// </summary>
        Images = 0x00000010,
        /// <summary>
        /// Disables downloading and installing of Active-X controls.Equivalent to DLCTL_NO_DLACTIVEXCTLS.
        /// </summary>
        NoActiveXDownload = 0x00000400,
        /// <summary>
        /// Disables web behaviours. Equivalent to DLCTL_NO_BEHAVIORS.
        /// </summary>
        NoBehaviors = 0x00008000,
        /// <summary>
        /// The browser suppresses any HTML charset specified.Equivalent to DLCTL_NO_METACHARSET.
        /// </summary>
        NoCharSets = 0x00010000,
        /// <summary>
        /// Indicates the browser will ignore client pulls.Equivalent to DLCTL_NO_CLIENTPULL.
        /// </summary>
        NoClientPull = 0x20000000,
        /// <summary>
        /// The browser will not download or display Java applets.Equivalent to DLCTL_NO_JAVA.
        /// </summary>
        NoJava = 0x00000100,
        /// <summary>
        /// The browser will download framesets and parse them, but will not download the frames
        /// contained inside those framesets.Equivalent to DLCTL_NO_FRAMEDOWNLOAD.
        /// </summary>
        NoFrameDownload = 0x00080000,
        /// <summary>
        /// The browser will not execute any scripts.Equivalent to DLCTL_NO_SCRIPTS.
        /// </summary>
        NoScripts = 0x00000080,
        /// <summary>
        /// If the browser cannot detect any internet connection, this causes it to default to
        /// offline mode.Equivalent to DLCTL_OFFLINEIFNOTCONNECTED.
        /// </summary>
        OfflineIfNotConnected = 0x80000000,
        /// <summary>
        /// Specifies that UTF8 should be used.Equivalent to DLCTL_URL_ENCODING_ENABLE_UTF8.
        /// </summary>
        Utf8 = 0x00040000,
        /// <summary>
        /// The browser will download and display video media.Equivalent to DLCTL_VIDEOS.
        /// </summary>
        Videos = 0x00000020
    }
}
