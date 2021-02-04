// ############################################################################
// ############################################################################
// ############################################################################
// ############################################################################
//
// File Name:     wglav5_demo.cs
// Namespace:     WGLAV5
// Class:         DEMO
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
// The DEMO class demonstrates the use of the WGLAV5 family of classes.
//
// The DEMO class is part of a family of classes in the WGLAV5 namespace:
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
using System.Drawing;

using System.Runtime.InteropServices;         // Necessary for [DllImport(...)]

using graduationDesign;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;

// ############################################################################
// ############################################################################
// ############################################################################
// ############################################################################
namespace WGLAV5
{
	public class DEMO
	{

		//------------2005.1.11------------------
		public bool	light = true;				// Lighting ON/OFF ( NEW )
		public bool blend = false;				// Blending OFF/ON? ( NEW )rr

		public bool	lp = false;					// L Pressed? ( NEW )
		public bool	fp = false;					// F Pressed? ( NEW )
		public bool	bp = false;					// B Pressed? ( NEW )

//		public float xrot = 0.0f;				// X-axis 旋转
//		public float yrot = 0.0f;				// Y-axis 旋转
//		public float zrot = 0.0f;				// Z-axis 旋转

		public float xspeed = 0.0f;				// X 旋转速度
		public float yspeed = 0.0f;				// Y 旋转速度
		public float z = -0.0f;				    // 在屏幕内的深度

		public uint box;						// 显示列表
		public uint top;

		/// <summary>
		/// 
		public int  m_nxAngle = -90;
		public int  m_nyAngle = 0;
		public int  m_nzAngle = 0;
		public bool m_bStop   = false;
		public bool m_bShowAxis = true;
		public bool enalbeTexture = true;


		PaintBorder pb=new PaintBorder();
        DrillHolesModel hm = new DrillHolesModel();
        LayerModel gl = new LayerModel();

        public uint[] texture = new uint[6];	// 纹理数组
		/// </summary>

		// 光照分量
		public float[] light_ambient  = {0.9f, 0.9f, 0.9f, 1.0f};
		public float[] light_diffuse  = {1.0f, 1.0f, 1.0f, 1.0f};
		public float[] light_specular = {1.0f, 1.0f, 1.0f, 1.0f};
		
		// 光照位置
		//public float[] light_position = {50.0f, 50.0f, 50.0f, 1.0f};
        public float[] light_position = { 1000,1000,2000,1.0f};


		public int filter = 0;					// 使用的滤波方式
		//------------2005.1.11------------------


		// ############################################################################
		// ############################################################################
		// ############################################################################
		// ############################################################################

		public uint            m_uint_AttemptedInitialization = 0;
		public IntPtr          m_IntPtr_HWND                  = IntPtr.Zero;
		public IntPtr          m_IntPtr_HDC                   = IntPtr.Zero;
		public IntPtr          m_IntPtr_HGLRC                 = IntPtr.Zero;
		public uint            m_uint_FrameCounter            = 0;
		public int             m_int_Milliseconds             = 0;
		public int             m_int_Input                    = 0;

		public uint            m_uint_FontDisplayListBase_BITMAP_Verdana   = 0;
		public uint            m_uint_FontDisplayListBase_BITMAP_Symbol    = 0;
		public uint            m_uint_FontDisplayListBase_BITMAP_Wingdings = 0;

		public uint            m_uint_FontDisplayListBase_POLYGONS_Verdana   = 0;
		public uint            m_uint_FontDisplayListBase_POLYGONS_Symbol    = 0;
		public uint            m_uint_FontDisplayListBase_POLYGONS_Wingdings = 0;



		// ############################################################################
		// ########################################################################
		// ############################################################################
		// ############################################################################

		// ============================================================================
		// The following function creates an OpenGL Rendering Context (RC), but please
		// call DemoInitOpenGL() instead.  See below.
		// ============================================================================

		private unsafe void DemoCreateRenderingContext
		(
		ref IntPtr  ref_IntPtr_HDC,
		ref IntPtr  ref_IntPtr_HGLRC
		) 
		{
			ref_IntPtr_HGLRC = IntPtr.Zero;

			GDI.PIXELFORMATDESCRIPTOR pfd = new GDI.PIXELFORMATDESCRIPTOR();

		// --------------------------------------------------------------------------
			pfd.nSize           = 40; // sizeof(PIXELFORMATDESCRIPTOR); 
			pfd.nVersion        = 1; 
			pfd.dwFlags         = (GDI.PFD_DRAW_TO_WINDOW | GDI.PFD_SUPPORT_OPENGL | GDI.PFD_DOUBLEBUFFER); 
			pfd.iPixelType      = (byte)(GDI.PFD_TYPE_RGBA);
			pfd.cColorBits      = 32; 
			pfd.cRedBits        = 0; 
			pfd.cRedShift       = 0; 
			pfd.cGreenBits      = 0; 
			pfd.cGreenShift     = 0; 
			pfd.cBlueBits       = 0; 
			pfd.cBlueShift      = 0; 
			pfd.cAlphaBits      = 0; 
			pfd.cAlphaShift     = 0; 
			pfd.cAccumBits      = 0; 
			pfd.cAccumRedBits   = 0; 
			pfd.cAccumGreenBits = 0;
			pfd.cAccumBlueBits  = 0; 
			pfd.cAccumAlphaBits = 0;
			pfd.cDepthBits      = 32; 
			pfd.cStencilBits    = 0; 
			pfd.cAuxBuffers     = 0; 
			pfd.iLayerType      = (byte)(GDI.PFD_MAIN_PLANE);
			pfd.bReserved       = 0; 
			pfd.dwLayerMask     = 0; 
			pfd.dwVisibleMask   = 0; 
			pfd.dwDamageMask    = 0; 
		// --------------------------------------------------------------------------


		// --------------------------------------------------------------------------
		// Choose Pixel Format
		// --------------------------------------------------------------------------
			int  iPixelFormat = 0;

			//fixed ( GDI.PIXELFORMATDESCRIPTOR * p_pfd = (&(pfd)) )
            GDI.PIXELFORMATDESCRIPTOR* p_pfd = (&(pfd));
			{
				iPixelFormat = GDI.ChoosePixelFormat( ref_IntPtr_HDC, p_pfd );
			}

			if (0 == iPixelFormat)
			{
				uint   uint_LastError = USER.GetLastError();
				string string_Message = "ChoosePixelFormat() FAILED:  Error: " + uint_LastError;
				USER.MessageBox( IntPtr.Zero, string_Message, "WGL.DemoGetRenderingContext() : ERROR", USER.MB_OK );
				return;
			}
		// --------------------------------------------------------------------------


		// --------------------------------------------------------------------------
		// Set Pixel Format
		// --------------------------------------------------------------------------
			int int_Result_SPF = 0;

			//fixed ( GDI.PIXELFORMATDESCRIPTOR * p_pfd = (&(pfd)) )
            //p_pfd = (&(pfd));
			{
				int_Result_SPF = GDI.SetPixelFormat( ref_IntPtr_HDC, iPixelFormat, p_pfd );
			}

			if (0 == int_Result_SPF)
			{
				uint   uint_LastError = USER.GetLastError();
				string string_Message = "SetPixelFormat() FAILED.  Error: " + uint_LastError;
				USER.MessageBox( IntPtr.Zero, string_Message, "WGL.DemoGetRenderingContext() : ERROR", USER.MB_OK );
				return;
			}
		// --------------------------------------------------------------------------


		// --------------------------------------------------------------------------
		// Create Rendering Context (RC)
		// NOTE: You will get the following error:
		//             126 : ERROR_MOD_NOT_FOUND
		// if you attempt to create a render context too soon after creating a
		// window and getting its Device Context (DC).
		// See the comments for WGL.DemoInitOpenGL() on how to use a call to
		// WGL.wglSwapBuffers( ref_uint_DC ) before attempting to create the RC.
		// --------------------------------------------------------------------------
			ref_IntPtr_HGLRC = WGL.wglCreateContext( ref_IntPtr_HDC );

			if (IntPtr.Zero == ref_IntPtr_HGLRC)
			{    
				uint   uint_LastError = USER.GetLastError();
				string string_Message = "wglCreateContext() FAILED.  Error: " + uint_LastError;
				USER.MessageBox( IntPtr.Zero, string_Message, "WGL.DemoGetRenderingContext() : ERROR", USER.MB_OK );
				return;
			}
		// --------------------------------------------------------------------------


		// --------------------------------------------------------------------------
		// Make the new Render Context (RC) the current Render Context (RC)
		// --------------------------------------------------------------------------
			int int_Result_MC = 0;

			int_Result_MC = WGL.wglMakeCurrent( ref_IntPtr_HDC, ref_IntPtr_HGLRC );

			if (0 == int_Result_MC)
			{
				uint   uint_LastError = USER.GetLastError();
				string string_Message = "wglMakeCurrent() FAILED.  Error: " + uint_LastError;
				USER.MessageBox( IntPtr.Zero, string_Message, "WGL.DemoGetRenderingContext() : ERROR", USER.MB_OK );
				// ***************************
				WGL.wglDeleteContext( ref_IntPtr_HGLRC );
				ref_IntPtr_HGLRC = IntPtr.Zero;
				// ***************************
				return;
			}
		// --------------------------------------------------------------------------        
		}


