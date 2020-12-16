using System;
using Movies.Models.Authentication;

namespace Movies.Models.Marks
{
    public class BaseMark
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        
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