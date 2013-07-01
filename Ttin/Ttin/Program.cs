
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Threading;
using System.Diagnostics;

public class Ttin : Game
{
    private GraphicsDeviceManager Gm;
    private Vector2 pos1, pos2;
    private SpriteBatch sprite;
    private Texture2D Tgazo, gazo2;
    int sw = 2, sw2 = -2;
    int p1 = 1, p2 = 0;
    int st = 0;
    private CPU cpu;
    private ATch atch;
    private Blast blast;
    private createmap cmap;
    public int[,] maptable, mapa;
    public bool flg = false;
    //FileStream stream = File.OpenRead("map.png");

    public Ttin()
    {
        Gm = new GraphicsDeviceManager(this);
        Gm.PreferredBackBufferHeight = 600;
        Gm.PreferredBackBufferWidth = 800;
        pos1.X = 1;
        pos1.Y = 1;
        pos2.X = 600;
        pos2.Y = 0;
        mapa = new int[,] { 
        {98,98,98,98,98,98,98,98,99,99,99,99,99,98,98},
        {98,99,99,99,99,99,99,99,45,44,44,44,44,99,98},
        {51,50,49,48,99,99,99,99,45,99,99,99,43,99,98},
        {98,99,99,47,99,99,99,99,45,99,99,99,43,99,98},
        {98,98,98,47,46,46,46,46,46,99,98,98,43,99,98},
        {98,98,98,98,98,98,98,98,98,98,98,98,43,99,98},
        {98,99,23,23,23,23,23,23,23,24,99,99,43,99,98},
        {98,99,17,99,99,99,99,99,99,24,99,99,43,99,98},
        {98,99,16,99,99,99,99,99,99,25,99,99,43,99,98},
        {98,99,15,99,98,98,98,98,98,26,99,99,43,99,98},
        {98,99,14,99,99,99,99,99,99,27,28,29,30,99,98}, 
        {98,99,13,99,99,99, 5,99,99,99,99,99,99,99,98},
        {98,99,12,11,10, 9, 8, 7, 6, 5, 4, 3, 2,99,98},
        {98,99,99,99,99,99,99,99,99,99,99,99, 1,99,98},
        {98,98,98,98,98,98,98,98,98,98,98,98, 0,98,98}
        };
        maptable = new int[,] { 
        {49,49,49,49,49,49,49,49,49,49,49,49,49,49,49},
        {17,17,17,17,17,17,17,17,17,17,17,17,17,17,49},
        { 7, 7, 7, 7,17,17,17,17, 7, 7, 7, 7, 7,17,49},
        {17,17,17, 7,17,17,17,17, 7,17,17,17, 7,17,49},
        {17,17,17, 7, 7, 7, 7, 7, 7,17,17,17, 7,17,49},
        {49,49,49,49,49,49,49,49,49,49,49,17, 7,17,49},
        {49,49,49,49,49,49,49,49,49,49,49,17, 7,17,49},
        {49,49,17, 7, 7, 7, 7, 7, 7, 7, 7,17, 7,17,49},
        {49,49,17, 7,17,17,17,17,17,17, 7,17, 7,17,49},
        {49,49,17, 7,17,17,17,17,17,17, 7,17, 7,17,49},
        {49,49,17, 7,17,49,49,49,49,17, 7, 7, 7,17,49}, 
        {49,49,17, 7,17,17,17,17,17,17,17,17,17,17,49},
        {49,49,17, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,17,49},
        {49,49,17,17,17,17,17,17,17,17,17,17, 7,17,49},
        {49,49,49,49,49,49,49,49,49,49,49,49, 7,49,49}
        };
    }

