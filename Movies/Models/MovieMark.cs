using System;

namespace Movies.Models
{
    public class MovieMark
    {
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        private int _mark;
        public int Mark
        {
            get => _mark;
            set
            {
                if (value < 0 || value > 10)
                {
                    throw new ArgumentOutOfRangeException();
                }

                _mark = value;
            }
        }
    }
}