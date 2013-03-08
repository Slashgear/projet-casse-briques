using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace CasseBriques
{
    public class MenuButton
    {
        Vector2 position;
        Texture2D texture;
        Rectangle rectangle;

        public Rectangle container
        {
            get { return rectangle; }
            set { rectangle = value; }
        }
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }
/*absence temporaire de constructeur vide*/
        public MenuButton(Vector2 position, Texture2D texture, Rectangle container)
        {
            this.position = position;
            this.container = container;
            this.texture = texture;
        }
        public Rectangle getContainer()
        {
            container = new Rectangle((int)position.X, (int)position.Y, ((int)texture.Width), ((int)texture.Height));
            return container;
        }
        public  void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }     
    }
}
