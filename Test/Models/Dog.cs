namespace Test.Models
{
    [Serializable]
    public class Dog
    {
        public int Id { get; set; }
        public int Tail_Length { get; set; } = 0;
        public int Weight { get; set; } = 0;
        public string Color { get; set; } = string.Empty;
        public string Name{ get; set; } = string.Empty;
        public Dog( int tail_Length,int weight, string color, string name)
        {
            Tail_Length= tail_Length;
            Weight = weight;
            Color = color;
            Name = name;
        }
    }
}
