using System;
using System.Drawing;
using System.Windows.Forms;
using VehicleServiceTracker.Services;

namespace VehicleServiceTracker.Forms
{
    public class ReportForm : Form
    {
        private readonly ServiceManager manager;
        private readonly int vehicleId;
        private TextBox outputBox;

        public ReportForm(ServiceManager manager, int vehicleId)
        {
            this.manager = manager;
            this.vehicleId = vehicleId;
            BuildUi();
        }

        private void BuildUi()
        {
            Text = "Reports";
            Width = 650;
            Height = 520;
            StartPosition = FormStartPosition.CenterParent;
            Font = new Font("Segoe UI", 10);

            Button historyButton = new Button { Text = "Service History", Left = 15, Top = 15, Width = 150 };
            Button costButton = new Button { Text = "Cost Summary", Left = 175, Top = 15, Width = 150 };
            historyButton.Click += ShowHistoryReport;
            costButton.Click += ShowCostReport;
            Controls.Add(historyButton);
            Controls.Add(costButton);

            outputBox = new TextBox
            {
                Left = 15,
                Top = 60,
                Width = 600,
                Height = 390,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                ReadOnly = true,
                Font = new Font("Consolas", 10)
            };
            Controls.Add(outputBox);
            ShowHistoryReport(null, null);
        }

        private void ShowHistoryReport(object sender, EventArgs e)
        {
            IServiceReport report = new ServiceHistoryReport(manager);
            outputBox.Text = report.GenerateReport(vehicleId);
        }

        private void ShowCostReport(object sender, EventArgs e)
        {
            IServiceReport report = new CostSummaryReport(manager);
            outputBox.Text = report.GenerateReport(vehicleId);
        }
    }
}
