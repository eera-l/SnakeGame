using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;



namespace SnakeGame
{
    class Snake
    {
        private List<Rectangle> body = new List<Rectangle>();
        private Rectangle head;
        private int speed;

        public Snake(Rectangle head)
        {
           body.Add(head);
           this.head = body[0];
        }

        public List<Rectangle> Body
        {
            get {  return body;}
        }

        public Rectangle Head
        {
            get {  return head;}
        }
    }
}
