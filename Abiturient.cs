using System;

namespace Сестричка_парсер
{
    [Serializable]
    public class Abiturient
    {
        public Abiturient()
        {

        }
        public string obj { get; set; }
        public string New { get; set; }
        public int Num { get; set; }
        public string FIO { get; set; }
        public int AllBalls { get; set; }
        public string Doc { get; set; }
        public string SpecialRights { get; set; }
        public string TargetDirection { get; set; }
    }
}
