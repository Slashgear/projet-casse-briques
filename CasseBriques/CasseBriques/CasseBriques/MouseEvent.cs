using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace CasseBriques
{
    public class MouseEvent
    {
        MouseState buttonPress;
        Rectangle mousedetection;

        public MouseEvent()
        {
        }
        public bool UpdateMouse()
        {
            buttonPress = Mouse.GetState();
            if (buttonPress.LeftButton == ButtonState.Pressed)
            { return true; }
            else { return false; }
        }
        public Rectangle GetMouseContainer()
        {
            mousedetection = new Rectangle((int)buttonPress.X, (int)buttonPress.Y, (int)1, (int)1);
            return mousedetection;
        }
    }

}
