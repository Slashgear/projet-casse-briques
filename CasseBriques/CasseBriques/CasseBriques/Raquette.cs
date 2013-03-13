using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CasseBriques
{
    class Raquette : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        //private Keys keyUp;
       // private Keys keyDown;
        private ObjetAnime uneraquette;
        // On encapsule le champ pour récupérer sa position relative
        // dans le moteur 2D 
        public ObjetAnime Uneraquette
        {
            get { return uneraquette; }
            set { uneraquette = value; }
        }
        private int maxX;
        private int maxY;
        private int minX=0;
        // on définit la taille de la raquette 
        private const int TAILLEX = 90;
        private const int TAILLEY = 18;      
        private const int VITESSE_RAQUETTE = 8;
        private Vector2 position_depart;
        private BoundingBox bbox;
        // on encapsule le champ pour la gestion des collisions
        public BoundingBox Bbox
        {
            get { return bbox; }
            set { bbox = value; }
        }
        private Balle balle;

        internal Balle Balle
        {
            get { return balle; }
            set { balle = value; }
        }

        public Raquette(Game game, int th, int tv)
            : base(game)
        {
           
            maxX = th;
            maxY = tv;
            this.Game.Components.Add(this);
        }

        public override void Initialize()
        {
            // On définit une position de départ
            this.position_depart = new Vector2((maxX - TAILLEX) / 2, maxY - TAILLEY - 20);
            minX = 0; 
            base.Initialize();  
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            uneraquette = new ObjetAnime(Game.Content.Load<Texture2D>(@"mesimages\raquette"), position_depart, new Vector2(TAILLEX, TAILLEY), new Vector2(VITESSE_RAQUETTE,0));
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(uneraquette.Texture, uneraquette.Position, Color.Azure);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            // On met à jour la bounding box
           this.bbox = new BoundingBox(new Vector3(uneraquette.Position.X, uneraquette.Position.Y, 0),
                                      new Vector3(uneraquette.Position.X + uneraquette.Texture.Width, uneraquette.Position.Y + uneraquette.Texture.Height, 0));

            // La classe Controls contient les constantes correspondantes aux contrôles définies sur la plate-forme
            // et des méthodes, pour chaque action possible dans le jeu, qui vérifient si les contrôles correspondants
            // ont été "enclenchés"
            if (Controls.CheckActionDroite())
            {
                if (!Moteur2D.testCollision(this, this.balle.Bbox))
                {
                    // Est-ce qu'on est tout à droite  ?
                if (uneraquette.Position.X + uneraquette.Texture.Width < maxX)
                {                 
                    // On passe par un vecteur intermédiaire 
                    // pour initialiser la nouvelle position 
                  float   tempo = uneraquette.Position.X;
                    tempo+= uneraquette.Vitesse.X;
                    Vector2 pos = new Vector2(  tempo, uneraquette.Position.Y);
                    uneraquette.Position = pos;
                }
                        
              }
                //else Console.WriteLine("CheckActionDown (joueur" + joueur + ") --> collision ");
            }
            else if (Controls.CheckActionGauche())
            {
                if (!Moteur2D.testCollision(this, this.balle.Bbox))
                {
                    // Est-ce qu'on est tout à gauche ?
                    if (uneraquette.Position.X > minX)
                    {
                        // On passe par un vecteur intermédiaire 
                        // pour initialiser la nouvelle position 
                        float tempo = uneraquette.Position.X;
                        tempo -= uneraquette.Vitesse.X;
                        Vector2 pos = new Vector2(tempo, uneraquette.Position.Y);
                        uneraquette.Position = pos;
                    }
                   
       
                }
               // else Console.WriteLine("CheckActionUp (joueur" + joueur + ") --> collision ");
            }

            base.Update(gameTime);
        }
    }
}
