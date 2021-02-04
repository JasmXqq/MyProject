// ############################################################################
// ############################################################################
// ############################################################################
// ############################################################################
//
// File Name:     wglav5_wgl.cs
// Namespace:     WGLAV5
// Class:         WGL
//
// Version History:  2005 Jan; 2003 Feb
// Latest Version:   http://www.colinfahey.com/opengl/csharp.htm
// Bug Reports:      cpfahey@earthlink.net (Colin P. Fahey)
//
// ############################################################################
// ############################################################################
// ############################################################################
// ############################################################################
//
// WARNING: This class must be compiled using the /unsafe C# compiler switch
//          since many OpenGL functions involve pointers.
//
// ############################################################################
// ############################################################################
// ############################################################################
// ############################################################################
//
// The WGL class wraps various wgl*() functions found in the opengl32
// library on the Windows platform.
//
// The WGL class is part of a family of classes in the WGLAV5 namespace:
//
// [1] WGLAV5.GL   : opengl32 gl*()  functions, constants, and data structures
// [2] WGLAV5.GLU  : glu32    glu*() functions, constants, and data structures
// [3] WGLAV5.WGL  : opengl32 wgl*() functions, constants, and data structures
// [4] WGLAV5.GDI  : gdi32    GDI    functions, constants, and data structures
// [5] WGLAV5.USER : user32          functions, constants, and data structures
// [6] WGLAV5.DEMO : demonstation    functions, constants, and data structures
//     
// ############################################################################
// ############################################################################
// ############################################################################
// ############################################################################

using System;

using System.Runtime.InteropServices;         // Necessary for [DllImport(...)]


// ############################################################################
// ############################################################################
// ############################################################################
// ############################################################################

