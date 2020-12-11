using System;

namespace Movies.Models
{
    public class ActorMark
    {
        public int ActorId { get; set; }
        public Actor Actor { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        
        private int _mark;
        public int Mark
        {
            get => _mark;
            set
            {
                if (value < 0 || value > 13)
                {
                    throw new ArgumentOutOfRangeException();
                }

                _mark = value;
            }
        }
    }
}