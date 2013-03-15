using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
namespace CasseBriques
{
    class Balle : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private const int NBLIGNES = 5; 
        private const int NBCOLONNES = 8;
        SpriteBatch spriteBatch;
        private int maxX;
        private int maxY;
        private int minX;
        private int minY;
        private int nbreballes;

        public int Nbreballes
        {
            get { return nbreballes;}
            set { nbreballes = value; }
        }
        // On définit la position de départ de la balle 
        // et sa position courante, champ qui sera accessible depuis un objet 
        private Vector2 position_depart;
       // on définit la taille de la raquette 
        private const int TAILLEX = 9;
        private const int TAILLEY = 12;

        private Vector2 v_min;
        private Vector2 v_max;
        private Vector2 vitesse_initiale = Vector2.Zero;
        

        // Informations sur les briques 
        private Brique[,] mesBriquesballe;

        public Brique[,] MesBriquesballe
        {
            get { return mesBriquesballe; }
            set { mesBriquesballe = value; }
        } 

        private ObjetAnime uneballe;
        // On encapsule le champ pour récupérer sa position relative
        // dans le moteur 2D 
        public ObjetAnime Uneballe
         {
           get { return uneballe; }
           set { uneballe = value; }
         }
        private SoundEffect soundRaquette;
        private SoundEffect soundMur;

        private BoundingBox bbox;

        public BoundingBox Bbox
        {
            get { return bbox; }
           
        }

        private Raquette raquette;

        public  Raquette Raquette
        {
            get { return raquette; }
            set { raquette = value; }
        }

        private Joueur joueur;

        public Joueur Joueur
        {
            get { return joueur; }
            set { joueur = value; }
        }

        


        public Balle(Game game, int th, int tv)
            : base(game)
        {
            /// On récupère la taille de sortie de l'écran 
           
            maxX = th;
            maxY = tv;
            
            this.Game.Components.Add(this);
           

        }

        public override void Initialize()
        {
            // On définit une vitesse initiale minimale 
            v_min = new Vector2(2, -2);
            // on fixe une vitesse maximale
            v_max = new Vector2(7, 8);
            this.vitesse_initiale = v_min;
            // On place la balle au centre de l'écran au dessus de la raquette 
            this.position_depart = new Vector2((maxX / 2) - 10, (maxY - 50));
            minX = 0;
            minY = 0;
            //On fixe le nombre de balles
            this.nbreballes = 3;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            uneballe  = new ObjetAnime(Game.Content.Load<Texture2D>(@"mesimages\balle"), position_depart, new Vector2(TAILLEX, TAILLEY), vitesse_initiale);
            soundRaquette = Game.Content.Load<SoundEffect>(@"sounds\rebond-raquette");
            soundMur = Game.Content.Load<SoundEffect>(@"sounds\rebond-terre_battue");
            // on met à jour la Bounding Box
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(uneballe.Texture, uneballe.Position, Color.Azure);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            bougeBalle();
            base.Update(gameTime);
        }

