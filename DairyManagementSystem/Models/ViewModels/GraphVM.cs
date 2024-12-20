namespace DairyManagementSystem.Models.ViewModels {
   public class CountGraphVM {
      public string Day { get; set; }
      public int Count { get; set; }
   }

   public class AmountGraphVM {
      public string Day { get; set; }
      public decimal Amount { get; set; }
   }

   public class GraphVM {
      public List<CountGraphVM> CurrentWeekSalesCount { get; set; }
      public List<CountGraphVM> LastWeekSalesCount { get; set; }
      public List<AmountGraphVM> CurrentWeekSalesAmount { get; set; }
      public List<AmountGraphVM> LastWeekSalesAmount { get; set; }
   }
}