namespace WGLAV5
{
public class WGL
{

// ############################################################################
// ############################################################################
// ############################################################################
// ############################################################################

// ============================================================================
// HGLRC wglCreateContext
//   (
//   HDC  hdc   // device context of device that the rendering context 
//              // will be suitable for
//   );
// ============================================================================

[DllImport("opengl32", SetLastError=true), System.Security.SuppressUnmanagedCodeSecurity]
public static extern
IntPtr 
wglCreateContext
( 
IntPtr  IntPtr_HDC 
);

// ############################################################################
// ############################################################################
// ############################################################################
// ############################################################################

// ============================================================================
// BOOL wglMakeCurrent
//   (
//   HDC  hdc,      // device context of device that OpenGL calls are 
//                  // to be drawn on
//   HGLRC  hglrc   // OpenGL rendering context to be made the calling 
//                  // thread's current rendering context
//   );
// ============================================================================

[DllImport("opengl32", SetLastError=true), System.Security.SuppressUnmanagedCodeSecurity]
public static extern
int 
wglMakeCurrent
( 
IntPtr   IntPtr_HDC, 
IntPtr   IntPtr_HGLRC 
);

// ############################################################################
// ############################################################################
// ############################################################################
// ############################################################################

// ============================================================================
// BOOL wglDeleteContext
//   (
//   HGLRC  hglrc   // handle to the OpenGL rendering context to delete
//   );
// ============================================================================

[DllImport("opengl32", SetLastError=true), System.Security.SuppressUnmanagedCodeSecurity]
public static extern
int 
wglDeleteContext
( 
IntPtr   IntPtr_HGLRC 
);

// ############################################################################
// ############################################################################
// ############################################################################
// ############################################################################

// ============================================================================
// BOOL wglSwapBuffers
//   (
//   HDC  hdc  // device context whose buffers get swapped
//   );
// ============================================================================

[DllImport("opengl32", SetLastError=true), System.Security.SuppressUnmanagedCodeSecurity]
public static extern
int 
wglSwapBuffers
( 
IntPtr IntPtr_HDC 
);

// ############################################################################
// ############################################################################
// ############################################################################
// ############################################################################

// ============================================================================
// HGLRC wglCreateLayerContext(
//   HDC  hdc,          // device context used for a rendering context
//   int  iLayerPlane   // specifies the layer plane that a rendering 
//                      // context is bound to
// );
// ============================================================================

[DllImport("opengl32", SetLastError=true), System.Security.SuppressUnmanagedCodeSecurity]
public static extern
IntPtr 
wglCreateLayerContext
( 
IntPtr  IntPtr_HGLRC, 
int      iLayerPlane 
);

// ############################################################################
// ############################################################################
// ############################################################################
// ############################################################################

// ============================================================================
// ============================================================================

[DllImport("opengl32", SetLastError=true), System.Security.SuppressUnmanagedCodeSecurity]
public static extern
int 
wglShareLists
( 
IntPtr  IntPtr_HGLRC_1, 
IntPtr  IntPtr_HGLRC_2 
);

// ############################################################################
// ############################################################################
// ############################################################################
// ############################################################################

// ============================================================================
// ============================================================================

[DllImport("opengl32", SetLastError=true), System.Security.SuppressUnmanagedCodeSecurity]
public static extern
int 
wglCopyContext
( 
IntPtr   IntPtr_HGLRC_Source, 
IntPtr   IntPtr_HGLRC_Destination, 
uint     mask 
);

// ############################################################################
// ############################################################################
// ############################################################################
// ############################################################################

// ============================================================================
// HGLRC wglGetCurrentContext(VOID);
// ============================================================================

[DllImport("opengl32", SetLastError=true), System.Security.SuppressUnmanagedCodeSecurity]
public static extern
IntPtr 
wglGetCurrentContext
(
);

// ############################################################################
// ############################################################################
// ############################################################################
// ############################################################################

// ============================================================================
// HDC wglGetCurrentDC(VOID);
// ============================================================================

[DllImport("opengl32", SetLastError=true), System.Security.SuppressUnmanagedCodeSecurity]
public static extern
IntPtr 
wglGetCurrentDC
( 
);

// ############################################################################
// ############################################################################
// ############################################################################
// ############################################################################

// ============================================================================
// PROC wglGetProcAddress(
//   LPCSTR  lpszProc   // name of the extension function
// );
// ============================================================================

[DllImport("opengl32", SetLastError=true), System.Security.SuppressUnmanagedCodeSecurity]
public static extern
IntPtr 
wglGetProcAddress
( 
String s 
);

// ############################################################################
// ############################################################################
// ############################################################################
// ############################################################################

// ============================================================================
// BOOL wglSwapLayerBuffers(
//   HDC  hdc,       // device context whose layer plane buffers are to 
//                   // be swapped
//   UINT  fuPlanes  // specifies the overlay, underlay and main planes 
//                   // to be swapped
// );
// ============================================================================

[DllImport("opengl32", SetLastError=true), System.Security.SuppressUnmanagedCodeSecurity]
public static extern
int 
wglSwapLayerBuffers
( 
IntPtr   IntPtr_HDC, 
uint     fuPlanes 
);

// ############################################################################
// ############################################################################
// ############################################################################
// ############################################################################

// ============================================================================
// int wglGetLayerPaletteEntries(
//   HDC  hdc,          // device context of a window whose layer 
//                      // planes are to be described
//   int  iLayerPlane,  // specifies an overlay or underlay plane
//   int  iStart,       // specifies the first palette entry to be set
//   int  cEntries,     // specifies the number of palette entries to 
//                      // be set
//   CONST COLORREF *pcr 
//                      // points to the first member of an array of 
//                      // COLORREF structures
// );
// ============================================================================

[DllImport("opengl32", SetLastError=true), System.Security.SuppressUnmanagedCodeSecurity]
public static extern unsafe
int 
wglGetLayerPaletteEntries
( 
IntPtr  IntPtr_HDC, 
int     iLayerPlane, 
int     iStart,
int     cEntries, 
uint *  pcr // Each uint in pcr is: 0x00bbggrr
); 

// ############################################################################
// ############################################################################
// ############################################################################
// ############################################################################

// ============================================================================
// int wglSetLayerPaletteEntries(
//   HDC  hdc,          // device context whose layer palette is to be 
//                      // set
//   int  iLayerPlane,  // specifies an overlay or underlay plane
//   int  iStart,       // specifies the first palette entry to be set
//   int  cEntries,     // specifies the number of palette entries to 
//                      // be set
//   CONST COLORREF *pcr
//                      // points to the first member of an array of 
//                      // COLORREF structures
// );
// ============================================================================

[DllImport("opengl32", SetLastError=true), System.Security.SuppressUnmanagedCodeSecurity]
public static extern unsafe
int 
wglSetLayerPaletteEntries
( 
IntPtr   IntPtr_HDC, 
int      iLayerPlane, 
int      iStart,
int      cEntries, 
uint *   pcr   // Each uint in pcr is: 0x00bbggrr
); 

// ############################################################################
// ############################################################################
// ############################################################################
// ############################################################################

// ============================================================================
// BOOL wglRealizeLayerPalette(
//   HDC  hdc,          // device context whose layer plane palette is 
//                      // to be realized
//   int  iLayerPlane,  // specifies an overlay or underlay plane
//   BOOL bRealize      // indicates whether the palette is to be 
//                      // realized into the physical palette
// );
// ============================================================================

[DllImport("opengl32", SetLastError=true), System.Security.SuppressUnmanagedCodeSecurity]
public static extern
int 
wglRealizeLayerPalette
( 
IntPtr   IntPtr_HDC, 
int      iLayerPlane, 
uint     bRealize 
);

// ############################################################################
// ############################################################################
// ############################################################################
// ############################################################################

// ============================================================================
// BOOL wglUseFontBitmaps(
//   HDC  hdc,         // device context whose font will be used
//   DWORD  first,     // glyph that is the first of a run of glyphs to 
//                     // be turned into bitmap display lists
//   DWORD  count,     // number of glyphs to turn into bitmap display 
//                     // lists
//   DWORD  listBase   // specifies starting display list
// );
// ============================================================================

[DllImport("opengl32", SetLastError=true), System.Security.SuppressUnmanagedCodeSecurity]
public static extern
int 
wglUseFontBitmaps
( 
IntPtr   IntPtr_HDC, 
uint     first, 
uint     count, 
uint     listBase 
);

// ############################################################################
// ############################################################################
// ############################################################################
// ############################################################################

// ============================================================================
// ============================================================================
public const int  WGL_FONT_LINES      = 0;
public const int  WGL_FONT_POLYGONS   = 1;

[DllImport("opengl32", SetLastError=true), System.Security.SuppressUnmanagedCodeSecurity]
public static extern 
int
wglUseFontOutlines
(
IntPtr  deviceContext,
uint    first,
uint    count,
uint    listBase,
float   deviation,
float   extrusion,
uint    format,
[Out, MarshalAs(UnmanagedType.LPArray)] GDI.GLYPHMETRICSFLOAT[] glyphMetrics
);

// ############################################################################
// ############################################################################
// ############################################################################
// ############################################################################

} // public class WGL
} // namespace OpenGL

// ############################################################################
// ############################################################################
// ############################################################################
// ############################################################################
