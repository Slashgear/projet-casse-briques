﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;


namespace CasseBriques
{
    /* 
     * Classe encapsulant la raquette d'un joueur donné
     * Elle gère aussi le score des joueurs
     */
    class Joueur
    {
        private int noJoueur;
        public int NoJoueur
        {
            get { return noJoueur; }
        }

        private Raquette raquette;
        public Raquette Raquette
        {
            get { return raquette; }
        }


        public const int SCORE_BUT = 10;
        private int scoreJoueur;
        public int ScoreJoueur
        {
            get { return scoreJoueur; }
        }

        private Vector2 posScoreJoueur;
        public Vector2 PosScoreJoueur
        {
            get { return posScoreJoueur; }
        }

        private int compteurCombo;
        public int CompteurCombo
        {
            get { return compteurCombo; }
            set { compteurCombo = value; }
        }


        public Joueur(Game game)
            : this(game, 1, 0)
        {
        }

        public Joueur(Game game, int noJoueur)
            : this(game, noJoueur, 0)
        {
        }

        public Joueur(Game game, int noJoueur, int scoreJoueur)
        {
            this.noJoueur = noJoueur;
            this.scoreJoueur = scoreJoueur;

            switch (noJoueur)
            {
                case 1:
                    this.raquette = new Raquette (game,1024, 680);
                    this.posScoreJoueur = new Vector2(50, 0);
                    break;
                case 2:
                    this.raquette = new Raquette(game,1024, 680);
                    this.posScoreJoueur = new Vector2(game.GraphicsDevice.Viewport.Width - 150, 0);
                    break;
            }
        }
        
        public void updateScore(int points)
        {
            scoreJoueur += points;
        }

        public void updateCombo()
        {
            compteurCombo++;
        }

        public void reinitialiserCombo()
        {
            compteurCombo = 0;
        }
    }
}
