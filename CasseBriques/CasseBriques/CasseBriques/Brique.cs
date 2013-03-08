﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CasseBriques
{
   public  class Brique 
   {
       // On massocie à la brique une enveloppe pour la gestion d'une collision
        private BoundingBox bbox;
        private Vector2 position;
        private Vector2 size;
        private Boolean marque;

       
        public Vector2 Size
        {
            get { return size; }
            set { size = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        } 
        public BoundingBox Bbox
        {
            get { return bbox; }
            set { bbox = value; }
        }

        public Boolean Marque
        {
            get { return marque; }
            set { marque = value; }
        }
       public Brique ( Vector2 p, Vector2 s)
       {
           this.Position = p;
           this.size = s;
           this.marque = false;
           
       }
       
    }
}
