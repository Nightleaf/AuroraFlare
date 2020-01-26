using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraFlare.Utilities
{
    class Settings
    {
        /**
         * Developer Settings
         * 
         **/ 
        public static bool Debugging = true;
        public static bool ShouldExit = false;
        public static bool GameIsActive = true;

        /**
         *  Profile Settings
         **/
        public static String PROFILE_TO_USE = ""; 
        public const String ProfileDirectory = @"./Profiles/";
        public const float STARTING_HEALTH = 250F;
        public const float STARTING_SHIELDS = 75F;


        /**
         *  Projectile Settings
         **/
        public static float BaseProjectileSpeed = 650f;
        public static float ShotThrottleLimit = 10.0f;

        /**
         *  Game Settings
         **/
        public static Boolean IsGamePaused = false;

        /**
         * Boosts
         **/
        public const int DAMAGE_BOOST = 0;
        public const int HEALTH_BOOST = 1;
        public const int SHIELD_BOOST = 2;
        public const int ACCELERATION_BOOST = 3;
   
        /**
         * Mouse Settings
         * 
         **/
        public static bool ShowMouse = true;

        /**
         * Window/Screen Configuration
         * 
         **/
        public static int ScreenResolutionHeight = 720;
        public static int ScreenResolutionWidth = 1280;

        /**
         * Audio/Volume Settings
         * 
         **/
        public static Boolean IsSoundEnabled = true;
        public static Boolean IsMusicEnabled = true;
        public static float VOLUME_MUSIC = 1.0f;
        public static float VOLUME_SFX = 1.0f;
    }
}

