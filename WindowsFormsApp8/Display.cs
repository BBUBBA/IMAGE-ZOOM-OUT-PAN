using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp8
{
    class Display : Panel
    {
        float _scale = 1f;
        float _translateY = 0f;
        float _translateX = 0f;

        float world_x = 0f;
        float world_y = 0f;

        double x0 = 0.0, y0 = 0.0, mx, my;

        void scr2obj(out double ox, out double oy, double sx, double sy)
        {
            //	ox=(sx-x0)/zoom; 
            //	oy=(sy-y0)/zoom; 
            ox = (sx / _scale) - x0;
            oy = (sy / _scale) - y0;
        }
        //--------------------------------------------------------------------------- 
        void obj2scr(ref double sx, ref double sy, double ox, double oy)
        {
            //	sx=x0+(ox*zoom); 
            //	sy=y0+(oy*zoom); 
            sx = (x0 + ox) * _scale;
            sy = (y0 + oy) * _scale;
        }

        public Display()
        {
            this.MouseWheel += new MouseEventHandler(this.onMouseWheel);
            this.MouseMove += Display_MouseMove;
            this.MouseDown += Display_MouseDown;
            this.MouseUp += Display_MouseUp;
        }

        private void Display_MouseUp(object sender, MouseEventArgs e)
        {
            isdown = false;
        }

        bool isdown;
        Point pointdown;

        private void Display_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isdown = true;
                pointdown = e.Location;
            }
        }

        private void Display_MouseMove(object sender, MouseEventArgs e)
        {
            if (isdown)
            {
                x0 += (e.X - pointdown.X) / _scale;
                y0 += (e.Y - pointdown.Y) / _scale;

                pointdown = e.Location;

                this.Invalidate();

            }
        }

        private void onMouseWheel(object sender, MouseEventArgs e)
        {

            //////screen_x = (world_x * _scale) + _translateX;
            //////screen_y = (world_y * _scale) + _translateY;

            //screen_x = (world_x + _translateX) * _scale;
            //screen_y = (world_y + _translateY) * _scale;

            //world_x = (screen_x - _translateX) / _scale;
            //world_y = (screen_y - _translateY) / _scale;

            mx = e.X;
            my = e.Y;

            if (e.Delta > 0)
            {
                //world_x = (e.X - _translateX) / _scale;
                //world_y = (e.Y - _translateY) / _scale;

                //_scale *= 1.25f;
                //_translateY = e.Y - 1.25f * (e.Y - _translateY);
                //_translateX = e.X - 1.25f * (e.X - _translateX);

                double mx0, my0;
                scr2obj(out mx0, out my0, mx, my);
                _scale *= 1.25f; // zoom in 
                obj2scr(ref mx0, ref my0, mx0, my0);
                x0 += (mx - mx0) / _scale;
                y0 += (my - my0) / _scale;

            }
            else
            {
                if (_scale <= 1) return;

                //_scale /= 1.25f;
                //_translateY = e.Y - 0.8f * (e.Y - _translateY);
                //_translateX = e.X - 0.8f * (e.X - _translateX);

                double mx0, my0;
                scr2obj(out mx0, out my0, mx, my);
                _scale /= 1.25f; // zoom in 
                obj2scr(ref mx0, ref my0, mx0, my0);
                x0 += (mx - mx0) / _scale;
                y0 += (my - my0) / _scale;

            }
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;

            g.ScaleTransform(_scale, _scale);
            //g.TranslateTransform(_translateX, _translateY);
            g.TranslateTransform((float)(x0), (float)(y0));

            Pen pen = new Pen(Color.Red);
            g.FillEllipse(pen.Brush, 50, 50, 10, 10);

            Pen pen1 = new Pen(Color.Black);
            g.FillEllipse(pen1.Brush, this.Width - 100, this.Height - 100, 10, 10);

            Pen pen2 = new Pen(Color.Blue);
            g.FillEllipse(pen2.Brush, this.Width - 200, this.Height - 200, 20, 20);

        }
    }
}