		// ############################################################################
		// ############################################################################
		// ############################################################################
		// ############################################################################

		// ============================================================================
		// The following function creates an OpenGL Rendering Context (RC).
		// ============================================================================

        public void DemoInitOpenGL
        (
        IntPtr IntPtr_HWND,  // in
        ref IntPtr ref_IntPtr_HDC,   // out
        ref IntPtr ref_IntPtr_HGLRC  // out
        )
        {
            ref_IntPtr_HDC = USER.GetDC(IntPtr_HWND);

            // --------------------------------------------------------------------------
            // CAUTION: Not doing the following WGL.wglSwapBuffers() on the DC will
            // result in a failure to subsequently create the RC.
            // --------------------------------------------------------------------------
            WGL.wglSwapBuffers(ref_IntPtr_HDC);

            // --------------------------------------------------------------------------
            // Create an OpenGL Rendering Context (RC)
            // --------------------------------------------------------------------------
            DemoCreateRenderingContext(ref ref_IntPtr_HDC, ref ref_IntPtr_HGLRC);

            if (IntPtr.Zero == ref_IntPtr_HGLRC)
            {
                USER.MessageBox(IntPtr.Zero, "Failed to create OpenGL Rendering Context (RC)",
                        "WGL.DemoInitOpenGL() : ERROR", USER.MB_OK);
                return;
            }

            //==========================
            ////////////////////////////////////////////////////////////////////


            GL.glClearColor(0, 0, 0, 0);				// 背景色为黑色
           // GL.glClearColor(0.4f, 0.5f, 1.0f, 0.0f);	// 背景色为天蓝色
          // GL.glClearColor(1, 1, 1, 1);

           // GL.glHint(GL.GL_PERSPECTIVE_CORRECTION_HINT, GL.GL_FASTEST);

            GL.glLightfv(GL.GL_LIGHT0, GL.GL_AMBIENT, light_ambient);		// 设置环境光Setup The Ambient Light
            GL.glLightfv(GL.GL_LIGHT0, GL.GL_DIFFUSE, light_diffuse);		// 设置散射光Setup The Diffuse Light
            GL.glLightfv(GL.GL_LIGHT0, GL.GL_SPECULAR, light_specular);		// 设置聚光Setup The Specular Light
            GL.glLightfv(GL.GL_LIGHT0, GL.GL_POSITION, light_position);

            GL.glEnable(GL.GL_LIGHT0);										// 启动0号光源Enable Light 0
            GL.glDepthFunc(GL.GL_LESS);


            GL.glEnable(GL.GL_DEPTH_TEST);		// 
            GL.glEnable(GL.GL_LIGHTING);		// 使用光照
            GL.glEnable(GL.GL_NORMALIZE);		// 
            //GL.glShadeModel(GL.GL_FLAT);		// 
            GL.glShadeModel(GL.GL_SMOOTH);									// 允许光滑阴影Enable Smooth Shading

            //	glCullFace(GL_FRONT);
            //	glEnable(GL_CULL_FACE);
            GL.glPolygonMode(GL.GL_FRONT, GL.GL_FILL);						// 前面用纹理填充
            //GL.glPolygonMode(GL.GL_FRONT, GL.GL_LINE);
            //GL.glPolygonMode(GL.GL_BACK, GL.GL_LINE);	            // 背面用线条表示

            if (this.light)
            {
                GL.glDisable(GL.GL_LIGHTING);
                GL.glDisable(GL.GL_LIGHT0);

                CreateDisplayLists();		//创建坐标轴显示列表
                pb.getScreenCoordinates();
                pb.showBorder();
                hm.getScreenCoordinates();

                //hm.showHoles();
                //hm.showHolesCylider();

                //gl.drawK1Surface();
                //gl.clearList();

                gl.drawK2Surface();
                gl.clearList();

                //gl.drawK3Surface();
                //gl.clearList();

                //gl.drawK4Surface();
                //gl.clearList();

                //gl.drawK5Surface();
                //gl.clearList();

                //gl.drawK6Surface();
                //gl.clearList();

                //gl.drawK7Surface();
                //gl.clearList();

                //gl.drawK8Surface();
                //gl.clearList();

                //gl.drawM3Coal();
                //gl.clearList();

                GL.glEnable(GL.GL_LIGHT0);
                GL.glEnable(GL.GL_LIGHTING);
                //			GL.glGenTextures(1, texture);
                //LoadTextures("grass.bmp");

                //			GL.glEnable(GL.GL_TEXTURE_2D);									// 启动纹理映射Enable Texture Mapping
                //			GL.glBindTexture(GL.GL_TEXTURE_2D, texture[0]);

            }
         
        }


        public void DemoInitOpenGLLayer
      (
      IntPtr IntPtr_HWND,  // in
      ref IntPtr ref_IntPtr_HDC,   // out
      ref IntPtr ref_IntPtr_HGLRC  // out
      )
        {
            ref_IntPtr_HDC = USER.GetDC(IntPtr_HWND);

            // --------------------------------------------------------------------------
            // CAUTION: Not doing the following WGL.wglSwapBuffers() on the DC will
            // result in a failure to subsequently create the RC.
            // --------------------------------------------------------------------------
            WGL.wglSwapBuffers(ref_IntPtr_HDC);

            // --------------------------------------------------------------------------
            // Create an OpenGL Rendering Context (RC)
            // --------------------------------------------------------------------------
            DemoCreateRenderingContext(ref ref_IntPtr_HDC, ref ref_IntPtr_HGLRC);

            if (IntPtr.Zero == ref_IntPtr_HGLRC)
            {
                USER.MessageBox(IntPtr.Zero, "Failed to create OpenGL Rendering Context (RC)",
                        "WGL.DemoInitOpenGL() : ERROR", USER.MB_OK);
                return;
            }

            //==========================
            ////////////////////////////////////////////////////////////////////


             GL.glClearColor(0, 0, 0, 0);				// 背景色为黑色
            //GL.glClearColor(0.4f, 0.5f, 1.0f, 0.0f);	// 背景色为天蓝色
            // GL.glClearColor(1, 1, 1, 1);

            // GL.glHint(GL.GL_PERSPECTIVE_CORRECTION_HINT, GL.GL_FASTEST);

            GL.glLightfv(GL.GL_LIGHT0, GL.GL_AMBIENT, light_ambient);		// 设置环境光Setup The Ambient Light
            GL.glLightfv(GL.GL_LIGHT0, GL.GL_DIFFUSE, light_diffuse);		// 设置散射光Setup The Diffuse Light
            GL.glLightfv(GL.GL_LIGHT0, GL.GL_SPECULAR, light_specular);		// 设置聚光Setup The Specular Light
            GL.glLightfv(GL.GL_LIGHT0, GL.GL_POSITION, light_position);

            GL.glEnable(GL.GL_LIGHT0);										// 启动0号光源Enable Light 0
            GL.glDepthFunc(GL.GL_LESS);


            GL.glEnable(GL.GL_DEPTH_TEST);		// 
            GL.glEnable(GL.GL_LIGHTING);		// 使用光照
            GL.glEnable(GL.GL_NORMALIZE);		// 
            //GL.glShadeModel(GL.GL_FLAT);		// 
            GL.glShadeModel(GL.GL_SMOOTH);									// 允许光滑阴影Enable Smooth Shading

            //	glCullFace(GL_FRONT);
            //	glEnable(GL_CULL_FACE);
            GL.glPolygonMode(GL.GL_FRONT, GL.GL_FILL);						// 前面用纹理填充
            //GL.glPolygonMode(GL.GL_FRONT, GL.GL_LINE);
            //GL.glPolygonMode(GL.GL_BACK, GL.GL_LINE);	            // 背面用线条表示

            if (this.light)
            {
                GL.glDisable(GL.GL_LIGHTING);
                GL.glDisable(GL.GL_LIGHT0);

                CreateDisplayLists();		//创建坐标轴显示列表
                pb.getScreenCoordinates();
                pb.showBorder();
                hm.getScreenCoordinates();

                //hm.showHoles();
                //hm.showHolesCylider();

                gl.drawK1Surface();
                gl.clearList();

                gl.drawK2Surface();
                gl.clearList();

                gl.drawK3Surface();
                gl.clearList();

                gl.drawK4Surface();
                gl.clearList();

                gl.drawK5Surface();
                gl.clearList();

                gl.drawK6Surface();
                gl.clearList();

                gl.drawK7Surface();
                gl.clearList();

                gl.drawK8Surface();
                gl.clearList();

                gl.drawM3Coal();
                gl.clearList();

                GL.glEnable(GL.GL_LIGHT0);
                GL.glEnable(GL.GL_LIGHTING);
                //			GL.glGenTextures(1, texture);
                //LoadTextures("grass.bmp");

                //			GL.glEnable(GL.GL_TEXTURE_2D);									// 启动纹理映射Enable Texture Mapping
                //			GL.glBindTexture(GL.GL_TEXTURE_2D, texture[0]);

            }

        }

