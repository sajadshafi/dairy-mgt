namespace DairyManagementSystem.Models.ViewModels {

   public class DashboardVM {
      public List<CountVM> Counts { get; set; }
   }

   public class CountVM {
      public string CountOf { get; set; }
      public string Icon { get; set; }
      public int Count { get; set; }
      public string Role { get; set; }
      public string NotInRole { get; set; }
   }

   public class DashboardSalesVM {
      public int TotalSalesCount { get; set; }
      public decimal TotalSalesAmount { get; set; }
   }
}
