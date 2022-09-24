namespace MedprMVC.Models
{
    public class DrugModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PharmGroup { get; set; }
        public int Price { get; set; }
    }
}