        public void DemoInitOpenGLDrill
        (
        IntPtr IntPtr_HWND,  // in
        ref IntPtr ref_IntPtr_HDC,   // out
        ref IntPtr ref_IntPtr_HGLRC  // out
        )
        {
            ref_IntPtr_HDC = USER.GetDC(IntPtr_HWND);

            // --------------------------------------------------------------------------
            // CAUTION: Not doing the following WGL.wglSwapBuffers() on the DC will
            // result in a failure to subsequently create the RC.
            // --------------------------------------------------------------------------
            WGL.wglSwapBuffers(ref_IntPtr_HDC);

            // --------------------------------------------------------------------------
            // Create an OpenGL Rendering Context (RC)
            // --------------------------------------------------------------------------
            DemoCreateRenderingContext(ref ref_IntPtr_HDC, ref ref_IntPtr_HGLRC);

            if (IntPtr.Zero == ref_IntPtr_HGLRC)
            {
                USER.MessageBox(IntPtr.Zero, "Failed to create OpenGL Rendering Context (RC)",
                        "WGL.DemoInitOpenGL() : ERROR", USER.MB_OK);
                return;
            }

            //==========================
            ////////////////////////////////////////////////////////////////////


            GL.glClearColor(0, 0, 0, 0);				// 背景色为黑色
            // GL.glClearColor(0.4f, 0.5f, 1.0f, 0.0f);	// 背景色为天蓝色
            // GL.glClearColor(1, 1, 1, 1);

            // GL.glHint(GL.GL_PERSPECTIVE_CORRECTION_HINT, GL.GL_FASTEST);

            GL.glLightfv(GL.GL_LIGHT0, GL.GL_AMBIENT, light_ambient);		// 设置环境光Setup The Ambient Light
            GL.glLightfv(GL.GL_LIGHT0, GL.GL_DIFFUSE, light_diffuse);		// 设置散射光Setup The Diffuse Light
            GL.glLightfv(GL.GL_LIGHT0, GL.GL_SPECULAR, light_specular);		// 设置聚光Setup The Specular Light
            GL.glLightfv(GL.GL_LIGHT0, GL.GL_POSITION, light_position);

            GL.glEnable(GL.GL_LIGHT0);										// 启动0号光源Enable Light 0
            GL.glDepthFunc(GL.GL_LESS);


            GL.glEnable(GL.GL_DEPTH_TEST);		// 
            GL.glEnable(GL.GL_LIGHTING);		// 使用光照
            GL.glEnable(GL.GL_NORMALIZE);		// 
            //GL.glShadeModel(GL.GL_FLAT);		// 
            GL.glShadeModel(GL.GL_SMOOTH);									// 允许光滑阴影Enable Smooth Shading

            //	glCullFace(GL_FRONT);
            //	glEnable(GL_CULL_FACE);
            GL.glPolygonMode(GL.GL_FRONT, GL.GL_FILL);						// 前面用纹理填充
            //GL.glPolygonMode(GL.GL_FRONT, GL.GL_LINE);
            //GL.glPolygonMode(GL.GL_BACK, GL.GL_LINE);	            // 背面用线条表示

            if (this.light)
            {
                GL.glDisable(GL.GL_LIGHTING);
                GL.glDisable(GL.GL_LIGHT0);

                CreateDisplayLists();		//创建坐标轴显示列表
                pb.getScreenCoordinates();
                pb.showBorder();
                hm.getScreenCoordinates();

                hm.showHoles();
                hm.showHolesCylider();

                GL.glEnable(GL.GL_LIGHT0);
                GL.glEnable(GL.GL_LIGHTING);
                //			GL.glGenTextures(1, texture);
                //LoadTextures("grass.bmp");

                //			GL.glEnable(GL.GL_TEXTURE_2D);									// 启动纹理映射Enable Texture Mapping
                //			GL.glBindTexture(GL.GL_TEXTURE_2D, texture[0]);

            }

        }

		//======================================================================
		//----------------装入纹理------2005.1.11-----------------
		// LoadTextures()方法装入文件名为textureFile的位图纹理。
		//
		// 分别采用三种纹理映射方式：Nearest/Linear/MipMap
		// 纹理数据存储在texture数组中
		//======================================================================
        unsafe protected bool LoadTextures(string textureFile)
        {
            Bitmap image = null;
            try
            {
                // 如果文件不存在或没有找到，抛出一个ArgumentException例外(否则仅仅返回null)
                image = new Bitmap(textureFile);								// 新建位图对象
            }
            catch (System.ArgumentException)
            {
                System.Windows.Forms.MessageBox.Show
                    ("不能装入纹理文件 " + textureFile + ". 文件不存在或不在应用程序运行的子文件夹中.",
                    "错误", System.Windows.Forms.MessageBoxButtons.OK);
            }
            if (image != null)
            {
                image.RotateFlip(RotateFlipType.RotateNoneFlipY);				// 指定位图对象的旋转、翻转或者同时旋转和翻转:
                // ---------没有后跟垂直翻转的旋转
                System.Drawing.Imaging.BitmapData bitmapdata;
                Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);// 创建用于锁定的image范围

                bitmapdata = image.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly,
                    System.Drawing.Imaging.PixelFormat.Format24bppRgb);			// 将对象image锁定在内存中

                GL.glGenTextures(4, this.texture);								// 产生4个纹理名

