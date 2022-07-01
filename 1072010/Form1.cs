using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tao.Platform;
using Tao.OpenGl;
using Tao.FreeGlut;
using Tao.DevIl;
using Tao.Math;

namespace _1072010
{
    public partial class Form1 : Form
    {
        Camera cam = new Camera();
        Model myModel;
        float modelSize; //用來儲存模型的約略大小
        float[] modelCenter = new float[3]; //用來儲存模型中心座標
        double ud1=0.005, ud2= 0.008, ud3= 0.002, ud4= 0.01;
        double y1=0.1, y2=-0.1, y3=0.05, y4=0.09;
        double rot = 0;
        double lightrot = 0, centerlightrot = 230;
        Boolean light0 = true;
        Boolean light1 = true;
        Boolean light2 = true;

        uint[] texName = new uint[6]; //建立儲存紋理編號的陣列
        public Form1()
        {
            InitializeComponent();
            this.openGLControl1.InitializeContexts(); //初始化
            Glut.glutInit();
            Il.ilInit();
            Ilu.iluInit();
            Gl.ReloadFunctions();
        }
        private void MyInit()
        {
            cam.SetPosition(1.5, 0.8, 3.5);
            cam.SetDirection(0.0, 0.0, -3.0);
            Gl.glClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            Gl.glClearDepth(1.0);
            Gl.glColorMaterial(Gl.GL_FRONT, Gl.GL_AMBIENT_AND_DIFFUSE);
            float[] global_ambient = new float[] { 0.2f, 0.2f, 0.2f }; //全域環境光的數值
            float[] light0_ambient = new float[] { 1.0f, 1.0f, 1.0f }; //環境光
            float[] light0_diffuse = new float[] { 1.0f, 1.0f, 1.0f }; //散射光
            float[] light0_specular = new float[] { 1.0f, 1.0f, 1.0f }; //鏡射光

            Gl.glLightModelfv(Gl.GL_LIGHT_MODEL_AMBIENT, global_ambient); //設定全域環境光
            Gl.glLightModeli(Gl.GL_LIGHT_MODEL_LOCAL_VIEWER, Gl.GL_TRUE); //觀者位於場景內
            Gl.glLightModeli(Gl.GL_LIGHT_MODEL_TWO_SIDE, Gl.GL_FALSE); //只對物體正面進行光影計算

            //Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_AMBIENT, light0_ambient); //設定第一個光源的環境光成份
            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_DIFFUSE, light0_diffuse); //設定第一個光源的散射光成份
            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_SPECULAR, light0_specular);

            Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_DIFFUSE, light0_diffuse); 
            Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_SPECULAR, light0_specular);
            Gl.glLightfv(Gl.GL_LIGHT2, Gl.GL_DIFFUSE, light0_diffuse);
            Gl.glLightfv(Gl.GL_LIGHT2, Gl.GL_SPECULAR, light0_specular);
            Gl.glLightfv(Gl.GL_LIGHT3, Gl.GL_DIFFUSE, light0_diffuse);
            Gl.glLightfv(Gl.GL_LIGHT3, Gl.GL_SPECULAR, light0_specular);
            Gl.glLightfv(Gl.GL_LIGHT4, Gl.GL_DIFFUSE, light0_diffuse); 
            Gl.glLightfv(Gl.GL_LIGHT4, Gl.GL_SPECULAR, light0_specular);
            Gl.glLightfv(Gl.GL_LIGHT5, Gl.GL_DIFFUSE, light0_diffuse);
            Gl.glLightfv(Gl.GL_LIGHT5, Gl.GL_SPECULAR, light0_specular);
            Gl.glLightfv(Gl.GL_LIGHT6, Gl.GL_DIFFUSE, light0_diffuse);
            Gl.glLightfv(Gl.GL_LIGHT6, Gl.GL_SPECULAR, light0_specular);
            Gl.glEnable(Gl.GL_COLOR_MATERIAL);
            Gl.glGenTextures(6, texName); //產生紋理物件
            LoadTexture(@"C:\Users\user\Desktop\final\1072010\img\pic2.jpg", texName[0]);
            LoadTexture(@"C:\Users\user\Desktop\final\1072010\img\pic.jpg", texName[1]);
            //設定紋理環境參數
            Gl.glTexEnvf(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_MODULATE);
            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glEnable(Gl.GL_LIGHTING);
            Gl.glDisable(Gl.GL_LIGHT0);
            Gl.glDisable(Gl.GL_LIGHT1);
            Gl.glDisable(Gl.GL_LIGHT2);
            Gl.glDisable(Gl.GL_LIGHT3);
            Gl.glEnable(Gl.GL_LIGHT4);
            Gl.glEnable(Gl.GL_LIGHT5);
            Gl.glEnable(Gl.GL_LIGHT6);
            Gl.glEnable(Gl.GL_NORMALIZE);
            Gl.glEnable(Gl.GL_CULL_FACE);
            //透明玻璃
            Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);


        }
        private void SetView()
        {
            Gl.glViewport(0, 0, openGLControl1.Size.Width, openGLControl1.Size.Height);

            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            double aspect = (double)openGLControl1.Size.Width /
                            (double)openGLControl1.Size.Height;

            Glu.gluPerspective(45, aspect, 0.1, 100.0);
        }
        private void openGLControl1_Load(object sender, EventArgs e)
        {
            MyInit();
            cam.SetViewVolume(45, openGLControl1.Size.Width, openGLControl1.Size.Height, 0.1, 10.0);

            //Gl.glGenTextures(3, texName); //產生紋理物件

            //LoadTexture(@"C:\Users\user\Desktop\1072010\1.jpg", texName[0]);
            //LoadTexture(@"C:\Users\user\Desktop\1072010\2.jpg", texName[1]);
            //LoadTexture(@"C:\Users\user\Desktop\1072010\3.jpg", texName[2]);


            //設定紋理環境參數
            Gl.glTexEnvf(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_MODULATE);

            myModel = new Model(@"C:\Users\user\Desktop\final\1072010\Arabian_Horse_Galloping_V2_L1.123c04971ec6-5ac5-40e7-bcac-936197b82865\horse.obj"); //載入模型檔

            float[] min = new float[3]; //用來儲存模型座標的最小值
            float[] max = new float[3]; //用來儲存模型座標的最大值

            myModel.ComputeBoundingBox(min, max); //計算模型所佔據的空間範圍
            modelSize = (float)Math.Max(max[0] - min[0], Math.Max(max[1] - min[1], max[2] - min[2])); //計算模型的約略大小
            modelCenter[0] = 0.5f * (min[0] + max[0]);  //計算模型中心x座標
            modelCenter[1] = 0.5f * (min[1] + max[1]);  //計算模型中心y座標
            modelCenter[2] = 0.5f * (min[2] + max[2]);  //計算模型中心z座標
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (lightrot < 360)
            {
                lightrot += 10;
            }
            if (lightrot == 360 && centerlightrot > 0)
            {
                Gl.glEnable(Gl.GL_LIGHT0);
                Gl.glEnable(Gl.GL_LIGHT1);
                Gl.glEnable(Gl.GL_LIGHT2);
                Gl.glEnable(Gl.GL_LIGHT3);
                centerlightrot -= 10;
            }
            rot += 5;
            if (y1 >= 0.045)
            {
                ud1 = -0.005;
            }
            else if (y1 < -0.05)
            {
                ud1 = 0.005;
            }
            y1 += ud1;
            if (y2 >= 0.045)
            {
                ud2 = -0.008;
            }
            else if (y2 < -0.05)
            {
                ud2 = 0.008;
            }
            y2 += ud2;
            if (y3 >= 0.045)
            {
                ud3 = -0.002;
            }
            else if (y3 < -0.05)
            {
                ud3 = 0.002;
            }
            y3 += ud3;
            if (y4 >= 0.045)
            {
                ud4 = -0.01;
            }
            else if (y4 < -0.05)
            {
                ud4 = 0.01;
            }
            y4 += ud4;
            this.openGLControl1.Refresh();
        }

        private void MySolidCube(double size, int slices)
        {
            double s = 1.0 / slices;
            Gl.glPushMatrix();
            Gl.glScaled(size, size, size);
            for (int i = 0; i < slices; i++)
            {
                for (int j = 0; j < slices; j++)
                {
                    Gl.glPushMatrix();
                    Gl.glTranslated(-0.5 + i * s, 0.0, -0.5 + j * s);
                    Gl.glScaled(s, 1.0, s);
                    Gl.glTranslated(0.5, 0.0, 0.5);
                    Glut.glutSolidCube(1.0);
                    Gl.glPopMatrix();
                }
            }
            Gl.glPopMatrix();
        }

        private void openGLControl1_Resize_1(object sender, EventArgs e)
        {
            cam.SetViewVolume(45, openGLControl1.Size.Width, openGLControl1.Size.Height, 0.1, 10.0);
        }

  

        private void openGLControl1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Space:
                    lightrot = 0;
                    centerlightrot = 230;
                    Gl.glDisable(Gl.GL_LIGHT0);
                    Gl.glDisable(Gl.GL_LIGHT1);
                    Gl.glDisable(Gl.GL_LIGHT2);
                    Gl.glDisable(Gl.GL_LIGHT3);
                    break;
                case Keys.D1:
                    if (light0)
                    {
                        Gl.glDisable(Gl.GL_LIGHT4);
                        light0 = false;
                    }
                    else
                    {
                        Gl.glEnable(Gl.GL_LIGHT4);
                        light0 = true;
                    }
                    this.openGLControl1.Refresh();
                    break;
                case Keys.D2:
                    if (light1)
                    {
                        Gl.glDisable(Gl.GL_LIGHT5);
                        light1 = false;
                    }
                    else
                    {
                        Gl.glEnable(Gl.GL_LIGHT5);
                        light1 = true;
                    }
                    this.openGLControl1.Refresh();
                    break;
                case Keys.D3:
                    if (light2)
                    {
                        Gl.glDisable(Gl.GL_LIGHT6);
                        light2 = false;
                    }
                    else
                    {
                        Gl.glEnable(Gl.GL_LIGHT6);
                        light2 = true;
                    }
                    this.openGLControl1.Refresh();
                    break;
                case Keys.Left:
                    if (e.Control) cam.HSlide(-0.1);
                    else if (e.Alt) cam.Roll(1.0);
                    else cam.Pan(1.0);
                    this.openGLControl1.Refresh();
                    break;
                case Keys.Right:
                    if (e.Control) cam.HSlide(0.1);
                    else if (e.Alt) cam.Roll(-1.0);
                    else cam.Pan(-1.0);
                    this.openGLControl1.Refresh();
                    break;
                case Keys.Up:
                    if (e.Control) cam.VSlide(0.1);
                    else cam.Tilt(1.0);
                    this.openGLControl1.Refresh();
                    break;
                case Keys.Down:
                    if (e.Control) cam.VSlide(-0.1);
                    else cam.Tilt(-1.0);
                    this.openGLControl1.Refresh();
                    break;
                case Keys.PageUp:
                    cam.Slide(0.1);
                    this.openGLControl1.Refresh();
                    break;
                case Keys.PageDown:
                    cam.Slide(-0.1);
                    this.openGLControl1.Refresh();
                    break;
                default:
                    break;
            }
            this.openGLControl1.Refresh();
        }

        private void openGLControl1_Paint(object sender, PaintEventArgs e)
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
            cam.LookAt();
            //後面靠近牆壁
            float[] light0_position = new float[] { 1.5f, 2.5f, 1.0f, 1.0f };
            float[] light0_direction = new float[] { 0.0f, -1.0f, 0.0f };
            
            Gl.glPushMatrix();
            Gl.glRotated(centerlightrot, 1.0, 0.0, 0.0);
            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_POSITION, light0_position);
            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_SPOT_DIRECTION, light0_direction);
            Gl.glLightf(Gl.GL_LIGHT0, Gl.GL_SPOT_CUTOFF, (float)(Math.Atan(0.3) * 180.0 / Math.PI));
            Gl.glLightf(Gl.GL_LIGHT0, Gl.GL_SPOT_EXPONENT, 10.0f);
            Gl.glPopMatrix();

            //前面靠近攝影機
            float[] light1_position = new float[] { 1.5f, 2.5f, 2.2f, 1.0f };
            float[] light1_direction = new float[] { 0.0f, -1.0f, 0.0f };

            Gl.glPushMatrix();
            Gl.glRotated(centerlightrot, -1.0, 0.0, 0.0);
            Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_POSITION, light1_position);
            Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_SPOT_DIRECTION, light1_direction);
            Gl.glLightf(Gl.GL_LIGHT1, Gl.GL_SPOT_CUTOFF, (float)(Math.Atan(0.3) * 180.0 / Math.PI));
            Gl.glLightf(Gl.GL_LIGHT1, Gl.GL_SPOT_EXPONENT, 10.0f);
            Gl.glPopMatrix();

            //左邊
            float[] light2_position = new float[] { 1.0f, 2.5f, 1.7f, 1.0f };
            float[] light2_direction = new float[] { 0.0f, -1.0f, 0.0f };

            Gl.glPushMatrix();
            Gl.glRotated(centerlightrot, 0.0, 0.0, 1.0);
            Gl.glLightfv(Gl.GL_LIGHT2, Gl.GL_POSITION, light2_position);
            Gl.glLightfv(Gl.GL_LIGHT2, Gl.GL_SPOT_DIRECTION, light2_direction);
            Gl.glLightf(Gl.GL_LIGHT2, Gl.GL_SPOT_CUTOFF, (float)(Math.Atan(0.3) * 180.0 / Math.PI));
            Gl.glLightf(Gl.GL_LIGHT2, Gl.GL_SPOT_EXPONENT, 10.0f);
            Gl.glPopMatrix();

            //右邊
            float[] light3_position = new float[] { 2.0f, 2.5f, 1.7f, 1.0f };
            float[] light3_direction = new float[] { 0.0f, -1.0f, 0.0f };

            Gl.glPushMatrix();
            Gl.glRotated(centerlightrot, 0.0, 0.0, -1.0);
            Gl.glLightfv(Gl.GL_LIGHT3, Gl.GL_POSITION, light3_position);
            Gl.glLightfv(Gl.GL_LIGHT3, Gl.GL_SPOT_DIRECTION, light3_direction);
            Gl.glLightf(Gl.GL_LIGHT3, Gl.GL_SPOT_CUTOFF, (float)(Math.Atan(0.3) * 180.0 / Math.PI));
            Gl.glLightf(Gl.GL_LIGHT3, Gl.GL_SPOT_EXPONENT, 10.0f);
            Gl.glPopMatrix();

            //前牆圓燈
            float[] light4_position = new float[] { 1.5f, 0.5f, 3.0f, 1.0f };
            float[] light4_direction = new float[] { 0.0f, 0.0f, -1.0f };

            Gl.glPushMatrix();
            Gl.glRotated(lightrot, 1.0, 0.0, 0.0);
            Gl.glLightfv(Gl.GL_LIGHT4, Gl.GL_POSITION, light4_position);
            Gl.glLightfv(Gl.GL_LIGHT4, Gl.GL_SPOT_DIRECTION, light4_direction);
            Gl.glLightf(Gl.GL_LIGHT4, Gl.GL_SPOT_CUTOFF, (float)(Math.Atan(0.3) * 180.0 / Math.PI));
            Gl.glLightf(Gl.GL_LIGHT4, Gl.GL_SPOT_EXPONENT, 10.0f);
            Gl.glPopMatrix();
            //右邊打到左邊
            float[] light5_position = new float[] { 1.5f, 2.0f, 1.5f, 1.0f };
            float[] light5_direction = new float[] { -1.0f, -1.0f, 0.0f };

            Gl.glPushMatrix();
            Gl.glRotated(lightrot, 0.0, 0.0, 1.0);
            Gl.glLightfv(Gl.GL_LIGHT5, Gl.GL_POSITION, light5_position);
            Gl.glLightfv(Gl.GL_LIGHT5, Gl.GL_SPOT_DIRECTION, light5_direction);
            Gl.glLightf(Gl.GL_LIGHT5, Gl.GL_SPOT_CUTOFF, (float)(Math.Atan(0.3) * 180.0 / Math.PI));
            Gl.glLightf(Gl.GL_LIGHT5, Gl.GL_SPOT_EXPONENT, 10.0f);
            Gl.glPopMatrix();
            //左邊打到右邊
            float[] light6_position = new float[] { 1.5f, 2.0f, 1.5f, 1.0f };
            float[] light6_direction = new float[] { 1.0f, -1.0f, 0.0f };

            Gl.glPushMatrix();
            Gl.glRotated(lightrot, 0.0, 0.0, -1.0);
            Gl.glLightfv(Gl.GL_LIGHT6, Gl.GL_POSITION, light6_position);
            Gl.glLightfv(Gl.GL_LIGHT6, Gl.GL_SPOT_DIRECTION, light6_direction);
            Gl.glLightf(Gl.GL_LIGHT6, Gl.GL_SPOT_CUTOFF, (float)(Math.Atan(0.3) * 180.0 / Math.PI));
            Gl.glLightf(Gl.GL_LIGHT6, Gl.GL_SPOT_EXPONENT, 10.0f);
            Gl.glPopMatrix();

            float[] mat_ambient = new float[3];
            float[] mat_diffuse = new float[3];
            float[] mat_specular = new float[3];
            float mat_shininess;

            // Brass 黃銅
            mat_ambient[0] = 0.329412f;
            mat_ambient[1] = 0.223529f;
            mat_ambient[2] = 0.027451f;
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_AMBIENT, mat_ambient);
            mat_diffuse[0] = 0.780392f;
            mat_diffuse[1] = 0.568627f;
            mat_diffuse[2] = 0.113725f;
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_DIFFUSE, mat_diffuse);
            mat_specular[0] = 0.992157f;
            mat_specular[1] = 0.941176f;
            mat_specular[2] = 0.807843f;
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_SPECULAR, mat_specular);
            mat_shininess = 27.8974f;
            Gl.glMaterialf(Gl.GL_FRONT, Gl.GL_SHININESS, mat_shininess);

            //馬
            Gl.glPushMatrix();
            Gl.glTranslated(1.5, 0.5, 1.5);
            Gl.glRotated(rot, 0.0, -1.0, 0.0);
            Gl.glTranslated(-0.5, y1, -0.5);
            Gl.glRotated(90, -1.0, 0.0, 0.0);
            Gl.glRotated(90, 0.0, 0.0, 1.0);
            Gl.glColor3d(1.0, 0.717, 0.85);//黃色
            Horse();
            Gl.glPopMatrix();

            mat_ambient[0] = 0.329412f;
            mat_ambient[1] = 0.223529f;
            mat_ambient[2] = 0.027451f;
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_AMBIENT, mat_ambient);
            mat_diffuse[0] = 0.780392f;
            mat_diffuse[1] = 0.568627f;
            mat_diffuse[2] = 0.113725f;
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_DIFFUSE, mat_diffuse);
            mat_specular[0] = 0.992157f;
            mat_specular[1] = 0.941176f;
            mat_specular[2] = 0.807843f;
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_SPECULAR, mat_specular);
            mat_shininess = 27.8974f;
            Gl.glMaterialf(Gl.GL_FRONT, Gl.GL_SHININESS, mat_shininess);

            Gl.glPushMatrix();
            Gl.glTranslated(1.5, 0.5, 1.5);
            Gl.glRotated(rot+90, 0.0, -1.0, 0.0);
            Gl.glTranslated(-0.5, y2, -0.5);
            //Gl.glTranslated(0.0, 0.0, -1.0);
            Gl.glRotated(90, -1.0, 0.0, 0.0);
            Gl.glRotated(90, 0.0, 0.0, 1.0);
            Gl.glColor3d(0.533, 0.611, 0.99);//黑色
            Horse();
            Gl.glPopMatrix();

            mat_ambient[0] = 0.329412f;
            mat_ambient[1] = 0.223529f;
            mat_ambient[2] = 0.027451f;
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_AMBIENT, mat_ambient);
            mat_diffuse[0] = 0.780392f;
            mat_diffuse[1] = 0.568627f;
            mat_diffuse[2] = 0.113725f;
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_DIFFUSE, mat_diffuse);
            mat_specular[0] = 0.992157f;
            mat_specular[1] = 0.941176f;
            mat_specular[2] = 0.807843f;
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_SPECULAR, mat_specular);
            mat_shininess = 27.8974f;
            Gl.glMaterialf(Gl.GL_FRONT, Gl.GL_SHININESS, mat_shininess);

            Gl.glPushMatrix();
            Gl.glTranslated(1.5, 0.5, 1.5);
            Gl.glRotated(rot + 180, 0.0, -1.0, 0.0);
            //Gl.glTranslated(-0.8, 0.0, -0.5);
            Gl.glTranslated(-0.5, y3, -0.5);
            Gl.glRotated(90, -1.0, 0.0, 0.0);
            Gl.glRotated(180, 0.0, 0.0, -1.0);
            Gl.glColor3d(0.89, 0.701, 0.117);//藍
            Horse();
            Gl.glPopMatrix();

            mat_ambient[0] = 0.329412f;
            mat_ambient[1] = 0.223529f;
            mat_ambient[2] = 0.027451f;
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_AMBIENT, mat_ambient);
            mat_diffuse[0] = 0.780392f;
            mat_diffuse[1] = 0.568627f;
            mat_diffuse[2] = 0.113725f;
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_DIFFUSE, mat_diffuse);
            mat_specular[0] = 0.992157f;
            mat_specular[1] = 0.941176f;
            mat_specular[2] = 0.807843f;
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_SPECULAR, mat_specular);
            mat_shininess = 27.8974f;
            Gl.glMaterialf(Gl.GL_FRONT, Gl.GL_SHININESS, mat_shininess);

            Gl.glPushMatrix();
            Gl.glTranslated(1.5, 0.5, 1.5);
            Gl.glRotated(rot + 270, 0.0, -1.0, 0.0);
            //Gl.glTranslated(0.8, 0.0, -0.5);
            Gl.glTranslated(-0.5, y4, -0.5);
            Gl.glRotated(90, -1.0, 0.0, 0.0);
            Gl.glRotated(180, 0.0, 0.0, -1.0);
            Gl.glColor3d(0.254, 0.0, 0.501);//咖啡色
            Horse();
            Gl.glPopMatrix();

            //三角錐
            mat_ambient[0] = 0.329412f;
            mat_ambient[1] = 0.223529f;
            mat_ambient[2] = 0.027451f;
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_AMBIENT, mat_ambient);
            mat_diffuse[0] = 0.780392f;
            mat_diffuse[1] = 0.568627f;
            mat_diffuse[2] = 0.113725f;
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_DIFFUSE, mat_diffuse);
            mat_specular[0] = 0.992157f;
            mat_specular[1] = 0.941176f;
            mat_specular[2] = 0.807843f;
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_SPECULAR, mat_specular);
            mat_shininess = 27.8974f;
            Gl.glMaterialf(Gl.GL_FRONT, Gl.GL_SHININESS, mat_shininess);
            Gl.glColor3d(0.52, 0.02, 0.07);
            Gl.glPushMatrix();
            Gl.glTranslated(1.5, 1.0, 1.5);
            Gl.glRotated(90, -1.0, 0.0, 0.0);
            Glut.glutSolidCone(1.0, 0.4, 30, 30);
            Gl.glPopMatrix();
            //中間柱子
            mat_ambient[0] = 0.329412f;
            mat_ambient[1] = 0.223529f;
            mat_ambient[2] = 0.027451f;
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_AMBIENT, mat_ambient);
            mat_diffuse[0] = 0.780392f;
            mat_diffuse[1] = 0.568627f;
            mat_diffuse[2] = 0.113725f;
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_DIFFUSE, mat_diffuse);
            mat_specular[0] = 0.992157f;
            mat_specular[1] = 0.941176f;
            mat_specular[2] = 0.807843f;
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_SPECULAR, mat_specular);
            mat_shininess = 27.8974f;
            Gl.glMaterialf(Gl.GL_FRONT, Gl.GL_SHININESS, mat_shininess);
            Gl.glColor3d(0.96, 0.73, 0.14);
            Gl.glPushMatrix(); 
            Gl.glTranslated(1.5, 0.0, 2.0);
            Gl.glRotated(90, -1.0, 0.0, 0.0);
            Glut.glutSolidCylinder(0.05, 1, 64, 64);
            Gl.glPopMatrix();
            //地面
            Gl.glColor3d(0.52, 0.02, 0.07);
            Gl.glPushMatrix();
            Gl.glTranslated(1.5, 0.05, 2.0);
            Gl.glRotated(90, -1.0, 0.0, 0.0);
            Glut.glutSolidCylinder(0.8, 0.05, 32, 32);
            Gl.glPopMatrix();
            //繪製牆面
            wall();
 
        }
        void face(int Slices)
        {
            double dx = 1.0 / Slices;
            double dz = 1.0 / Slices;
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glNormal3d(0.0, 1.0, 0.0);
            for (double x = 0; x < 1.0; x += dx)
            {
                for (double z = 0; z < 1.0; z += dz)
                {
                    Gl.glTexCoord2d(x, z);
                    Gl.glVertex3d(x, 0.0, z);
                    Gl.glTexCoord2d(x, z + dz);
                    Gl.glVertex3d(x, 0.0, z + dz);
                    Gl.glTexCoord2d(x + dx, z + dz);
                    Gl.glVertex3d(x + dx, 0.0, z + dz);
                    Gl.glTexCoord2d(x + dx, z);
                    Gl.glVertex3d(x + dx, 0.0, z);
                }
            }
            Gl.glEnd();
        }
        void wall()
        {
            //牆面貼圖
            //天花板
            Gl.glColor3ub(255, 255, 255);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texName[1]);
            Gl.glEnable(Gl.GL_TEXTURE_2D);
            Gl.glPushMatrix();
            Gl.glScaled(3.0, 1.0, 3.0);
            Gl.glRotated(180, 0.0, 0.0, 1.0);
            Gl.glTranslated(-1.0, -2.0, 0.0);
            face(100);
            Gl.glPopMatrix();
            Gl.glDisable(Gl.GL_TEXTURE_2D);
            //地板
            Gl.glColor3ub(255, 255, 255);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texName[1]);
            Gl.glEnable(Gl.GL_TEXTURE_2D);
            Gl.glPushMatrix();
            Gl.glScaled(3.0, 1.0, 3.0);
            //Gl.glTranslated(-0.5, 0.002, -0.5);
            face(100);
            Gl.glPopMatrix();
            Gl.glDisable(Gl.GL_TEXTURE_2D);
            //左牆
            Gl.glColor3ub(255, 255, 255);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texName[0]);
            Gl.glEnable(Gl.GL_TEXTURE_2D);
            Gl.glPushMatrix();
            Gl.glScaled(3.0, 2.0, 3.0);
            Gl.glRotated(90, 0.0, 0.0, -1.0);
            Gl.glTranslated(-1.0, 0.0, 0.0);
            face(100);
            Gl.glPopMatrix();
            Gl.glDisable(Gl.GL_TEXTURE_2D);
            //右牆
            Gl.glColor3ub(255, 255, 255);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texName[0]);
            Gl.glEnable(Gl.GL_TEXTURE_2D);
            Gl.glPushMatrix();
            Gl.glScaled(3.0, 2.0, 3.0);
            Gl.glRotated(90, 0.0, 0.0, 1.0);
            Gl.glTranslated(0.0, -1.0, 0.0);
            face(100);
            Gl.glPopMatrix();
            Gl.glDisable(Gl.GL_TEXTURE_2D);
            //前牆
            Gl.glColor3ub(255, 255, 255);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texName[0]);
            Gl.glEnable(Gl.GL_TEXTURE_2D);
            Gl.glPushMatrix();
            Gl.glScaled(3.0, 2.0, 3.0);
            Gl.glRotated(90, 1.0, 0.0, 0.0);
            Gl.glTranslated(0.0, 0.0, -1.0);
            face(100);
            Gl.glPopMatrix();
            Gl.glDisable(Gl.GL_TEXTURE_2D);
        }
        void Horse()
        {
            Gl.glPushMatrix();
            //Gl.glColor3d(0.403, 0.121, 0.129);
            Glut.glutSolidCylinder(0.01, 0.55, 32, 32);
            Gl.glPopMatrix();
            float scale = 0.7f / modelSize; //計算模型縮放倍率
            Gl.glScalef(scale, scale, scale); //進行模型的縮放
            Gl.glTranslatef(-modelCenter[0], -modelCenter[1], -modelCenter[2]); //將模型移到座標原點
            myModel.DrawByOpenGL2();//用OpenGL 2版本以前的OpenGL指令進行模型的繪製

        }
        private void LoadTexture(string filename, uint texture)
        {
            if (Il.ilLoadImage(filename)) //載入影像檔
            {
                int BitsPerPixel = Il.ilGetInteger(Il.IL_IMAGE_BITS_PER_PIXEL); //取得儲存每個像素的位元數
                int Depth = Il.ilGetInteger(Il.IL_IMAGE_DEPTH);
                Ilu.iluScale(512, 512, Depth);
                Ilu.iluFlipImage(); //顛倒影像
                Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture); //連結紋理物件
                                                             //設定紋理參數
                Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_REPEAT);
                Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_REPEAT);
                Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
                Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);
                //建立紋理物件
                if (BitsPerPixel == 24) Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGB, 512, 512, 0,
                 Il.ilGetInteger(Il.IL_IMAGE_FORMAT), Il.ilGetInteger(Il.IL_IMAGE_TYPE), Il.ilGetData());
                if (BitsPerPixel == 32) Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGBA, 512, 512, 0,
                 Il.ilGetInteger(Il.IL_IMAGE_FORMAT), Il.ilGetInteger(Il.IL_IMAGE_TYPE), Il.ilGetData());
                //Gl.glGenerateMipmap(Gl.GL_TEXTURE_2D);
            }
            else
            {   // 若檔案無法開啟，顯示錯誤訊息
                string message = "Cannot open file " + filename + ".";
                MessageBox.Show(message, "Image file open error!!!", MessageBoxButtons.OK,
                                MessageBoxIcon.Exclamation);
            }

        }
    }
}