        // Test la collision avec les briques 
        private void gestionCollisionBrique()
        {
            //fonction qui teste la collision entre une Brique et la Balle, elle gère les mouvements de la balle suite à cette collision
            BoundingBox bbox_brique;
           Brique unebrique;
            Vector2 v;
            Boolean collision = false;
            float[] infosBalle = { uneballe.Position.X, uneballe.Position.Y, TAILLEX, TAILLEY };
            int[] posRel;
            int x = 0;
            int y = 0;
            int tempx=0, tempy=0;
            v = uneballe.Vitesse;
            // on teste une collision avec une éventuelle brique
            while (x < NBLIGNES && !collision)
            {
                y = 0;
                while (y < NBCOLONNES  && !collision)
                {
                    unebrique = mesBriquesballe[x, y];
                    // On définit uen enveloppe pour la brique
                    bbox_brique = new BoundingBox(new Vector3(unebrique.Position.X, unebrique.Position.Y, 0),
                                 new Vector3(unebrique.Position.X + unebrique.Size.X, unebrique.Position.Y + unebrique.Size.Y, 0));
                    
                   
                    // on mémorise l'enveloppe
                    if (!unebrique.Marque && Moteur2D.testCollision(this,bbox_brique))
                    {
                        // Le prochain mouvement entraîne une collision, on évalue la position relative de la balle
                        // par rapport à la raquette pour mettre à jour le vecteur vitesse
                        float[] infosBrique = { unebrique.Position.X, unebrique.Position.Y, unebrique.Size.X, unebrique.Size.Y };
                        posRel = Moteur2D.getRelativePosition(infosBalle, infosBrique);

                        
                        // Si les 2 objets se croisent sur l'axe des Y
                        if ((posRel[1] == Moteur2D.EN_DESSOUS) || (posRel[1] == Moteur2D.AU_DESSUS))
                        {
                            v.Y *= -1;


                            if ((Math.Abs(v.Y) < v_max.Y) && (Math.Abs(v.X) < v_max.X))
                            {
                                v.Y *= 1.1f;
                                v.X *= 1.1f;
                            }
                            uneballe.Vitesse = v;
                        }
                        else
                        {
                            // Si les 2 objets se croisent sur l'axe des X
                            if (((posRel[0] == Moteur2D.A_DROITE) || (posRel[0] == Moteur2D.A_GAUCHE)))
                            {
                                v.X *= -1;
                                if ((Math.Abs(v.Y) < v_max.Y) && (Math.Abs(v.X) < v_max.X))
                                {
                                    v.Y *= 1.1f;
                                    v.X *= 1.1f;
                                }
                                uneballe.Vitesse = v;
                            }
                        }
                        SoundEffectInstance soundInstRaquette = soundRaquette.CreateInstance();
                        soundInstRaquette.Volume = 0.8f;
                        soundInstRaquette.Play();
                        collision = true;
                        tempx = x;
                        tempy = y;
                    }
                    y++;
                }
                x++;
            }
            if (collision)
            {
                mesBriquesballe[tempx, tempy].Marque = true; //la brique est cassée

                joueur.updateScore(100);
                joueur.updateCombo(true);
                if (joueur.CompteurCombo <= 2)
                {
                    joueur.updateScore(50 * (joueur.CompteurCombo));
                }
                
                
            }
            
        }
                

           


        
        private void gestionCollision()
        {
            // Test la collision  entre la balle et les murs et la balle et la raquette et elle modifie le vecteur vitesse en fonction
            Vector2 v;
            // Test de collision
            float[] infosBalle = { uneballe.Position.X, uneballe.Position.Y, TAILLEX,TAILLEY };
            int[] posRel;
            int[] posRel_centre;
            double theta;
            double pourcentage;
            double norme;

            // avec les raquettes
            // On récupère la vitesse courante
            v = uneballe.Vitesse;
            if (Moteur2D.testCollision(this, this.raquette.Bbox))
            {
                // Le prochain mouvement entraîne une collision, on évalue la position relative de la balle
                // par rapport à la raquette pour mettre à jour le vecteur vitesse
                float[] infosRaquette = { raquette.Uneraquette.Position.X, raquette.Uneraquette.Position.Y, raquette.Uneraquette.Size.X, raquette.Uneraquette.Size.Y };
                posRel = Moteur2D.getRelativePosition(infosBalle, infosRaquette);
                float[] infoscentreRaquette={(raquette.Uneraquette.Position.X+(raquette.Uneraquette.Size.X)/2),(raquette.Uneraquette.Position.Y+(raquette.Uneraquette.Size.Y)/2),0,0};
                posRel_centre=Moteur2D.getRelativePosition(infosBalle, infoscentreRaquette);

                // Si les 2 objets se croisent sur l'axe des Y
                if (posRel[1] == Moteur2D.AU_DESSUS)
                {
                    
                    pourcentage =Math.Abs((infosBalle[0]+(infosBalle[2]/2) - infoscentreRaquette[0])/(raquette.Uneraquette.Size.X/2));
                    theta = (pourcentage * (Math.PI / 5))+(((Math.PI / 5)-(pourcentage * (Math.PI / 5)))*(5/2));
                    norme = Math.Sqrt((v.X*v.X) + (v.Y*v.Y));
                    if (posRel_centre[0] == Moteur2D.A_GAUCHE)
                    {
                        v.Y = (float)-(norme*Math.Sin(theta));
                        v.X = (float)-(norme*Math.Cos(theta));
                        
                    }
                    else 
                    {
                        v.Y = (float)-(norme*Math.Sin(theta));
                        v.X = (float)(norme * Math.Cos(theta));
                    }

                    // if (Math.Abs(v.Y) < v_max.Y)
                    //    v.Y *= 1.1f;
                    uneballe.Vitesse = v;
                }
                else
                {
                    if (posRel[1] == Moteur2D.EN_DESSOUS)
                    {
                        v.Y *= -1;

                        // if (Math.Abs(v.Y) < v_max.Y)
                        //    v.Y *= 1.1f;
                        uneballe.Vitesse = v;
                    }
                    else
                    {
                        // Si les 2 objets se croisent sur l'axe des X
                        if ((posRel[0] == Moteur2D.A_DROITE) || (posRel[0] == Moteur2D.A_GAUCHE))
                        {
                            v.X *= -1;
                            v.Y *= -1;
                            // if (Math.Abs(v.X) < v_max.X)
                            //    v.X *= 1.1f;
                            uneballe.Vitesse = v;
                        }
                    }
                }

                SoundEffectInstance soundInstRaquette = soundRaquette.CreateInstance();
                soundInstRaquette.Volume = 0.8f;
                soundInstRaquette.Play();
                joueur.reinitialiserCombo();
            }
           
            else
            {
                // avec les murs
                bool collision_murs = false;

                // collision avec le mur gauche
                if (uneballe.Position.X + uneballe.Size.X <= minX)
                {
                    v.X *= -1;
                    uneballe.Vitesse = v;
                    collision_murs = true;
                }
                // collision avec le mur droit
                if (uneballe.Position.X >= maxX)
                {
                    v.X *= - 1;
                    uneballe.Vitesse = v;
                    collision_murs = true;
                }
                // On passe au dessus du nur 
                if (uneballe.Position.Y + uneballe.Size.Y <= minY )
                {
                    v.Y *= -1;
                    collision_murs = true;
                    uneballe.Vitesse = v;
                }

                // on perd la balle 
                if (uneballe.Position.Y >= maxY)
                {
                     v.Y*=-1;
                    collision_murs = true;
                    uneballe.Vitesse = v;
                    this.nbreballes--;

                }

                if (collision_murs)
                {
                    SoundEffectInstance soundInstMur = soundMur.CreateInstance();
                    soundInstMur.Volume = 0.6f;
                    soundInstMur.Play();
                    joueur.updateCombo(false);
                }
            }
        }
        
        private void bougeBalle()
        {
            // on met à jour la Bounding Box
            this.bbox = new BoundingBox(new Vector3(uneballe.Position.X, uneballe.Position.Y, 0),
                new Vector3(uneballe.Position.X + TAILLEX, uneballe.Position.Y + TAILLEY, 0));
          
            // Test la collision et modifie le vecteur vitesse en fonction
            gestionCollision();
            gestionCollisionBrique(); 
            uneballe.Position += uneballe.Vitesse;
        }
         
    }
}