    public static void Main(string[] arg)
    {
        using (Game g = new Ttin())
        {
            g.IsMouseVisible = true;
            g.Run();
        }
    }
    protected override void LoadContent()
    {

        sprite = new SpriteBatch(GraphicsDevice);
        cpu = new CPU(Gm.GraphicsDevice, sprite, mapa);
        atch = new ATch();
        blast = new Blast(Gm.GraphicsDevice, sprite);
        cmap = new createmap(Gm.GraphicsDevice, sprite);
        Tgazo = Content.Load<Texture2D>("Content/sampgame");
        gazo2 = Content.Load<Texture2D>("Content/sampgame2");
        base.LoadContent();
    }
    protected override void Update(GameTime gameTime)
    {
        KeyboardState state = Keyboard.GetState();
        if (state[Keys.Up] == KeyState.Down) pos1.Y -= 5;
        if (state[Keys.Down] == KeyState.Down) pos1.Y += 5;
        if (state[Keys.Left] == KeyState.Down) pos1.X -= 5;
        if (state[Keys.Right] == KeyState.Down) pos1.X += 5;

        mousePressChk();
        if (st == 30)
        {
            cpu.setCPU(1, 1);
            st = 0;
        }
        else
        {
            st++;
        }
        
        base.Update(gameTime);

    }
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);
        sprite.Begin();
        sprite.Draw(Tgazo, pos1, Color.White);
        if (flg)
        {
            cmap.paintmap(maptable);
            //sprite.Draw(Tgazo, pos1, Color.White);
            sprite.Draw(gazo2, pos2, Color.White);
            cpu.paintCPU();
            blast.paintBlast();
        }
        else
        {
            sprite.Draw(Tgazo, pos1, Color.White);

        }
        sprite.End();

        base.Draw(gameTime);
    }
    private void mousePressChk()
    {
        MouseState state = Mouse.GetState();
        if (state.LeftButton == ButtonState.Pressed)
        {
            flg = true;
            //pos2.X = state.X;
            //pos2.Y = state.Y;
            //sprite.Begin();
            //sprite.Draw(gazo2, pos2, Color.White);
            //blast.setBlast(state.X - 24, state.Y - 24);
            //sprite.End();
        }
        if (state.RightButton == ButtonState.Pressed)
        {
            flg = false;
            //pos2.X = state.X;
            //pos2.Y = state.Y;
            //sprite.Begin();
            //sprite.Draw(gazo2, pos2, Color.White);
            //blast.setBlast(state.X - 24, state.Y - 24);
            //sprite.End();
        }
    }

}

