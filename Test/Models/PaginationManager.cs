namespace Test.Models
{
    public static class PaginationManager
    {
        public static int CurrentPage { get; set;} = 0;

        public static int MaxPageElements { get; set; } = 2;
        public static int Maxpages(List<Dog> dogs)
        {
            return dogs.Count*MaxPageElements/2+1;
        }
    }
}
