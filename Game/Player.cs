using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameProject {
    static class Player {
        public const float MoveSpeed = 20;
        public static Vector2 Position;
        public static Rectangle Hitbox { get; private set; }

        public static void Update() {
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, 20, 20);

            var move = Vector2.Zero;
            if (Input.AnyKeyHeld(Keys.W, Keys.Up))
                move.Y = -MoveSpeed;
            if (Input.AnyKeyHeld(Keys.S, Keys.Down))
                move.Y += MoveSpeed;
            if (Input.AnyKeyHeld(Keys.A, Keys.Left))
                move.X = -MoveSpeed;
            if (Input.AnyKeyHeld(Keys.D, Keys.Right))
                move.X += MoveSpeed;
            move.Normalize();
            move *= MoveSpeed;

            if (!float.IsNaN(move.X)) {
                float addX = move.X * Time.Delta,
                    newX = Position.X + addX;
                for (int i = 0; i < GameScr.SolidHitboxes.Length; i++) {
                    var solid = GameScr.SolidHitboxes[i];
                    if (newX < solid.X + solid.Width && solid.X < newX + Hitbox.Width &&
                        Position.Y < solid.Y + solid.Height && solid.Y < Position.Y + Hitbox.Height) {
                        newX = addX > 0 ? solid.X : solid.X + solid.Width;
                    }
                }
                Position.X = newX;
            }

            if (!float.IsNaN(move.Y)) {
                float addY = move.Y * Time.Delta,
                    newY = Position.Y + addY;
                for (int i = 0; i < GameScr.SolidHitboxes.Length; i++) {
                    var solid = GameScr.SolidHitboxes[i];
                    if (Position.X < solid.X + solid.Width && solid.X < Position.X + Hitbox.Width &&
                        newY < solid.Y + solid.Height && solid.Y < newY + Hitbox.Height) {
                        newY = addY > 0 ? solid.Y : solid.Y + solid.Height;
                    }
                }
                Position.Y = newY;
            }
        }

        public static void Draw(SpriteBatch s) {
            s.FillRectangle(Hitbox, Color.Blue * .75f, 0,
                origin : new Vector2(Hitbox.Width * .5f, Hitbox.Height * .5f));
        }
    }
}