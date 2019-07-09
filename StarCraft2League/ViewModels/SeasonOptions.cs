using System;
using System.ComponentModel.DataAnnotations;

namespace StarCraft2League.ViewModels
{
    public class SeasonOptions
    {
        public const byte DefaultDaysBetweenRounds = 3;
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        public TimeSpan StartMomentTime { get; set; }
        [Range(0, 255)]
        public byte DaysBetweenRounds { get; set; }
        public TimeSpan TimeBetweenRounds { get; set; }
    }
}