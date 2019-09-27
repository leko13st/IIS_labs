using System;

namespace lab2_ExpertSystem
{
    [Serializable]
    public class Rules
    {
        public string Name { get; set; }
        public string[] Conds { get; set; }
        public string Ans { get; set; }
        public bool isExist { get; set; }
    }
}
