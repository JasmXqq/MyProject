// ############################################################################
// ############################################################################
// ############################################################################
// ############################################################################
//
// File Name:     wglav5_gdi.cs
// Namespace:     WGLAV5
// Class:         GDI
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
// The GDI class wraps various functions found in the gdi32
// library on the Windows platform.
//
// The GDI class is part of a family of classes in the WGLAV5 namespace:
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
	public class GDI
	{

		// ############################################################################
		// ############################################################################
		// ############################################################################
		// ############################################################################
		//
		// Data Structures
		//
		// ############################################################################
		// ############################################################################
		// ############################################################################
		// ############################################################################

		// ============================================================================
		//
		//  typedef struct tagPIXELFORMATDESCRIPTOR { // pfd   
		//    WORD  nSize; 
		//    WORD  nVersion; 
		//    DWORD dwFlags; 
		//    BYTE  iPixelType; 
		//    BYTE  cColorBits; 
		//    BYTE  cRedBits; 
		//    BYTE  cRedShift; 
		//    BYTE  cGreenBits; 
		//    BYTE  cGreenShift; 
		//    BYTE  cBlueBits; 
		//    BYTE  cBlueShift; 
		//    BYTE  cAlphaBits; 
		//    BYTE  cAlphaShift; 
		//    BYTE  cAccumBits; 
		//    BYTE  cAccumRedBits; 
		//    BYTE  cAccumGreenBits; 
		//    BYTE  cAccumBlueBits; 
		//    BYTE  cAccumAlphaBits; 
		//    BYTE  cDepthBits; 
		//    BYTE  cStencilBits; 
		//    BYTE  cAuxBuffers; 
		//    BYTE  iLayerType; 
		//    BYTE  bReserved; 
		//    DWORD dwLayerMask; 
		//    DWORD dwVisibleMask; 
		//    DWORD dwDamageMask; 
		//  } PIXELFORMATDESCRIPTOR; 
		//  
		// ============================================================================

		[StructLayout(LayoutKind.Sequential)] 
		public struct PIXELFORMATDESCRIPTOR 
		{
			public ushort  nSize; 
			public ushort  nVersion; 
			public uint    dwFlags; 
			public byte    iPixelType; 
			public byte    cColorBits; 
			public byte    cRedBits; 
			public byte    cRedShift; 
			public byte    cGreenBits; 
			public byte    cGreenShift; 
			public byte    cBlueBits; 
			public byte    cBlueShift; 
			public byte    cAlphaBits; 
			public byte    cAlphaShift; 
			public byte    cAccumBits; 
			public byte    cAccumRedBits; 
			public byte    cAccumGreenBits; 
			public byte    cAccumBlueBits; 
			public byte    cAccumAlphaBits; 
			public byte    cDepthBits; 
			public byte    cStencilBits; 
			public byte    cAuxBuffers; 
			public byte    iLayerType; 
			public byte    bReserved; 
			public uint    dwLayerMask; 
			public uint    dwVisibleMask; 
			public uint    dwDamageMask; 
			// 40 bytes total
		}


		// ############################################################################
		// ############################################################################
		// ############################################################################
		// ############################################################################

		/* pixel types */
		public const uint  PFD_TYPE_RGBA        = 0;
		public const uint  PFD_TYPE_COLORINDEX  = 1;

		/* layer types */
		public const uint  PFD_MAIN_PLANE       = 0;
		public const uint  PFD_OVERLAY_PLANE    = 1;
		public const uint  PFD_UNDERLAY_PLANE   = 0xff; // (-1)

		/* PIXELFORMATDESCRIPTOR flags */
		public const uint  PFD_DOUBLEBUFFER            = 0x00000001;
		public const uint  PFD_STEREO                  = 0x00000002;
		public const uint  PFD_DRAW_TO_WINDOW          = 0x00000004;
		public const uint  PFD_DRAW_TO_BITMAP          = 0x00000008;
		public const uint  PFD_SUPPORT_GDI             = 0x00000010;
		public const uint  PFD_SUPPORT_OPENGL          = 0x00000020;
		public const uint  PFD_GENERIC_FORMAT          = 0x00000040;
		public const uint  PFD_NEED_PALETTE            = 0x00000080;
		public const uint  PFD_NEED_SYSTEM_PALETTE     = 0x00000100;
		public const uint  PFD_SWAP_EXCHANGE           = 0x00000200;
		public const uint  PFD_SWAP_COPY               = 0x00000400;
		public const uint  PFD_SWAP_LAYER_BUFFERS      = 0x00000800;
		public const uint  PFD_GENERIC_ACCELERATED     = 0x00001000;
		public const uint  PFD_SUPPORT_DIRECTDRAW      = 0x00002000;

		/* PIXELFORMATDESCRIPTOR flags for use in ChoosePixelFormat only */
		public const uint  PFD_DEPTH_DONTCARE          = 0x20000000;
		public const uint  PFD_DOUBLEBUFFER_DONTCARE   = 0x40000000;
		public const uint  PFD_STEREO_DONTCARE         = 0x80000000;

		// ############################################################################
		// ############################################################################
		// ############################################################################
		// ############################################################################

		// ============================================================================
		//
		//  typedef struct tagBITMAPINFOHEADER {
		//      DWORD  biSize;
		//      LONG   biWidth;
		//      LONG   biHeight;
		//      WORD   biPlanes;
		//      WORD   biBitCount;
		//      DWORD  biCompression;
		//      DWORD  biSizeImage;
		//      LONG   biXPelsPerMeter;
		//      LONG   biYPelsPerMeter;
		//      DWORD  biClrUsed;
		//      DWORD  biClrImportant;
		//  } BITMAPINFOHEADER;
		//  
		// ============================================================================

		[StructLayout(LayoutKind.Sequential)] 
		public struct BITMAPINFOHEADER 
		{
			public uint    biSize;
			public int     biWidth;
			public int     biHeight;
			public short   biPlanes;
			public short   biBitCount;
			public uint    biCompression;
			public uint    biSizeImage;
			public int     biXPelsPerMeter;
			public int     biYPelsPerMeter;
			public uint    biClrUsed;
			public uint    biClrImportant;
			// 40 bytes total
		}

		// ############################################################################
		// ############################################################################
		// ############################################################################
		// ############################################################################

		// ============================================================================
		//
		//  typedef struct tagRGBQUAD 
		//  {
		//  BYTE    rgbBlue; 
		//  BYTE    rgbGreen; 
		//  BYTE    rgbRed; 
		//  BYTE    rgbReserved; 
		//  } 
		//  RGBQUAD; 
		//  
		// ============================================================================

		[StructLayout(LayoutKind.Sequential)] 
		public struct RGBQUAD 
		{
			public byte rgbBlue; 
			public byte rgbGreen; 
			public byte rgbRed; 
			public byte rgbReserved; 
			// 4 bytes total
		}

		// ############################################################################
		// ############################################################################
		// ############################################################################
		// ############################################################################

		// ============================================================================
		//
		//  typedef struct tagBITMAPINFO 
		//  { 
		//  BITMAPINFOHEADER   bmiHeader; 
		//  RGBQUAD            bmiColors[1]; 
		//  } 
		//  BITMAPINFO, * PBITMAPINFO; 
		//  
		// ============================================================================

		[StructLayout(LayoutKind.Sequential)] 
		public struct BITMAPINFO
		{
			public        BITMAPINFOHEADER  bmiHeader;
			public unsafe RGBQUAD *         bmiColors;
			// 44+ bytes total
		}

		// ############################################################################
		// ############################################################################
		// ############################################################################
		// ############################################################################

		// ============================================================================
		//  typedef struct _POINTFLOAT        // ptf
		//  { 
		//  FLOAT       x; 
		//  FLOAT       y; 
		//  }
		//  POINTFLOAT; 
		//
		// ============================================================================

		[StructLayout(LayoutKind.Sequential)] 
		public struct POINTFLOAT 
		{
			public float x;
			public float y;
			// 8 bytes total
		}

		// ############################################################################
		// ############################################################################
		// ############################################################################
		// ############################################################################

		// ============================================================================
		//  typedef struct _GLYPHMETRICSFLOAT        // gmf 
		//  { 
		//  FLOAT       gmfBlackBoxX; 
		//  FLOAT       gmfBlackBoxY; 
		//  POINTFLOAT  gmfptGlyphOrigin; 
		//  FLOAT       gmfCellIncX; 
		//  FLOAT       gmfCellIncY; 
		//  }
		//  GLYPHMETRICSFLOAT; 
		//
		// ============================================================================

		[StructLayout(LayoutKind.Sequential)] 
		public struct GLYPHMETRICSFLOAT 
		{
			public float        gmfBlackBoxX;
			public float        gmfBlackBoxY;
			public POINTFLOAT   gmfptGlyphOrigin;
			public float        gmfCellIncX; 
			public float        gmfCellIncY; 
			// 24 bytes total
		}

		// ############################################################################
		// ############################################################################
		// ############################################################################
		// ############################################################################

		// From wingdi.h:
		/* Stock Logical Objects */
		public const int WHITE_BRUSH         = 0;
		public const int LTGRAY_BRUSH        = 1;
		public const int GRAY_BRUSH          = 2;
		public const int DKGRAY_BRUSH        = 3;
		public const int BLACK_BRUSH         = 4;
		public const int NULL_BRUSH          = 5;
		public const int HOLLOW_BRUSH        = NULL_BRUSH;
		public const int WHITE_PEN           = 6;
		public const int BLACK_PEN           = 7;
		public const int NULL_PEN            = 8;
		public const int OEM_FIXED_FONT      = 10;
		public const int ANSI_FIXED_FONT     = 11;
		public const int ANSI_VAR_FONT       = 12;
		public const int SYSTEM_FONT         = 13;
		public const int DEVICE_DEFAULT_FONT = 14;
		public const int DEFAULT_PALETTE     = 15;
		public const int SYSTEM_FIXED_FONT   = 16;

		// ############################################################################
		// ############################################################################
		// ############################################################################
		// ############################################################################
		//
		// Functions
		//
		// ############################################################################
		// ############################################################################
		// ############################################################################
		// ############################################################################

		// ============================================================================
		// int ChoosePixelFormat
		//   (
		//   HDC  hdc,  // device context to search for a best pixel format 
		//              // match
		//   CONST PIXELFORMATDESCRIPTOR *  ppfd 
		//              // pixel format for which a best match is sought
		//   );
		// ============================================================================

		[DllImport("gdi32", SetLastError=true), System.Security.SuppressUnmanagedCodeSecurity]
		public static extern unsafe
		int 
		ChoosePixelFormat
		( 
			IntPtr                   IntPtr_HDC, 
			PIXELFORMATDESCRIPTOR *  p_pfd 
		);

		// ############################################################################
		// ############################################################################
		// ############################################################################
		// ############################################################################


		// ============================================================================
		// BOOL SetPixelFormat
		//   (
		//   HDC  hdc,  // device context whose pixel format the function 
		//              // attempts to set
		//   int  iPixelFormat,
		//              // pixel format index (one-based)
		//   CONST PIXELFORMATDESCRIPTOR *  ppfd 
		//              // pointer to logical pixel format specification
		//   );
		// ============================================================================

		[DllImport("gdi32", SetLastError=true), System.Security.SuppressUnmanagedCodeSecurity]
		public static extern unsafe
		int 
		SetPixelFormat
		( 
			IntPtr                   IntPtr_HDC, 
			int                      iPixelFormat, 
			PIXELFORMATDESCRIPTOR *  p_pfd 
		);


		// ############################################################################
		// ############################################################################
		// ############################################################################
		// ############################################################################

		// ============================================================================
		// HBITMAP CreateDIBitmap(
		//   HDC hdc,                        // handle to DC
		//   CONST BITMAPINFOHEADER *lpbmih, // bitmap data
		//   DWORD fdwInit,                  // initialization option
		//   CONST VOID *lpbInit,            // initialization data
		//   CONST BITMAPINFO *lpbmi,        // color-format data
		//   UINT fuUsage                    // color-data usage
		// );
		// ============================================================================

		[DllImport("gdi32", SetLastError=true), System.Security.SuppressUnmanagedCodeSecurity]
		public static extern unsafe
		IntPtr 
		CreateDIBitmap
		( 
			IntPtr               IntPtr_HDC, 
			BITMAPINFOHEADER *   lpbmih, 
			uint                 fdwInit,
			byte[]               dataInit, 
			BITMAPINFO *         lpbmi, 
			uint                 fuUsage 
		);

		// ############################################################################
		// ############################################################################
		// ############################################################################
		// ############################################################################

		// ============================================================================
		// HBITMAP CreateDIBSection(
		//   HDC hdc,                 // handle to DC
		//   CONST BITMAPINFO *pbmi,  // bitmap data
		//   UINT iUsage,             // data type indicator
		//   VOID **ppvBits,          // bit values
		//   HANDLE hSection,         // handle to file mapping object
		//   DWORD dwOffset           // offset to bitmap bit values
		// );
		// ============================================================================

		[DllImport("gdi32", SetLastError=true), System.Security.SuppressUnmanagedCodeSecurity]
		public static extern unsafe
		IntPtr 
		CreateDIBSection
		(
			IntPtr          IntPtr_HDC,
			BITMAPINFO *    lpbmi, 
			uint            iUsage,
			byte[]          dataBits, 
			IntPtr          hSection, 
			uint            dwOffset 
		);


		// ############################################################################
		// ############################################################################
		// ############################################################################
		// ############################################################################

		// ============================================================================
		// ============================================================================

		[DllImport("gdi32", SetLastError=true), System.Security.SuppressUnmanagedCodeSecurity]
		public static extern
		int 
		DeleteObject
		( 
			IntPtr IntPtr_HOBJECT 
		);


		// ############################################################################
		// ############################################################################
		// ############################################################################
		// ############################################################################

		// ============================================================================
		// HGDIOBJ SelectObject(  HDC hdc, HGDIOBJ hgdiobj );
		// ============================================================================

		[DllImport("gdi32", SetLastError=true), System.Security.SuppressUnmanagedCodeSecurity]
		public static extern
		IntPtr 
		SelectObject
		( 
			IntPtr   IntPtr_HDC, 
			IntPtr   IntPtr_HGDIOBJ 
		);

		// ############################################################################
		// ############################################################################
		// ############################################################################
		// ############################################################################


		// HGDIOBJ GetStockObject( int fnObject );
		[DllImport("gdi32", SetLastError=true), System.Security.SuppressUnmanagedCodeSecurity]
		public static extern
		IntPtr 
		GetStockObject
		( 
			int fnObject 
		);


		// ############################################################################
		// ############################################################################
		// ############################################################################
		// ############################################################################

	} // public class GDI
} // namespace OpenGL

// ############################################################################
// ############################################################################
// ############################################################################
// ############################################################################
