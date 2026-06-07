using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using VehicleServiceTracker.Models;
using VehicleServiceTracker.Services;

namespace VehicleServiceTracker.Forms
{
    public class MaintenanceForm : Form
    {
        private readonly ServiceManager manager;
        private ComboBox vehicleCombo;
        private DateTimePicker datePicker;
        private TextBox descriptionBox, costBox;

        public MaintenanceForm(ServiceManager manager, int selectedVehicleId = 0)
        {
            this.manager = manager;
            BuildUi(selectedVehicleId);
        }

        private void BuildUi(int selectedVehicleId)
        {
            Text = "Add Maintenance Record";
            Width = 500;
            Height = 300;
            StartPosition = FormStartPosition.CenterParent;
            Font = new Font("Segoe UI", 10);

            AddLabel("Vehicle", 25, 25);
            vehicleCombo = new ComboBox { Left = 180, Top = 22, Width = 270, DropDownStyle = ComboBoxStyle.DropDownList };
            vehicleCombo.DataSource = manager.Vehicles.Select(v => new { v.VehicleId, Display = v.GetDisplayName() }).ToList();
            vehicleCombo.DisplayMember = "Display";
            vehicleCombo.ValueMember = "VehicleId";
            if (selectedVehicleId > 0) vehicleCombo.SelectedValue = selectedVehicleId;
            Controls.Add(vehicleCombo);

            AddLabel("Service Date", 25, 75);
            datePicker = new DateTimePicker { Left = 180, Top = 72, Width = 270, Format = DateTimePickerFormat.Short };
            Controls.Add(datePicker);

            AddLabel("Description", 25, 125);
            descriptionBox = new TextBox { Left = 180, Top = 122, Width = 270 };
            Controls.Add(descriptionBox);

            AddLabel("Cost", 25, 175);
            costBox = new TextBox { Left = 180, Top = 172, Width = 270 };
            Controls.Add(costBox);

            Button save = new Button { Text = "Save", Left = 280, Top = 220, Width = 80 };
            Button cancel = new Button { Text = "Cancel", Left = 370, Top = 220, Width = 80 };
            save.Click += SaveRecord;
            cancel.Click += (s, e) => Close();
            Controls.Add(save);
            Controls.Add(cancel);
        }

        private void AddLabel(string text, int left, int top)
        {
            Controls.Add(new Label { Text = text, Left = left, Top = top, Width = 140 });
        }

        private void SaveRecord(object sender, EventArgs e)
        {
            try
            {
                if (vehicleCombo.SelectedValue == null) throw new InvalidOperationException("Please add/select a vehicle first.");
                decimal cost;
                if (!decimal.TryParse(costBox.Text, out cost)) throw new ArgumentException("Cost must be a number.");

                MaintenanceRecord record = new MaintenanceRecord
                {
                    VehicleId = Convert.ToInt32(vehicleCombo.SelectedValue),
                    ServiceDate = datePicker.Value.Date,
                    Description = descriptionBox.Text,
                    Cost = cost
                };
                manager.AddMaintenanceRecord(record);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Record Error");
            }
        }
    }
}