                // 创建最近的滤波纹理Create Nearest Filtered Texture
                GL.glBindTexture(GL.GL_TEXTURE_2D, this.texture[0]);			// 定义纹理
                GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MAG_FILTER, (int)GL.GL_NEAREST);
                GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MIN_FILTER, (int)GL.GL_NEAREST);
                GL.glTexImage2D(GL.GL_TEXTURE_2D, 0, (int)GL.GL_RGB, image.Width, image.Height, 0,
                    GL.GL_BGR_EXT, GL.GL_UNSIGNED_BYTE, (void*)bitmapdata.Scan0);

                // 创建线性滤波纹理Create Linear Filtered Texture
                GL.glBindTexture(GL.GL_TEXTURE_2D, this.texture[1]);			// 定义纹理
                GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MAG_FILTER, (int)GL.GL_LINEAR);
                GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MIN_FILTER, (int)GL.GL_LINEAR);
                GL.glTexImage2D(GL.GL_TEXTURE_2D, 0, (int)GL.GL_RGB, image.Width, image.Height, 0,
                    GL.GL_BGR_EXT, GL.GL_UNSIGNED_BYTE, (void*)bitmapdata.Scan0);

                // 创建位映射纹理Create MipMapped Texture
                GL.glBindTexture(GL.GL_TEXTURE_2D, this.texture[2]);			// 定义纹理
                GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MAG_FILTER, (int)GL.GL_LINEAR);
                GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MIN_FILTER, (int)GL.GL_LINEAR_MIPMAP_NEAREST);
                GLU.gluBuild2DMipmaps(GL.GL_TEXTURE_2D, (int)GL.GL_RGB, image.Width, image.Height,
                    GL.GL_BGR_EXT, GL.GL_UNSIGNED_BYTE, (void*)bitmapdata.Scan0);

                // 创建位映射纹理Create MipMapped Texture
                GL.glBindTexture(GL.GL_TEXTURE_2D, this.texture[3]);			// 定义纹理
                GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MAG_FILTER, (int)GL.GL_LINEAR_MIPMAP_NEAREST);
                GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MIN_FILTER, (int)GL.GL_NEAREST_MIPMAP_NEAREST);
                GLU.gluBuild2DMipmaps(GL.GL_TEXTURE_2D, (int)GL.GL_RGB, image.Width, image.Height,
                    GL.GL_BGR_EXT, GL.GL_UNSIGNED_BYTE, (void*)bitmapdata.Scan0);

                image.UnlockBits(bitmapdata);									// 从系统内存中解锁image对象
                image.Dispose();												// 释放image对象
                return true;
            }
            return false;
        }
		//----------------装入纹理------2005.1.11-----------------
		//===============================================



		// ############################################################################
		// ############################################################################
		// ############################################################################
		// ############################################################################

		// ============================================================================
		// The following function creates and OpenGL Rendering Context (RC), and sets
		// up other resources specific to this demonstration code, such as fonts.
		// ============================================================================


        public void OneTimeInitialize(IntPtr IntPtr_HWND)
        {
            m_IntPtr_HWND = IntPtr_HWND;

            DemoInitOpenGL(m_IntPtr_HWND, ref m_IntPtr_HDC, ref m_IntPtr_HGLRC);

            //	DemoInitializeOpenGLFont( m_IntPtr_HDC );
        }
		public void OneTimeInitializeLayer( IntPtr IntPtr_HWND )
		{
			m_IntPtr_HWND = IntPtr_HWND;

			DemoInitOpenGLLayer( m_IntPtr_HWND, ref m_IntPtr_HDC, ref m_IntPtr_HGLRC );

		//	DemoInitializeOpenGLFont( m_IntPtr_HDC );
		}

        public void OneTimeInitializeDrill(IntPtr IntPtr_HWND)
        {
            m_IntPtr_HWND = IntPtr_HWND;

            DemoInitOpenGLDrill(m_IntPtr_HWND, ref m_IntPtr_HDC, ref m_IntPtr_HGLRC);

            //	DemoInitializeOpenGLFont( m_IntPtr_HDC );
        }
     
		// ############################################################################
		// ############################################################################
		// ############################################################################
		// ############################################################################


		// ============================================================================
		// The following is an example of OpenGL rendering code, complete with
		// buffer swapping.  This function can be called by a Form's "OnPaint()"
		// method if a previous WGL.DemoInitOpenGL() call (for example) has
		// already successfully established a valid Render Context (RC).
		// ============================================================================

		public void DemoMainDrawFunction( int int_WindowWidth, int int_WindowHeight )
		{
			m_uint_FrameCounter++;
			m_int_Milliseconds = (int)(System.Environment.TickCount);

			// --------------------------------------------------------------------------
			// 建立基本的渲染环境 Establish basic rendering conditions
			// --------------------------------------------------------------------------
			if (int_WindowWidth  <= 0)  int_WindowWidth  = 1;
			if (int_WindowHeight <= 0)  int_WindowHeight = 1;

			GL.glViewport( 0, 0, int_WindowWidth, int_WindowHeight );

			/*GL.glEnable     ( GL.GL_DEPTH_TEST );
			GL.glDepthFunc  ( GL.GL_LEQUAL     );
//			GL.glEnable     ( GL.GL_CULL_FACE  );
//			GL.glCullFace   ( GL.GL_BACK       );
			GL.glFrontFace  ( GL.GL_CCW );

			//-------------2005.1.12-----------------

			GL.glEnable(GL.GL_TEXTURE_2D);									// 启动纹理映射Enable Texture Mapping
			GL.glShadeModel(GL.GL_SMOOTH);									// 允许光滑阴影Enable Smooth Shading
			GL.glClearColor(0.0f, 0.0f, 0.0f, 0.5f);						// 黑色背景Black Background
			GL.glClearDepth(1.0f);											// 深度缓冲区设置Depth Buffer Setup
			GL.glHint(GL.GL_PERSPECTIVE_CORRECTION_HINT, GL.GL_NICEST);		// Really Nice Perspective Calculations

			GL.glLightfv(GL.GL_LIGHT1, GL.GL_AMBIENT,  this.LightAmbient);	// 设置环境光Setup The Ambient Light
			GL.glLightfv(GL.GL_LIGHT1, GL.GL_DIFFUSE,  this.LightDiffuse);	// 设置散射光Setup The Diffuse Light
			GL.glLightfv(GL.GL_LIGHT1, GL.GL_POSITION, this.LightPosition);	// 设置光源位置Position The Light
			GL.glEnable(GL.GL_LIGHT1);										// 启动一号光源Enable Light One
*/
/*			if (this.light)													// If lighting, enable it to start
				GL.glEnable(GL.GL_LIGHTING);
			else
				GL.glDisable(GL.GL_LIGHTING);
*/
			if (this.blend)													// If blending, turn it on and depth testing off
			{
//				GL.glEnable(GL.GL_BLEND);
//				GL.glDisable(GL.GL_DEPTH_TEST);
				GL.glColor4f(1.0f, 1.0f, 1.0f, 0.5f);							// Full Brightness.  50% Alpha
				GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE);						// Set The Blending Function For Translucency
			}


			//-------------2005.1.12-----------------
			// PROJECTION matrix, which we'll use to do perspective correction.
			// 投影矩阵(PROJECTION)模式，将用于透视校正
			GL.glMatrixMode ( GL.GL_PROJECTION );
			GL.glLoadIdentity();

