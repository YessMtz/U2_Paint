using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace PaintProgram
{
    public partial class Pinturitas : Form
    {
        //VARIABLES PARA DIBUJAR
        Bitmap bmPic;
        Graphics gPic;
        bool paint = false;
        Point px, py;

        //objeto lapiz de color negro y grosor 1
        Pen lapiz = new Pen(Color.Black, 1);

        //objeto lapiz borrador y grosor 10
        Pen borrador = new Pen(Color.White, 10);

        //VARIABLES PARA LOS PUNTOS DEL CIRCULO
        int x, y, sx, sy, cX, cY;
        
        int index;

        Color NuevoColor;
        ColorDialog PaletaColores = new ColorDialog();

        //Boton Lapiz
        private void button1_Click(object sender, EventArgs e)
        {
            index = 1; 
        }

        //Boton Borrar
        private void button5_Click(object sender, EventArgs e)
        {
            index = 2;
        }
        //boton circulo
        private void Circulo_Click(object sender, EventArgs e)
        {
            index = 3;
        }
        
        //BOTON RECTANGULO
        private void Rectangulo_Click(object sender, EventArgs e)
        {
            index = 4;
        }
        
        //BOTON LINEA RECTA
        private void Line_Click(object sender, EventArgs e)
        {
            index = 5;
        }
        //cubeta metodo
        private void Cubeta_Click(object sender, EventArgs e)
        {
            index = 6;
        }

        //BORRAR TODO
        private void Nuevo_Click(object sender, EventArgs e)
        {
            gPic.Clear(Color.White);
            Pic.Image = bmPic;
            index = 0;
        }


        //INICIALIZAR FORMA
        public Pinturitas()
        {
            InitializeComponent();
            bmPic = new Bitmap(Pic.Width, Pic.Height);
            gPic = Graphics.FromImage(bmPic);

            //CAMBIA EL COLOR DEL LAPIZ PARA BORRAR
            gPic.Clear(Color.White);

            Pic.Image = bmPic;
        }

        //METODO CLIC PARA EL PICTUREBOX
        private void PicClick(object sender, EventArgs e)
        {

        }
        
        //PALETA DE COLORES
        private void Colores_Click(object sender, EventArgs e)
        {
            PaletaColores.ShowDialog();
            NuevoColor = PaletaColores.Color;
            Pic.BackColor = PaletaColores.Color;
            lapiz.Color = PaletaColores.Color;

        }

        //Agregar Nuevo color
        private void Pic_MouseClick(object sender, MouseEventArgs e)
        {
            if(index ==6)
            {
                Point punto = set_Point(Pic, e.Location);
                Fill(bmPic, punto.X, punto.Y, NuevoColor);
            }
        }
        
        //ACCIONES DEL MOUSE HACIA ABAJO
        private void Pic_MouseDown(object sender, MouseEventArgs e)
        {
            // si pinta hacia abajo
             paint = true;
            py = e.Location;

            //DIBUJAR EL CIRCULO EN LAS DIMENSIONES X & Y
            cX = e.X;
            cY = e.Y;
        }
        //ACCIONES DEL MOUSE SE MUEVE
        private void Pic_MouseMove(object sender, MouseEventArgs e)
        {
            if (paint)
            {
                //para dibujar a mano alzada
                if (index == 1)
                {
                    //punto x en localizacion
                    px = e.Location;
                    //pintar una linea tipo Glapiz
                    gPic.DrawLine(lapiz, px, py);
                    py = px;

                }

                //para borrar lo que se dibujo
                if (index == 2)
                {
                    //punto x en localizacion
                    px = e.Location;
                    //pintar una linea tipo PEN+BORRADOR
                    gPic.DrawLine(borrador, px, py);
                    py = px;

                }


                
            }                
            Pic.Refresh();
            
            //variables para el ciculo 
                x = e.X;
                y = e.Y;
                sx = cX - cX;
                sy = cY - cY;
        }

        //ACCIONES DEL MOUSE HACIA ARRIBA
        private void Pic_MouseUp(object sender, MouseEventArgs e)
        {
            paint = false;

            sx = x - cX;
            sy = y - cY;

            //PARA DIBUJAR EL CIRCULO
            if (index == 3)
            {
                gPic.DrawEllipse(lapiz, cX, cY, sx, sy);
            }

            //PARA DIBUJAR EL RECTANGULO
            if (index == 4)
            {
                gPic.DrawRectangle(lapiz, cX, cY, sx, sy);
            }

            //PARA DIBUJAR LINEA RECTA
            if (index == 5)
            {
                gPic.DrawLine(lapiz, cX, cY, x, y);
            }

        }



        
        //BOTON GUARDAR
        private void Guardar_Click(object sender, EventArgs e)
        {
            var GuardarDoc = new SaveFileDialog();
            GuardarDoc.Filter = "Image(*.jpg)|*.jpg|(*.*)|*.*";
            if(GuardarDoc.ShowDialog()== DialogResult.OK)
            {
                Bitmap docSave = bmPic.Clone(new Rectangle(0, 0, Pic.Width, Pic.Height), bmPic.PixelFormat);
                docSave.Save(GuardarDoc.FileName, ImageFormat.Jpeg);

            }
        }

        private void Pinturitas_Load(object sender, EventArgs e)
        {

        }

        //para pintar las figuras al tama;o que deseas
        private void Pic_Paint(object sender, PaintEventArgs e)
        {
            Graphics gPic = e.Graphics;
            if (paint)
            {
                if (index == 3)
                {
                    gPic.DrawEllipse(lapiz, cX, cY, sx, sy);
                }

                //PARA DIBUJAR EL RECTANGULO
                if (index == 4)
                {
                    gPic.DrawRectangle(lapiz, cX, cY, sx, sy);
                }

                
            }
        }


        //CUBETA
        static Point set_Point(PictureBox cajita, Point punto)
        {
            float px = 1f * cajita.Width / cajita.Width;
            float py = 1f * cajita.Height / cajita.Height;
            return new Point((int)(punto.X * px), (int)(punto.Y * py));
        }

        //validacion
        private void Validate(Bitmap bmValdate, Stack<Point> StackPoint, int x, int y, Color Old_Color, Color nuevoColor)
        {
            Color ColorX = bmValdate.GetPixel(x, y);
            if (ColorX == Old_Color)
            {

                StackPoint.Push(new Point(x, y));
                bmValdate.SetPixel(x, y, nuevoColor);
            }

        }

        //cubeta para pintar
        public void Fill(Bitmap bmPic, int x, int y, Color New_Color)
        {
            Color Old_Color = bmPic.GetPixel(x, y);
            Stack<Point> pixel = new Stack<Point>();
            pixel.Push(new Point(x, y));
            bmPic.SetPixel(x, y, New_Color);

            if (Old_Color == New_Color)
            {
                return;
            }

            while (pixel.Count > 0)
            {
                Point puntoNuevo = (Point)pixel.Pop();
                if (puntoNuevo.X > 0 && puntoNuevo.Y > 0 && puntoNuevo.X < bmPic.Width - 1 && puntoNuevo.Y < bmPic.Height - 1)
                {
                    Validate(bmPic, pixel, puntoNuevo.X - 1, puntoNuevo.Y, Old_Color, New_Color);
                    Validate(bmPic, pixel, puntoNuevo.X, puntoNuevo.Y - 1, Old_Color, New_Color);
                    Validate(bmPic, pixel, puntoNuevo.X + 1, puntoNuevo.Y, Old_Color, New_Color);
                    Validate(bmPic, pixel, puntoNuevo.X, puntoNuevo.Y + 1, Old_Color, New_Color);
                }
            }
        }


    }
}
