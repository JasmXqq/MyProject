// ############################################################################
// ############################################################################
// ############################################################################
// ############################################################################
//
// File Name:     wglav5_glu.cs
// Namespace:     WGLAV5
// Class:         GLU
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
// The GLU class wraps various glu*() functions found in the glu32
// library on the Windows platform.
//
// The GLU class is part of a family of classes in the WGLAV5 namespace:
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
//
// Development Notes:
//
//   This file was almost entirely generated from the following file:
// "C:\\Program Files\\Microsoft Visual Studio\\VC98\\Include\\GL\\GLU.H"
// (18,282 bytes; Friday, April 24, 1998, 12:00:00 AM).
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
	public class GLU
	{
		public const string GLU_DLL = "glu32";


		// ############################################################################
		// ############################################################################
		// ############################################################################
		// ############################################################################


		/*++ BUILD Version: 0004    // Increment this if a change has global effects

		Copyright (c) 1985-95, Microsoft Corporation

		Module Name:

			glu.h

		Abstract:

			Procedure declarations, constant definitions and macros for the OpenGL
			Utility Library.

		--*/

		/*
		** Copyright 1991-1993, Silicon Graphics, Inc.
		** All Rights Reserved.
		** 
		** This is UNPUBLISHED PROPRIETARY SOURCE CODE of Silicon Graphics, Inc.;
		** the contents of this file may not be disclosed to third parties, copied or
		** duplicated in any form, in whole or in part, without the prior written
		** permission of Silicon Graphics, Inc.
		** 
		** RESTRICTED RIGHTS LEGEND:
		** Use, duplication or disclosure by the Government is subject to restrictions
		** as set forth in subdivision (c)(1)(ii) of the Rights in Technical Data
		** and Computer Software clause at DFARS 252.227-7013, and/or in similar or
		** successor clauses in the FAR, DOD or NASA FAR Supplement. Unpublished -
		** rights reserved under the Copyright Laws of the United States.
		*/

		/*
		** Return the error string associated with a particular error code.
		** This will return 0 for an invalid error code.
		**
		** The generic function prototype that can be compiled for ANSI or Unicode
		** is defined as follows:
		**
		** LPCTSTR APIENTRY gluErrorStringWIN (GLenum errCode);
		*/

		/* backwards compatibility: */

		/* backwards compatibility: */

		/****           Callback function prototypes    ****/

		/* gluQuadricCallback */

		/* gluTessCallback */

		/* gluNurbsCallback */

		/****           Generic constants               ****/

		/* Version */
		public const uint  GLU_VERSION_1_1                      =            1;
		public const uint  GLU_VERSION_1_2                      =            1;

		/* Errors: (return value 0 = no error) */
		public const uint  GLU_INVALID_ENUM                     =       100900;
		public const uint  GLU_INVALID_VALUE                    =       100901;
		public const uint  GLU_OUT_OF_MEMORY                    =       100902;
		public const uint  GLU_INCOMPATIBLE_GL_VERSION          =       100903;

		/* StringName */
		public const uint  GLU_VERSION                          =       100800;
		public const uint  GLU_EXTENSIONS                       =       100801;

		/* Boolean */
		public const uint  GLU_TRUE                             =            1; // GL_TRUE;
		public const uint  GLU_FALSE                            =            0; // GL_FALSE;

		/****           Quadric constants               ****/

		/* QuadricNormal */
		public const uint  GLU_SMOOTH                           =       100000;
		public const uint  GLU_FLAT                             =       100001;
		public const uint  GLU_NONE                             =       100002;

		/* QuadricDrawStyle */
		public const uint  GLU_POINT                            =       100010;
		public const uint  GLU_LINE                             =       100011;
		public const uint  GLU_FILL                             =       100012;
		public const uint  GLU_SILHOUETTE                       =       100013;

		/* QuadricOrientation */
		public const uint  GLU_OUTSIDE                          =       100020;
		public const uint  GLU_INSIDE                           =       100021;

		/* Callback types: */
		/*      GLU_ERROR               100103 */

		/****           Tesselation constants           ****/

		public const double  GLU_TESS_MAX_COORD                 =      1.0e150;

		/* TessProperty */
		public const uint  GLU_TESS_WINDING_RULE                =       100140;
		public const uint  GLU_TESS_BOUNDARY_ONLY               =       100141;
		public const uint  GLU_TESS_TOLERANCE                   =       100142;

		/* TessWinding */
		public const uint  GLU_TESS_WINDING_ODD                 =       100130;
		public const uint  GLU_TESS_WINDING_NONZERO             =       100131;
		public const uint  GLU_TESS_WINDING_POSITIVE            =       100132;
		public const uint  GLU_TESS_WINDING_NEGATIVE            =       100133;
		public const uint  GLU_TESS_WINDING_ABS_GEQ_TWO         =       100134;

		/* TessCallback */
		public const uint  GLU_TESS_BEGIN                       =       100100; /* void (CALLBACK*)(GLenum    type)  */
		public const uint  GLU_TESS_VERTEX                      =       100101; /* void (CALLBACK*)(void      *data) */
		public const uint  GLU_TESS_END                         =       100102; /* void (CALLBACK*)(void)            */
		public const uint  GLU_TESS_ERROR                       =       100103; /* void (CALLBACK*)(GLenum    errno) */
		public const uint  GLU_TESS_EDGE_FLAG                   =       100104; /* void (CALLBACK*)(GLboolean boundaryEdge)  */
		public const uint  GLU_TESS_COMBINE                     =       100105; /* void (CALLBACK*)(GLdouble  coords[3],
																									void      *data[4],
																									GLfloat   weight[4],
																									void      **dataOut)     */
		public const uint  GLU_TESS_BEGIN_DATA                  =       100106; /* void (CALLBACK*)(GLenum    type,  
																									void      *polygon_data) */
		public const uint  GLU_TESS_VERTEX_DATA                 =       100107; /* void (CALLBACK*)(void      *data, 
																									void      *polygon_data) */
		public const uint  GLU_TESS_END_DATA                    =       100108; /* void (CALLBACK*)(void      *polygon_data) */
		public const uint  GLU_TESS_ERROR_DATA                  =       100109; /* void (CALLBACK*)(GLenum    errno, 
																									void      *polygon_data) */
		public const uint  GLU_TESS_EDGE_FLAG_DATA              =       100110; /* void (CALLBACK*)(GLboolean boundaryEdge,
																									void      *polygon_data) */
		public const uint  GLU_TESS_COMBINE_DATA                =       100111; /* void (CALLBACK*)(GLdouble  coords[3],
																									void      *data[4],
																									GLfloat   weight[4],
																									void      **dataOut,
																									void      *polygon_data) */

		/* TessError */
		public const uint  GLU_TESS_ERROR1                      =       100151;
		public const uint  GLU_TESS_ERROR2                      =       100152;
		public const uint  GLU_TESS_ERROR3                      =       100153;
		public const uint  GLU_TESS_ERROR4                      =       100154;
		public const uint  GLU_TESS_ERROR5                      =       100155;
		public const uint  GLU_TESS_ERROR6                      =       100156;
		public const uint  GLU_TESS_ERROR7                      =       100157;
		public const uint  GLU_TESS_ERROR8                      =       100158;

		public const uint  GLU_TESS_MISSING_BEGIN_POLYGON       = GLU_TESS_ERROR1;
		public const uint  GLU_TESS_MISSING_BEGIN_CONTOUR       = GLU_TESS_ERROR2;
		public const uint  GLU_TESS_MISSING_END_POLYGON         = GLU_TESS_ERROR3;
		public const uint  GLU_TESS_MISSING_END_CONTOUR         = GLU_TESS_ERROR4;
		public const uint  GLU_TESS_COORD_TOO_LARGE             = GLU_TESS_ERROR5;
		public const uint  GLU_TESS_NEED_COMBINE_CALLBACK       = GLU_TESS_ERROR6;

		/****           NURBS constants                 ****/

		/* NurbsProperty */
		public const uint  GLU_AUTO_LOAD_MATRIX                 =       100200;
		public const uint  GLU_CULLING                          =       100201;
		public const uint  GLU_SAMPLING_TOLERANCE               =       100203;
		public const uint  GLU_DISPLAY_MODE                     =       100204;
		public const uint  GLU_PARAMETRIC_TOLERANCE             =       100202;
		public const uint  GLU_SAMPLING_METHOD                  =       100205;
		public const uint  GLU_U_STEP                           =       100206;
		public const uint  GLU_V_STEP                           =       100207;

		/* NurbsSampling */
		public const uint  GLU_PATH_LENGTH                      =       100215;
		public const uint  GLU_PARAMETRIC_ERROR                 =       100216;
		public const uint  GLU_DOMAIN_DISTANCE                  =       100217;

		/* NurbsTrim */
		public const uint  GLU_MAP1_TRIM_2                      =       100210;
		public const uint  GLU_MAP1_TRIM_3                      =       100211;

		/* NurbsDisplay */
		/*      GLU_FILL                100012 */
		public const uint  GLU_OUTLINE_POLYGON                  =       100240;
		public const uint  GLU_OUTLINE_PATCH                    =       100241;

		/* NurbsCallback */
		/*      GLU_ERROR               100103 */

		/* NurbsErrors */
		public const uint  GLU_NURBS_ERROR1                     =       100251;
		public const uint  GLU_NURBS_ERROR2                     =       100252;
		public const uint  GLU_NURBS_ERROR3                     =       100253;
		public const uint  GLU_NURBS_ERROR4                     =       100254;
		public const uint  GLU_NURBS_ERROR5                     =       100255;
		public const uint  GLU_NURBS_ERROR6                     =       100256;
		public const uint  GLU_NURBS_ERROR7                     =       100257;
		public const uint  GLU_NURBS_ERROR8                     =       100258;
		public const uint  GLU_NURBS_ERROR9                     =       100259;
		public const uint  GLU_NURBS_ERROR10                    =       100260;
		public const uint  GLU_NURBS_ERROR11                    =       100261;
		public const uint  GLU_NURBS_ERROR12                    =       100262;
		public const uint  GLU_NURBS_ERROR13                    =       100263;
		public const uint  GLU_NURBS_ERROR14                    =       100264;
		public const uint  GLU_NURBS_ERROR15                    =       100265;
		public const uint  GLU_NURBS_ERROR16                    =       100266;
		public const uint  GLU_NURBS_ERROR17                    =       100267;
		public const uint  GLU_NURBS_ERROR18                    =       100268;
		public const uint  GLU_NURBS_ERROR19                    =       100269;
		public const uint  GLU_NURBS_ERROR20                    =       100270;
		public const uint  GLU_NURBS_ERROR21                    =       100271;
		public const uint  GLU_NURBS_ERROR22                    =       100272;
		public const uint  GLU_NURBS_ERROR23                    =       100273;
		public const uint  GLU_NURBS_ERROR24                    =       100274;
		public const uint  GLU_NURBS_ERROR25                    =       100275;
		public const uint  GLU_NURBS_ERROR26                    =       100276;
		public const uint  GLU_NURBS_ERROR27                    =       100277;
		public const uint  GLU_NURBS_ERROR28                    =       100278;
		public const uint  GLU_NURBS_ERROR29                    =       100279;
		public const uint  GLU_NURBS_ERROR30                    =       100280;
		public const uint  GLU_NURBS_ERROR31                    =       100281;
		public const uint  GLU_NURBS_ERROR32                    =       100282;
		public const uint  GLU_NURBS_ERROR33                    =       100283;
		public const uint  GLU_NURBS_ERROR34                    =       100284;
		public const uint  GLU_NURBS_ERROR35                    =       100285;
		public const uint  GLU_NURBS_ERROR36                    =       100286;
		public const uint  GLU_NURBS_ERROR37                    =       100287;

		/****           Backwards compatibility for old tesselator           ****/

		/* Contours types -- obsolete! */
		public const uint  GLU_CW                               =       100120;
		public const uint  GLU_CCW                              =       100121;
		public const uint  GLU_INTERIOR                         =       100122;
		public const uint  GLU_EXTERIOR                         =       100123;
		public const uint  GLU_UNKNOWN                          =       100124;

		/* Names without "TESS_" prefix */
		public const uint  GLU_BEGIN                            = GLU_TESS_BEGIN;
		public const uint  GLU_VERTEX                           = GLU_TESS_VERTEX;
		public const uint  GLU_END                              = GLU_TESS_END;
		public const uint  GLU_ERROR                            = GLU_TESS_ERROR;
		public const uint  GLU_EDGE_FLAG                        = GLU_TESS_EDGE_FLAG;

		/* __GLU_H__ */
		/* __glu_h__ */


		// ############################################################################
		// ############################################################################
		// ############################################################################
		// ############################################################################
		//
		//
		// OpenGL functions from GL.H with pointer types intact.
		//
		//
		// ############################################################################
		// ############################################################################
		// ############################################################################
		// ############################################################################


		[DllImport(GLU_DLL)] public unsafe static extern String gluErrorString           ( uint errCode );
		[DllImport(GLU_DLL)] public unsafe static extern String gluGetString             ( uint name );
		[DllImport(GLU_DLL)] public unsafe static extern void   gluOrtho2D               ( double left, double right, double bottom, double top );
		[DllImport(GLU_DLL)] public unsafe static extern void   gluPerspective           ( double fovy, double aspect, double zNear, double zFar );
		[DllImport(GLU_DLL)] public unsafe static extern void   gluPickMatrix            ( double x, double y, double width, double height, int * viewport );
		[DllImport(GLU_DLL)] public unsafe static extern void   gluLookAt                ( double eyex, double eyey, double eyez, double centerx, double centery, double centerz, double upx, double upy, double upz );
		[DllImport(GLU_DLL)] public unsafe static extern int    gluProject               ( double objx, double objy, double objz, double * modelMatrix, double * projMatrix, int * viewport, double * winx, double * winy, double * winz );
		[DllImport(GLU_DLL)] public unsafe static extern int    gluUnProject             ( double winx, double winy, double winz, double * modelMatrix, double * projMatrix, int * viewport, double * objx, double * objy, double * objz );
		[DllImport(GLU_DLL)] public unsafe static extern int    gluScaleImage            ( uint format, int widthin, int heightin, uint typein, void * datain, int widthout, int heightout, uint typeout, void * dataout );
		[DllImport(GLU_DLL)] public unsafe static extern int    gluBuild1DMipmaps        ( uint target, int components, int width, uint format, uint type, void * data );
		[DllImport(GLU_DLL)] public unsafe static extern int    gluBuild2DMipmaps        ( uint target, int components, int width, int height, uint format, uint type, void * data );
		[DllImport(GLU_DLL)] public unsafe static extern IntPtr gluNewQuadric            ( );
		[DllImport(GLU_DLL)] public unsafe static extern void   gluDeleteQuadric         ( IntPtr state );
		[DllImport(GLU_DLL)] public unsafe static extern void   gluQuadricNormals        ( IntPtr quadObject, uint normals );
		[DllImport(GLU_DLL)] public unsafe static extern void   gluQuadricTexture        ( IntPtr quadObject, byte textureCoords );
		[DllImport(GLU_DLL)] public unsafe static extern void   gluQuadricOrientation    ( IntPtr quadObject, uint orientation );
		[DllImport(GLU_DLL)] public unsafe static extern void   gluQuadricDrawStyle      ( IntPtr quadObject, uint drawStyle );
		[DllImport(GLU_DLL)] public unsafe static extern void   gluCylinder              ( IntPtr qobj, double baseRadius, double topRadius, double height, int slices, int stacks );
		[DllImport(GLU_DLL)] public unsafe static extern void   gluDisk                  ( IntPtr qobj, double innerRadius, double outerRadius, int slices, int loops );
		[DllImport(GLU_DLL)] public unsafe static extern void   gluPartialDisk           ( IntPtr qobj, double innerRadius, double outerRadius, int slices, int loops, double startAngle, double sweepAngle );
		[DllImport(GLU_DLL)] public unsafe static extern void   gluSphere                ( IntPtr qobj, double radius, int slices, int stacks );
		[DllImport(GLU_DLL)] public unsafe static extern void   gluQuadricCallback       ( IntPtr qobj, uint which, IntPtr fn );
		[DllImport(GLU_DLL)] public unsafe static extern IntPtr gluNewTess               ( );
		[DllImport(GLU_DLL)] public unsafe static extern void   gluDeleteTess            ( IntPtr tess );
		[DllImport(GLU_DLL)] public unsafe static extern void   gluTessBeginPolygon      ( IntPtr tess, void * polygon_data );
		[DllImport(GLU_DLL)] public unsafe static extern void   gluTessBeginContour      ( IntPtr tess );
		[DllImport(GLU_DLL)] public unsafe static extern void   gluTessVertex            ( IntPtr tess, double coords, void * data );
		[DllImport(GLU_DLL)] public unsafe static extern void   gluTessEndContour        ( IntPtr tess );
		[DllImport(GLU_DLL)] public unsafe static extern void   gluTessEndPolygon        ( IntPtr tess );
		[DllImport(GLU_DLL)] public unsafe static extern void   gluTessProperty          ( IntPtr tess, uint which, double valuex );
		[DllImport(GLU_DLL)] public unsafe static extern void   gluTessNormal            ( IntPtr tess, double x, double y, double z );
		[DllImport(GLU_DLL)] public unsafe static extern void   gluTessCallback          ( IntPtr tess, uint which, IntPtr fn );
		[DllImport(GLU_DLL)] public unsafe static extern void   gluGetTessProperty       ( IntPtr tess, uint which, double * valuex );
		[DllImport(GLU_DLL)] public unsafe static extern IntPtr gluNewNurbsRenderer      ( );
		[DllImport(GLU_DLL)] public unsafe static extern void   gluDeleteNurbsRenderer   ( IntPtr nobj );
		[DllImport(GLU_DLL)] public unsafe static extern void   gluBeginSurface          ( IntPtr nobj );
		[DllImport(GLU_DLL)] public unsafe static extern void   gluBeginCurve            ( IntPtr nobj );
		[DllImport(GLU_DLL)] public unsafe static extern void   gluEndCurve              ( IntPtr nobj );
		[DllImport(GLU_DLL)] public unsafe static extern void   gluEndSurface            ( IntPtr nobj );
		[DllImport(GLU_DLL)] public unsafe static extern void   gluBeginTrim             ( IntPtr nobj );
		[DllImport(GLU_DLL)] public unsafe static extern void   gluEndTrim               ( IntPtr nobj );
		[DllImport(GLU_DLL)] public unsafe static extern void   gluPwlCurve              ( IntPtr nobj, int count, float * array, int stride, uint type );
		[DllImport(GLU_DLL)] public unsafe static extern void   gluNurbsCurve            ( IntPtr nobj, int nknots, float * knot, int stride, float * ctlarray, int order, uint type );
		[DllImport(GLU_DLL)] public unsafe static extern void   gluNurbsSurface          ( IntPtr nobj, int sknot_count, float * sknot, int tknot_count, float * tknot, int s_stride, int t_stride, float * ctlarray, int sorder, int torder, uint type );
		[DllImport(GLU_DLL)] public unsafe static extern void   gluLoadSamplingMatrices  ( IntPtr nobj, float * modelMatrix, float * projMatrix, int * viewport );
		[DllImport(GLU_DLL)] public unsafe static extern void   gluNurbsProperty         ( IntPtr nobj, uint property, float valuex );
		[DllImport(GLU_DLL)] public unsafe static extern void   gluGetNurbsProperty      ( IntPtr nobj, uint property, float * valuex );
		[DllImport(GLU_DLL)] public unsafe static extern void   gluNurbsCallback         ( IntPtr nobj, uint which, IntPtr fn );
		[DllImport(GLU_DLL)] public unsafe static extern void   gluBeginPolygon          ( IntPtr tess );
		[DllImport(GLU_DLL)] public unsafe static extern void   gluNextContour           ( IntPtr tess, uint type );
		[DllImport(GLU_DLL)] public unsafe static extern void   gluEndPolygon            ( IntPtr tess );


		// ############################################################################
		// ############################################################################
		// ############################################################################
		// ############################################################################
		//
		//
		// C# Alternative Overrides for OpenGL functions involving pointers.
		//
		//
		// ############################################################################
		// ############################################################################
		// ############################################################################
		// ############################################################################


		public unsafe static int    gluProject               ( double objx, double objy, double objz, double[] modelMatrix, double[] projMatrix, int[] viewport, double[] winx, double[] winy, double[] winz )
		{
		fixed ( double * p_modelMatrix = modelMatrix ) { 
		fixed ( double * p_projMatrix  = projMatrix  ) { 
		fixed ( int    * p_viewport    = viewport    ) { 
		fixed ( double * p_winx        = winx        ) { 
		fixed ( double * p_winy        = winy        ) { 
		fixed ( double * p_winz        = winz        ) { 
		return( gluProject( objx, objy, objz, p_modelMatrix, p_projMatrix, p_viewport, p_winx, p_winy, p_winz ) );
		} } } } } } 
		}


		public unsafe static int    gluUnProject             ( double winx, double winy, double winz, double[] modelMatrix, double[] projMatrix, int[] viewport, double[] objx, double[] objy, double[] objz )
		{
		fixed ( double * p_modelMatrix = modelMatrix ) { 
		fixed ( double * p_projMatrix  = projMatrix  ) { 
		fixed ( int    * p_viewport    = viewport    ) { 
		fixed ( double * p_objx        = objx        ) { 
		fixed ( double * p_objy        = objy        ) { 
		fixed ( double * p_objz        = objz        ) { 
		return( gluUnProject( winx, winy, winz, p_modelMatrix, p_projMatrix, p_viewport, p_objx, p_objy, p_objz ) );
		} } } } } }
		}


		public unsafe static int    gluScaleImage            ( uint format, int widthin, int heightin, uint typein, IntPtr datain, int widthout, int heightout, uint typeout, IntPtr dataout )
		{
		return( gluScaleImage( format, widthin, heightin, typein, datain, widthout, heightout, typeout, dataout ) );
		}

		public unsafe static int    gluBuild1DMipmaps        ( uint target, int components, int width, uint format, uint type, IntPtr data )
		{
		return( gluBuild1DMipmaps( target, components, width, format, type, data ) );
		}

		public unsafe static int    gluBuild2DMipmaps        ( uint target, int components, int width, int height, uint format, uint type, IntPtr data )
		{
		return( gluBuild2DMipmaps( target, components, width, height, format, type, data ) );
		}



		public unsafe static void   gluPwlCurve              ( IntPtr nobj, int count, float[] array, int stride, uint type )
		{
		fixed ( float * p_array = array ) { 
		gluPwlCurve( nobj, count, p_array, stride, type );
		} 
		}

		public unsafe static void   gluNurbsCurve            ( IntPtr nobj, int nknots, float[] knot, int stride, float[] ctlarray, int order, uint type )
		{
		fixed ( float * p_knot = knot ) { 
		fixed ( float * p_ctlarray = ctlarray ) { 
		gluNurbsCurve( nobj, nknots, p_knot, stride, p_ctlarray, order, type );
		} } 
		}

		public unsafe static void   gluNurbsSurface          ( IntPtr nobj, int sknot_count, float[] sknot, int tknot_count, float[] tknot, int s_stride, int t_stride, float[] ctlarray, int sorder, int torder, uint type )
		{
		fixed ( float * p_sknot = sknot ) { 
		fixed ( float * p_tknot = tknot ) { 
		fixed ( float * p_ctlarray = ctlarray ) { 
		gluNurbsSurface( nobj, sknot_count, p_sknot, tknot_count, p_tknot, s_stride, t_stride, p_ctlarray, sorder, torder, type );
		} } } 
		}


		public unsafe static void   gluGetNurbsProperty      ( IntPtr nobj, uint property, float[] valuex )
		{
		fixed ( float * p_valuex = valuex ) { 
		gluGetNurbsProperty( nobj, property, p_valuex );
		} 
		}


		// ############################################################################
		// ############################################################################
		// ############################################################################
		// ############################################################################


	} // public class GLU
} // namespace OpenGL


// ############################################################################
// ############################################################################
// ############################################################################
// ############################################################################