//			GL.glFrustum(-200, 200, -200, 200, 0.10, 4000);

			GLU.gluPerspective
				( 
				60.0,     // 视角字段Field of view angle (Y angle; degrees)
				((double)(Screen.PrimaryScreen.Bounds.Width/Screen.PrimaryScreen.Bounds.Height)), 
				0.1,      // 近平面Near plane
				4000.0    // 远平面Far  plane
				);

			// MODELVIEW matrix, which we will use to transform individual models
			// 模型矩阵(MODELVIEW),将用于变换独立的模型
			GL.glMatrixMode ( GL.GL_MODELVIEW );
			GL.glLoadIdentity();
			// --------------------------------------------------------------------------

			// Custom drawing code
			DemoSecondaryDrawFunction( int_WindowWidth, int_WindowHeight );


			// --------------------------------------------------------------------------
			// Flush all the current rendering and flip the back buffer to the front.
			// --------------------------------------------------------------------------
			GL.glFlush();
			WGL.wglSwapBuffers( m_IntPtr_HDC );
			// --------------------------------------------------------------------------
		}

		// ############################################################################
		// ############################################################################
		// ############################################################################
		// ############################################################################

		public void OneTimeShutdown ( )
		{
		}

		// ############################################################################
		// ############################################################################
		// ############################################################################
		// ############################################################################

		// ============================================================================
		// The following method can be called by a System.Windows.Forms form OnPaint()
		// method:
		//
		//      protected override void OnPaint( System.Windows.Forms.PaintEventArgs e )
		//      {
		//        m_WGLAV5_DEMO.OnPaint( this.Handle, this.Size.Width, this.Size.Height );
		//        System.Threading.Thread.Sleep( 10 ); // 10 msec --> 100 frames per second, max.
		//        Invalidate(false); // Force OnPaint() to get called again.
		//      }
		//
		// ============================================================================

		public void OnPaint ( IntPtr IntPtr_HWND, int i32_Width, int i32_Height )
		{
			if (0 == m_uint_AttemptedInitialization)
			{
				m_uint_AttemptedInitialization = 1;
				OneTimeInitialize ( IntPtr_HWND );
			}
			if (IntPtr.Zero != m_IntPtr_HGLRC)
			{
				DemoMainDrawFunction( i32_Width, i32_Height );
			}
		}
        public void OnPaintLayer(IntPtr IntPtr_HWND, int i32_Width, int i32_Height)
        {
            if (0 == m_uint_AttemptedInitialization)
            {
                m_uint_AttemptedInitialization = 1;
                OneTimeInitializeLayer(IntPtr_HWND);
            }
            if (IntPtr.Zero != m_IntPtr_HGLRC)
            {
                DemoMainDrawFunction(i32_Width, i32_Height);
            }
        }

        public void OnPaintDrill(IntPtr IntPtr_HWND, int i32_Width, int i32_Height)
        {
            if (0 == m_uint_AttemptedInitialization)
            {
                m_uint_AttemptedInitialization = 1;
                OneTimeInitializeDrill(IntPtr_HWND);
            }
            if (IntPtr.Zero != m_IntPtr_HGLRC)
            {
                DemoMainDrawFunction(i32_Width, i32_Height);
            }
        }

		// ############################################################################
		// ############################################################################
		// ############################################################################
		// ############################################################################

		// ============================================================================
		// The following method can be called by a System.Windows.Forms form OnPaint()
		// method:
		//
		// protected override void OnKeyDown( System.Windows.Forms.KeyEventArgs e )
		// {
		//   m_WGLAV5_DEMO.OnKeyDown( e.KeyValue );
		// }
		//
		// ============================================================================
		public void OnKeyDown ( int int_Key )
		{
			m_int_Input = int_Key;
		}


		public void OnKeyDown ( System.Windows.Forms.KeyEventArgs e )
		{
			m_int_Input = e.KeyValue;

            if (e.KeyData == System.Windows.Forms.Keys.Escape)				// 如果按Escape键，应用程序结束
            {
                System.Windows.Forms.Application.Exit();
            }
            else if (e.KeyData == System.Windows.Forms.Keys.L && !this.lp)	// 如果按L键，转换光照模式
            {
                this.lp = true;
                this.light = !this.light;
                if (this.light)
                    GL.glEnable(GL.GL_LIGHTING);
                else
                    GL.glDisable(GL.GL_LIGHTING);
            }
            else if ((e.KeyData == System.Windows.Forms.Keys.F) && !this.fp)	// 按F键，循环纹理滤波
            {
                this.fp = true;
                //				this.filter = (filter + 1) % 3;
                this.filter = (filter + 1) % 4;
            }
            else if (e.KeyData == System.Windows.Forms.Keys.B && !this.bp)	// Blending code starts here
            {
                this.bp = true;
                this.blend = !this.blend;
                if (this.blend)
                {
                    GL.glEnable(GL.GL_BLEND);			// Turn Blending On
                    GL.glDisable(GL.GL_DEPTH_TEST);		// Turn Depth Testing Off
                }
                else
                {
                    GL.glDisable(GL.GL_BLEND);			// Turn Blending Off
                    GL.glEnable(GL.GL_DEPTH_TEST);		// Turn Depth Testing On
                }
            }											// Blending Code Ends Here

            else if (e.KeyData == System.Windows.Forms.Keys.PageUp)			// 按PageUp键，缩小
                this.z += 20f;
            else if (e.KeyData == System.Windows.Forms.Keys.PageDown)		// 按PageDown键，放大
                this.z -= 20f;
            else if (e.KeyData == System.Windows.Forms.Keys.Space)
                m_bStop = !m_bStop;
            else if (e.KeyData == System.Windows.Forms.Keys.Left)
                m_nyAngle--;
            else if (e.KeyData == System.Windows.Forms.Keys.Right)
            {
                m_nyAngle++;
            }
            else if (e.KeyData == System.Windows.Forms.Keys.Down)
            {
                m_nzAngle -= 5;
                m_nxAngle -= 5;
            }
            else if (e.KeyData == System.Windows.Forms.Keys.Up)
            {
                m_nzAngle += 5;
                m_nxAngle += 5;
            }
            else if (e.KeyData == System.Windows.Forms.Keys.F2)
                m_bShowAxis = !m_bShowAxis;

			ProcessDialogKey(e.KeyData);

		}


        public void OnKeyUp(System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.L)					// 释放L键，释放光照转换键锁定
                this.lp = false;
            else if (e.KeyCode == System.Windows.Forms.Keys.F)				// 释放F键，释放滤波循环键锁定
                this.fp = false;
            else if (e.KeyCode == System.Windows.Forms.Keys.B)				// 释放B键，释放融合转换键锁定
                this.bp = false;

        }
		protected  bool ProcessDialogKey(System.Windows.Forms.Keys keyData)
		{
			if (keyData == System.Windows.Forms.Keys.Up)					// 按Up和Down键改变X轴方向的旋转速度
				this.xspeed -= 0.01f;
			else if (keyData == System.Windows.Forms.Keys.Down)
				this.xspeed += 0.01f;
			else if (keyData == System.Windows.Forms.Keys.Right)			// 按Right和Left键改变Y轴方向的旋转速度
				this.yspeed += 0.01f;
			else if (keyData == System.Windows.Forms.Keys.Left)
				this.yspeed -= 0.01f;
		
			return true;
		}



		// ############################################################################
		// ############################################################################
		// ############################################################################
		// ############################################################################

        //             调用显示列表
        //**********************************
        //             显示列表编号
        //             200绘制边界
        //             201显示钻孔点
        //             202显示某层的grid高程点
        //             203绘制K1块体
        //             204绘制K2块体
        //             205绘制K3块体
        //             206绘制K4块体
        //             207绘制K5块体
        //             208绘制K6块体
        //             209绘制K7块体
        //             210绘制K8块体
        //             211绘制M3煤
        //**********************************
       public void DrawMyModel()
        {
            GL.glClear(GL.GL_COLOR_BUFFER_BIT | GL.GL_DEPTH_BUFFER_BIT);        
            GL.glLoadIdentity();
            GLU.gluLookAt(0, 1700, 1500, 0, 1000, 600, 0, 1, 0);
            GL.glTranslated(-0.5*Screen.PrimaryScreen.Bounds.Width,-0.5*Screen.PrimaryScreen.Bounds.Height,z);
            if (m_bStop)
                m_nyAngle += 1;
            if (m_nyAngle > 359)
                m_nyAngle = 0;
            if (m_nyAngle < 0)
                m_nyAngle = 359;
            GL.glRotatef((float)m_nyAngle, 0.0f, 1.0f, 0.0f);
            GL.glRotatef((float)m_nxAngle, 1.0f, 0.0f, 0.0f);
            GL.glRotatef((float)m_nzAngle, 0.0f, 0.0f, 1.0f);
            GL.glScaled(1,1,2);
            GL.glColor3f(0, 0, 0);
            
           // hm.drawHoles(201);            
             if (this.light)
               {
                   GL.glDisable(GL.GL_LIGHTING);
                   GL.glDisable(GL.GL_LIGHT0);
               }
               pb.Draw(200);
               GL.glEnable(GL.GL_LIGHT0);
               GL.glEnable(GL.GL_LIGHTING);
               for (uint i = 203; i <= 211; i++)
                {
                    gl.draw(i);
                }
                gl.draw(2000);
           //画坐标轴
           if (m_bShowAxis)
               {
                   GL.glScalef(600.0f, 600.0f, 600.0f);
                   GL.glCallList(22);
                   GL.glEnable(GL.GL_LIGHTING);
                   GL.glEnable(GL.GL_LIGHT0);
               }
            
               GL.glFlush();

        }

			// 创建坐标轴显示列表 
		public void CreateDisplayLists()
		{
			 //the coordinate axes display list
              GL.glNewList(22,GL.GL_COMPILE);
                // No light for axes
              if (this.light)
              {
                  GL.glDisable(GL.GL_LIGHTING);
                  GL.glDisable(GL.GL_LIGHT0);
              }

                GL.glColor3f(1.0f, 0.0f, 0.0f); // red
                GL.glBegin(GL.GL_LINES); // X axis
                GL.glVertex3f(1.0f,0.0f,0.0f);
                GL.glVertex3f(-1.0f,0.0f,0.0f);
                GL.glEnd();

                // 显示X坐标轴的指针
                GL.glPushMatrix();
                GL.glTranslatef(1.0f, 0.0f, 0.0f);
                GL.glRotatef(90.0f, 0.0f, 1.0f, 0.0f);
                GLUT.glutSolidCone(0.02, 0.08, 10, 10);
                GL.glPopMatrix();

                GL.glColor3f(0.0f,1.0f,0.0f); // green
                GL.glBegin(GL.GL_LINES); // Y axis
                GL.glVertex3f(0.0f,1.0f,0.0f);
                GL.glVertex3f(0.0f,-1.0f,0.0f);
        //			glVertex3f(0.0f,0.0f,0.0f);
                GL.glEnd();

                // 显示Y坐标轴的指针
                GL.glPushMatrix();
                GL.glTranslatef(0.0f, 1.0f, 0.0f);
                GL.glRotatef(-90.0f, 1.0f, 0.0f, 0.0f);
                GLUT.glutSolidCone(0.02, 0.08, 10, 10);
                GL.glPopMatrix();

                GL.glColor3f(0.0f, 0.0f, 1.0f); // blue
                GL.glBegin(GL.GL_LINES); // Z axis
                GL.glVertex3f(0.0f, 0.0f, 1.0f);
                GL.glVertex3f(0.0f, 0.0f, -1.0f);
        //			glVertex3f(0.0f,0.0f,0.0f);
                GL.glEnd();

                // 显示Z坐标轴的指针
                GL.glPushMatrix();
        //		glTranslatef(0.0f, 0.0f, -1.0f);
                GL.glTranslatef(0.0f, 0.0f, 1.0f);
        //		glRotatef(180.0f, 1.0f, 0.0f, 0.0f);
                GLUT.glutSolidCone(0.02, 0.08, 10, 10);
                GL.glPopMatrix();

            GL.glEndList(); // AXES
		}

		//=============================================================================
		//
		// 建立显示列表
		//
		//
		//=============================================================================

    /*    public void BuildDisplayLists()
        {

            this.box = GL.glGenLists(1);						// Generate 2 Different Lists
            GL.glNewList(box, GL.GL_COMPILE);				// Start With The Box List
            GL.glBegin(GL.GL_QUADS);					//绘制四面体
            // 前面
            GL.glNormal3f(0.0f, 0.0f, 1.0f);

            GL.glTexCoord2f(1.0f, 1.0f);			// 右上角纹理坐标
            GL.glVertex3f(1.0f, 1.0f, 1.0f);		// 四面体前面的右上角坐标
            GL.glTexCoord2f(0.0f, 1.0f);			// 左上角纹理坐标
            GL.glVertex3f(-1.0f, 1.0f, 1.0f);		// 四面体前面的左上角坐标
            GL.glTexCoord2f(0.0f, 0.0f);			// 左上角纹理坐标
            GL.glVertex3f(-1.0f, -1.0f, 1.0f);	// 四面体前面的左下角坐标
            GL.glTexCoord2f(1.0f, 0.0f);			// 右下角纹理坐标
            GL.glVertex3f(1.0f, -1.0f, 1.0f);		// 四面体前面的右下角坐标

            // 后面
            GL.glNormal3f(0.0f, 0.0f, -1.0f);

            GL.glTexCoord2f(1.0f, 1.0f);			// 右上角纹理坐标
            GL.glVertex3f(-1.0f, 1.0f, -1.0f);	// 四面体后面的右上角坐标top right of quad
            GL.glTexCoord2f(0.0f, 1.0f);			// 左上角纹理坐标top left of texture
            GL.glVertex3f(1.0f, 1.0f, -1.0f);		// 四面体后面的左上角坐标top left of quad
            GL.glTexCoord2f(0.0f, 0.0f);			// 左上角纹理坐标
            GL.glVertex3f(1.0f, -1.0f, -1.0f);	// 四面体后面的左下角坐标
            GL.glTexCoord2f(1.0f, 0.0f);			// 右下角纹理坐标
            GL.glVertex3f(-1.0f, -1.0f, -1.0f);	// 四面体后面的右下角坐标

            // 顶面
            GL.glNormal3f(0.0f, 1.0f, 0.0f);

            GL.glTexCoord2f(1.0f, 1.0f);			// 右上角纹理坐标
            GL.glVertex3f(1.0f, 1.0f, -1.0f);		// 四面体上面的右上角坐标
            GL.glTexCoord2f(0.0f, 1.0f);			// 左上角纹理坐标
            GL.glVertex3f(-1.0f, 1.0f, -1.0f);	// 四面体上面的左上角坐标
            GL.glTexCoord2f(0.0f, 0.0f);			// 左上角纹理坐标
            GL.glVertex3f(-1.0f, 1.0f, 1.0f);		// 四面体上面的左下角坐标
            GL.glTexCoord2f(1.0f, 0.0f);			// 右下角纹理坐标
            GL.glVertex3f(1.0f, 1.0f, 1.0f);		// 四面体上面的右下角坐标


            // 底面
            GL.glNormal3f(0.0f, -1.0f, 0.0f);

            GL.glTexCoord2f(1.0f, 1.0f);			// 右上角纹理坐标
            GL.glVertex3f(1.0f, -1.0f, 1.0f);		// 四面体底面的右上角坐标
            GL.glTexCoord2f(0.0f, 1.0f);			// 左上角纹理坐标
            GL.glVertex3f(-1.0f, -1.0f, 1.0f);	// 四面体底面的左上角坐标
            GL.glTexCoord2f(0.0f, 0.0f);			// 左上角纹理坐标
            GL.glVertex3f(-1.0f, -1.0f, -1.0f);	// 四面体底面的左下角坐标
            GL.glTexCoord2f(1.0f, 0.0f);			// 右下角纹理坐标
            GL.glVertex3f(1.0f, -1.0f, -1.0f);	// 四面体底面的右下角坐标

            // 右面
            GL.glNormal3f(1.0f, 0.0f, 0.0f);

            GL.glTexCoord2f(1.0f, 1.0f);			// 右上角纹理坐标
            GL.glVertex3f(1.0f, 1.0f, -1.0f);		// 四面体右面的右上角坐标
            GL.glTexCoord2f(0.0f, 1.0f);			// 左上角纹理坐标
            GL.glVertex3f(1.0f, 1.0f, 1.0f);		// 四面体右面的左上角坐标
            GL.glTexCoord2f(0.0f, 0.0f);			// 左上角纹理坐标
            GL.glVertex3f(1.0f, -1.0f, 1.0f);		// 四面体右面的左下角坐标
            GL.glTexCoord2f(1.0f, 0.0f);			// 右下角纹理坐标
            GL.glVertex3f(1.0f, -1.0f, -1.0f);	// 四面体右面的右下角坐标

            // 左面
            GL.glNormal3f(-1.0f, 0.0f, 0.0f);

            GL.glTexCoord2f(1.0f, 1.0f);			// 右上角纹理坐标
            GL.glVertex3f(-1.0f, 1.0f, 1.0f);		// 四面体左面的右上角坐标
            GL.glTexCoord2f(0.0f, 1.0f);			// 左上角纹理坐标
            GL.glVertex3f(-1.0f, 1.0f, -1.0f);	// 四面体左面的左上角坐标
            GL.glTexCoord2f(0.0f, 0.0f);			// 左上角纹理坐标
            GL.glVertex3f(-1.0f, -1.0f, -1.0f);	// 四面体左面的左下角坐标
            GL.glTexCoord2f(1.0f, 0.0f);			// 右下角纹理坐标
            GL.glVertex3f(-1.0f, -1.0f, 1.0f);	// 四面体左面的右下角坐标
            GL.glEnd();
            GL.glEndList();
        }*/
