/* global Chart:false */

$(async function () {
   'use strict'

   // #region Calling API
   const weekDays = ["MON", "TUE", "WED", "THU", "FRI", "SAT", "SUN"]


   const response = await fetch("/dashboard/getgraphdetails")
      .then(response => response.json())
      .then(data => {
         return data
      })
      .catch(error => { console.log("Error: ", error) });

   // #endregion

   // #region LastWeekSales
   // Create a map to store the sales data for each day
   const salesDataMap = new Map();

   // Initialize sales data for each day to 0
   for (const day of weekDays) {
      salesDataMap.set(day, 0);
   }

   // Update sales data from fetched API response
   response.lastWeekSalesCount.forEach(item => {
      salesDataMap.set(item.day, item.count);
   });

   // Convert the sales data map to an array of objects
   const salesData = Array.from(salesDataMap, ([day, count]) => ({ day, count }));
   // #endregion
   // #region CurrentWeekSales
   // Create a map to store the sales data for each day
   const salesDataMapCurrentWeek = new Map();

   // Initialize sales data for each day to 0
   for (const day of weekDays) {
      salesDataMapCurrentWeek.set(day, 0);
   }

   // Update sales data from fetched API response
   response.currentWeekSalesCount.forEach(item => {
      salesDataMapCurrentWeek.set(item.day, item.count);
   });

   // Convert the sales data map to an array of objects
   const salesCurrentWeek = Array.from(salesDataMapCurrentWeek, ([day, count]) => ({ day, count }));
   // #endregion


   // #region CurrentWeekSalesAmount
   // Create a map to store the sales data for each day
   const salesAmountMapCurrentWeek = new Map();

   // Initialize sales data for each day to 0
   for (const day of weekDays) {
      salesAmountMapCurrentWeek.set(day, 0);
   }

   // Update sales data from fetched API response
   response.currentWeekSalesAmount.forEach(item => {
      salesAmountMapCurrentWeek.set(item.day, item.amount);
   });

   // Convert the sales data map to an array of objects
   const salesCurrentWeekAmount = Array.from(salesAmountMapCurrentWeek, ([day, amount]) => ({ day, amount }));
   // #endregion

   // #region Previous Week Sales Amount
   // Create a map to store the sales data for each day
   const salesAmountMapLastWeek = new Map();

   // Initialize sales data for each day to 0
   for (const day of weekDays) {
      salesAmountMapLastWeek.set(day, 0);
   }

   // Update sales data from fetched API response
   response.lastWeekSalesAmount.forEach(item => {
      salesAmountMapLastWeek.set(item.day, item.amount);
   });

   // Convert the sales data map to an array of objects
   const salesLastWeekAmount = Array.from(salesAmountMapLastWeek, ([day, amount]) => ({ day, amount }));
   // #endregion

   var ticksStyle = {
      fontColor: '#495057',
      fontStyle: 'bold'
   }

   var mode = 'index'
   var intersect = true

   var $salesChart = $('#sales-chart')
   // eslint-disable-next-line no-unused-vars
   var salesChart = new Chart($salesChart, {
      type: 'bar',
      data: {
         labels: salesCurrentWeekAmount.map(item => item.day),
         datasets: [
            {
               backgroundColor: '#007bff',
               borderColor: '#007bff',
               data: salesCurrentWeekAmount.map(item => item.amount)
            },
            {
               backgroundColor: '#ced4da',
               borderColor: '#ced4da',
               data: salesLastWeekAmount.map(item => item.amount)
            }
         ]
      },
      options: {
         maintainAspectRatio: false,
         tooltips: {
            mode: mode,
            intersect: intersect
         },
         hover: {
            mode: mode,
            intersect: intersect
         },
         legend: {
            display: false
         },
         scales: {
            yAxes: [{
               // display: false,
               gridLines: {
                  display: true,
                  lineWidth: '4px',
                  color: 'rgba(0, 0, 0, .2)',
                  zeroLineColor: 'transparent'
               },
               ticks: $.extend({
                  beginAtZero: true,

                  // Include a dollar sign in the ticks
                  callback: function (value) {
                     if (value >= 1000) {
                        value /= 1000
                        value += 'k'
                     }

                     return 'Rs ' + value
                  }
               }, ticksStyle)
            }],
            xAxes: [{
               display: true,
               gridLines: {
                  display: false
               },
               ticks: ticksStyle
            }]
         }
      }
   })

   var $visitorsChart = $('#visitors-chart')
   // eslint-disable-next-line no-unused-vars
   var visitorsChart = new Chart($visitorsChart, {
      data: {
         labels: salesData.map(item => item.day),
         datasets: [{
            type: 'line',
            data: salesCurrentWeek.map(item => item.count),
            backgroundColor: 'transparent',
            borderColor: '#007bff',
            pointBorderColor: '#007bff',
            pointBackgroundColor: '#007bff',
            fill: false
            // pointHoverBackgroundColor: '#007bff',
            // pointHoverBorderColor    : '#007bff'
         },
         {
            type: 'line',
            data: salesData.map(item => item.count),
            backgroundColor: 'tansparent',
            borderColor: '#ced4da',
            pointBorderColor: '#ced4da',
            pointBackgroundColor: '#ced4da',
            fill: false
            // pointHoverBackgroundColor: '#ced4da',
            // pointHoverBorderColor    : '#ced4da'
         }]
      },
      options: {
         maintainAspectRatio: false,
         tooltips: {
            mode: mode,
            intersect: intersect
         },
         hover: {
            mode: mode,
            intersect: intersect
         },
         legend: {
            display: false
         },
         scales: {
            yAxes: [{
               // display: false,
               gridLines: {
                  display: true,
                  lineWidth: '4px',
                  color: 'rgba(0, 0, 0, .2)',
                  zeroLineColor: 'transparent'
               },
               ticks: $.extend({
                  beginAtZero: true,
                  suggestedMax: 20
               }, ticksStyle)
            }],
            xAxes: [{
               display: true,
               gridLines: {
                  display: false
               },
               ticks: ticksStyle
            }]
         }
      }
   })
})

// lgtm [js/unused-local-variable]
