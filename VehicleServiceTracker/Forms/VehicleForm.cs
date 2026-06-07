// Vehicle class stores basic vehicle details.
using System;
using System.Drawing;
using System.Windows.Forms;
using VehicleServiceTracker.Models;
using VehicleServiceTracker.Services;

namespace VehicleServiceTracker.Forms
{
    public class VehicleForm : Form
    {
        private readonly ServiceManager manager;
        private readonly int editingVehicleId;
        private TextBox regBox, makeBox, modelBox, yearBox, ownerBox, contactBox;

        public VehicleForm(ServiceManager manager, int vehicleId = 0)
        {
            this.manager = manager;
            editingVehicleId = vehicleId;
            BuildUi();
            LoadVehicle();
        }

        private void BuildUi()
        {
            Text = editingVehicleId == 0 ? "Add Vehicle" : "Edit Vehicle";
            Width = 460;
            Height = 380;
            StartPosition = FormStartPosition.CenterParent;
            Font = new Font("Segoe UI", 10);

            regBox = AddField("Registration Number", 25, 25);
            makeBox = AddField("Make", 25, 75);
            modelBox = AddField("Model", 25, 125);
            yearBox = AddField("Year", 25, 175);
            ownerBox = AddField("Owner Name", 25, 225);
            contactBox = AddField("Contact Number", 25, 275);

            Button save = new Button { Text = "Save", Left = 250, Top = 300, Width = 80 };
            Button cancel = new Button { Text = "Cancel", Left = 340, Top = 300, Width = 80 };
            save.Click += SaveVehicle;
            cancel.Click += (s, e) => Close();
            Controls.Add(save);
            Controls.Add(cancel);
        }

        private TextBox AddField(string labelText, int left, int top)
        {
            Label label = new Label { Text = labelText, Left = left, Top = top, Width = 160 };
            TextBox box = new TextBox { Left = 190, Top = top - 3, Width = 230 };
            Controls.Add(label);
            Controls.Add(box);
            return box;
        }

        private void LoadVehicle()
        {
            if (editingVehicleId == 0) return;
            Vehicle vehicle = manager.GetVehicleById(editingVehicleId);
            if (vehicle == null) return;

            regBox.Text = vehicle.RegistrationNumber;
            makeBox.Text = vehicle.Make;
            modelBox.Text = vehicle.Model;
            yearBox.Text = vehicle.Year.ToString();
            ownerBox.Text = vehicle.Owner.Name;
            contactBox.Text = vehicle.Owner.ContactNumber;
        }

        private void SaveVehicle(object sender, EventArgs e)
        {
            try
            {
                int year;
                if (!int.TryParse(yearBox.Text, out year)) throw new ArgumentException("Year must be a number.");

                Owner owner = new Owner(editingVehicleId, ownerBox.Text, contactBox.Text);
                Car car = new Car(editingVehicleId, regBox.Text, makeBox.Text, modelBox.Text, year, owner);

                if (editingVehicleId == 0) manager.AddVehicle(car);
                else manager.UpdateVehicle(car);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Validation Error");
            }
        }
    }
}