/*			this.box = GL.glGenLists(2);						// Generate 2 Different Lists
			GL.glNewList(this.box, GL.GL_COMPILE);				// Start With The Box List
			GL.glBegin(GL.GL_QUADS);
			// Bottom Face
			GL.glNormal3f( 0.0f,-1.0f, 0.0f);
			GL.glTexCoord2f(1.0f, 1.0f); GL.glVertex3f(-1.0f, -1.0f, -1.0f);
			GL.glTexCoord2f(0.0f, 1.0f); GL.glVertex3f( 1.0f, -1.0f, -1.0f);
			GL.glTexCoord2f(0.0f, 0.0f); GL.glVertex3f( 1.0f, -1.0f,  1.0f);
			GL.glTexCoord2f(1.0f, 0.0f); GL.glVertex3f(-1.0f, -1.0f,  1.0f);
			// Front Face
			GL.glNormal3f( 0.0f, 0.0f, 1.0f);
			GL.glTexCoord2f(0.0f, 0.0f); GL.glVertex3f(-1.0f, -1.0f,  1.0f);
			GL.glTexCoord2f(1.0f, 0.0f); GL.glVertex3f( 1.0f, -1.0f,  1.0f);
			GL.glTexCoord2f(1.0f, 1.0f); GL.glVertex3f( 1.0f,  1.0f,  1.0f);
			GL.glTexCoord2f(0.0f, 1.0f); GL.glVertex3f(-1.0f,  1.0f,  1.0f);
			// Back Face
			GL.glNormal3f( 0.0f, 0.0f,-1.0f);
			GL.glTexCoord2f(1.0f, 0.0f); GL.glVertex3f(-1.0f, -1.0f, -1.0f);
			GL.glTexCoord2f(1.0f, 1.0f); GL.glVertex3f(-1.0f,  1.0f, -1.0f);
			GL.glTexCoord2f(0.0f, 1.0f); GL.glVertex3f( 1.0f,  1.0f, -1.0f);
			GL.glTexCoord2f(0.0f, 0.0f); GL.glVertex3f( 1.0f, -1.0f, -1.0f);
			// Right face
			GL.glNormal3f( 1.0f, 0.0f, 0.0f);
			GL.glTexCoord2f(1.0f, 0.0f); GL.glVertex3f( 1.0f, -1.0f, -1.0f);
			GL.glTexCoord2f(1.0f, 1.0f); GL.glVertex3f( 1.0f,  1.0f, -1.0f);
			GL.glTexCoord2f(0.0f, 1.0f); GL.glVertex3f( 1.0f,  1.0f,  1.0f);
			GL.glTexCoord2f(0.0f, 0.0f); GL.glVertex3f( 1.0f, -1.0f,  1.0f);
			// Left Face
			GL.glNormal3f(-1.0f, 0.0f, 0.0f);
			GL.glTexCoord2f(0.0f, 0.0f); GL.glVertex3f(-1.0f, -1.0f, -1.0f);
			GL.glTexCoord2f(1.0f, 0.0f); GL.glVertex3f(-1.0f, -1.0f,  1.0f);
			GL.glTexCoord2f(1.0f, 1.0f); GL.glVertex3f(-1.0f,  1.0f,  1.0f);
			GL.glTexCoord2f(0.0f, 1.0f); GL.glVertex3f(-1.0f,  1.0f, -1.0f);
			GL.glEnd();
			GL.glEndList();
			this.top = this.box + 1;							// Storage For "Top" Is "Box" Plus One
			GL.glNewList(this.top, GL.GL_COMPILE);				// Now The "Top" Display List
			GL.glBegin(GL.GL_QUADS);
			// Top Face
			GL.glNormal3f( 0.0f, 1.0f, 0.0f);
			GL.glTexCoord2f(0.0f, 1.0f); GL.glVertex3f(-1.0f,  1.0f, -1.0f);
			GL.glTexCoord2f(0.0f, 0.0f); GL.glVertex3f(-1.0f,  1.0f,  1.0f);
			GL.glTexCoord2f(1.0f, 0.0f); GL.glVertex3f( 1.0f,  1.0f,  1.0f);
			GL.glTexCoord2f(1.0f, 1.0f); GL.glVertex3f( 1.0f,  1.0f, -1.0f);
			GL.glEnd();
			GL.glEndList();
		}
*/
		// ############################################################################
		// ############################################################################
		// ############################################################################
		// ############################################################################

		public void PrintViaSpecifiedFontOpenGLDisplayList 
			( 
			uint    uint_FontOpenGLDisplayListBaseIndex,
			int     int_WindowWidth,
			int     int_WindowHeight,
			int     int_TextX, 
			int     int_TextY, 
			String  String_Text
			)
		{
			int int_StringLength         = 0;
			int int_StringCharacterIndex = 0;
			int int_ASCII_Character      = 0;

			// --------------------------------------------------------------------------
			// Change rendering conditions
			// --------------------------------------------------------------------------
			GL.glDisable    ( GL.GL_DEPTH_TEST );
			GL.glDisable    ( GL.GL_CULL_FACE  );
			// --------------------------------------------------------------------------


			// --------------------------------------------------------------------------
			// Preserve current matrices, and switch to an orthographic view, and 
			//   do scaling and translation as necessary.
			// --------------------------------------------------------------------------
			GL.glMatrixMode ( GL.GL_PROJECTION );
			GL.glPushMatrix ();
			GL.glMatrixMode ( GL.GL_MODELVIEW  );
			GL.glPushMatrix ();							

			GL.glMatrixMode(GL.GL_MODELVIEW);
			GL.glLoadIdentity(); 

			// Only affects by BITMAP fonts:
			GL.glRasterPos2i( int_TextX, int_TextY );
			// Only affects POLYGON fonts:
			GL.glTranslatef ( (float)(int_TextX), (float)(int_TextY), 0.0f ); 
			GL.glScalef     ( 64.0f, 64.0f, 64.0f ); 
			// --------------------------------------------------------------------------


			// --------------------------------------------------------------------------
			// Call a display list for each character to be rendered.  The ASCII code
			// is used as the display list number (of the 256 display lists for this
			// font), which is added to the base number of the set of display list
			// indices.
			// --------------------------------------------------------------------------
			int_StringLength = String_Text.Length;
			for (
				int_StringCharacterIndex = 0; 
				int_StringCharacterIndex < int_StringLength; 
				int_StringCharacterIndex++ 
				)
			{
				int_ASCII_Character = (int)(String_Text[ int_StringCharacterIndex ]);
				GL.glCallList( (uint)(uint_FontOpenGLDisplayListBaseIndex + int_ASCII_Character) );
			}
			// --------------------------------------------------------------------------


			// --------------------------------------------------------------------------
			// Restore original matrices.
			// --------------------------------------------------------------------------
			GL.glMatrixMode ( GL.GL_MODELVIEW  );
			GL.glPopMatrix  ();
			GL.glMatrixMode ( GL.GL_PROJECTION );
			GL.glPopMatrix  ();
			// --------------------------------------------------------------------------

			// --------------------------------------------------------------------------
			// Restore rendering conditions
			// --------------------------------------------------------------------------
			GL.glFrontFace  ( GL.GL_CCW        ); // MUST DO AFTER USING wglUseFontOutlines LISTS!!!
			GL.glEnable     ( GL.GL_DEPTH_TEST );
			GL.glEnable     ( GL.GL_CULL_FACE  );
			// --------------------------------------------------------------------------
		}

		// ############################################################################
		// ############################################################################
		// ############################################################################
		// ############################################################################

		private void DemoCreateBitmapFont
		( 
		IntPtr   ref_IntPtr_HDC,  // [in]
		String   String_FontName,  // [in] "Verdana", "Arial", "Courier New", "Symbol", "Wingdings", "Wingdings 3"
		uint     uint_BitmapFontOpenGLDisplayListBase  // [in]
		)
		{
		int int_Result = 0;

		// --------------------------------------------------------------------------
		// IT IT AN ABSOLUTE NECESSITY TO SELECT A FONT IN TO THE DC FOR THE 
		// wglUseFontOutlines() METHOD TO SUCCEED WITHOUT A MYSTERIOUS 
		// ERROR 126: ERROR_MOD_NOT_FOUND : The specified module could not be found.
		// THAT ERROR CODE, OF COURSE, IS MISLEADING.
		// --------------------------------------------------------------------------
		System.Drawing.Font font =
			new System.Drawing.Font
			(
			String_FontName, // "Verdana", "Arial", "Courier New", "Symbol", "Wingdings", "Wingdings 3"
			9F,
			System.Drawing.FontStyle.Regular,
			System.Drawing.GraphicsUnit.Point,
			((System.Byte)(0))
			);
		// --------------------------------------------------------------------------


		// --------------------------------------------------------------------------
		IntPtr fontH = font.ToHfont();
		//_OldFontH = 
		GDI.SelectObject( ref_IntPtr_HDC, fontH );
		// --------------------------------------------------------------------------


		// --------------------------------------------------------------------------
		int_Result =
			WGL.wglUseFontBitmaps
			( 
			ref_IntPtr_HDC, 
			0, 
			255, 
			uint_BitmapFontOpenGLDisplayListBase
			);

		if (0 == int_Result)
			{
			uint uint_Error = USER.GetLastError();
			String s = "WGL.wglUseFontBitmaps Error: " + uint_Error;
			USER.MessageBox( IntPtr.Zero, s, "ERROR", USER.MB_OK );
			}
		// --------------------------------------------------------------------------
		}


		// ############################################################################
		// ############################################################################
		// ############################################################################
		// ############################################################################


		private void DemoCreatePolygonFont
		( 
		IntPtr   ref_IntPtr_HDC,   // [in]
		String   String_FontName,  // [in] "Verdana", "Arial", "Courier New", "Symbol", "Wingdings", "Wingdings 3"
		uint     uint_PolygonFontOpenGLDisplayListBase  // [in]
		)
		{
		int int_Result = 0;

		// --------------------------------------------------------------------------
		// IT IT AN ABSOLUTE NECESSITY TO SELECT A FONT IN TO THE DC FOR THE 
		// wglUseFontOutlines() METHOD TO SUCCEED WITHOUT A MYSTERIOUS 
		// ERROR 126: ERROR_MOD_NOT_FOUND : The specified module could not be found.
		// THAT ERROR CODE, OF COURSE, IS MISLEADING.
		// --------------------------------------------------------------------------
		System.Drawing.Font font =
			new System.Drawing.Font
			(
			String_FontName, // "Verdana", "Arial", "Courier New", "Symbol", "Wingdings", "Wingdings 3"
			9F,
			System.Drawing.FontStyle.Regular,
			System.Drawing.GraphicsUnit.Point,
			((System.Byte)(0))
			);
		// --------------------------------------------------------------------------


		// --------------------------------------------------------------------------
		IntPtr fontH = font.ToHfont();
		//_OldFontH = 
		GDI.SelectObject( ref_IntPtr_HDC, fontH );
		// --------------------------------------------------------------------------

		// --------------------------------------------------------------------------
		GDI.GLYPHMETRICSFLOAT[] agmf = new GDI.GLYPHMETRICSFLOAT [ 256 ];   

		int_Result =
			WGL.wglUseFontOutlines
			( 
			ref_IntPtr_HDC,        // DC with font   
			0,                     // Starting Character
			255,                   // Number Of Display Lists To Build
			uint_PolygonFontOpenGLDisplayListBase,                  // Starting Display List index
			0.0f,                  // Deviation From The True Outlines
			0.15f,                  // Font Thickness In The Z Direction (Extrusion)
			WGL.WGL_FONT_POLYGONS, // Format: WGL.WGL_FONT_LINES, WGL.WGL_FONT_POLYGONS
			agmf                   // Metrics pointer
			); 

		if (0 == int_Result)
			{
			uint uint_Error = USER.GetLastError();
			String s = "wglUseFontOutlines Error: " + uint_Error;
			USER.MessageBox( IntPtr.Zero, s, "WGL.GetLastError()", USER.MB_OK );
			}
		// --------------------------------------------------------------------------   
		}

		// ############################################################################
		// ############################################################################
		// ############################################################################
		// ############################################################################

		public unsafe void DemoInitializeOpenGLFont
		(
		IntPtr ref_IntPtr_HDC
		)
		{
		m_uint_FontDisplayListBase_BITMAP_Verdana     = GL.glGenLists( 256 );
		m_uint_FontDisplayListBase_BITMAP_Symbol      = GL.glGenLists( 256 );
		m_uint_FontDisplayListBase_BITMAP_Wingdings   = GL.glGenLists( 256 );

		m_uint_FontDisplayListBase_POLYGONS_Verdana   = GL.glGenLists( 256 );
		m_uint_FontDisplayListBase_POLYGONS_Symbol    = GL.glGenLists( 256 );
		m_uint_FontDisplayListBase_POLYGONS_Wingdings = GL.glGenLists( 256 );

		// Font name examples:
		//   "Verdana", "Arial", "Courier New", "Symbol", "Wingdings", "Wingdings 3"

		DemoCreateBitmapFont ( ref_IntPtr_HDC, "Verdana", m_uint_FontDisplayListBase_BITMAP_Verdana   );
		DemoCreatePolygonFont( ref_IntPtr_HDC, "Verdana", m_uint_FontDisplayListBase_POLYGONS_Verdana );

		DemoCreateBitmapFont ( ref_IntPtr_HDC, "Symbol", m_uint_FontDisplayListBase_BITMAP_Symbol   );
		DemoCreatePolygonFont( ref_IntPtr_HDC, "Symbol", m_uint_FontDisplayListBase_POLYGONS_Symbol );

		DemoCreateBitmapFont ( ref_IntPtr_HDC, "Wingdings", m_uint_FontDisplayListBase_BITMAP_Wingdings   );
		DemoCreatePolygonFont( ref_IntPtr_HDC, "Wingdings", m_uint_FontDisplayListBase_POLYGONS_Wingdings );
		}

		// ############################################################################
		// ############################################################################
		// ############################################################################
		// ############################################################################

		public void DemoSecondaryDrawFunction( int i32_WindowWidth, int i32_WindowHeight )
		{
			
            //GL.glTranslatef( 0.0f, 0.0f, -600.0f );

			// --------------------------------------------------------------------------
			// Demonstrate object rendering
			// --------------------------------------------------------------------------
			//System.Random  System_Random = new System.Random(0);
            //int int_Phase = (int)(m_int_Milliseconds % 120000);
            //float float_Phase = (float)(0.3f * (int_Phase));

/*			int i;
//			for ( i = 0; i < 64; i++ )
			{
//				GL.glPushMatrix();

//				GL.glTranslatef
				( 
				(float)(400 * (i % 4)) - 600.0f, 
				(float)(400 * ((i / 4) % 4)) - 600.0f, 
				(float)(400 * ((i / 16) % 4)) - 1800.0f 
				);

			GL.glRotatef( (0.11f * float_Phase), 1.0f, 0.0f, 0.0f );
			GL.glRotatef( (0.31f * float_Phase), 0.0f, 1.0f, 0.0f );
			GL.glRotatef( (0.19f * float_Phase), 0.0f, 0.0f, 1.0f );
//*/

            DrawMyModel();
			GL.glPopMatrix();
//			}
		// --------------------------------------------------------------------------


			// --------------------------------------------------------------------------
		/*  // Demonstrate font rendering
		// --------------------------------------------------------------------------
			GL.glColor3f( 1.0f, 1.0f, 1.0f );

			PrintViaSpecifiedFontOpenGLDisplayList
			(
			m_uint_FontDisplayListBase_POLYGONS_Verdana,
			i32_WindowWidth, 
			i32_WindowHeight, 
			10, 
			i32_WindowHeight - 20 - 64, 
			"WGLAV5:"
			);

		PrintViaSpecifiedFontOpenGLDisplayList
			(
			m_uint_FontDisplayListBase_POLYGONS_Verdana,
			i32_WindowWidth, 
			i32_WindowHeight, 
			10, 
			i32_WindowHeight - 20 - 2*64, 
			"C# OpenGL Wrapper"
			);

		PrintViaSpecifiedFontOpenGLDisplayList
			(
			m_uint_FontDisplayListBase_POLYGONS_Verdana,
			i32_WindowWidth, 
			i32_WindowHeight, 
			10, 
			i32_WindowHeight - 20 - 3*64, 
			"Colin P. Fahey"
			);

		PrintViaSpecifiedFontOpenGLDisplayList
			(
			m_uint_FontDisplayListBase_POLYGONS_Symbol,
			i32_WindowWidth, 
			i32_WindowHeight, 
			10, 
			i32_WindowHeight - 20 - 4*64, 
			"ABCDEFGHIJKLMNOPQRSTUVWXYZ"
			);

		PrintViaSpecifiedFontOpenGLDisplayList
			(
			m_uint_FontDisplayListBase_POLYGONS_Wingdings,
			i32_WindowWidth, 
			i32_WindowHeight, 
			10, 
			i32_WindowHeight - 20 - 5*64, 
			"ABCDEFGHIJKLMNOPQRSTUVWXYZ"
			);




		// BITMAP fonts; small, fixed size.
		PrintViaSpecifiedFontOpenGLDisplayList
			(
			m_uint_FontDisplayListBase_BITMAP_Verdana,
			i32_WindowWidth, 
			i32_WindowHeight, 
			10, 
			i32_WindowHeight - 20 - 6*64, 
			"ABCDEFGHIJKLMNOPQRSTUVWXYZ"
			);

		PrintViaSpecifiedFontOpenGLDisplayList
			(
			m_uint_FontDisplayListBase_BITMAP_Symbol,
			i32_WindowWidth, 
			i32_WindowHeight, 
			10, 
			i32_WindowHeight - 20 - 6*64 - 1*16, 
			"ABCDEFGHIJKLMNOPQRSTUVWXYZ"
			);

		PrintViaSpecifiedFontOpenGLDisplayList
			(
			m_uint_FontDisplayListBase_BITMAP_Wingdings,
			i32_WindowWidth, 
			i32_WindowHeight, 
			10, 
			i32_WindowHeight - 20 - 6*64 - 2*16, 
			"ABCDEFGHIJKLMNOPQRSTUVWXYZ"
			);


		// Information presented with a bitmap font
		PrintViaSpecifiedFontOpenGLDisplayList
			(
			m_uint_FontDisplayListBase_BITMAP_Verdana,
			i32_WindowWidth, 
			i32_WindowHeight, 
			10, 
			i32_WindowHeight - 20 - 6*64 - 3*16, 
			"Most Recent Key Code: " + this.m_int_Input 
			+ "  Frame Number: " + this.m_uint_FrameCounter 
			+ " Time: " + this.m_int_Milliseconds
			);

		*/  
			// --------------------------------------------------------------------------
		}//public void DemoSecondaryDrawFunction

// ############################################################################
// ############################################################################
// ############################################################################
// ############################################################################

	} // public class DEMO
} // namespace OpenGL

// ############################################################################
// ############################################################################
// ############################################################################
// ############################################################################