class ATch
{
    public int attackcheck(float atx, float aty)
    {

        //if (atx >= 560)
        //{

        //    return 1;
        //}
        //else if (aty > 560)
        //{
        //    return 2;
        //}
        //else if (atx < 0)
        //{
        //    return 3;
        //}
        //else if (aty <= 0)
        //{
        //    return 4;
        //}

        return 0;
    }
    public Boolean clikch(int clix, int cliy, float cpux, float cpuy, int w1, int w2, int h1, int h2)
    {
        if (clix + w1 >= cpux && clix < cpux + w2)
        {
            if (cpuy + h2 < cliy && cliy + h1 < cpuy + h2 || cpuy < cliy && cliy < cpuy + h2)
            {
                return true;

            }
        }


        return false;


    }
    public bool atcmap(int x1, int y1, int w1, int h1, int[,] map)
    {
        //for(int i=0;i )
        for (int i = 0; i < 15; i++)
        {
            for (int j = 0; j < 15; j++)
            {
                if (map[j, i] != 0)
                {

                    if (this.clikch(x1, y1, i * 40, j * 40, w1, 24, h1, 24))
                    {
                        Debug.WriteLine("x:" + i + " y:" + j);
                        return true;
                    }

                }
            }
        }
        return false;
    }
    public bool atlect(int x1, int y1, int[,] map)
    {

        int ax, ay, ang;
        ang = (12) + (20);
        for (int i = 0; i < 15; i++)
        {
            for (int j = 0; j < 15; j++)
            {
                if (map[i, j] != 0)
                {

                    ax = System.Math.Abs((int)((x1 + 12) - ((i * 40) + 20)));
                    ay = System.Math.Abs((int)((y1 + 12) - ((j * 40) + 20)));

                    ax = (int)(System.Math.Sqrt(ax));
                    ay = (int)(System.Math.Sqrt(ay));
                    ang = (int)(System.Math.Sqrt(ang));

                    if (ax + ay <= ang)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    public int mapch(int x1, int y1, int[,] map)
    {
         int lv=99;
        int ax = (y1 / 40);
        int ay = (x1 / 40);
        if (ax > 0 && ay > 0)
        {
            lv = map[ax, ay];
        }
        if (lv <= 0)
        {
            return 0;
        }
        if (ax > 0)
        {
            if (lv > map[ax, ay + 1])
            {
                return 1;
            }
            if (lv > map[ax + 1, ay])
            {
                return 2;
            }
        }
        if (ay > 0)
        {
            if (lv > map[ax, ay - 1])
            {
                return 3;
            }
            if (lv > map[ax - 1, ay])
            {
                return 4;
            }
        }

        return -1;
    }
}
class Unit
{
    const int BLASTSU = 10;
    private int[] x, y, mapNo;


    private Texture2D[] TBlast;
    private SpriteBatch sprite;
    public Unit(GraphicsDevice g, SpriteBatch _sprite)
    {
        TBlast = new Texture2D[10];
        for (int i = 0; i < 8; i++)
        {
            using (Stream stream = File.OpenRead("T" + i + ".png"))
            {
                TBlast[i] = Texture2D.FromStream(g, stream);

            }
        }
        sprite = _sprite;

        x = new int[BLASTSU];
        y = new int[BLASTSU];
        mapNo = new int[BLASTSU];

        for (int i = 0; i < BLASTSU; i++)
        {
            mapNo[i] = -1;
        }
    }
    public void setBlast(int _x, int _y)
    {
        for (int i = 0; i < BLASTSU; i++)
        {
            if (mapNo[i] == -1)
            {
                x[i] = _x;
                y[i] = _y;
                mapNo[i] = 0;
                break;
            }
        }
    }
    int cnt = 0;
    public void animNoUpdate()
    {
        cnt++;
        if (cnt < 5) return;
        cnt = 0;
        for (int i = 0; i < BLASTSU; i++)
        {
            if (mapNo[i] != -1)
            {
                mapNo[i]++;
                if (mapNo[i] >= 7) mapNo[i] = -1;
            }
        }
    }
    public void paintBlast()
    {
        Vector2 pos;
        for (int i = 0; i < BLASTSU; i++)
        {

            if (mapNo[i] != -1)
            {
                pos.X = x[i];
                pos.Y = y[i];
                sprite.Draw(TBlast[mapNo[i]], pos, Color.White);
            }


        }
        animNoUpdate();
    }

}
class CPU
{
    const int BLASTSU = 10;
    public int[] x, y, mapNo, x1, y1, hp, lv;

    int[] gname;

    Vector2[] pos;
    public ATch at;
    public Blast blast;
    private Texture2D[] TBlast;
    private SpriteBatch sprite;
    int[,] map;
    public int[] swh;
    public CPU(GraphicsDevice g, SpriteBatch _sprite, int[,] _m)
    {
        at = new ATch();
        pos = new Vector2[BLASTSU];
        x = new int[BLASTSU];
        y = new int[BLASTSU];
        x1 = new int[BLASTSU];
        y1 = new int[BLASTSU];
        swh = new int[BLASTSU];
        hp = new int[BLASTSU];
        mapNo = new int[BLASTSU];
        lv = new int[BLASTSU];
        map = _m;

        TBlast = new Texture2D[32];
        gname = new int[BLASTSU];
        for (int i = 0; i < BLASTSU; i++)
        {
            gname[i] = 0;
            pos[i].X = 0;
            pos[i].Y = 80;
            x1[i] = 0;
            y1[i] = 0;
            swh[i] = 0;
            mapNo[i] = -1;
            hp[i] = 500;
            lv[i] = 99;
        }

        for (int i = 0; i < 32; i++)
        {
            using (Stream stream = File.OpenRead("Tr" + i + ".png"))
            {
                TBlast[i] = Texture2D.FromStream(g, stream);

            }
        }
        sprite = _sprite;

        //blast = new Blast(Ttin.Gm.GraphicsDevice, sprite);


    }
    public void setCPU(int _x, int _y)
    {
        for (int i = 0; i < BLASTSU; i++)
        {
            if (mapNo[i] == -1)
            {
                x[i] = _x;
                y[i] = _y;
                mapNo[i] = 0;
                break;
            }
        }
    }
    int cnt = 0;
    public void animNoUpdate()
    {
        cnt++;
        if (cnt < 5) return;
        cnt = 0;
        for (int i = 0; i < BLASTSU; i++)
        {
            if (mapNo[i] != -1)
            {
                mapNo[i]++;
                if (mapNo[i] >= 7) mapNo[i] = 0;
            }
        }
    }
    public void paintCPU()
    {

        for (int i = 0; i < BLASTSU; i++)
        {

            if (mapNo[i] != -1)
            {
                //if(at.mapch((int)pos[i].X,(int)pos[i].Y,map ,lv[i] ) ==0);
                //if (at.atcmap((int)(pos[i].X + x1[i]*2), (int)(pos[i].Y + y1[i]*2), 24, 24, map))
                //if (at.atlect((int)(pos[i].X + x1[i]), (int)(pos[i].Y + y1[i]),map))
                //{
                swh[i] = at.mapch((int)pos[i].X, (int)pos[i].Y, map);
                //Debug.WriteLine(i+": "+swh[i]);

                switch (at.mapch((int)pos[i].X, (int)pos[i].Y, map))
                {
                    case 0:
                        gname[i] = 0;
                        pos[i].X = -30;
                        pos[i].Y = 80;
                        x1[i] = 1;
                        y1[i] = 0;
                        mapNo[i] = -1;
                        hp[i] = 500;
                        break;
                    case 1:
                        gname[i] = 0;
                        x1[i] = 1;
                        y1[i] = 0;
                        pos[i].X += x1[i];
                        pos[i].Y += y1[i];
                        break;

                    case 2:
                        gname[i] = 8;
                        x1[i] = 0;
                        y1[i] = 1;
                        pos[i].X += x1[i];
                        pos[i].Y += y1[i];
                        break;
                    case 3:
                        gname[i] = 16;
                        x1[i] = -1;
                        y1[i] = 0;
                        pos[i].X += x1[i];
                        pos[i].Y += y1[i];
                        break;
                    case 4:
                        gname[i] = 24;
                        x1[i] = 0;
                        y1[i] = -1;
                        pos[i].X += x1[i];
                        pos[i].Y += y1[i];
                        break;

                    case -1:
                        pos[i].X += x1[i];
                        pos[i].Y += y1[i];
                        break;
                }

                if (mapNo[i] != -1)
                    sprite.Draw(TBlast[mapNo[i] + gname[i]], pos[i], Color.White);

                if (hp[i] <= 0)
                {
                    gname[i] = 0;
                    pos[i].X = 40;
                    pos[i].Y = 40;
                    x1[i] = 1;
                    y1[i] = 0;
                    mapNo[i] = -1;
                    hp[i] = 500;

                }
            }


        }
        animNoUpdate();
    }

}
class Blast
{
    const int BLASTSU = 100;
    public int[] x, y, mapNo;


    private Texture2D[] TBlast;
    private SpriteBatch sprite;
    public Blast(GraphicsDevice g, SpriteBatch _sprite)
    {
        TBlast = new Texture2D[10];
        for (int i = 0; i < 10; i++)
        {
            using (Stream stream = File.OpenRead("B" + i + ".png"))
            {
                TBlast[i] = Texture2D.FromStream(g, stream);

            }
        }
        sprite = _sprite;

        x = new int[BLASTSU];
        y = new int[BLASTSU];
        mapNo = new int[BLASTSU];

        for (int i = 0; i < BLASTSU; i++)
        {
            mapNo[i] = -1;
        }
    }
    public void setBlast(int _x, int _y)
    {
        for (int i = 0; i < BLASTSU; i++)
        {
            if (mapNo[i] == -1)
            {
                x[i] = _x;
                y[i] = _y;
                mapNo[i] = 0;
                break;
            }
        }
    }
    int cnt = 0;
    public void animNoUpdate()
    {
        cnt++;
        if (cnt < 5) return;
        cnt = 0;
        for (int i = 0; i < BLASTSU; i++)
        {
            if (mapNo[i] != -1)
            {
                mapNo[i]++;
                if (mapNo[i] >= 9) mapNo[i] = -1;
            }
        }
    }
    public void paintBlast()
    {
        Vector2 pos;
        for (int i = 0; i < BLASTSU; i++)
        {

            if (mapNo[i] != -1)
            {
                pos.X = x[i];
                pos.Y = y[i];
                sprite.Draw(TBlast[mapNo[i]], pos, Color.White);
            }


        }
        animNoUpdate();
    }

}
class createmap
{
    //private int[,] maptable;
    private GraphicsDevice g;
    public SpriteBatch s;
    private Texture2D map;

    public createmap(GraphicsDevice _g, SpriteBatch _s)
    {
        g = _g;
        s = _s;
        FileStream stream = File.OpenRead("mapTin.png");
        map = Texture2D.FromStream(_g, stream);

    }
    public void paintmap(int[,] maptable)
    {

        Vector2 pos;
        Rectangle rect;
        int w, mlow, mcol;
        pos.X = 0;
        pos.Y = 0;

        for (int i = 0; i < maptable.GetLength(0); i++)
        {
            for (int j = 0; j < maptable.GetLength(1); j++)
            {
                pos.X = j * 40;
                w = maptable[i, j];
                mlow = w / 16;
                mcol = w % 16;
                rect.X = mcol * 40;
                rect.Y = mlow * 40;
                rect.Width = 40;
                rect.Height = 40;
                s.Draw(map, pos, rect, Color.White);
            }
            pos.Y += 40;
        }
    }
    //public void setmap()
    //{
    //       maptable = new int[,] { 
    //    {17,17,17,17, 4, 4, 4,17,17,17,17,17},
    //    {17,17,17,17,20,21,20,21,17,17,17,17},
    //    {17,17,18,19,36,37,36,37,17,17,17,17},
    //    {17,17,34,35,17,17,17,17,17,17,17,17},
    //    {17,17,17,17,17,18,17,17,17,17,17,17},
    //    {17,17,17,17,17,72,73,17,17,17,17,17},
    //    {17,17,17,17,17,88,89,17,17,17,17,17},
    //    {17,17,18,19,17,17,17,70,71,17,17,17},
    //    {17,17,34,35,17,17,17,86,87,17,17,17},
    //    {17,17,17,17,17,17,17,17,17,17,17,17},
    //    {17,17,17,17,17,17,17,17,17,17,17,17},
    //    {17,17,17,17,17,17,17,17,17,17,17,17}
    //    };
    //    g.Clear(Color.White);
    //    Vector2 pos;
    //    Rectangle rect;
    //    int w,mlow, mcol;
    //    pos.X = 0;
    //    pos.Y = 0;

    //    for (int i = 0; i < 12; i++)
    //    {
    //        for (int j = 0; j < 12; j++) {
    //            pos.X = j * 40;
    //            w = maptable[i, j];
    //            mlow = w / 16;
    //            mcol = w % 16;
    //            rect.X = mcol * 40;
    //            rect.Y = mlow * 40;
    //            rect.Width = 40;
    //            rect.Height = 40;
    //            s.Draw(Tgazo, pos, rect , Color.White);
    //        }
    //        pos.Y += 40;
    //    }
    //}
}