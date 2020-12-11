using System;

namespace Movies.Models
{
    public class Like<T>
    {
        public int UserId { get; set; }
        public int ObjectId { get; set; }

        public int Mark
        {
            get => Mark;
            set
            {
                if (value < 0 || value > 10)
                {
                    throw new ArgumentOutOfRangeException();
                }

                Mark = value;
            }
        }
    }
}