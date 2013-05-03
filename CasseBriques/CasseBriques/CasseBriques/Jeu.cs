using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace CasseBriques
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Jeu : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;

        private SpriteBatch spriteBatch;
        private const int TAILLEH = 1024;
        private const int TAILLEV = 680;
        private const int TAILLEBRIQUEX = 119;
        private const int TAILLEBRIQUEY = 50;
        private const int NBLIGNES = 5;
        private const int NBBRIQUES = 8;
        // ondéfinit un tableau pour mémoriser la position des briques et 
        // la définition d'une enveloppe pour les collisions
        private Brique[,] mesBriques = new Brique[NBLIGNES, NBBRIQUES];
        
        // Déclaration des objets brique raquette balle
        private ObjetAnime briquegrise, briquebleue, briqueorange, briquepoint, briquerouge,briqueviolet/*, balle, raquette*/;
        private SpriteFont textFont;
        private Texture2D unebriquenoire;
        private Balle uneballe;
        //création du menu fail
        private MenuButton boutonplay;
        private MenuButton boutonoptions;
        private MenuButton boutonexit;
        private MouseEvent mouseEvent;

        //declaration d'un joueur
        private Joueur unjoueur;

        enum GameState
        {
            MainMenu,
            Play,
            Options,
       
        }
        GameState CurrentGameState = GameState.MainMenu;

        public Jeu()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>

        protected void Initialize2()
        {

            int offsetX = 40;
            int offsetY = 40;
            // TODO: Add your initialization logic here
            uneballe = new Balle(this, TAILLEH, TAILLEV);

            //Raquette raquette = new Raquette(this, TAILLEH ,TAILLEV );
            unjoueur = new Joueur(this, 1, 0);

            Raquette raquette = unjoueur.Raquette;


            // Les objets raquette et balle doivent se connaître 
            // On passe la raquette à la balle 
            uneballe.Raquette = raquette;
            // on passe la balle à la raquette 
            raquette.Balle = uneballe;

            //on passe le joueur a la balle pour gérer le score
            uneballe.Joueur = unjoueur;

            // On passe à la balle le tableau de briques
            int xpos, ypos;
            for (int x = 0; x < NBLIGNES; x++)
            {
                ypos = offsetY + x * TAILLEBRIQUEY;
                for (int y = 0; y < NBBRIQUES; y++)
                {
                    xpos = offsetX + y * TAILLEBRIQUEX;

                    Vector2 pos = new Vector2(xpos, ypos);
                    // On mémorise les positions de la brique
                    mesBriques[x, y] = new Brique(pos, new Vector2(TAILLEBRIQUEX, TAILLEBRIQUEY));
                }
            }

            uneballe.MesBriquesballe = mesBriques;
            
        }

        protected override void Initialize()
        {
            
            
            boutonplay = new MenuButton(new Vector2(450,230), Content.Load<Texture2D>(@"mesimages\play"), new Rectangle(450, 350, 150, 60));
            boutonoptions = new MenuButton(new Vector2(450, 300), Content.Load<Texture2D>(@"mesimages\options"), new Rectangle(450, 420, 150, 60));
            boutonexit = new MenuButton(new Vector2(450, 370), Content.Load<Texture2D>(@"mesimages\exit"), new Rectangle(450, 490, 150, 60));

            mouseEvent = new MouseEvent();

        
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // On initialise la taille de l'écran de sortie
            graphics.PreferredBackBufferWidth= TAILLEH;
            graphics.PreferredBackBufferHeight = TAILLEV;
            graphics.ApplyChanges();

            // TODO: use this.Content to load your game content here

            // On initialise les différents objets du jeu
            /// lignes de briques
            /// la balle
            /// la raquette
            /// 
            briquegrise = new ObjetAnime(Content.Load<Texture2D>(@"mesimages\briquegrise"), new Vector2(0f, 0f), new Vector2(TAILLEBRIQUEX, TAILLEBRIQUEY), Vector2.Zero);
            briquebleue = new ObjetAnime(Content.Load<Texture2D>(@"mesimages\briquebleue"), new Vector2(0f, 0f), new Vector2(TAILLEBRIQUEX, TAILLEBRIQUEY), Vector2.Zero);
            briqueorange = new ObjetAnime(Content.Load<Texture2D>(@"mesimages\briqueorange"), new Vector2(0f, 0f), new Vector2(TAILLEBRIQUEX, TAILLEBRIQUEY), Vector2.Zero);
            briquepoint = new ObjetAnime(Content.Load<Texture2D>(@"mesimages\briquepoint"), new Vector2(0f, 0f), new Vector2(TAILLEBRIQUEX, TAILLEBRIQUEY), Vector2.Zero);
            briquerouge = new ObjetAnime(Content.Load<Texture2D>(@"mesimages\briquerouge"), new Vector2(0f, 0f), new Vector2(TAILLEBRIQUEX, TAILLEBRIQUEY), Vector2.Zero);
            briqueviolet = new ObjetAnime(Content.Load<Texture2D>(@"mesimages\briqueviolet"), new Vector2(0f, 0f), new Vector2(TAILLEBRIQUEX, TAILLEBRIQUEY), Vector2.Zero);
            unebriquenoire = Content.Load<Texture2D>(@"mesimages\briquenoire");
            // On charge la police
            
            this.textFont = Content.Load<SpriteFont>(@"font\MyFont");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            // TODO: Add your update logic here
            //System.Diagnostics.Debug.WriteLine(mouseEvent.GetMouseContainer.getX() + mouseEvent.GetMouseContainer.getY());
            switch (CurrentGameState)
            {
                case GameState.MainMenu:
               

            if (mouseEvent.GetMouseContainer().Intersects(boutonplay.getContainer()))
            {
                if (mouseEvent.UpdateMouse() == true)
                {
                    CurrentGameState = GameState.Play;
                    Initialize2();
                }
            }
            if (mouseEvent.GetMouseContainer().Intersects(boutonoptions.getContainer()))
            {
                if (mouseEvent.UpdateMouse() == true)
                {

                }
            }
            if (mouseEvent.GetMouseContainer().Intersects(boutonexit.getContainer()))
            {
                if (mouseEvent.UpdateMouse() == true)
                {
                    this.Exit();
                }
            }
                    break;

                case GameState.Options:

                    break;

                case GameState.Play:
                if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.P))
                    {
                        CurrentGameState = GameState.MainMenu;
                        Initialize();
                    }
                    break;
            }
            /*if (mouseEvent.GetMouseContainer().Intersects(boutonplay.getContainer()))
            {
                if (mouseEvent.UpdateMouse() == true)
                {
                    
                }
            }
            if (mouseEvent.GetMouseContainer().Intersects(boutonoptions.getContainer()))
            {
                if (mouseEvent.UpdateMouse() == true)
                {

                }
            }
            if (mouseEvent.GetMouseContainer().Intersects(boutonexit.getContainer()))
            {
                if (mouseEvent.UpdateMouse() == true)
                {
                    this.Exit();
                }
            }
            */
          
            base.Update(gameTime);
        }

       
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
           
            Vector2 pos ;
            GraphicsDevice.Clear(Color.Black);
            this.IsMouseVisible = true;

            // TODO: Add your drawing code here
             spriteBatch.Begin();
             switch (CurrentGameState)
             {
                 case GameState.MainMenu:
             
             boutonplay.DrawButton(spriteBatch);
             boutonoptions.DrawButton(spriteBatch);
             boutonexit.DrawButton(spriteBatch);
                    break;

                 case GameState.Play:

             string afficheNbBalles = string.Format("Balles restantes: {0}", uneballe.Nbreballes);
             spriteBatch.DrawString(this.textFont, afficheNbBalles, new Vector2((TAILLEH - 180), 5), Color.White);
             string afficheScore = string.Format("Score:{0}", unjoueur.ScoreJoueur);
             spriteBatch.DrawString(this.textFont, afficheScore, new Vector2(10, 5), Color.White);

            // Boucle permettant de dessiner les briques des murs
           
             if (uneballe.Nbreballes == 0)
             {
                 spriteBatch.DrawString(this.textFont, "Game Over ... ! Vous avez épuisé toutes vos balles", new Vector2(13 * 20, 18 * 20), Color.Yellow);
                 if (Controls.CheckActionSpace())
                 {
                     //System.Threading.Thread.Sleep(10000);
                     this.Exit();
                 }
             }
             for (int x = 0; x < NBLIGNES; x++)
             {
                 for (int y = 0; y < NBBRIQUES; y++)
                 {

                     pos = mesBriques[x, y].Position;
                     if (!mesBriques[x, y].Marque)
                         switch (x)
                         {
                             case 0: spriteBatch.Draw(briquepoint.Texture, pos, Color.Azure); break;
                             case 1: spriteBatch.Draw(briquegrise.Texture, pos, Color.Gray); break;
                             case 2: spriteBatch.Draw(briquerouge.Texture, pos, Color.Red); break;
                             case 3: spriteBatch.Draw(briqueorange.Texture, pos, Color.Orange); break;
                             case 4: spriteBatch.Draw(briqueviolet.Texture, pos, Color.Violet); break;
                         }
                     else
                     {
                         spriteBatch.Draw(unebriquenoire, pos, Color.Black);
                     }


                 }
             }

                     break;

                 case GameState.Options:

                     break;

             }
             /*boutonplay.DrawButton(spriteBatch);
             boutonoptions.DrawButton(spriteBatch);
             boutonexit.DrawButton(spriteBatch);
             string afficheNbBalles = string.Format("Balles restantes: {0}", uneballe.Nbreballes);
             spriteBatch.DrawString(this.textFont, afficheNbBalles, new Vector2((TAILLEH - 180), 5), Color.White);

             string afficheScore = string.Format("Score:{0}", unjoueur.ScoreJoueur);
             spriteBatch.DrawString(this.textFont, afficheScore, new Vector2(10, 5), Color.White);

            // Boucle permettant de dessiner les briques des murs
           
             if (uneballe.Nbreballes == 0)
             {
                 spriteBatch.DrawString(this.textFont, "Game Over ... ! Vous avez épuisé toutes vos balles", new Vector2(13 * 20, 18 * 20), Color.Yellow);
                 if (Controls.CheckActionSpace())
                 {
                     //System.Threading.Thread.Sleep(10000);
                     this.Exit();
                 }
             }
            for (int x = 0; x < NBLIGNES; x++)
            {
                for (int y = 0; y < NBBRIQUES; y++)
                {
                    
                    pos = mesBriques[x, y].Position;
                    if (! mesBriques[x, y].Marque)
                        switch (x)
                        {
                            case 0: spriteBatch.Draw(briquepoint.Texture, pos, Color.Azure); break;
                            case 1: spriteBatch.Draw(briquegrise.Texture, pos, Color.Gray); break;
                            case 2: spriteBatch.Draw(briquerouge.Texture, pos, Color.Red); break;
                            case 3: spriteBatch.Draw(briqueorange.Texture, pos, Color.Orange); break;
                            case 4: spriteBatch.Draw(briqueviolet.Texture, pos, Color.Violet); break;
                        }
                    else
                    {
                        spriteBatch.Draw(unebriquenoire, pos, Color.Black); 
                    }

                    
                }
            }*/         
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
